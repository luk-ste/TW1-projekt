using EventManagement.API.Dtos;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _service;

        public RegistrationController(IRegistrationService service)
        {
            _service = service;
        }

        private RegistrationResponseDto ToDto(Registration r)
        {
            return new RegistrationResponseDto
            {
                Id = r.Id,
                UserId = r.UserId,
                Username = r.User?.Username,
                EventId = r.EventId,
                RegistrationDate = r.RegistrationDate,
                IsConfirmed = r.IsConfirmed
            };
        }

        // GET: api/Registration/GetByEvent/{eventId} — Admin and User
        [HttpGet("[action]/{eventId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetByEventAsync([FromRoute] int eventId)
        {
            var registrations = await _service.GetByEventIdAsync(eventId);
            return Ok(registrations.Select(ToDto).ToList());
        }

        // GET: api/Registration/GetByUser/{userId} — Admin and User
        [HttpGet("[action]/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetByUserAsync([FromRoute] int userId)
        {
            var registrations = await _service.GetByUserIdAsync(userId);
            return Ok(registrations.Select(ToDto).ToList());
        }

        // GET: api/Registration/{id} — Admin and User
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<RegistrationResponseDto>> GetByIdAsync([FromRoute] int id)
        {
            var registration = await _service.GetByIdAsync(id);
            if (registration == null)
                return NotFound($"Registration with id {id} not found");
            return Ok(ToDto(registration));
        }

        // POST: api/Registration/Create — Admin and User
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<RegistrationResponseDto>> CreateAsync([FromBody] CreateRegistrationDto dto)
        {
            var registration = await _service.CreateAsync(dto.UserId, dto.EventId);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = registration.Id }, ToDto(registration));
        }

        // PUT: api/Registration/Confirm/{id} — Admin only
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ConfirmAsync([FromRoute] int id)
        {
            await _service.ConfirmAsync(id);
            return Ok($"Registration {id} confirmed");
        }

        // DELETE: api/Registration/Delete/{id} — Admin and User
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateRegistrationDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}