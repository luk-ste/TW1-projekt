using EventManagement.DAL.Models;

namespace EventManagement.BL.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task<Location> CreateAsync(string name, string address);
        Task<Location?> UpdateAsync(int id, string name, string address);
        Task DeleteAsync(int id);
    }
}
