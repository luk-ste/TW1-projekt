using EventManagement.DAL.Database;
using EventManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.DAL.Repositories
{
    public  class RegistrationRepository : IRegistrationRepository
    {
        private readonly EventManagementDbContext _context;

        public RegistrationRepository(EventManagementDbContext context) => _context = context;

        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            return await _context.Registrations.Include(r => r.User).Where(r => r.EventId == eventId).ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetByUserIdAsync(int userId)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Registration?> GetByEventAndUserAsync(int eventId, int userId)
        {
            return await _context.Registrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);
        }

        public async Task<Registration?> GetByIdAsync(int id) => await _context.Registrations.FirstOrDefaultAsync(r => r.Id == id);
        public async Task AddAsync(Registration registration) { await _context.Registrations.AddAsync(registration); await _context.SaveChangesAsync(); }
        public async Task UpdateConfirmationAsync(int id, bool isConfirmed)
        {
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == id);
            if (registration != null) 
            {
                registration.IsConfirmed = isConfirmed; await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == id);
            if (registration != null) { 
                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync(); 
            }
        }
    }
}
