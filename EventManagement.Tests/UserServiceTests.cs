using EventManagement.BL.Exceptions;
using EventManagement.BL.Services;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EventManagement.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRoleRepository> _mockRoleRepo;
        private readonly Mock<IUserRoleRepository> _mockUserRoleRepo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRoleRepo = new Mock<IRoleRepository>();
            _mockUserRoleRepo = new Mock<IUserRoleRepository>();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["JWT:SecureKey"]).Returns("E(H+MbQeThWmZq4t6w9z$C&F)J@NcRfU");
            _service = new UserService(
                _mockUserRepo.Object,
                _mockRoleRepo.Object,
                _mockUserRoleRepo.Object,
                mockConfig.Object);
                 
        }

        
        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
           
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1" },
                new User { Id = 2, Username = "user2" }
            };
            _mockUserRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

           
            var result = await _service.GetAllAsync();

            
            Assert.Equal(2, result.Count());
        }

        
        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenExists()
        {
            
            var user = new User { Id = 1, Username = "user1" };
            _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            
            var result = await _service.GetByIdAsync(1);

            
            Assert.NotNull(result);
            Assert.Equal("user1", result.Username);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            
            _mockUserRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

            
            var result = await _service.GetByIdAsync(99);

           
            Assert.Null(result);
        }

        
        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenUserNotFound()
        {
            
            _mockUserRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_WhenUserExists()
        {
            
            var user = new User { Id = 1, Username = "user1" };
            _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUserRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            
            await _service.DeleteAsync(1);

            
            _mockUserRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        
        [Fact]
        public async Task PromoteToAdminAsync_ThrowsNotFoundException_WhenUserNotFound()
        {
           
            _mockUserRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.PromoteToAdminAsync(99));
        }

        [Fact]
        public async Task PromoteToAdminAsync_AssignsAdminRole_WhenUserExists()
        {
            
            var user = new User { Id = 1, Username = "user1" };
            var adminRole = new Role { Id = 2, Name = "Admin" };

            _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockRoleRepo.Setup(r => r.GetByNameAsync("Admin")).ReturnsAsync(adminRole);
            _mockUserRoleRepo.Setup(r => r.AssignRoleAsync(It.IsAny<UserRole>())).Returns(Task.CompletedTask);

            
            await _service.PromoteToAdminAsync(1);

            _mockUserRoleRepo.Verify(r => r.AssignRoleAsync(It.IsAny<UserRole>()), Times.Once);
        }

        
        [Fact]
        public async Task AssignDefaultRoleAsync_ThrowsNotFoundException_WhenRoleNotFound()
        {
            
            _mockRoleRepo.Setup(r => r.GetByNameAsync("User")).ReturnsAsync((Role?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.AssignDefaultRoleAsync(1));
        }

        [Fact]
        public async Task AssignDefaultRoleAsync_AssignsUserRole_WhenRoleExists()
        {
            
            var userRole = new Role { Id = 1, Name = "User" };
            _mockRoleRepo.Setup(r => r.GetByNameAsync("User")).ReturnsAsync(userRole);
            _mockUserRoleRepo.Setup(r => r.AssignRoleAsync(It.IsAny<UserRole>())).Returns(Task.CompletedTask);

            
            await _service.AssignDefaultRoleAsync(1);

            
            _mockUserRoleRepo.Verify(r => r.AssignRoleAsync(It.IsAny<UserRole>()), Times.Once);
        }
    }
}