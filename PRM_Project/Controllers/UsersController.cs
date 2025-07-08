using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly SalesAppDbContext dbContext;

        public Users(SalesAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var user = await dbContext.Users.ToListAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> GetUserbyID(int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(AddUserDTO user)
        {
            var userObject = new User()
            {
                Username = user.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role,
            };

            dbContext.Users.Add(userObject);
            await dbContext.SaveChangesAsync();

            return Ok(userObject);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<User>>> UpdateUser(int id, UpdateUserDTO updateUser)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(id);
            }

            user.Username = updateUser.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.PasswordHash);
            user.Email = updateUser.Email;
            user.PhoneNumber = updateUser.PhoneNumber;
            user.Address = updateUser.Address;
            user.Role = updateUser.Role;

            await dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
