using EventManagement.API.Controllers;
using EventManagement.API.Dtos;
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagement.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

      
        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenUsersExist()
        {
            
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@test.com", FirstName = "John", LastName = "Doe", UserRoles = new List<UserRole>() },
                new User { Id = 2, Username = "user2", Email = "user2@test.com", FirstName = "Jane", LastName = "Doe", UserRoles = new List<UserRole>() }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

           
            var result = await _controller.GetAllAsync();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        
        [Fact]
        public async Task GetByIdAsync_ReturnsOk_WhenUserExists()
        {
            
            var user = new User
            {
                Id = 1,
                Username = "user1",
                Email = "user1@test.com",
                FirstName = "John",
                LastName = "Doe",
                UserRoles = new List<UserRole>()
            };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

            
            var result = await _controller.GetByIdAsync(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenUserDoesNotExist()
        {
            
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((User?)null);

            
            var result = await _controller.GetByIdAsync(99);

            
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        
        

        // DELETE
        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenUserExists()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new NotFoundException("User with id 99 not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteAsync(99));
        }

      
        [Fact]
        public async Task PromoteUserAsync_ReturnsOk_WhenUserExists()
        {
            
            _mockService.Setup(s => s.PromoteToAdminAsync(1)).Returns(Task.CompletedTask);

          
            var result = await _controller.PromoteUserAsync(1);

            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PromoteUserAsync_ThrowsNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.PromoteToAdminAsync(99)).ThrowsAsync(new NotFoundException("User with id 99 not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.PromoteUserAsync(99));
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            
            var dto = new UserLoginDto { Username = "testuser", Password = "testpassword" };
            _mockService.Setup(s => s.LoginAsync(dto.Username, dto.Password))
                .ReturnsAsync("fake.jwt.token");

         
            var result = await _controller.Login(dto);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("fake.jwt.token", okResult.Value);
        }

        [Fact]
        public async Task Login_ThrowsUnauthorizedException_WhenCredentialsAreInvalid()
        {
           
            var dto = new UserLoginDto { Username = "wrong", Password = "wrong" };
            _mockService.Setup(s => s.LoginAsync(dto.Username, dto.Password))
                .ThrowsAsync(new UnauthorizedException("Incorrect username or password"));
            await Assert.ThrowsAsync<UnauthorizedException>(() => _controller.Login(dto));
        }
    }
}