using EventManagement.API.Controllers;
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagement.Tests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

        
        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenCategoriesExist()
        {
            
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Music" },
                new Category { Id = 2, Name = "Sports" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            
            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategories = Assert.IsType<List<Category>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count);
        }

        
        [Fact]
        public async Task GetByIdAsync_ReturnsOk_WhenCategoryExists()
        {
            
            var category = new Category { Id = 1, Name = "Music" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);

           
            var result = await _controller.GetByIdAsync(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal("Music", returnedCategory.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Category?)null);

            
            var result = await _controller.GetByIdAsync(99);

       
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

      
        [Fact]
        public async Task CreateAsync_ReturnsCreated_WhenValid()
        {
            
            var dto = new CreateCategoryDto { Name = "Music" };
            var category = new Category { Id = 1, Name = "Music" };
            _mockService.Setup(s => s.CreateAsync("Music")).ReturnsAsync(category);

            
            var result = await _controller.CreateAsync(dto);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCategory = Assert.IsType<Category>(createdResult.Value);
            Assert.Equal("Music", returnedCategory.Name);
        }

       
        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenCategoryExists()
        {
            
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

           
            var result = await _controller.DeleteAsync(1);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenCategoryDoesNotExist()
        {
            
            _mockService.Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new NotFoundException("Category with id 99 not found"));

            
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteAsync(99));
        }
    }

    public class LocationControllerTests
    {
        private readonly Mock<ILocationService> _mockService;
        private readonly LocationController _controller;

        public LocationControllerTests()
        {
            _mockService = new Mock<ILocationService>();
            _controller = new LocationController(_mockService.Object);
        }

        
        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenLocationsExist()
        {
            
            var locations = new List<Location>
            {
                new Location { Id = 1, Name = "Arena", Address = "Street 1" },
                new Location { Id = 2, Name = "Stadium", Address = "Street 2" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(locations);

            
            var result = await _controller.GetAllAsync();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLocations = Assert.IsType<List<Location>>(okResult.Value);
            Assert.Equal(2, returnedLocations.Count);
        }

        
        [Fact]
        public async Task GetByIdAsync_ReturnsOk_WhenLocationExists()
        {
            
            var location = new Location { Id = 1, Name = "Arena", Address = "Street 1" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(location);

            var result = await _controller.GetByIdAsync(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLocation = Assert.IsType<Location>(okResult.Value);
            Assert.Equal("Arena", returnedLocation.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenLocationDoesNotExist()
        {
            
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Location?)null);

           
            var result = await _controller.GetByIdAsync(99);

           
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

       
        [Fact]
        public async Task CreateAsync_ReturnsCreated_WhenValid()
        {
            
            var dto = new CreateLocationDto { Name = "Arena", Address = "Street 1" };
            var location = new Location { Id = 1, Name = "Arena", Address = "Street 1" };
            _mockService.Setup(s => s.CreateAsync("Arena", "Street 1")).ReturnsAsync(location);

            
            var result = await _controller.CreateAsync(dto);

           
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedLocation = Assert.IsType<Location>(createdResult.Value);
            Assert.Equal("Arena", returnedLocation.Name);
        }

        
        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenLocationExists()
        {
            
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

           
            var result = await _controller.DeleteAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenLocationDoesNotExist()
        {
           
            _mockService.Setup(s => s.DeleteAsync(99))
                .ThrowsAsync(new NotFoundException("Location with id 99 not found"));

            
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteAsync(99));
        }
    }
}