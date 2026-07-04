
using EventManagement.DAL.Models;

namespace EventManagement.BL.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId);
        Task<IEnumerable<Registration>> GetByUserIdAsync(int userId);
        Task<Registration?> GetByIdAsync(int id);
        Task<Registration> CreateAsync(int userId, int eventId);
        Task ConfirmAsync(int id);
        Task DeleteAsync(int id);
    }

}
