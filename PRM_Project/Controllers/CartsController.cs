using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public CartsController(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cart>>> GetAllCarts()
        {
            var carts = await dbContext.Carts.ToListAsync();
            return Ok(carts);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Cart>> GetCartbyID(int id)
        {
            var cart = await dbContext.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<List<Cart>>> AddCart(AddCartDTO cart)
        {
            var cartObject = new Cart()
            {
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
                Status = cart.Status,
            };

            dbContext.Carts.Add(cartObject);
            await dbContext.SaveChangesAsync();

            return Ok(cartObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Cart>>> UpdateCart(int id,UpdateCartDTO updateCart) 
        {
            var cart = await dbContext.Carts.FindAsync(id);

            if(cart == null)
            {
                return NotFound(id);
            }

            cart.TotalPrice = updateCart.TotalPrice;
            cart.Status = updateCart.Status;

            await dbContext.SaveChangesAsync();

            return Ok(cart);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Cart>>> DeleteCart(int id) 
        {
            var cart = await dbContext.Carts.FindAsync(id);
            if( cart == null)
            {
                return NotFound();
            }

            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
