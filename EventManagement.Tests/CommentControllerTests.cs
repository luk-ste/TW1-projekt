using EventManagement.API.Controllers;
using EventManagement.API.Dtos;
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagement.Tests
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> _mockService;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _mockService = new Mock<ICommentService>();
            _controller = new CommentController(_mockService.Object);
        }

        
        [Fact]
        public async Task GetByEventAsync_ReturnsOk_WhenCommentsExist()
        {
            
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", EventId = 1, UserId = 1, CreatedAt = DateTime.UtcNow },
                new Comment { Id = 2, Content = "Comment 2", EventId = 1, UserId = 2, CreatedAt = DateTime.UtcNow }
            };
            _mockService.Setup(s => s.GetByEventIdAsync(1)).ReturnsAsync(comments);

            
            var result = await _controller.GetByEventAsync(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedComments = Assert.IsType<List<CommentResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedComments.Count);
        }

        
        [Fact]
        public async Task CreateAsync_ReturnsOk_WhenValid()
        {
            
            var dto = new CreateCommentDto { Content = "Test comment", UserId = 1, EventId = 1 };
            var comment = new Comment
            {
                Id = 1,
                Content = dto.Content,
                UserId = dto.UserId,
                EventId = dto.EventId,
                CreatedAt = DateTime.UtcNow
            };
            _mockService.Setup(s => s.CreateAsync(1, 1, "Test comment")).ReturnsAsync(comment);

            
            var result = await _controller.CreateAsync(dto);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedComment = Assert.IsType<CommentResponseDto>(okResult.Value);
            Assert.Equal("Test comment", returnedComment.Content);
        }

        [Fact]
        public async Task CreateAsync_ThrowsNotFoundException_WhenEventDoesNotExist()
        {
            
            var dto = new CreateCommentDto { Content = "Test", UserId = 1, EventId = 99 };
            _mockService.Setup(s => s.CreateAsync(1, 99, "Test"))
                .ThrowsAsync(new NotFoundException("Event with id 99 not found"));

            
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.CreateAsync(dto));
        }

       
        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenCommentExists()
        {
            
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            
            var result = await _controller.DeleteAsync(1);

            
            Assert.IsType<NoContentResult>(result);
        }
    }
}