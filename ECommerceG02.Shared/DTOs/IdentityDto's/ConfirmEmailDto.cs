using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class ConfirmEmailDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Confirmation token is required")]
        public string Token { get; set; }
    }
}
