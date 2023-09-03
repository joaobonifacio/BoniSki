using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration config;

        public ProductUrlResolver(IConfiguration iconfig)
        {
            config = iconfig;   
        }

        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, 
            ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}