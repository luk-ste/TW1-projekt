using EventManagement.DAL.Models;

namespace EventManagement.BL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<string> RegisterAsync(string username, string password, string firstName, string lastName, string email, string? phone);
        Task<string> LoginAsync(string username, string password);
        Task ChangePasswordAsync(string username, string oldPassword, string newPassword);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task AssignDefaultRoleAsync(int userId);
        Task PromoteToAdminAsync(int id);
    }
}