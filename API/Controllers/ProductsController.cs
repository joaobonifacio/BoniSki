using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repo;

        public ProductsController(IProductRepository repository) 
        {  
            repo = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await repo.GetProductsAsync(); 
            
            if(products.Count == 0){
                return NoContent();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        //[HttpGet("getproduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){

            var product = await repo.GetProductByIdAsync(id);
            
            //ctx.Products.Where(p=>p.Id==id).FirstOrDefaultAsync();

            if(product == null)
            {
                return NoContent();
            }

            return Ok(product);
        }

        //[HttpGet]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await repo.GetProductTypesAsync(); 
            
            if(productTypes.Count == 0){
                return NoContent();
            }

            return Ok(productTypes);
        }

        [HttpGet("types/{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id){

            var productType = await repo.GetProductTypeByIdAsync(id);
            
            if(productType == null)
            {
                return NoContent();
            }

            return Ok(productType);
        }

        //[HttpGet]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await repo.GetProductBrandsAsync(); 
            
            if(productBrands.Count == 0){
                return NoContent();
            }

            return Ok(productBrands);
        }

        [HttpGet("brands/{id}")]
        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id){

            var productBrand = await repo.GetProductBrandByIdAsync(id);
            
            if(productBrand == null)
            {
                return NoContent();
            }

            return Ok(productBrand);
        }
    }
}