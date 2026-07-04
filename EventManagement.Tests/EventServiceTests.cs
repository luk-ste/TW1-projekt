using EventManagement.BL.Exceptions;
using EventManagement.BL.Services;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Moq;

namespace EventManagement.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockRepo;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _mockRepo = new Mock<IEventRepository>();
            _service = new EventService(_mockRepo.Object);
        }

        
        [Fact]
        public async Task GetAllAsync_ReturnsAllEvents()
        {
            
            var events = new List<Event>
            {
                new Event { Id = 1, Title = "Event 1" },
                new Event { Id = 2, Title = "Event 2" }
            };
            _mockRepo.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(events);

            
            var result = await _service.GetAllAsync();

            
            Assert.Equal(2, result.Count());
        }

        
        [Fact]
        public async Task GetByIdAsync_ReturnsEvent_WhenExists()
        {
            
            var ev = new Event { Id = 1, Title = "Event 1" };
            _mockRepo.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ev);

            
            var result = await _service.GetByIdAsync(1);

            
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdWithDetailsAsync(99)).ReturnsAsync((Event?)null);

            
            var result = await _service.GetByIdAsync(99);

            
            Assert.Null(result);
        }

        
        [Fact]
        public async Task CreateAsync_AddsEvent_AndReturnsIt()
        {
            
            var ev = new Event { Id = 1, Title = "New Event" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Event>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.GetByIdWithDetailsAsync(ev.Id)).ReturnsAsync(ev);

            
            var result = await _service.CreateAsync(ev);

            
            Assert.NotNull(result);
            Assert.Equal("New Event", result.Title);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
        }

        
        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedEvent_WhenExists()
        {
            
            var existing = new Event { Id = 1, Title = "Old Title", Description = "Old", MaxCapacity = 100, StartDateTime = DateTime.UtcNow };
            var updated = new Event { Title = "New Title", Description = "New", MaxCapacity = 200, StartDateTime = DateTime.UtcNow, LocationId = 1, CategoryId = 1 };

            _mockRepo.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Event>())).Returns(Task.CompletedTask);

            
            var result = await _service.UpdateAsync(1, updated);

            
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal(200, result.MaxCapacity);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenEventNotFound()
        {
            
            _mockRepo.Setup(r => r.GetByIdWithDetailsAsync(99)).ReturnsAsync((Event?)null);

            
            var result = await _service.UpdateAsync(99, new Event());

            
            Assert.Null(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }

       
        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

           
            await _service.DeleteAsync(1);

            
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}