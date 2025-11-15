using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class ErrorResponseDto
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public string ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
