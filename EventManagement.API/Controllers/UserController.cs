using EventManagement.API.Dtos;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        private UserResponseDto ToDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Roles = user.UserRoles?
                    .Where(ur => ur.Role != null)
                    .Select(ur => ur.Role.Name)
                    .ToList() ?? new List<string>()
            };
        }

        // POST: api/User/Register — public
        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var username = await _service.RegisterAsync(
                dto.Username, dto.Password,
                dto.FirstName, dto.LastName,
                dto.Email, dto.Phone);
            return Ok(new { Username = username });
        }

        // POST: api/User/Login — public
        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _service.LoginAsync(dto.Username, dto.Password);
            return Ok(token);
        }

        // PUT: api/User/ChangePassword — Admin,User
        [HttpPut("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
        {
            await _service.ChangePasswordAsync(dto.Username, dto.OldPassword, dto.NewPassword);
            return Ok("Password changed successfully");
        }

        // GET: api/User/GetAll — Admin only
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllAsync()
        {
            var users = await _service.GetAllAsync();
            return Ok(users.Select(ToDto).ToList());
        }

        // GET: api/User/{id} — Admin only
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseDto>> GetByIdAsync([FromRoute] int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with id {id} not found");
            return Ok(ToDto(user));
        }

        // DELETE: api/User/Delete/{id} — Admin only
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/User/PromoteUser/{id} — Admin only
        [HttpPost("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PromoteUserAsync([FromRoute] int id)
        {
            await _service.PromoteToAdminAsync(id);
            return Ok($"User {id} promoted to Admin");
        }
    }

    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordDto
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}