using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure
{
    public class BasketRepository : IBasketRepository
    {
        private IDatabase database { get; }

        public BasketRepository(IConnectionMultiplexer redis){
            
            database = redis.GetDatabase();
        }
        
        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : 
                JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await database.StringSetAsync(basket.Id, 
                JsonSerializer.Serialize<CustomerBasket>(basket), TimeSpan.FromDays(30));

            if(!created){
                return null;
            }

            return await GetBasketAsync(basket.Id);
        }
    }
}