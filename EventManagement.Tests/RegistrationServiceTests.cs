using EventManagement.BL.Exceptions;
using EventManagement.BL.Services;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Moq;

namespace EventManagement.Tests
{
    public class RegistrationServiceTests
    {
        private readonly Mock<IRegistrationRepository> _mockRegistrationRepo;
        private readonly Mock<IEventRepository> _mockEventRepo;
        private readonly RegistrationService _service;

        public RegistrationServiceTests()
        {
            _mockRegistrationRepo = new Mock<IRegistrationRepository>();
            _mockEventRepo = new Mock<IEventRepository>();
            _service = new RegistrationService(_mockRegistrationRepo.Object, _mockEventRepo.Object);
        }

        
        [Fact]
        public async Task CreateAsync_ReturnsRegistration_WhenValid()
        {
            
            var ev = new Event { Id = 1, MaxCapacity = 10 };
            var existingRegistrations = new List<Registration>(); // empty — not full

            _mockEventRepo.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ev);
            _mockRegistrationRepo.Setup(r => r.GetByEventIdAsync(1)).ReturnsAsync(existingRegistrations);
            _mockRegistrationRepo.Setup(r => r.GetByEventAndUserAsync(1, 1)).ReturnsAsync((Registration?)null);
            _mockRegistrationRepo.Setup(r => r.AddAsync(It.IsAny<Registration>())).Returns(Task.CompletedTask);

            
            var result = await _service.CreateAsync(1, 1);

            
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal(1, result.EventId);
            Assert.False(result.IsConfirmed);
            _mockRegistrationRepo.Verify(r => r.AddAsync(It.IsAny<Registration>()), Times.Once);
        }

        
        [Fact]
        public async Task CreateAsync_ThrowsNotFoundException_WhenEventNotFound()
        {
            
            _mockEventRepo.Setup(r => r.GetByIdWithDetailsAsync(99)).ReturnsAsync((Event?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(1, 99));
        }

        
        [Fact]
        public async Task CreateAsync_ThrowsBadRequestException_WhenEventFull()
        {
            
            var ev = new Event { Id = 1, MaxCapacity = 2 };
            var existingRegistrations = new List<Registration>
            {
                new Registration(),
                new Registration()  
            };

            _mockEventRepo.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ev);
            _mockRegistrationRepo.Setup(r => r.GetByEventIdAsync(1)).ReturnsAsync(existingRegistrations);

            
            await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(1, 1));
        }

        
        [Fact]
        public async Task CreateAsync_ThrowsBadRequestException_WhenAlreadyRegistered()
        {
            
            var ev = new Event { Id = 1, MaxCapacity = 10 };
            var existingRegistrations = new List<Registration> { new Registration() };
            var existingRegistration = new Registration { UserId = 1, EventId = 1 };

            _mockEventRepo.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ev);
            _mockRegistrationRepo.Setup(r => r.GetByEventIdAsync(1)).ReturnsAsync(existingRegistrations);
            _mockRegistrationRepo.Setup(r => r.GetByEventAndUserAsync(1, 1)).ReturnsAsync(existingRegistration);

            
            await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(1, 1));
        }

        
        [Fact]
        public async Task ConfirmAsync_UpdatesConfirmation_WhenRegistrationExists()
        {
            
            var registration = new Registration { Id = 1, IsConfirmed = false };
            _mockRegistrationRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(registration);
            _mockRegistrationRepo.Setup(r => r.UpdateConfirmationAsync(1, true)).Returns(Task.CompletedTask);

            
            await _service.ConfirmAsync(1);

            
            _mockRegistrationRepo.Verify(r => r.UpdateConfirmationAsync(1, true), Times.Once);
        }

        [Fact]
        public async Task ConfirmAsync_ThrowsNotFoundException_WhenRegistrationNotFound()
        {
            
            _mockRegistrationRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Registration?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.ConfirmAsync(99));
        }

        
        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenRegistrationNotFound()
        {
            
            _mockRegistrationRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Registration?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_WhenRegistrationExists()
        {
            
            var registration = new Registration { Id = 1 };
            _mockRegistrationRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(registration);
            _mockRegistrationRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

           
            await _service.DeleteAsync(1);

            
            _mockRegistrationRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
