using EventManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.DAL.Repositories
{
    public  interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByEventIdAsync(int eventId);
        Task AddAsync(Comment comment);
        Task DeleteAsync(int id);
    }
}
