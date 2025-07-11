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
    public class OrdersController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public OrdersController(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrder()
        {
            var order = await dbContext.Orders.ToListAsync();
            return Ok(order);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Order>> GetOrderbyID(int id)
        {
            var order = await dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<List<Order>>> AddOrder(AddOrderDTO order)
        {
            var orderObject = new Order()
            {
                CartId = order.CartId,
                UserId = order.UserId,
                PaymentMethod = order.PaymentMethod,
                BillingAddress = order.BillingAddress,
                OrderStatus = order.OrderStatus,
                OrderDate = DateTime.Now,
            };

            dbContext.Orders.Add(orderObject);
            await dbContext.SaveChangesAsync();

            return Ok(orderObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Order>>> UpdateOrder(int id, UpdateOrderDTO updateOrder)
        {
            var order = await dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound(id);
            }

            order.CartId = updateOrder.CartId;
            order.UserId = updateOrder.UserId;
            order.PaymentMethod = updateOrder.PaymentMethod;
            order.BillingAddress = updateOrder.BillingAddress;
            order.OrderStatus = updateOrder.OrderStatus;

            await dbContext.SaveChangesAsync();

            return Ok(order);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Order>>> DeleteOrder(int id)
        {
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
