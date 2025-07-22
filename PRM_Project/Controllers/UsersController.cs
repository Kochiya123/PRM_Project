using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRM_Project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _config;
        private readonly TokenHelper _jwtHelper;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration config, TokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _jwtHelper = jwtTokenHelper;
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

        [HttpPost("signup")]
        [AllowAnonymous]
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

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, string? phoneNumber)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
            {
                return NotFound("username not found!");
            }else if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized("password isn't correct!");
            }

            /* var authClaims = new List<Claim>
 {
     new Claim(ClaimTypes.Name, user.UserName),
     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
 };

         var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

         var token = new JwtSecurityToken(
             issuer: _config["Jwt:Issuer"],
             audience: _config["Jwt:Audience"],
             expires: DateTime.UtcNow.AddHours(3),
             claims: authClaims,
             signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
         );

         return Ok(new
         {
             token = new JwtSecurityTokenHandler().WriteToken(token),
             expiration = token.ValidTo
         });*/
            var token = _jwtHelper.GenerateToken(user);
            return Ok(new {
                user.Id,
                user.UserName,
                user.Email,
                user.Address,
                user.PhoneNumber,
                user.Role,
                Token = token
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
