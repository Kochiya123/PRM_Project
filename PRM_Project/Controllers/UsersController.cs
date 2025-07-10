using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_Project.Models;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public Users(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var user = await _userManager.Users.ToListAsync();
            return Ok(user);
        }

        /*[HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> GetUserbyID(int id)
        {
            var user = await _userManager.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }*/

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDTO dto)
        {
            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = dto.Role
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            // Create role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(dto.Role));
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.Address,
                user.Role
            });
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(id);

            user.UserName = dto.Username;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.Address = dto.Address;
            user.Role = dto.Role;

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            return Ok(user);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            return Ok();

        }
    }
}
