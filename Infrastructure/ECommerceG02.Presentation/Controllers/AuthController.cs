using ECommerceG02.Abstractions.Services;
using ECommerceG02.Shared.DTOs.AddressDtos;
using ECommerceG02.Shared.DTOs.IdentityDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceG02.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationServices _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthenticationServices authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(ErrorResponseDto), 409)]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponseDto { Message = "Invalid input data" });

            try
            {
                var response = await _authService.RegisterUserAsync(model);

                if (!response.Success)
                {
                    return Conflict(new ErrorResponseDto { Message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new ErrorResponseDto { Message = "An error occurred during registration" });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 401)]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponseDto { Message = "Invalid input data" });

            try
            {
                var response = await _authService.AuthenticateUserAsync(model);

                if (!response.Success)
                {
                    return Unauthorized(new ErrorResponseDto { Message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new ErrorResponseDto { Message = "An error occurred during login" });
            }
        }
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 401)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                // Use service to get current user DTO directly
                var user = await _authService.GetCurrentUserAsync();

                if (user == null)
                {
                    return Unauthorized(new ErrorResponseDto { Message = "User not authenticated" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching current user info");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred while fetching user info"
                });
            }
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto model)
        {
            try
            {
                var result = await _authService.ConfirmUserEmailAsync(model.UserId, model.Token);

                if (!result)
                {
                    return BadRequest(new ErrorResponseDto
                    {
                        Message = "Invalid or expired confirmation token"
                    });
                }

                _logger.LogInformation($"Email confirmed for user: {model.UserId}");

                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "Email confirmed successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred during email confirmation"
                });
            }
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            try
            {
                var userId = await _authService.GetUserIdByEmailAsync(model.Email);

                if (!string.IsNullOrEmpty(userId))
                {
                    var resetLink = await _authService.GeneratePasswordResetLinkAsync(userId);

                    // TODO: Send password reset email
                    // await _emailService.SendPasswordResetEmailAsync(model.Email, resetLink);

                    _logger.LogInformation($"Password reset requested for: {model.Email}");
                }

                // Always return success to prevent email enumeration
                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "If the email exists, a password reset link has been sent"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset request");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred during password reset request"
                });
            }
        }


        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            try
            {
                if (!await _authService.IsPasswordStrongAsync(model.NewPassword))
                {
                    return BadRequest(new ErrorResponseDto
                    {
                        Message = "Password must be at least 8 characters with uppercase, lowercase, number, and special character"
                    });
                }

                var result = await _authService.ResetUserPasswordAsync(model.UserId, model.Token, model.NewPassword);

                if (!result)
                {
                    return BadRequest(new ErrorResponseDto
                    {
                        Message = "Invalid or expired reset token"
                    });
                }

                _logger.LogInformation($"Password reset successfully for user: {model.UserId}");

                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "Password reset successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred during password reset"
                });
            }
        }


        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            try
            {
                var userId = await _authService.GetCurrentUserIdAsync();

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ErrorResponseDto { Message = "User not authenticated" });
                }

                if (!await _authService.IsPasswordStrongAsync(model.NewPassword))
                {
                    return BadRequest(new ErrorResponseDto
                    {
                        Message = "Password must be at least 8 characters with uppercase, lowercase, number, and special character"
                    });
                }

                var result = await _authService.ChangeUserPasswordAsync(userId, model.CurrentPassword, model.NewPassword);

                if (!result)
                {
                    return BadRequest(new ErrorResponseDto { Message = "Current password is incorrect" });
                }

                _logger.LogInformation($"Password changed for user: {userId}");

                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "Password changed successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred during password change"
                });
            }
        }


        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            try
            {
                var userId = await _authService.GetCurrentUserIdAsync();

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ErrorResponseDto { Message = "User not authenticated" });
                }

                // Validate new username if provided
                if (!string.IsNullOrEmpty(model.Username))
                {
                    if (!await _authService.IsUsernameValidAsync(model.Username))
                    {
                        return BadRequest(new ErrorResponseDto
                        {
                            Message = "Invalid username format"
                        });
                    }

                    var existingUser = await _authService.GetUserIdByUsernameAsync(model.Username);
                    if (!string.IsNullOrEmpty(existingUser) && existingUser != userId)
                    {
                        return BadRequest(new ErrorResponseDto { Message = "Username already taken" });
                    }
                }

                // Validate new email if provided
                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (!await _authService.IsEmailValidAsync(model.Email))
                    {
                        return BadRequest(new ErrorResponseDto { Message = "Invalid email format" });
                    }

                    var existingUser = await _authService.GetUserIdByEmailAsync(model.Email);
                    if (!string.IsNullOrEmpty(existingUser) && existingUser != userId)
                    {
                        return BadRequest(new ErrorResponseDto { Message = "Email already in use" });
                    }
                }

                var result = await _authService.UpdateUserAsync(userId, model.Username, model.Email);

                if (!result)
                {
                    return BadRequest(new ErrorResponseDto { Message = "Failed to update profile" });
                }

                _logger.LogInformation($"Profile updated for user: {userId}");

                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "Profile updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred while updating profile"
                });
            }
        }


        [HttpPost("validate-token")]
        [ProducesResponseType(typeof(TokenValidationResponseDto), 200)]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto model)
        {
            try
            {
                var isValid = await _authService.ValidateJwtTokenAsync(model.Token);

                if (!isValid)
                {
                    return Ok(new TokenValidationResponseDto
                    {
                        IsValid = false,
                        Message = "Invalid or expired token"
                    });
                }

                var userId = await _authService.GetUserIdByTokenAsync(model.Token);

                return Ok(new TokenValidationResponseDto
                {
                    IsValid = true,
                    UserId = userId,
                    Message = "Token is valid"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred during token validation"
                });
            }
        }


        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = await _authService.GetCurrentUserIdAsync();
                await _authService.SignOutUserAsync(userId);

                _logger.LogInformation($"User logged out: {userId}");

                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "Logged out successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred during logout"
                });
            }
        }


        [HttpDelete("account")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto model)
        {
            try
            {
                var userId = await _authService.GetCurrentUserIdAsync();

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ErrorResponseDto { Message = "User not authenticated" });
                }

                // Verify password before deletion
                var username = await _authService.GetUserNameAsync(userId);
                if (!await _authService.AreUserCredentialsValidAsync(username, model.Password))
                {
                    return BadRequest(new ErrorResponseDto { Message = "Invalid password" });
                }

                var result = await _authService.DeleteUserAsync(userId);

                if (!result)
                {
                    return BadRequest(new ErrorResponseDto { Message = "Failed to delete account" });
                }

                _logger.LogInformation($"Account deleted for user: {userId}");

                return Ok(new SuccessResponseDto
                {
                    Success = true,
                    Message = "Account deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred while deleting account"
                });
            }
        }


        [HttpGet("check-email")]
        [ProducesResponseType(typeof(AvailabilityResponseDto), 200)]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest(new ErrorResponseDto { Message = "Email is required" });
                }

                var exists = await _authService.IsUserExistByEmailAsync(email);

                return Ok(new AvailabilityResponseDto
                {
                    Available = !exists,
                    Message = exists ? "Email already in use" : "Email is available"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email availability");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred while checking email availability"
                });
            }
        }


        [HttpGet("check-username")]
        [ProducesResponseType(typeof(AvailabilityResponseDto), 200)]
        public async Task<IActionResult> CheckUsernameAvailability([FromQuery] string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest(new ErrorResponseDto { Message = "Username is required" });
                }

                var exists = await _authService.IsUserExistByUsernameAsync(username);

                return Ok(new AvailabilityResponseDto
                {
                    Available = !exists,
                    Message = exists ? "Username already taken" : "Username is available"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username availability");
                return StatusCode(500, new ErrorResponseDto
                {
                    Message = "An error occurred while checking username availability"
                });
            }
        }

        [HttpGet("Address")]
        [Authorize]
        [ProducesResponseType(typeof(AddressDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 401)]
        public async Task<IActionResult> GetUserAddress()
        {
            var userId = await _authService.GetCurrentUserIdAsync();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ErrorResponseDto { Message = "User not authenticated" });

            var address = await _authService.GetUserAddressAsync(userId);
            return Ok(address);
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAddress([FromBody] AddressDto addressDto)
        {
            var userId = await _authService.GetCurrentUserIdAsync();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ErrorResponseDto { Message = "User not authenticated" });

            var updatedAddress = await _authService.UpdateUserAddressAsync(userId, addressDto);
            return Ok(updatedAddress);
        }

    }
}