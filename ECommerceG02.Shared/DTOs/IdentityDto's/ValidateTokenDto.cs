using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class ValidateTokenDto
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }

}
