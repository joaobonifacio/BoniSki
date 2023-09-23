using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem(){}

        public OrderItem(int quantity, decimal price, ProductItemOrdered itemOrdered)
        {
            Quantity = quantity;
            Price = price;
            ItemOrdered = itemOrdered;
        }
        
        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}