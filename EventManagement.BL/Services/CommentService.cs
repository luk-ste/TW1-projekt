
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;

namespace EventManagement.BL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IEventRepository _eventRepository;

        public CommentService(
            ICommentRepository commentRepository,
            IEventRepository eventRepository)
        {
            _commentRepository = commentRepository;
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Comment>> GetByEventIdAsync(int eventId)
        {
            return await _commentRepository.GetByEventIdAsync(eventId);
        }

        public async Task<Comment> CreateAsync(int userId, int eventId, string content)
        {
            // Check event exists
            var ev = await _eventRepository.GetByIdWithDetailsAsync(eventId);
            if (ev == null)
                throw new NotFoundException($"Event with id {eventId} not found");

            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                EventId = eventId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);
            return comment;
        }

        public async Task DeleteAsync(int id)
        {
            await _commentRepository.DeleteAsync(id);
        }
    }
}
