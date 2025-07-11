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
    public class PaymentsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public PaymentsController(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetAllPayment()
        {
            var payment = await dbContext.Payments.ToListAsync();
            return Ok(payment);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Payment>> GetPaymentbyID(int id)
        {
            var payment = await dbContext.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<List<Payment>>> AddPayment(AddPaymentDTO payment)
        {
            var paymentObject = new Payment()
            {
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentDate = DateTime.Now,
            };

            dbContext.Payments.Add(paymentObject);
            await dbContext.SaveChangesAsync();

            return Ok(paymentObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Payment>>> UpdatePayment(int id, UpdatePaymentDTO updatePayment)
        {
            var payment = await dbContext.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound(id);
            }

            payment.OrderId = updatePayment.OrderId;
            payment.Amount = updatePayment.Amount;
            payment.PaymentDate = DateTime.Now;

            await dbContext.SaveChangesAsync();

            return Ok(payment);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Payment>>> DeletePayment(int id)
        {
            var payment = await dbContext.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            dbContext.Payments.Remove(payment);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
