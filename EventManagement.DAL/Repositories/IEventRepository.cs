using EventManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.DAL.Repositories
{
  
        public interface IEventRepository
        {
            Task<IEnumerable<Event>> GetAllWithDetailsAsync();
            Task<Event?> GetByIdWithDetailsAsync(int id);
            Task AddAsync(Event ev);
            Task UpdateAsync(Event ev);
            Task DeleteAsync(int id);
        }
    }
