using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Reset token is required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
