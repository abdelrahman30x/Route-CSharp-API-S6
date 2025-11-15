using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.BasketDto_s
{
    public class BasketItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;


        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public int Quantity { get; set; }

    }
}
