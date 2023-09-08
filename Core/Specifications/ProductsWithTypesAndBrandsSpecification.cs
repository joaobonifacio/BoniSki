using Core.Entities;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams)
        : base(x=>
        ( string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(
            productSpecParams.Search.ToLower())) &&
        (!productSpecParams.BrandId.HasValue || x.ProductBrand.Id==productSpecParams.BrandId )
        && (!productSpecParams.TypeId.HasValue || x.ProductType.Id==productSpecParams.TypeId))
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrderBy(x=>x.Name);
            ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex -1), 
                productSpecParams.PageSize);


            if(!String.IsNullOrEmpty(productSpecParams.Sort))  
            {
                switch(productSpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x=>x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x=>x.Price);
                        break;
                    default:
                        AddOrderBy(x=>x.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) 
        : base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}