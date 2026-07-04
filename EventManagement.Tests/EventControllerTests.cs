using EventManagement.API.Controllers;
using EventManagement.API.Dtos;
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagement.Tests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _mockService;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockService = new Mock<IEventService>();
            _controller = new EventController(_mockService.Object);
        }

       
        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenEventsExist()
        {
            
            var events = new List<Event>
            {
                new Event { Id = 1, Title = "Event 1", Description = "Desc 1", StartDateTime = DateTime.UtcNow, MaxCapacity = 100 },
                new Event { Id = 2, Title = "Event 2", Description = "Desc 2", StartDateTime = DateTime.UtcNow, MaxCapacity = 50 }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(events);

            
            var result = await _controller.GetAllAsync();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvents = Assert.IsType<List<EventResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedEvents.Count);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoEventsExist()
        {
            
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Event>());

           
            var result = await _controller.GetAllAsync();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvents = Assert.IsType<List<EventResponseDto>>(okResult.Value);
            Assert.Empty(returnedEvents);
        }

        
        [Fact]
        public async Task GetByIdAsync_ReturnsOk_WhenEventExists()
        {
            
            var ev = new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = "Desc",
                StartDateTime = DateTime.UtcNow,
                MaxCapacity = 100,
                Registrations = new List<Registration>(),
                Comments = new List<Comment>()
            };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(ev);

            
            var result = await _controller.GetByIdAsync(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvent = Assert.IsType<EventDetailResponseDto>(okResult.Value);
            Assert.Equal(1, returnedEvent.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenEventDoesNotExist()
        {
            
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Event?)null);

            
            var result = await _controller.GetByIdAsync(99);

           
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        
        [Fact]
        public async Task CreateAsync_ReturnsCreated_WhenEventIsValid()
        {
            
            var dto = new CreateEventDto
            {
                Title = "New Event",
                Description = "Description",
                StartDateTime = DateTime.UtcNow,
                MaxCapacity = 100,
                OrganizerId = 1,
                LocationId = 1,
                CategoryId = 1
            };

            var createdEvent = new Event
            {
                Id = 1,
                Title = dto.Title,
                Description = dto.Description,
                StartDateTime = dto.StartDateTime,
                MaxCapacity = dto.MaxCapacity,
                OrganizerId = dto.OrganizerId,
                LocationId = dto.LocationId,
                CategoryId = dto.CategoryId,
                Registrations = new List<Registration>(),
                Comments = new List<Comment>()
            };

            _mockService.Setup(s => s.CreateAsync(It.IsAny<Event>())).ReturnsAsync(createdEvent);

            
            var result = await _controller.CreateAsync(dto);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedEvent = Assert.IsType<EventResponseDto>(createdResult.Value);
            Assert.Equal("New Event", returnedEvent.Title);
        }

        
        [Fact]
        public async Task UpdateAsync_ReturnsOk_WhenEventExists()
        {
            
            var dto = new UpdateEventDto
            {
                Title = "Updated Event",
                Description = "Updated Desc",
                StartDateTime = DateTime.UtcNow,
                MaxCapacity = 200,
                LocationId = 1,
                CategoryId = 1
            };

            var updatedEvent = new Event
            {
                Id = 1,
                Title = dto.Title,
                Description = dto.Description,
                StartDateTime = dto.StartDateTime,
                MaxCapacity = dto.MaxCapacity
            };

            _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<Event>())).ReturnsAsync(updatedEvent);

            
            var result = await _controller.UpdateAsync(1, dto);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvent = Assert.IsType<EventResponseDto>(okResult.Value);
            Assert.Equal("Updated Event", returnedEvent.Title);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenEventDoesNotExist()
        {
            
            var dto = new UpdateEventDto
            {
                Title = "Updated Event",
                Description = "Updated Desc",
                StartDateTime = DateTime.UtcNow,
                MaxCapacity = 200,
                LocationId = 1,
                CategoryId = 1
            };

            _mockService.Setup(s => s.UpdateAsync(99, It.IsAny<Event>())).ReturnsAsync((Event?)null);

            
            var result = await _controller.UpdateAsync(99, dto);

            
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        
        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenEventExists()
        {
            
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            
            var result = await _controller.DeleteAsync(1);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenEventDoesNotExist()
        {
            
            _mockService.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new NotFoundException("Event with id 99 not found"));

           
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteAsync(99));
        }
    }
}