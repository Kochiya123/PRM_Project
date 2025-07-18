using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;
        private ILogger<ProductsController> logger;

        public ProductsController(SalesAppDbContext dbContext, ILogger<ProductsController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            var product = await dbContext.Products.ToListAsync();
            return Ok(product);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetProductbyID(int id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("/category/{categoryId:int}")]
        public async Task<ActionResult<List<Product>>> GetProductByCategory(int categoryId)
        {
            var product = new List<Product>();
            try
            {
                product = await dbContext.Products
                    .Where(product => product.CategoryId == categoryId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("/search")]
        public async Task<ActionResult<List<Product>>> SearchProducts(String name)
        {
            var product = await dbContext.Products.FindAsync(name);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(AddProductDTO product)
        {
            var productObject = new Product()
            {
                ProductName = product.ProductName,
                BriefDescription = product.BriefDescription,
                FullDescription = product.FullDescription,
                TechnicalSpecifications = product.TechnicalSpecifications,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
            };

            dbContext.Products.Add(productObject);
            await dbContext.SaveChangesAsync();

            return Ok(productObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Product>>> UpdateProduct(int id, UpdateProductDTO updateProduct)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(id);
            }

            product.ProductName = updateProduct.ProductName;
            product.BriefDescription = updateProduct.BriefDescription;
            product.FullDescription = updateProduct.FullDescription;
            product.TechnicalSpecifications = updateProduct.TechnicalSpecifications;
            product.Price = updateProduct.Price;
            product.ImageUrl = updateProduct.ImageUrl;
            product.CategoryId = updateProduct.CategoryId;

            await dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
