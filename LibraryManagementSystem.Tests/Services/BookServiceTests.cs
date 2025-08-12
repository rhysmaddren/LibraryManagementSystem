using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Properties;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;
using Moq;

namespace LibraryManagementSystem.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book" } };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

            var result = await _bookService.GetAllAsync();

            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBook_WhenBookExists()
        {
            var book = new Book { Id = 1, Title = "Test" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

            var result = await _bookService.GetByIdAsync(1);

            Assert.Equal(book, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Book?)null);

            var result = await _bookService.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ThrowsInvalidOperationException_WhenISBNNotUnique()
        {
            var dto = new AddBookDTO { ISBN = "123", Title = "Title", AuthorId = 1, PublishedYear = 2020 };
            _mockRepository.Setup(r => r.IsISBNUniqueAsync(dto.ISBN)).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _bookService.AddAsync(dto));
            Assert.Equal(Resource.ISBNNotUniqueMessage, ex.Message);
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentException_WhenPublishedYearInFuture()
        {
            var dto = new AddBookDTO
            {
                ISBN = "123",
                Title = "Title",
                AuthorId = 1,
                PublishedYear = DateTime.Now.Year + 1
            };
            _mockRepository.Setup(r => r.IsISBNUniqueAsync(dto.ISBN)).ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _bookService.AddAsync(dto));
            Assert.Equal(Resource.PublishedYearInFutureMessage, ex.Message);
        }

        [Fact]
        public async Task AddAsync_AddsBook_WhenValid()
        {
            var dto = new AddBookDTO { ISBN = "123", Title = "Title", AuthorId = 1, PublishedYear = 2020 };
            _mockRepository.Setup(r => r.IsISBNUniqueAsync(dto.ISBN)).ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var result = await _bookService.AddAsync(dto);

            _mockRepository.Verify(r => r.AddAsync(It.Is<Book>(b =>
                b.ISBN == dto.ISBN &&
                b.Title == dto.Title &&
                b.AuthorId == dto.AuthorId &&
                b.PublishedYear == dto.PublishedYear
            )), Times.Once);

            Assert.Equal(dto.ISBN, result.ISBN);
            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(dto.AuthorId, result.AuthorId);
            Assert.Equal(dto.PublishedYear, result.PublishedYear);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Book?)null);

            var dto = new UpdateBookDTO { ISBN = "123", Title = "New Title", AuthorId = 2, PublishedYear = 2019 };

            var result = await _bookService.UpdateAsync(1, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsInvalidOperationException_WhenISBNNotUniqueAndChanged()
        {
            var existingBook = new Book { Id = 1, ISBN = "111", Title = "Old Title", AuthorId = 1, PublishedYear = 2018 };
            var dto = new UpdateBookDTO { ISBN = "123", Title = "New Title", AuthorId = 2, PublishedYear = 2019 };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingBook);
            _mockRepository.Setup(r => r.IsISBNUniqueAsync(dto.ISBN)).ReturnsAsync(true); // ISBN not unique

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _bookService.UpdateAsync(1, dto));
            Assert.Equal(Resource.ISBNNotUniqueMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsArgumentException_WhenPublishedYearInFuture()
        {
            var existingBook = new Book { Id = 1, ISBN = "111", Title = "Old Title", AuthorId = 1, PublishedYear = 2018 };
            var dto = new UpdateBookDTO { ISBN = "111", Title = "New Title", AuthorId = 2, PublishedYear = DateTime.Now.Year + 1 };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingBook);
            _mockRepository.Setup(r => r.IsISBNUniqueAsync(dto.ISBN)).ReturnsAsync(false); // ISBN is unique or unchanged

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _bookService.UpdateAsync(1, dto));
            Assert.Equal(Resource.PublishedYearInFutureMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesBook_WhenValid()
        {
            var existingBook = new Book { Id = 1, ISBN = "111", Title = "Old Title", AuthorId = 1, PublishedYear = 2018 };
            var dto = new UpdateBookDTO { ISBN = "111", Title = "New Title", AuthorId = 2, PublishedYear = 2019 };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingBook);
            _mockRepository.Setup(r => r.IsISBNUniqueAsync(dto.ISBN)).ReturnsAsync(false);
            _mockRepository.Setup(r => r.UpdateAsync(existingBook)).Returns(Task.CompletedTask);

            var result = await _bookService.UpdateAsync(1, dto);

            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Book>(b =>
                b.Id == existingBook.Id &&
                b.ISBN == dto.ISBN &&
                b.Title == dto.Title &&
                b.AuthorId == dto.AuthorId &&
                b.PublishedYear == dto.PublishedYear
            )), Times.Once);

            Assert.Equal(dto.Title, result?.Title);
            Assert.Equal(dto.AuthorId, result?.AuthorId);
            Assert.Equal(dto.PublishedYear, result?.PublishedYear);
            Assert.Equal(dto.ISBN, result?.ISBN);
        }

        [Fact]
        public async Task DeleteAsync_DelegatesToRepository()
        {
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _bookService.DeleteAsync(1);

            Assert.True(result);
        }
    }
}
