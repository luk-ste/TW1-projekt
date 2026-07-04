using EventManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.DAL.Repositories
{
    public  interface IUserRoleRepository
    {
        Task AssignRoleAsync(UserRole userRole);
        Task RemoveRoleAsync(int userId, int roleId);
    }
}
