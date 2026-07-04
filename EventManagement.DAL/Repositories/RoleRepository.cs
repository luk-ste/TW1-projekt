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
      public class RoleRepository:IRoleRepository
    {
        private readonly EventManagementDbContext _context;
        public RoleRepository(EventManagementDbContext context) => 
            _context = context;

        public async Task<IEnumerable<Role>> GetAllAsync() => await _context.Roles.ToListAsync();
        public async Task<Role?> GetByIdAsync(int id) => await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        public async Task<Role?> GetByNameAsync(string name) => await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

    }
}
