using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class RegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Url]
        public string ProfilePictureUrl { get; set; }

        [StringLength(500)]
        public string Bio { get; set; }

        [EmailAddress]
        public string AlternativeEmail { get; set; }

        [Phone]
        public string MobileNumber { get; set; }   // بدل PhoneNumber

        [StringLength(20)]
        public string PreferredLanguage { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; }
    }
}
