using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Infrastructure.Data;
using Core.Specifications;
using API.DTOs;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<ProductType> productTypeRepo; 
        private readonly IGenericRepository<ProductBrand> productBrandRepo;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> productRepository, 
        IGenericRepository<ProductType> productTypeRepository, 
        IGenericRepository<ProductBrand> productBrandRepository,
        IMapper imapper) 
        {  
            productRepo = productRepository;
            productTypeRepo = productTypeRepository;
            productBrandRepo = productBrandRepository;  
            mapper = imapper; 
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
        {   
            var spec = new ProductsWithTypesAndBrandsSpecification();

            //Este método já o novo que criámos no generic repository
            var products = await productRepo.ListAsync(spec); 
            
            if(products.Count == 0){
                return NoContent();
            }

            var mappedProducts = mapper.Map<IReadOnlyList<Product>, 
                IReadOnlyList<ProductToReturnDTO>>(products);

            return Ok(mappedProducts);

            // return Ok(products.Select(p=> new ProductToReturnDTO()
            // {
            //     Id = p.Id,
            //     Description = p.Description,
            //     Name = p.Name,
            //     Price = p.Price,
            //     ProductBrand = p.ProductBrand.Name,
            //     ProductType = p.ProductType.Name
            //     //PictureUrl = product.PictureUrl
            // }).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){

            var spec = new ProductsWithTypesAndBrandsSpecification(id){};

            var product = await productRepo.GetEntityWithSpec(spec);
            
            if(product == null)
            {
                return NoContent();
            }

            var mappedProduct = mapper.Map<Product, ProductToReturnDTO>(product);

            return Ok(mappedProduct);

            // return Ok(new ProductToReturnDTO()
            // {
            //     Id = product.Id,
            //     Description = product.Description,
            //     Name = product.Name,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            //     //PictureUrl = product.PictureUrl
            // });
        }

        //[HttpGet]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await productTypeRepo.ListAllAsync();
            
            if(productTypes.Count == 0){
                return NoContent();
            }

            return Ok(productTypes);
        }

        [HttpGet("types/{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id){

            var productType = await productTypeRepo.GetByIdAsync(id);
            
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
            var productBrands = await productBrandRepo.ListAllAsync();
            
            if(productBrands.Count == 0){
                return NoContent();
            }

            return Ok(productBrands);
        }

        [HttpGet("brands/{id}")]
        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id){

            var productBrand = await productBrandRepo.GetByIdAsync(id);
            
            if(productBrand == null)
            {
                return NoContent();
            }

            return Ok(productBrand);
        }
    }
}