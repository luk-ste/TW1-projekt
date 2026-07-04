using EventManagement.DAL.Database;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.DAL.Repositories
{
    public  class EventRepository:IEventRepository
    {
        private readonly EventManagementDbContext _context;
        public EventRepository(EventManagementDbContext context) => _context = context;

        public async Task<IEnumerable<Event>> GetAllWithDetailsAsync()
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .Include(e => e.Registrations).ThenInclude(r => r.User)
                .Include(e => e.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Event ev) { await _context.Events.AddAsync(ev); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Event ev) { _context.Events.Update(ev); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (ev != null) { _context.Events.Remove(ev);await _context.SaveChangesAsync(); }
        }
    }

}

