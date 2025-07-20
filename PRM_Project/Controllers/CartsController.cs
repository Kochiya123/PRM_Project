using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;
using System.Security.Claims;
using PRM_Project.Services;
using PRM_Project.Controllers;
using Microsoft.AspNet.Identity;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;
        ILogger<CartController> logger;
        private readonly CartService cartService;

        public CartController(SalesAppDbContext dbContext, ILogger<CartController> logger, CartService cartService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetUserCarts()
        {
            var carts = cartService.FindCartByUserToken(User);

            if (carts == null)
            {
                return NotFound("Cart not found.");
            }

            // Map to response
            var response = new CartResponse
            {
                CartId = carts.CartId,
                UserId = carts.UserId,
                Status = carts.Status,
                Items = carts.CartItems?.Select(ci => new CartItemResponse
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    ProductImage = ci.Product?.ImageUrl ?? string.Empty,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Price
                }).ToList() ?? new List<CartItemResponse>(),
                ItemCount = carts.CartItems?.Sum(ci => ci.Quantity) ?? 0,
                TotalPrice = carts.CartItems?.Sum(ci => ci.Price) ?? 0
            };

            return Ok(response);
        }

        /*[HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Cart>> GetCartbyID(int id)
        {
            var cart = await dbContext.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }*/

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

        [HttpPost("items")]
        public async Task<ActionResult<Cart>> AddItemToCart([FromBody] AddCartItemDTO addItem)
        {
            var cart = cartService.FindCartByUserToken(User);
            var product = await dbContext.Products.FindAsync(addItem.ProductId);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (cart == null)
            {
                        cart = new Cart()
                        {
                    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                    Status = "Active",
                    CartItems = new List<CartItem>()
                };
                dbContext.Carts.Add(cart);
                await dbContext.SaveChangesAsync(); // Generate CartId
            }

            // Check if the item already exists in the cart
            var existingItem = dbContext.CartItems
                .FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == addItem.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += addItem.Quantity;
                existingItem.Price = existingItem.Quantity * product.Price;
        
            }
            else
            {
                CartItem newItem = new CartItem()
                {
                    CartId = cart.CartId,
                    ProductId = addItem.ProductId,
                    Quantity = addItem.Quantity,
                    Price = addItem.Quantity * product.Price
                };
                dbContext.CartItems.Add(newItem);
            }
            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Price);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }

            // Reload the cart with items (optional but recommended for latest state)
            var updatedCart = dbContext.Carts
            .Include(c => c.CartItems)
            .First(c => c.CartId == cart.CartId);

            var response = new CartResponse
            {
                CartId = updatedCart.CartId,
                UserId = updatedCart.UserId,
                Status = updatedCart.Status,
                Items = updatedCart.CartItems.Select(ci => new CartItemResponse
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList(),
                ItemCount = updatedCart.CartItems.Sum(ci => ci.Quantity),
                TotalPrice = updatedCart.CartItems.Sum(ci => ci.Price) // already includes quantity
            };

            return Ok(response);
        }


        [HttpPut]
        [Route("items")]
        public async Task<ActionResult<List<CartItem>>> UpdateCartItemQuantity([FromBody] UpdateCartItemDTO updateCartItem)
        {
            var carts = cartService.FindCartByUserToken(User);
            if (carts == null)
            {
                return NotFound();
            }

            var item = carts.CartItems.First(ci => ci.CartItemId == (int) updateCartItem.cartItemId);
            item.Quantity = updateCartItem.Quantity;
            item.Price = item.Quantity * item.Product.Price;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }
            // Reload the cart with items (optional but recommended for latest state)
            var updatedCart = cartService.FindCartByUserToken(User);


            var response = new CartResponse
            {
                CartId = updatedCart.CartId,
                UserId = updatedCart.UserId,
                Status = updatedCart.Status,
                Items = updatedCart.CartItems?.Select(ci => new CartItemResponse
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    ProductImage = ci.Product?.ImageUrl ?? string.Empty,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Price
                }).ToList() ?? new List<CartItemResponse>(),
                ItemCount = updatedCart.CartItems?.Sum(ci => ci.Quantity) ?? 0,
                TotalPrice = updatedCart.CartItems?.Sum(ci => ci.Price) ?? 0
            };

            return Ok(response);
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
        [Route("items/{cartItemId}")]
        public async Task<ActionResult<CartItem>> DeleteCartItem(int cartItemId)
        {
            var cart = cartService.FindCartByUserToken(User);
            if (cart == null)
            {
                return NotFound();
            }

            CartItem cartItem = cart.CartItems.First(c => c.CartItemId == cartItemId);
            if(cartItem == null)
            {
                return NotFound("Item already deleted!");
            }

            try
            {
                cart.CartItems.Remove(cartItem);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }

            var updatedCart = cartService.FindCartByUserToken(User);


            var response = new CartResponse
            {
                CartId = updatedCart.CartId,
                UserId = updatedCart.UserId,
                Status = updatedCart.Status,
                Items = updatedCart.CartItems?.Select(ci => new CartItemResponse
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    ProductImage = ci.Product?.ImageUrl ?? string.Empty,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Price
                }).ToList() ?? new List<CartItemResponse>(),
                ItemCount = updatedCart.CartItems?.Sum(ci => ci.Quantity) ?? 0,
                TotalPrice = updatedCart.CartItems?.Sum(ci => ci.Price) ?? 0
            };

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Cart>> clearCart()
        {
            var cart = cartService.FindCartByUserToken(User);
            if (cart == null)
            {
                return NotFound();
            }

            try
            {
                cart.CartItems.Clear();
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }

            return Ok();
        }

        /*[HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Cart>>> DeleteCart(int id) 
        {
            var cart = await dbContext.Carts.FindAsync(id);
            if( cart == null)
            {
                return NotFound();
            }
            try
            {
                dbContext.Carts.Remove(cart);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FatalError!!");
                return StatusCode(500, "Please try again later");
            }

            return Ok();
        }*/

    }
}
