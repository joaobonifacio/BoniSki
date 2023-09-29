using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderByPaymentIntentIdSpecification: BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
        : base(x=>x.PaymentIntentId==paymentIntentId)
        {

        }
    }
}