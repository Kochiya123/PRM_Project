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
            var product = await dbContext.Products
                .Include(p => p.Category)
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    BriefDescription = p.BriefDescription,
                    FullDescription = p.FullDescription,
                    TechnicalSpecifications = p.TechnicalSpecifications,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = p.Category == null ? null : new CategoryDTO
                    {
                        CategoryId = p.Category.CategoryId, 
                        CategoryName = p.Category.CategoryName
                    }
                })
                .ToListAsync();

            return Ok(product);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetProductbyID(int id)
        {
            var product = await dbContext.Products
                .FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("category/{categoryId:int}")]
        public async Task<ActionResult<List<Product>>> GetProductByCategory(int categoryId)
        {
            List<Product> product = await dbContext.Products
                    .Where(p => p.CategoryId == categoryId)
                    .ToListAsync();

            if (product == null) {
                return NotFound("No product was found!");
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("search")]
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

            try
            {
                dbContext.Products.Add(productObject);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }
            
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
