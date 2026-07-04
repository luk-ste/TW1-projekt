using EventManagement.DAL.Models;

namespace EventManagement.BL.Interfaces   
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> CreateAsync(Event ev);
        Task<Event?> UpdateAsync(int id, Event ev);
        Task DeleteAsync(int id);
    }
}
