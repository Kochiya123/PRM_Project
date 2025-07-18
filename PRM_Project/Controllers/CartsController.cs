using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;
using System.Security.Claims;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;
        private CartItemsController cartItemsController;
        ILogger<CartsController> logger;

        public CartsController(SalesAppDbContext dbContext, CartItemsController cartItemsController, ILogger<CartsController> logger)
        {
            this.dbContext = dbContext;
            this.cartItemsController = cartItemsController;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetUserCarts()
        {
            var carts = FindCartByUserToken();

            if (carts == null)
            {
                return NotFound();
            }
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

        /*[HttpPost]
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
        }*/

        [HttpGet]
        [Route("/items")]
        public async Task<ActionResult<CartItem>> AddItemToCart(CartItem addItem)
        {
            var carts = FindCartByUserToken();
            if(carts == null)
            {
                return NotFound();
            }

            try
            {
                carts.CartItems.Add(addItem);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }

            return Ok(carts);
        }


        [HttpPost]
        [Route("/items")]
        public async Task<ActionResult<List<CartItem>>> UpdateCartItemQuantity(UpdateCartItemDTO updateCartItem)
        {
            var carts = FindCartByUserToken();
            if(carts == null)
            {
                return NotFound();
            }

            List<CartItem> items = carts.CartItems.ToList();

            CartItem item = items.Single(c => c.ProductId == updateCartItem.ProductId);
            if(item == null)
            {
                return NotFound("Item not found");
            }
            try
            {
                item.Quantity = updateCartItem.Quantity;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }
            

            return Ok(carts);
        }

        /*[HttpPut]
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
        }*/

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






        public Cart FindCartByUserToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var carts = new Cart();

            carts = dbContext.Carts.
                Find(int.Parse(userId));

            if (carts == null)
            {
                return null;
            }
            return carts;
        }
    }
}
