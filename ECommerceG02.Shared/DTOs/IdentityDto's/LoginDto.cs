using System.ComponentModel.DataAnnotations;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class LoginDto
    {
        [Required(ErrorMessage = "UserName or email is required")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
