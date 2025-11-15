using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class SuccessResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

}
