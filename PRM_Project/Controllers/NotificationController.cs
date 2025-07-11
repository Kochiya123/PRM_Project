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
    public class NotificationsController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public NotificationsController(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notification>>> GetAllNotification()
        {
            var notify = await dbContext.Notifications.ToListAsync();
            return Ok(notify);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Notification>> GetNotificationbyID(int id)
        {
            var notify = await dbContext.Notifications.FindAsync(id);

            if (notify == null)
            {
                return NotFound();
            }

            return Ok(notify);
        }

        [HttpPost]
        public async Task<ActionResult<List<Notification>>> AddNotification(AddNotificationDTO notify)
        {
            var notifyObject = new Notification()
            {
                UserId = notify.UserId,
                Message = notify.Message,
                IsRead = false,
                CreatedAt = DateTime.Now,
            };
            
            dbContext.Notifications.Add(notifyObject);
            await dbContext.SaveChangesAsync();

            return Ok(notifyObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Notification>>> Updatenotification(int id, UpdateNotificationDTO updateNotification)
        {
            var notify = await dbContext.Notifications.FindAsync(id);

            if (notify == null)
            {
                return NotFound(id);
            }

            notify.Message = updateNotification.Message;
            notify.IsRead = updateNotification.IsRead;

            await dbContext.SaveChangesAsync();

            return Ok(notify);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Notification>>> DeleteNotification(int id)
        {
            var notify = await dbContext.Notifications.FindAsync(id);
            if (notify == null)
            {
                return NotFound();
            }

            dbContext.Notifications.Remove(notify);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
