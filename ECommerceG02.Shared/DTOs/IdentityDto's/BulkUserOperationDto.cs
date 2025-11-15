using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class BulkUserOperationDto
    {
        [Required(ErrorMessage = "User IDs are required")]
        public List<string> UserIds { get; set; } = new List<string>();

        [Required(ErrorMessage = "Operation is required")]
        public string Operation { get; set; } 
    }
}
