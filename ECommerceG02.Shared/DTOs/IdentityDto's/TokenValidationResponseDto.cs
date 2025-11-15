using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class TokenValidationResponseDto
    {
        public bool IsValid { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
