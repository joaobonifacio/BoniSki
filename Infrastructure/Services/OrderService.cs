using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepo;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IBasketRepository  basketRepository, IUnitOfWork iunitOfWork)
        {
            this.basketRepo = basketRepository;
            this.unitOfWork = iunitOfWork;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, 
            Address shippingAddress)
        {
            // get basket from basket repo
            var basket = await basketRepo.GetBasketAsync(basketId);

            // get the items from product repo
            var items = new List<OrderItem>();

            foreach(BasketItem coisa in basket.Items)
            {
              //var productItem = await productRepo.GetByIdAsync(coisa.Id);
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(coisa.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, 
                    productItem.PictureUrl);
                var quantity = coisa.Quantity;
                var price = productItem.Price;

                items.Add(new OrderItem(quantity, price, itemOrdered));      
            }

            //var method = await deliveryMethodRepo.GetByIdAsync(deliveryMethod);
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().
                GetByIdAsync(deliveryMethodId);

            //calculate sub total
            var subtotal = items.Sum(item=>item.Price*item.Quantity);

            //Verificar se temos Order
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order != null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.SubTotal = subtotal;
                unitOfWork.Repository<Order>().Update(order);

            }
            else
            {
                //create the order 
                order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, 
                    basket.PaymentIntentId);
                
                unitOfWork.Repository<Order>().Add(order);

            }
            
            //save order to db 
            var result = await unitOfWork.Complete();

            if(result <= 0)
            {
                return null;
            }

            // //Delete basket
            // await basketRepo.DeleteBasketAsync(basket.Id);

            //return
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderById(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var specList = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await unitOfWork.Repository<Order>().ListAsync(specList);
        }
    }
}