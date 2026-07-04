
using EventManagement.DAL.Models;

namespace EventManagement.BL.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetByEventIdAsync(int eventId);
        Task<Comment> CreateAsync(int userId, int eventId, string content);
        Task DeleteAsync(int id);
    }

}
