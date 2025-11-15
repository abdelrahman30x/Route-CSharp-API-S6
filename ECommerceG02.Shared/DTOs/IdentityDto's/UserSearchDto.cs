using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.IdentityDto_s
{
    public class UserSearchDto
    {
        public string SearchTerm { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public DateTime? RegisteredAfter { get; set; }
        public DateTime? RegisteredBefore { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }

}
