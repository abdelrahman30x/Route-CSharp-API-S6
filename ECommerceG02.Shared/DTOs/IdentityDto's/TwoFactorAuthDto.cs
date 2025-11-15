using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class TwoFactorAuthDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Two-factor code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Two-factor code must be 6 digits")]
        public string Code { get; set; }

        public bool RememberDevice { get; set; } = false;
    }

}
