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
    public class UserRoleRepository:IUserRoleRepository
    {
        private readonly EventManagementDbContext _context;
        public UserRoleRepository(EventManagementDbContext context) => 
            _context = context;

        public async Task AssignRoleAsync(UserRole userRole) 
        { 
            await _context.UserRoles.AddAsync(userRole); 
            await _context.SaveChangesAsync(); 
        }
        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (userRole != null) 
            {
                _context.UserRoles.Remove(userRole); await _context.SaveChangesAsync(); 
            }
        }
    }
}
