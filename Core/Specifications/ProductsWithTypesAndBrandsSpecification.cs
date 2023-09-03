using Core.Entities;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(int id) 
        : base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }

        public ProductsWithTypesAndBrandsSpecification()
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}