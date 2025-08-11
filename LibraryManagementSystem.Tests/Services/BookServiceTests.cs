using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Tests.Services
{
    public class BookServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddBook_WhenValid()
        {
            var service = new BookService(new List<Book>(), startingId: 1);
            var dto = new AddBookDTO
            {
                Title = "Test Book",
                AuthorId = 1,
                PublishedYear = 2000,
                ISBN = "unique-isbn"
            };

            var addedBook = await service.AddAsync(dto);

            Assert.Equal(1, addedBook.Id);
            Assert.Equal(dto.Title, addedBook.Title);
            Assert.Equal(dto.ISBN, addedBook.ISBN);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenISBNNotUnique()
        {
            var initialBooks = new List<Book> { new Book { Id = 1, ISBN = "same-isbn" } };
            var service = new BookService(initialBooks);

            var dto = new AddBookDTO
            {
                Title = "Another Book",
                AuthorId = 1,
                PublishedYear = 2000,
                ISBN = "same-isbn"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPublishedYearInFuture()
        {
            var service = new BookService(new List<Book>());

            var dto = new AddBookDTO
            {
                Title = "Future Book",
                AuthorId = 1,
                PublishedYear = DateTime.Now.Year + 1,
                ISBN = "unique-isbn"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBook_WhenValid()
        {
            var book = new Book { Id = 1, Title = "Old Title", AuthorId = 1, PublishedYear = 1990, ISBN = "old-isbn" };
            var service = new BookService(new List<Book> { book });

            var dto = new UpdateBookDTO
            {
                Title = "New Title",
                AuthorId = 2,
                PublishedYear = 1995,
                ISBN = "new-isbn"
            };

            var updatedBook = await service.UpdateAsync(1, dto);

            Assert.NotNull(updatedBook);
            Assert.Equal("New Title", updatedBook!.Title);
            Assert.Equal("new-isbn", updatedBook.ISBN);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenBookNotFound()
        {
            var service = new BookService(new List<Book>());

            var dto = new UpdateBookDTO
            {
                Title = "New Title",
                AuthorId = 1,
                PublishedYear = 2000,
                ISBN = "new-isbn"
            };

            var updatedBook = await service.UpdateAsync(123, dto);

            Assert.Null(updatedBook);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenBookDeleted()
        {
            var book = new Book { Id = 1, ISBN = "isbn" };
            var service = new BookService(new List<Book> { book });

            var result = await service.DeleteAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenBookNotFound()
        {
            var service = new BookService(new List<Book>());

            var result = await service.DeleteAsync(1);

            Assert.False(result);
        }
    }
}
