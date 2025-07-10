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
    public class CartItems : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public CartItems(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartItem>>> GetAllCartItems()
        {
            var cartitems = await dbContext.CartItems.ToListAsync();
            return Ok(cartitems);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<CartItem>> GetCartItembyID(int id)
        {
            var cartItems = await dbContext.CartItems.FindAsync(id);

            if (cartItems == null)
            {
                return NotFound();
            }

            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<ActionResult<List<CartItem>>> AddCartItem(AddCartItemDTO cartItem)
        {
            var cartItemObject = new CartItem()
            {
                CartId  = cartItem.CartId,
                ProductId  = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
            };

            dbContext.CartItems.Add(cartItemObject);
            await dbContext.SaveChangesAsync();

            return Ok(cartItemObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<CartItem>>> UpdateCartItem(int id, UpdateCartItemDTO updateCartItem)
        {
            var cartItem = await dbContext.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound(id);
            }

            cartItem.CartId = updateCartItem.CartId;
            cartItem.ProductId = updateCartItem.ProductId;
            cartItem.Quantity = updateCartItem.Quantity;
            cartItem.Price = updateCartItem.Price;

            await dbContext.SaveChangesAsync();

            return Ok(cartItem);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<CartItem>>> DeleteCartItem(int id)
        {
            var cartItem = await dbContext.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            dbContext.CartItems.Remove(cartItem);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        

    }
}
