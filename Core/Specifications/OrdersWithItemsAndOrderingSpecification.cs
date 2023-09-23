using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification :BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string email)
        : base(x=>x.BuyerEmail==email)
        {
            //Este lista todos
            AddInclude(x=>x.OrderItems);
            AddInclude(x=>x.DeliveryMethod);
            AddOrderByDescending(o=>o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(int id, string email)
        : base(o=>o.BuyerEmail==email && o.Id==id)
        {
            //Este devolve só 
            //porque incluímos o id no criteria do constructor
            AddInclude(o=>o.OrderItems);
            AddInclude(o=>o.DeliveryMethod);
        }
    }
}