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
    public  class LocationRepository:ILocationRepository
    {
        private readonly EventManagementDbContext _context;
        public LocationRepository(EventManagementDbContext context) => _context = context;

        public async Task<IEnumerable<Location>> GetAllAsync() => await _context.Locations.ToListAsync();
        public async Task<Location?> GetByIdAsync(int id) => await _context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        public async Task AddAsync(Location location) { await _context.Locations.AddAsync(location); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Location location) { _context.Locations.Update(location); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == id);
            if (location != null) 
            { 
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync(); 
            
            }
        }
    }
}
