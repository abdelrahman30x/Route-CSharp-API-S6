using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.BasketDto_s
{
    public class BasketDto
    {
        public string Id { get; set; }
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

    }
}
