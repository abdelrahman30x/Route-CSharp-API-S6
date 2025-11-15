using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class PasswordRequirements
    {
        public bool HasMinimumLength { get; set; }
        public bool HasUpperCase { get; set; }
        public bool HasLowerCase { get; set; }
        public bool HasDigit { get; set; }
        public bool HasSpecialCharacter { get; set; }
        public int MinimumLength { get; set; } = 8;
    }

}
