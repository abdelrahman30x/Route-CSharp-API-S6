using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.BasketDto_s
{
    public class BasketDeleteDto
    {
        public bool Deleted { get; set; } = false;

        public string Message { get; set; } = null!;

    }
}
