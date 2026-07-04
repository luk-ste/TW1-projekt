using EventManagement.BL.Exceptions;
using EventManagement.BL.Services;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Moq;

namespace EventManagement.Tests
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepo;
        private readonly Mock<IEventRepository> _mockEventRepo;
        private readonly CommentService _service;

        public CommentServiceTests()
        {
            _mockCommentRepo = new Mock<ICommentRepository>();
            _mockEventRepo = new Mock<IEventRepository>();
            _service = new CommentService(_mockCommentRepo.Object, _mockEventRepo.Object);
        }

       
        [Fact]
        public async Task CreateAsync_ReturnsComment_WhenValid()
        {
            
            var ev = new Event { Id = 1 };
            _mockEventRepo.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ev);
            _mockCommentRepo.Setup(r => r.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);

            
            var result = await _service.CreateAsync(1, 1, "Test comment");

            
            Assert.NotNull(result);
            Assert.Equal("Test comment", result.Content);
            Assert.Equal(1, result.UserId);
            Assert.Equal(1, result.EventId);
            _mockCommentRepo.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Once);
        }

        
        [Fact]
        public async Task CreateAsync_ThrowsNotFoundException_WhenEventNotFound()
        {
            
            _mockEventRepo.Setup(r => r.GetByIdWithDetailsAsync(99)).ReturnsAsync((Event?)null);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(1, 99, "Test"));
        }

        
        [Fact]
        public async Task GetByEventIdAsync_ReturnsComments()
        {
            
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", EventId = 1 },
                new Comment { Id = 2, Content = "Comment 2", EventId = 1 }
            };
            _mockCommentRepo.Setup(r => r.GetByEventIdAsync(1)).ReturnsAsync(comments);

         
            var result = await _service.GetByEventIdAsync(1);

           
            Assert.Equal(2, result.Count());
        }

        
        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            
            _mockCommentRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            
            await _service.DeleteAsync(1);

            
            _mockCommentRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}