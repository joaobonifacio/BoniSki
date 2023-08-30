
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext ctx;

        public ProductsController(StoreContext context) 
        {  
            ctx = context;
        }

        [HttpGet]
        //[HttpGet("getproducts")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await ctx.Products.ToListAsync();

            if(products.Count == 0){
                return NoContent();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        //[HttpGet("getproduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){

            var product = await ctx.Products.Where(p=>p.Id==id).FirstOrDefaultAsync();

            if(product == null)
            {
                return NoContent();
            }

            return Ok(product);
        }
    }
}