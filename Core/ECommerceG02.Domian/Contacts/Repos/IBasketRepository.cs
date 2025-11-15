using ECommerceG02.Domian.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Contacts.Repos
{ 
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);

        Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket);

        Task<bool> DeleteBasketAsync(string basketId);
    }
}
