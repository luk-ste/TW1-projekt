using EventManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.DAL.Repositories
{
    public  interface IRegistrationRepository
    {
        Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId);
        Task<IEnumerable<Registration>> GetByUserIdAsync(int userId);

        Task<Registration?> GetByEventAndUserAsync(int eventId, int userId);    
        Task<Registration?> GetByIdAsync(int id);
        Task AddAsync(Registration registration);
        Task UpdateConfirmationAsync(int id, bool isConfirmed);
        Task DeleteAsync(int id);
    }
}
