using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class DeleteAccountDto
    {
        [Required(ErrorMessage = "Password is required to delete account")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm account deletion")]
        public bool ConfirmDeletion { get; set; }
    }
}
