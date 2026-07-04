
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;

namespace EventManagement.BL.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task<Location> CreateAsync(string name, string address)
        {
            var location = new Location { Name = name, Address = address };
            await _locationRepository.AddAsync(location);
            return location;
        }

        public async Task<Location?> UpdateAsync(int id, string name, string address)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
                return null;

            location.Name = name;
            location.Address = address;
            await _locationRepository.UpdateAsync(location);
            return location;
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
                throw new NotFoundException($"Location with id {id} not found");

            await _locationRepository.DeleteAsync(id);
        }
    }
}
