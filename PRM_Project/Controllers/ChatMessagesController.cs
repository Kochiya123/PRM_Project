using FirebaseAdmin;
using Firebase.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;
using PRM_Project.Services;
using System.Data.Entity;
using System.Security.Claims;
using Firebase.Database.Query;
using AutoMapper;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatMessagesController : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;
        private readonly FirebaseClient firebaseClient;
        private readonly IMapper mapper;

        public ChatMessagesController(SalesAppDbContext dbContext, FirebaseClient firebaseClient, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.firebaseClient = firebaseClient;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatMessage>>> GetAllChatMessages()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var chatMessages = await dbContext.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            if(chatMessages ==  null || chatMessages.Count == 0) 
            {
                return NotFound("No messages has been sent yet!");
            }

            return Ok(chatMessages);
        }

        /*[HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ChatMessage>> GetchatMessagebyID(int id)
        {
            var chatMessage = await dbContext.ChatMessages.FindAsync(id);

            if (chatMessage == null)
            {
                return NotFound();
            }

            return Ok(chatMessage);
        }*/

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddChatMessage([FromBody] AddChatMessageDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) 
                return Unauthorized();

            dto.UserId = int.Parse(userId);
            var message = mapper.Map<ChatMessage>(dto);

            dbContext.ChatMessages.Add(message);
            await dbContext.SaveChangesAsync();

            // Push to Firebase
            await firebaseClient
                .Child("store-chats")
                .Child(userId)
                .Child("messages")
                .PostAsync(new
                {
                    ChatMessageId = message.ChatMessageId,
                    sender = userId,
                    userId = userId,
                    message = dto.Message,
                    SentAt = DateTime.UtcNow.ToString()
                });

            return Ok();
        }

        /*[HttpPut]
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
        }*/

    }
}
