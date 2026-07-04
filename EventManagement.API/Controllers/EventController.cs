using EventManagement.API.Dtos;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;

        public EventController(IEventService service)
        {
            _service = service;
        }

        private EventResponseDto ToDto(Event ev)
        {
            return new EventResponseDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDateTime = ev.StartDateTime,
                MaxCapacity = ev.MaxCapacity,
                OrganizerUsername = ev.Organizer?.Username,
                LocationName = ev.Location?.Name,
                CategoryName = ev.Category?.Name
            };
        }

        private EventDetailResponseDto ToDetailDto(Event ev)
        {
            return new EventDetailResponseDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDateTime = ev.StartDateTime,
                MaxCapacity = ev.MaxCapacity,
                OrganizerUsername = ev.Organizer?.Username,
                LocationName = ev.Location?.Name,
                CategoryName = ev.Category?.Name,
                Registrations = ev.Registrations.Select(r => new RegistrationResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    Username = r.User?.Username,
                    EventId = r.EventId,
                    RegistrationDate = r.RegistrationDate,
                    IsConfirmed = r.IsConfirmed
                }).ToList(),
                Comments = ev.Comments.Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                    Username = c.User?.Username,
                    EventId = c.EventId
                }).ToList()
            };
        }

        // GET: api/Event/GetAll — Admin and User
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAllAsync()
        {
            var events = await _service.GetAllAsync();
            return Ok(events.Select(ToDto).ToList());
        }

        // GET: api/Event/{id} — Admin and User
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<EventDetailResponseDto>> GetByIdAsync([FromRoute] int id)
        {
            var ev = await _service.GetByIdAsync(id);
            if (ev == null)
                return NotFound($"Event with id {id} not found");
            return Ok(ToDetailDto(ev));
        }

        // POST: api/Event/Create — Admin and User
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<EventResponseDto>> CreateAsync([FromBody] CreateEventDto dto)
        {
            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDateTime = dto.StartDateTime,
                MaxCapacity = dto.MaxCapacity,
                OrganizerId = dto.OrganizerId,
                LocationId = dto.LocationId,
                CategoryId = dto.CategoryId
            };

            var created = await _service.CreateAsync(ev);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, ToDto(created));
        }

        // PUT: api/Event/Update/{id} — Admin and User
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<EventResponseDto>> UpdateAsync([FromRoute] int id, [FromBody] UpdateEventDto dto)
        {
            var updated = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDateTime = dto.StartDateTime,
                MaxCapacity = dto.MaxCapacity,
                LocationId = dto.LocationId,
                CategoryId = dto.CategoryId
            };

            var result = await _service.UpdateAsync(id, updated);
            if (result == null)
                return NotFound($"Event with id {id} not found");
            return Ok(ToDto(result));
        }
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public int MaxCapacity { get; set; }
        public int OrganizerId { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public int MaxCapacity { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
    }
}