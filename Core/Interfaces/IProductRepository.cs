using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetProductByIdAsync(int id);

        //Esta lista só será lida
        public Task <IReadOnlyList<Product>> GetProductsAsync();

        public Task<ProductBrand> GetProductBrandByIdAsync(int id);

        public Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();

        public Task<ProductType> GetProductTypeByIdAsync(int id);

        public Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }
}