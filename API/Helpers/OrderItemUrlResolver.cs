using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        public IConfiguration config { get; }
        public OrderItemUrlResolver(IConfiguration configuration)
        {
            this.config = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, 
            ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return config["ApiUrl"] + source.ItemOrdered.PictureUrl;
            }

            return null;
        }
    }
}