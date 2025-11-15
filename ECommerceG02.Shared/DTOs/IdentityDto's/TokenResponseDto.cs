using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class TokenResponseDto
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; } // in seconds
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
