using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        //get basket
        public Task<CustomerBasket> GetBasketAsync(string basketId);

        //create or update basket
        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);

        //delete basket
        public Task<bool> DeleteBasketAsync(string basketId);
    }
}