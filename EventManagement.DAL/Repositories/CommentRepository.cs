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
    public class CommentRepository: ICommentRepository
    {
        private readonly EventManagementDbContext _context;
        public CommentRepository(EventManagementDbContext context) => _context = context;

        public async Task<IEnumerable<Comment>> GetByEventIdAsync(int eventId)
        {
            return await _context.Comments.Include(c => c.User).Where(c => c.EventId == eventId).OrderByDescending(c => c.CreatedAt).ToListAsync();
        }
        public async Task AddAsync(Comment comment) 
        { 
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(); 
        }
        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null) { 
                _context.Comments.Remove(comment); await _context.SaveChangesAsync(); 
            }
        }
    }
}
