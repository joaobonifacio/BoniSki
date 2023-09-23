using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace API.DTOs
{
    public class OrderDTO
    {
        public int DeliveryMethodId { get; set;} 
        public string BasketId { get; set;} 
        public AddressDTO ShipToAddress { get; set;}

    }
}