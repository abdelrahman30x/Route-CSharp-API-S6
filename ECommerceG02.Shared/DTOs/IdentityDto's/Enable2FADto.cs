using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class Enable2FADto
    {
        [Required(ErrorMessage = "Password is required to enable 2FA")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
