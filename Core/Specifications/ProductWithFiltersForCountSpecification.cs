using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams specParams)
        : base(x=>
        (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(
            specParams.Search.ToLower())) &&
        (!specParams.BrandId.HasValue || x.ProductBrand.Id==specParams.BrandId )
        && (!specParams.TypeId.HasValue || x.ProductType.Id==specParams.TypeId))
        { 
        }
    }
}