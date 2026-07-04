
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/Category/GetAll — Admin and User
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllAsync()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/Category/{id} — Admin and User
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Category>> GetByIdAsync([FromRoute] int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound($"Category with id {id} not found");
            return Ok(category);
        }

        // POST: api/Category/Create — Admin only
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> CreateAsync([FromBody] CreateCategoryDto dto)
        {
            var category = await _service.CreateAsync(dto.Name);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = category.Id }, category);
        }

        // PUT: api/Category/Update/{id} — Admin only
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> UpdateAsync([FromRoute] int id, [FromBody] CreateCategoryDto dto)
        {
            var category = await _service.UpdateAsync(id, dto.Name);
            if (category == null)
                return NotFound($"Category with id {id} not found");
            return Ok(category);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; }
    }
}