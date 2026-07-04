using EventManagement.BL.Exceptions;
using EventManagement.BL.Services;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Moq;

namespace EventManagement.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _service = new CategoryService(_mockRepo.Object);
        }

        // GET ALL
        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Music" },
                new Category { Id = 2, Name = "Sports" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        // GET BY ID
        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Music" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Music", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        // CREATE
        [Fact]
        public async Task CreateAsync_ReturnsCategory()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync("Music");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Music", result.Name);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        // UPDATE
        [Fact]
        public async Task UpdateAsync_ReturnsUpdated_WhenExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Old Name" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(1, "New Name");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            var result = await _service.UpdateAsync(99, "New Name");

            
            Assert.Null(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

      
        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_WhenExists()
        {
            
            var category = new Category { Id = 1, Name = "Music" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

          
            await _service.DeleteAsync(1);

            
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }

    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepository> _mockRepo;
        private readonly LocationService _service;

        public LocationServiceTests()
        {
            _mockRepo = new Mock<ILocationRepository>();
            _service = new LocationService(_mockRepo.Object);
        }

        
        [Fact]
        public async Task GetAllAsync_ReturnsAllLocations()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location { Id = 1, Name = "Arena", Address = "Street 1" },
                new Location { Id = 2, Name = "Stadium", Address = "Street 2" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(locations);

            
            var result = await _service.GetAllAsync();

            
            Assert.Equal(2, result.Count());
        }

        
        [Fact]
        public async Task CreateAsync_ReturnsLocation()
        {
            
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Location>())).Returns(Task.CompletedTask);

            
            var result = await _service.CreateAsync("Arena", "Street 1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Arena", result.Name);
            Assert.Equal("Street 1", result.Address);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Location>()), Times.Once);
        }

       
        [Fact]
        public async Task UpdateAsync_ReturnsUpdated_WhenExists()
        {
           
            var location = new Location { Id = 1, Name = "Old Name", Address = "Old Address" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Location>())).Returns(Task.CompletedTask);

            
            var result = await _service.UpdateAsync(1, "New Name", "New Address");

           
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
            Assert.Equal("New Address", result.Address);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location?)null);

            
            var result = await _service.UpdateAsync(99, "New Name", "New Address");

            
            Assert.Null(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Location>()), Times.Never);
        }

        
        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenNotExists()
        {
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_WhenExists()
        {
            
            var location = new Location { Id = 1, Name = "Arena", Address = "Street 1" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            
            await _service.DeleteAsync(1);

            
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}