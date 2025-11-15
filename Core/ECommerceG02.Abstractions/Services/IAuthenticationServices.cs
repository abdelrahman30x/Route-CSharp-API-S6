using ECommerceG02.Shared.DTOs.AddressDtos;
using ECommerceG02.Shared.DTOs.IdentityDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IAuthenticationServices
    {
        Task<AddressDto> GetUserAddressAsync(string userId);
        Task<AddressDto> UpdateUserAddressAsync(string userId, AddressDto addressDto);
        Task<bool> CheakEmailExistAsync(string email);
        Task<string> GenerateSaltAsync();
        Task<bool> IsPasswordStrongAsync(string password);
        Task<bool> IsEmailValidAsync(string email);
        Task<bool> IsUsernameValidAsync(string username);
        Task<bool> IsUserExistByEmailAsync(string email);
        Task<bool> IsUserExistByUsernameAsync(string username);
        Task<bool> IsUserExistByIdAsync(string userId);
        Task<AuthResponseDto> AuthenticateUserAsync(LoginDto loginDto);
        Task<bool> AreUserCredentialsValidAsync(string usernameOrEmail, string password);
        Task<bool> ValidateJwtTokenAsync(string token);
        Task<string> GenerateJwtTokenAsync(string userId, string email, IList<string> roles);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<string> GenerateEmailVerificationTokenAsync(string email);
        Task<string> GeneratePasswordResetLinkAsync(string userId);
        Task<string> GenerateEmailConfirmationLinkAsync(string userId);
        Task<string> GenerateEmailVerificationLinkAsync(string email);
        Task<bool> ConfirmUserEmailAsync(string userId, string confirmationToken);
        Task<bool> ResetUserPasswordAsync(string userId, string resetToken, string newPassword);
        Task<bool> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> UpdateUserAsync(string userId, string newUsername, string newEmail);
        Task<bool> DeleteUserAsync(string userId);
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto registerDto);
        Task<bool> SignInUserAsync(string userId, string password);
        Task<bool> SignOutUserAsync(string userId);
        Task<string> GetUserTokenAsync(string userId);
        Task<string> GetUserIdByTokenAsync(string token);
        Task<string> GetUserIdAsync(string usernameOrEmail);
        Task<string> GetUserEmailAsync(string userId);
        Task<string> GetUserNameAsync(string userId);
        Task<string> GetUserRoleAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> IsUserInRoleAsync(string userId, string role);
        Task<string> GetCurrentUsernameAsync();
        Task<string> GetCurrentUserEmailAsync();
        Task<string> GetCurrentUserIdAsync();
        Task<UserDto> GetCurrentUserAsync();
        Task<bool> IsAuthenticated();
        Task<bool> AuthenticateUserByTokenAsync(string token);
        Task<bool> IsUserAuthenticatedAsync(string token);
        Task<bool> IsUserIdVerifiedAsync(string userId);
        Task<bool> IsUserUsernameVerifiedAsync(string username);
        Task<bool> IsUserEmailVerifiedAsync(string email);
        Task<bool> IsUserEmailConfirmedAsync(string userId);
        Task<string> CreateUserAsync(string username, string email, string password);
        Task<string> GetJwtTokenByUserIdAsync(string userId);
        Task<string> GetUserIdByJwtTokenAsync(string token);
        Task<string> GetUserIdByUsernameAsync(string username);
        Task<string> GetUserIdByEmailAsync(string email);
        Task<string> GetUserIdByUsernameOrEmailAsync(string usernameOrEmail);
     
        Task<bool> VerifyPasswordAsync(string hashedPassword, string providedPassword);
        Task<string> HashPasswordAsync(string password);
        Task<string> HashPasswordWithSaltAsync(string password, string salt);

    }
}
