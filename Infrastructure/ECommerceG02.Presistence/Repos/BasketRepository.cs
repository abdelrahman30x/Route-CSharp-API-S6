using ECommerceG02.Domian.Contacts.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceG02.Domian.Models.Baskets;
using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace ECommerceG02.Presistence.Repos
{
    public class BasketRepository(IConnectionMultiplexer connection, ILogger<BasketRepository> logger): IBasketRepository
    {
        public readonly IDatabase _database = connection.GetDatabase();
        public readonly ILogger<BasketRepository> _logger = logger;
        public async Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket)
        {
           var JsonBasket =  JsonSerializer.Serialize(basket);
            _logger.LogDebug("Serialized Basket: {JsonBasket}", JsonBasket);
            var IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, TimeSpan.FromDays(30));
            if (!IsCreatedOrUpdated)
                return null;
            var createdOrUpdatedBasket = await GetBasketAsync(basket.Id);
            return createdOrUpdatedBasket;

        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var Bakset = await _database.StringGetAsync(basketId);
            if (Bakset.IsNullOrEmpty)
                return null;
            else
            {
                return JsonSerializer.Deserialize<CustomerBasket>(Bakset);
            }
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
           return await _database.KeyDeleteAsync(basketId);
        }

    }
}
