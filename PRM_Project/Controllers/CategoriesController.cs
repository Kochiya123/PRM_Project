using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public CategoriesController(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await dbContext.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetCategorybyID(int id)
        {
            var category = await dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<List<Category>>> Addcategoryegory(CategoryDTO category)
        {
            var categoryObject = new Category()
            {
                CategoryName = category.CategoryName,
            };

            dbContext.Categories.Add(categoryObject);
            await dbContext.SaveChangesAsync();

            return Ok(categoryObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Updatecategoryegory(int id, CategoryDTO updatecategory)
        {
            var category = await dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(id);
            }

            category.CategoryName = updatecategory.CategoryName;

            await dbContext.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Deletecategoryegory(int id)
        {
            var category = await dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
