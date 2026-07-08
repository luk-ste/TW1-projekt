
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;

namespace EventManagement.BL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventRepository.GetAllWithDetailsAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _eventRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Event> CreateAsync(Event ev)
        {
            await _eventRepository.AddAsync(ev);
            // Reload with details so navigation properties are populated for DTO mapping
            var created = await _eventRepository.GetByIdWithDetailsAsync(ev.Id);
            return created;
        }

        public async Task<Event?> UpdateAsync(int id, Event updated)
        {
            var existing = await _eventRepository.GetByIdWithDetailsAsync(id);
            if (existing == null)
                return null;

            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.StartDateTime = updated.StartDateTime;
            existing.MaxCapacity = updated.MaxCapacity;
            existing.LocationId = updated.LocationId;
            existing.CategoryId = updated.CategoryId;

            await _eventRepository.UpdateAsync(existing);
            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var ev = await _eventRepository.GetByIdWithDetailsAsync(id);
            if (ev == null)
                throw new NotFoundException($"Event with id {id} not found");

            await _eventRepository.DeleteAsync(id);
        }
    }
}