using ECommerceG02.Shared.DTOs.BasketDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IBasketServices
    {
        Task<BasketDto?> GetBasketAsync(string basketId);
        Task<BasketDto?> CreateUpdateBasketAsync(BasketDto basket);
        Task<BasketDeleteDto> DeleteBasketAsync(string basketId);
    }
}
