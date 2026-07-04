
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _service;

        public LocationController(ILocationService service)
        {
            _service = service;
        }

        // GET: api/Location/GetAll — Admin and User
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllAsync()
        {
            var locations = await _service.GetAllAsync();
            return Ok(locations);
        }

        // GET: api/Location/{id} — Admin and User
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Location>> GetByIdAsync([FromRoute] int id)
        {
            var location = await _service.GetByIdAsync(id);
            if (location == null)
                return NotFound($"Location with id {id} not found");
            return Ok(location);
        }

        // POST: api/Location/Create — Admin only
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Location>> CreateAsync([FromBody] CreateLocationDto dto)
        {
            var location = await _service.CreateAsync(dto.Name, dto.Address);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = location.Id }, location);
        }

        // PUT: api/Location/Update/{id} — Admin only
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Location>> UpdateAsync([FromRoute] int id, [FromBody] CreateLocationDto dto)
        {
            var location = await _service.UpdateAsync(id, dto.Name, dto.Address);
            if (location == null)
                return NotFound($"Location with id {id} not found");
            return Ok(location);
        }

        // DELETE: api/Location/Delete/{id} — Admin only
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateLocationDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}