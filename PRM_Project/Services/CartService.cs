using System.Data.Entity;
using System.Security.Claims;
using PRM_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace PRM_Project.Services
{
    public class CartService
    {
        private readonly SalesAppDbContext dbContext;

        public CartService(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Cart FindCartByUserToken(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return null;

            var cart = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
            .Include(dbContext.Carts, c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.UserId == int.Parse(userId));

            return cart;
        }
    }
}
