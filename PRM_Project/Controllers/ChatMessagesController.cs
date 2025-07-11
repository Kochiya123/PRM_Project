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
    public class ChatMessagesController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public ChatMessagesController(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatMessage>>> GetAllChatMessages()
        {
            var chatMessages = await dbContext.ChatMessages.ToListAsync();
            return Ok(chatMessages);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ChatMessage>> GetchatMessagebyID(int id)
        {
            var chatMessage = await dbContext.ChatMessages.FindAsync(id);

            if (chatMessage == null)
            {
                return NotFound();
            }

            return Ok(chatMessage);
        }

        [HttpPost]
        public async Task<ActionResult<List<ChatMessage>>> AddchatMessage(AddChatMessageDTO chatMessage)
        {
            var chatMessageObject = new ChatMessage()
            {
                UserId = chatMessage.UserId,
                Message = chatMessage.Message,
                SentAt = DateTime.Now,
            };

            dbContext.ChatMessages.Add(chatMessageObject);
            await dbContext.SaveChangesAsync();

            return Ok(chatMessageObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<ChatMessage>>> UpdatechatMessage(int id, UpdateChatMessageDTO updateMessage)
        {
            var chatMessage = await dbContext.ChatMessages.FindAsync(id);

            if (chatMessage == null)
            {
                return NotFound(id);
            }

            chatMessage.Message = updateMessage.Message;

            await dbContext.SaveChangesAsync();

            return Ok(chatMessage);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<ChatMessage>>> DeletechatMessage(int id)
        {
            var chatMessage = await dbContext.ChatMessages.FindAsync(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            dbContext.ChatMessages.Remove(chatMessage);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
