using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Tests.Repositories
{
    public class BookRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            var initialBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Book1", AuthorId = 1, PublishedYear = 2000, ISBN = "isbn1" },
                new Book { Id = 2, Title = "Book2", AuthorId = 2, PublishedYear = 2001, ISBN = "isbn2" }
            };

            var repository = new BookRepository(initialBooks);

            var books = await repository.GetAllAsync();

            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBook_WhenExists()
        {
            var book = new Book { Id = 1, Title = "Book1", AuthorId = 1, PublishedYear = 2000, ISBN = "isbn1" };
            var repository = new BookRepository(new List<Book> { book });

            var result = await repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Book1", result?.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var repository = new BookRepository();

            var result = await repository.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsBookAndAssignsId()
        {
            var repository = new BookRepository(new List<Book>(), startingId: 1);
            var book = new Book { Title = "New Book", AuthorId = 1, PublishedYear = 2020, ISBN = "isbn123" };

            await repository.AddAsync(book);

            Assert.Equal(1, book.Id);

            var allBooks = await repository.GetAllAsync();

            Assert.Contains(allBooks, b => b.Id == book.Id && b.Title == "New Book");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingBook()
        {
            var book = new Book { Id = 1, Title = "Original", AuthorId = 1, PublishedYear = 2000, ISBN = "isbn1" };
            var repository = new BookRepository(new List<Book> { book });

            book.Title = "Updated";
            await repository.UpdateAsync(book);

            var updated = await repository.GetByIdAsync(1);

            Assert.Equal("Updated", updated?.Title);
        }

        [Fact]
        public async Task UpdateAsync_DoesNothing_WhenBookNotFound()
        {
            var repository = new BookRepository(new List<Book>());
            var book = new Book { Id = 42, Title = "Nonexistent", AuthorId = 1, PublishedYear = 2000, ISBN = "isbn42" };

            await repository.UpdateAsync(book);

            var result = await repository.GetByIdAsync(42);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBookAndReturnsTrue_WhenBookExists()
        {
            var book = new Book { Id = 1, Title = "Book", AuthorId = 1, PublishedYear = 2000, ISBN = "isbn1" };
            var repo = new BookRepository(new List<Book> { book });

            var result = await repo.DeleteAsync(1);

            Assert.True(result);
            Assert.Null(await repo.GetByIdAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenBookNotFound()
        {
            var repo = new BookRepository();

            var result = await repo.DeleteAsync(999);

            Assert.False(result);
        }

        [Fact]
        public async Task IsISBNUniqueAsync_ReturnsTrue_WhenISBNExists()
        {
            var book = new Book { Id = 1, ISBN = "isbn1", Title = "Book", AuthorId = 1, PublishedYear = 2000 };
            var repo = new BookRepository(new List<Book> { book });

            var result = await repo.IsISBNUniqueAsync("isbn1");

            Assert.True(result);
        }

        [Fact]
        public async Task IsISBNUniqueAsync_ReturnsFalse_WhenISBNNotFound()
        {
            var repo = new BookRepository();

            var result = await repo.IsISBNUniqueAsync("notfound");

            Assert.False(result);
        }
    }
}
