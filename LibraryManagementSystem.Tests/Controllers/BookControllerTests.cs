using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagementSystem.Tests.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockService = new Mock<IBookService>();
            _controller = new BookController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsSortedBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "B", PublishedYear = 2000 },
                new Book { Id = 2, Title = "A", PublishedYear = 2010 }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAll(sortBy: "Title", page: 1, pageSize: 5);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);

            // Assert
            Assert.Equal(new[] { "A", "B" }, returnedBooks.Select(b => b.Title));
        }

        [Fact]
        public async Task GetById_BookExists_ReturnsOk()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(book, okResult.Value);
        }

        [Fact]
        public async Task GetById_BookNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Book?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Add_ValidBook_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new AddBookDTO { Title = "New", AuthorId = 1, PublishedYear = 2020, ISBN = "123" };
            var created = new Book { Id = 10, Title = "New" };
            _mockService.Setup(s => s.AddAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(created, createdResult.Value);
        }

        [Fact]
        public async Task Add_InvalidBook_ReturnsBadRequest()
        {
            // Arrange
            var dto = new AddBookDTO();
            _mockService.Setup(s => s.AddAsync(dto)).ThrowsAsync(new InvalidOperationException("ISBN must be unique."));

            // Act
            var result = await _controller.Add(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("ISBN must be unique.", badRequest.Value);
        }

        [Fact]
        public async Task Update_BookExists_ReturnsOk()
        {
            // Arrange
            var dto = new UpdateBookDTO { Title = "Updated", AuthorId = 1, PublishedYear = 2021, ISBN = "456" };
            var updatedBook = new Book { Id = 1, Title = "Updated" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(updatedBook);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(updatedBook, okResult.Value);
        }

        [Fact]
        public async Task Update_BookNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new UpdateBookDTO();
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync((Book?)null);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_BookExists_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_BookNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}