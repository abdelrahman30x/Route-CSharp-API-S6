using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class VerifyPhoneNumberDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Verification code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Verification code must be 6 digits")]
        public string VerificationCode { get; set; }
    }
}
