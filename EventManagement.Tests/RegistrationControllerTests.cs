using EventManagement.API.Controllers;
using EventManagement.API.Dtos;
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagement.Tests
{
    public class RegistrationControllerTests
    {
        private readonly Mock<IRegistrationService> _mockService;
        private readonly RegistrationController _controller;

        public RegistrationControllerTests()
        {
            _mockService = new Mock<IRegistrationService>();
            _controller = new RegistrationController(_mockService.Object);
        }

       
        [Fact]
        public async Task GetByIdAsync_ReturnsOk_WhenRegistrationExists()
        {
            
            var registration = new Registration
            {
                Id = 1,
                UserId = 1,
                EventId = 1,
                RegistrationDate = DateTime.UtcNow,
                IsConfirmed = false
            };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(registration);

            
            var result = await _controller.GetByIdAsync(1);

           
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRegistration = Assert.IsType<RegistrationResponseDto>(okResult.Value);
            Assert.Equal(1, returnedRegistration.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenRegistrationDoesNotExist()
        {
            
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Registration?)null);

           
            var result = await _controller.GetByIdAsync(99);

            
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        
        [Fact]
        public async Task CreateAsync_ReturnsCreated_WhenValid()
        {
            
            var dto = new CreateRegistrationDto { UserId = 1, EventId = 1 };
            var registration = new Registration
            {
                Id = 1,
                UserId = 1,
                EventId = 1,
                RegistrationDate = DateTime.UtcNow,
                IsConfirmed = false
            };
            _mockService.Setup(s => s.CreateAsync(1, 1)).ReturnsAsync(registration);

            
            var result = await _controller.CreateAsync(dto);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedRegistration = Assert.IsType<RegistrationResponseDto>(createdResult.Value);
            Assert.Equal(1, returnedRegistration.UserId);
        }

        [Fact]
        public async Task CreateAsync_ThrowsBadRequestException_WhenEventFull()
        {
            
            var dto = new CreateRegistrationDto { UserId = 1, EventId = 1 };
            _mockService.Setup(s => s.CreateAsync(1, 1))
                .ThrowsAsync(new BadRequestException("Event has reached maximum capacity"));

            
            await Assert.ThrowsAsync<BadRequestException>(() => _controller.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ThrowsBadRequestException_WhenAlreadyRegistered()
        {
            
            var dto = new CreateRegistrationDto { UserId = 1, EventId = 1 };
            _mockService.Setup(s => s.CreateAsync(1, 1))
                .ThrowsAsync(new BadRequestException("User is already registered for this event"));

            
            await Assert.ThrowsAsync<BadRequestException>(() => _controller.CreateAsync(dto));
        }

        
        [Fact]
        public async Task ConfirmAsync_ReturnsOk_WhenRegistrationExists()
        {
            
            _mockService.Setup(s => s.ConfirmAsync(1)).Returns(Task.CompletedTask);

            
            var result = await _controller.ConfirmAsync(1);

            
            Assert.IsType<OkObjectResult>(result);
        }

       
        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenRegistrationExists()
        {
           
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            
            var result = await _controller.DeleteAsync(1);

            
            Assert.IsType<NoContentResult>(result);
        }
    }
}