using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseAPIController
    {   
        public IBasketRepository basketRepo { get; }

        public BasketController(IBasketRepository basketRepository)
        {
            basketRepo = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await basketRepo.GetBasketAsync(id);

            if(basket == null)
            {
                return new CustomerBasket(id);
            }

            //O ?? verifica se basket é null, se não for devolve basket, else new etc
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updatedBasket = await basketRepo.UpdateBasketAsync(basket);
            
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await basketRepo.DeleteBasketAsync(id);
        }

    }
}