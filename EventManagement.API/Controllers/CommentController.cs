using EventManagement.API.Dtos;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }

        private CommentResponseDto ToDto(Comment c)
        {
            return new CommentResponseDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                Username = c.User?.Username,
                EventId = c.EventId
            };
        }

        // GET: api/Comment/GetByEvent/{eventId} — Admin and User
        [HttpGet("[action]/{eventId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetByEventAsync([FromRoute] int eventId)
        {
            var comments = await _service.GetByEventIdAsync(eventId);
            return Ok(comments.Select(ToDto).ToList());
        }

        // POST: api/Comment/Create — Admin and User
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<CommentResponseDto>> CreateAsync([FromBody] CreateCommentDto dto)
        {
            var comment = await _service.CreateAsync(dto.UserId, dto.EventId, dto.Content);
            return CreatedAtAction(nameof(GetByEventAsync), new { eventId = comment.EventId }, ToDto(comment));
        }

        // DELETE: api/Comment/Delete/{id} — Admin only
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateCommentDto
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}