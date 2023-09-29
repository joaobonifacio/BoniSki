using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseAPIController
    {   
        public IBasketRepository basketRepo { get; }
        public IMapper mapper { get; }

        public BasketController(IBasketRepository basketRepository, IMapper _mapper)
        {
            basketRepo = basketRepository;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await basketRepo.GetBasketAsync(id);

            if(basket == null)
            {
                return new CustomerBasket(id);
            }

            //O ?? verifica se basket é null, se não for devolve basket, else new basket
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDTO basket)
        {
            var customerBasketasket = mapper.Map<CustomerBasketDTO, CustomerBasket>(basket);
            
            var updatedBasket = await basketRepo.UpdateBasketAsync(customerBasketasket);
            
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await basketRepo.DeleteBasketAsync(id);
        }
    }
}