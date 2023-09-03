using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext ctx;

        public ProductRepository(StoreContext context) 
        {  
            ctx = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await ctx.Products
                            .Include(p=>p.ProductType)
                            .Include(p=>p.ProductBrand)
                            .FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await ctx.Products
                                    .Include(p=> p.ProductType)
                                    .Include(p=>p.ProductBrand)
                                    .ToListAsync();
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await ctx.ProductTypes.FirstOrDefaultAsync(pt=>pt.Id==id);
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await ctx.ProductTypes.ToListAsync();
        }

        public async Task<ProductBrand> GetProductBrandByIdAsync(int id)
        {
            return await ctx.ProductBrands.FirstOrDefaultAsync(pb=>pb.Id==id);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await ctx.ProductBrands.ToListAsync();
        }
    }
}