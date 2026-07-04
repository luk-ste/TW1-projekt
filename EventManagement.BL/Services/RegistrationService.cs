using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;

namespace EventManagement.BL.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEventRepository _eventRepository;

        public RegistrationService(
            IRegistrationRepository registrationRepository,
            IEventRepository eventRepository)
        {
            _registrationRepository = registrationRepository;
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            return await _registrationRepository.GetByEventIdAsync(eventId);
        }

        public async Task<IEnumerable<Registration>> GetByUserIdAsync(int userId)
        {
            return await _registrationRepository.GetByUserIdAsync(userId);
        }

        public async Task<Registration?> GetByIdAsync(int id)
        {
            return await _registrationRepository.GetByIdAsync(id);
        }

        public async Task<Registration> CreateAsync(int userId, int eventId)
        {
            var ev = await _eventRepository.GetByIdWithDetailsAsync(eventId);
            if (ev == null)
                throw new NotFoundException($"Event with id {eventId} not found");

        
            var currentRegistrations = await _registrationRepository.GetByEventIdAsync(eventId);
            if (currentRegistrations.Count() >= ev.MaxCapacity)
                throw new BadRequestException("Event has reached maximum capacity");

            var existing = await _registrationRepository.GetByEventAndUserAsync(eventId, userId);
            if (existing != null)
                throw new BadRequestException("User is already registered for this event");

            var registration = new Registration
            {
                UserId = userId,
                EventId = eventId,
                RegistrationDate = DateTime.UtcNow,
                IsConfirmed = false
            };

            await _registrationRepository.AddAsync(registration);
            return registration;
        }

        public async Task ConfirmAsync(int id)
        {
            var registration = await _registrationRepository.GetByIdAsync(id);
            if (registration == null)
                throw new NotFoundException($"Registration with id {id} not found");

            await _registrationRepository.UpdateConfirmationAsync(id, true);
        }

        public async Task DeleteAsync(int id)
        {
            var registration = await _registrationRepository.GetByIdAsync(id);
            if (registration == null)
                throw new NotFoundException($"Registration with id {id} not found");

            await _registrationRepository.DeleteAsync(id);
        }
    }
}
