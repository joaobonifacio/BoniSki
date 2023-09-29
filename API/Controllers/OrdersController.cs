using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   [Authorize]
    public class OrdersController :BaseAPIController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService _orderService, IMapper imapper)
        {
            this.orderService = _orderService;
            this.mapper = imapper;
        }

        [HttpPost("createorder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            
            var address = mapper.Map<AddressDTO, Address>(orderDTO.ShipToAddress);

            var order =  await orderService.CreateOrderAsync(email, orderDTO.DeliveryMethodId, 
            orderDTO.BasketId, address);

            if(order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            }

            return Ok(order);
        }

        [HttpGet("getorders")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await orderService.GetOrdersForUserAsync(email);

            if(orders == null)
            {
                return BadRequest(new ApiResponse(400, "Problem gettint orders"));
            }

            return Ok(mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order =  await orderService.GetOrderById(id, email);

            if(order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem getting order"));
            }

            return Ok(mapper.Map<OrderToReturnDTO>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await orderService.GetDeliveryMethodsAsync());
        }
    }
}