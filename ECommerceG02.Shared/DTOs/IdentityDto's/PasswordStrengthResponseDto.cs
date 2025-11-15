using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class PasswordStrengthResponseDto
    {
        public bool IsStrong { get; set; }
        public int Score { get; set; } // 0-5 score
        public List<string> Suggestions { get; set; } = new List<string>();
        public PasswordRequirements Requirements { get; set; }
    }
}
