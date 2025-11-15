using AutoMapper;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Exceptions.NotFound;
using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Presistence.Identity.Models;
using ECommerceG02.Shared.DTOs.AddressDtos;
using ECommerceG02.Shared.DTOs.IdentityDto_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using JwtSecurityToken = System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
using JwtSecurityTokenHandler = System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler;

namespace ECommerceG02.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly string _jwtSecretKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;

        public AuthenticationServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILogger<AuthenticationServices> logger
            ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;
            _jwtSecretKey = _configuration["Jwt:SecretKey"] ?? "YourDefaultSecretKeyHere";
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? "YourApp";
            _jwtAudience = _configuration["Jwt:Audience"] ?? "YourAppUsers";
            _jwtExpirationMinutes = Convert.ToInt32(_configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto)
        {
            try
            {
                _logger.LogInformation("Starting user registration for {UserName}", dto.UserName);

                var user = _mapper.Map<ApplicationUser>(dto);
                _logger.LogInformation("Mapped RegisterDto to ApplicationUser: {@User}", user);

                user.EmailConfirmed = false;

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("User creation failed: {@Errors}", result.Errors);
                    return null;
                }

                _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

                user = await _userManager.FindByNameAsync(user.UserName);
                _logger.LogInformation("Fetched user from database: {@User}", user);

                var roles = new List<string>(); // أو حط هنا roles افتراضية لو تحب
                var addRolesResult = await _userManager.AddToRolesAsync(user, roles);
                if (!addRolesResult.Succeeded)
                {
                    _logger.LogWarning("Adding roles failed: {@Errors}", addRolesResult.Errors);
                }
                else
                {
                    _logger.LogInformation("Roles added: {@Roles}", roles);
                }

                var token = await GenerateJwtTokenAsync(user.Id, user.Email, roles);
                _logger.LogInformation("Generated JWT token for user {UserId}", user.Id);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roles;

                var response = new AuthResponseDto
                {
                    Success = true,
                    Message = "User registered successfully",
                    Token = token,
                    User = userDto
                };

                _logger.LogInformation("Returning AuthResponseDto: {@Response}", response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during user registration for {UserName}", dto.UserName);
                throw;
            }
        }


        public async Task<AuthResponseDto> AuthenticateUserAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

            if (user == null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwtTokenAsync(user.Id, user.Email, roles);

            var response = new AuthResponseDto
            {
                Success = true,
                Message = "User login successfully",
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
            response.User.Roles = roles.ToList();

            return response;
        }
        public async Task<AddressDto> GetUserAddressAsync(string userId)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new UserNotFoundException(userId);
            if (user.Address == null)
            {
                throw new AddressNotFoundException(userId);
            }
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(string userId, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new UserNotFoundException(userId);
            if (user.Address == null)
            {
                user.Address = new Address();
            }
            user.Address.AddressLine1 = addressDto.AddressLine1;
            user.Address.AddressLine2 = addressDto.AddressLine2;
            user.Address.City = addressDto.City;
            user.Address.State = addressDto.State;
            user.Address.State = addressDto.State;
            user.Address.Country = addressDto.Country;
            user.Address.PostalCode = addressDto.PostalCode;
            user.Address.PhoneNumber = addressDto.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to update address.");
            return _mapper.Map<AddressDto>(user.Address);
        }



        public async Task<bool> CheakEmailExistAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return true;
            return false;
        }
        public async Task<string> GenerateSaltAsync()
        {
            var saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public async Task<bool> IsPasswordStrongAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Check minimum length
            if (password.Length < 8)
                return false;

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
                return false;

            // Check for at least one lowercase letter
            if (!password.Any(char.IsLower))
                return false;

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
                return false;

            // Check for at least one special character
            var specialChars = @"!@#$%^&*()_+-=[]{};':,.<>?/\|`~";
            if (!password.Any(c => specialChars.Contains(c)))
                return false;

            return await Task.FromResult(true);
        }

        public async Task<bool> IsEmailValidAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

            return await Task.FromResult(regex.IsMatch(email));
        }

        public async Task<bool> IsUsernameValidAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Username should be 3-20 characters, alphanumeric with underscores allowed
            var usernamePattern = @"^[a-zA-Z0-9_]{3,20}$";
            var regex = new Regex(usernamePattern);

            return await Task.FromResult(regex.IsMatch(username));
        }

        public async Task<bool> IsUserExistByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> IsUserExistByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<bool> IsUserExistByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }

        public async Task<bool> AreUserCredentialsValidAsync(string usernameOrEmail, string password)
        {
            var user = await FindUserByUsernameOrEmailAsync(usernameOrEmail);
            if (user == null)
                return false;

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<bool> ValidateJwtTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GenerateJwtTokenAsync(string userId, string email, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add role claims
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> GenerateEmailVerificationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetLinkAsync(string userId)
        {
            var token = await GeneratePasswordResetTokenAsync(userId);
            if (string.IsNullOrEmpty(token))
                return null;

            var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:5001";
            var encodedToken = Uri.EscapeDataString(token);
            return $"{baseUrl}/reset-password?userId={userId}&token={encodedToken}";
        }

        public async Task<string> GenerateEmailConfirmationLinkAsync(string userId)
        {
            var token = await GenerateEmailConfirmationTokenAsync(userId);
            if (string.IsNullOrEmpty(token))
                return null;

            var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:5001";
            var encodedToken = Uri.EscapeDataString(token);
            return $"{baseUrl}/confirm-email?userId={userId}&token={encodedToken}";
        }

        public async Task<string> GenerateEmailVerificationLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var token = await GenerateEmailVerificationTokenAsync(email);
            if (string.IsNullOrEmpty(token))
                return null;

            var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:5001";
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(email);
            return $"{baseUrl}/verify-email?email={encodedEmail}&token={encodedToken}";
        }

        public async Task<bool> ConfirmUserEmailAsync(string userId, string confirmationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            return result.Succeeded;
        }

        public async Task<bool> ResetUserPasswordAsync(string userId, string resetToken, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(string userId, string newUsername, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var updateSuccessful = true;

            if (!string.IsNullOrWhiteSpace(newUsername) && user.UserName != newUsername)
            {
                var setUsernameResult = await _userManager.SetUserNameAsync(user, newUsername);
                updateSuccessful = updateSuccessful && setUsernameResult.Succeeded;
            }

            if (!string.IsNullOrWhiteSpace(newEmail) && user.Email != newEmail)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, newEmail);
                updateSuccessful = updateSuccessful && setEmailResult.Succeeded;
            }

            return updateSuccessful;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> SignInUserAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Succeeded;
        }

        public async Task<bool> SignOutUserAsync(string userId)
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<string> GetUserTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return await GenerateJwtTokenAsync(user.Id, user.Email, roles);
        }

        public async Task<string> GetUserIdByTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return await Task.FromResult(userIdClaim?.Value);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetUserIdAsync(string usernameOrEmail)
        {
            var user = await FindUserByUsernameOrEmailAsync(usernameOrEmail);
            return user?.Id;
        }

        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName;
        }

        public async Task<string> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<UserDto> GetCurrentUserAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (string.IsNullOrEmpty(userId))
                return null;

            var username = await GetUserNameAsync(userId);
            var email = await GetUserEmailAsync(userId);
            var roles = await GetUserRolesAsync(userId);
            var isEmailConfirmed = await IsUserEmailConfirmedAsync(userId);

            return new UserDto
            {
                Id = userId,
                UserName = username,
                Email = email,
                Roles = roles?.ToList() ?? new List<string>(),
                EmailConfirmed = isEmailConfirmed
            };
        }

        public async Task<string> GetCurrentUsernameAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (string.IsNullOrEmpty(userId))
                return null;

            return await GetUserNameAsync(userId);
        }

        public async Task<string> GetCurrentUserEmailAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (string.IsNullOrEmpty(userId))
                return null;

            return await GetUserEmailAsync(userId);
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return null;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            return await Task.FromResult(userIdClaim?.Value);
        }

        public async Task<bool> IsAuthenticated()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return await Task.FromResult(user?.Identity?.IsAuthenticated ?? false);
        }

        public async Task<bool> AuthenticateUserByTokenAsync(string token)
        {
            if (!await ValidateJwtTokenAsync(token))
                return false;

            var userId = await GetUserIdByTokenAsync(token);
            return !string.IsNullOrEmpty(userId);
        }

        public async Task<bool> IsUserAuthenticatedAsync(string token)
        {
            return await ValidateJwtTokenAsync(token);
        }

        public async Task<bool> IsUserIdVerifiedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.EmailConfirmed ?? false;
        }

        public async Task<bool> IsUserUsernameVerifiedAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user?.EmailConfirmed ?? false;
        }

        public async Task<bool> IsUserEmailVerifiedAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.EmailConfirmed ?? false;
        }

        public async Task<bool> IsUserEmailConfirmedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.EmailConfirmed ?? false;
        }

        public async Task<string> CreateUserAsync(string username, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return user.Id;
            }

            return null;
        }

        public async Task<string> GetJwtTokenByUserIdAsync(string userId)
        {
            return await GetUserTokenAsync(userId);
        }

        public async Task<string> GetUserIdByJwtTokenAsync(string token)
        {
            return await GetUserIdByTokenAsync(token);
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user?.Id;
        }

        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.Id;
        }

        public async Task<string> GetUserIdByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await GetUserIdAsync(usernameOrEmail);
        }

        public async Task<bool> VerifyPasswordAsync(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
                return false;

            var hasher = new PasswordHasher<ApplicationUser>();
            var verificationResult = hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);

            return await Task.FromResult(verificationResult != PasswordVerificationResult.Failed);
        }

        public async Task<string> HashPasswordAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return null;

            var hasher = new PasswordHasher<ApplicationUser>();
            return await Task.FromResult(hasher.HashPassword(null, password));
        }

        public async Task<string> HashPasswordWithSaltAsync(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(salt))
                return null;

            var saltedPassword = password + salt;

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return await Task.FromResult(Convert.ToBase64String(hashBytes));
            }
        }

        private async Task<ApplicationUser> FindUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail))
                return null;

            var user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(usernameOrEmail);
            }

            return user;
        }

      
    }
}