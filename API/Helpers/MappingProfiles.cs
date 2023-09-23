using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d=>d.ProductBrand, o => o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d=>d.ProductType, o => o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d=>d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            //Address de Identity
            CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
            //Address de OrderAggregated
            CreateMap<AddressDTO, Core.Entities.OrderAggregate.Address>();

            CreateMap<CustomerBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d=>d.DeliveryMethod, o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.ShippingPrice, o=>o.MapFrom(s=>s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d=>d.ProductId, o=>o.MapFrom(s=>s.ItemOrdered.ProductItemId))
                .ForMember(d=>d.ProductName, o=>o.MapFrom(s=>s.ItemOrdered.ProductName))
                .ForMember(d=>d.PictureUrl, o=>o.MapFrom(s=>s.ItemOrdered.PictureUrl))
                .ForMember(d=>d.PictureUrl, o=>o.MapFrom<OrderItemUrlResolver>());
        }
    }
}