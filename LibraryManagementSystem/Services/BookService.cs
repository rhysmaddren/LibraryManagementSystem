using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    /// <summary>
    /// Provides an in-memory asynchronous implementation of <see cref="IBookService"/>.
    /// </summary>
    public class BookService : IBookService
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        /// <summary>
        /// Initializes the <see cref="BookService"/> with demo data.
        /// </summary>
        /// <param name="initialBooks">Optional initial books to populate the service with.</param>
        /// <param name="startingId">The starting ID for the first book.</param>
        public BookService(List<Book>? initialBooks = null, int startingId = 1)
        {
            _books = initialBooks ?? new List<Book>();

            if (_books.Any())
            {
                _nextId = _books.Max(b => b.Id) + 1;
            }
            else
            { 
                _nextId = startingId;
            }

            // Only add demo data if no initialBooks provided
            if (initialBooks == null)
            {
                _books.AddRange(new List<Book>
                {
                    new Book { Id = _nextId++, Title = "The Fellowship of the Ring", AuthorId = 1, PublishedYear = 1954, ISBN = "978-0547928210" },
                    new Book { Id = _nextId++, Title = "The Two Towers", AuthorId = 1, PublishedYear = 1954, ISBN = "978-0547928203" },
                    new Book { Id = _nextId++, Title = "The Return of the King", AuthorId = 1, PublishedYear = 1955, ISBN = "978-0547928197" }
                });
            }
        }

        /// <inheritdoc />
        public Task<IEnumerable<Book>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Book>>(_books);
        }

        /// <inheritdoc />
        public Task<Book?> GetByIdAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            return Task.FromResult(book);
        }

        /// <inheritdoc />
        public Task<Book> AddAsync(AddBookDTO bookDTO)
        {
            if (_books.Any(b => b.ISBN == bookDTO.ISBN))
            {
                throw new InvalidOperationException("ISBN must be unique.");
            }

            if (bookDTO.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Published year cannot be in the future.");
            }

            var book = CreateBookFromAddBookDTO(bookDTO);
            _books.Add(book);

            return Task.FromResult(book);
        }

        /// <inheritdoc />
        public Task<Book?> UpdateAsync(int id, UpdateBookDTO bookDTO)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return Task.FromResult<Book?>(null);
            }

            if (_books.Any(b => b.ISBN == bookDTO.ISBN && b.Id != id) && bookDTO.ISBN != existingBook.ISBN)
            {
                throw new InvalidOperationException("ISBN must be unique.");
            }

            if (bookDTO.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Published year cannot be in the future.");
            }

            existingBook.Title = bookDTO.Title;
            existingBook.AuthorId = bookDTO.AuthorId;
            existingBook.PublishedYear = bookDTO.PublishedYear;
            existingBook.ISBN = bookDTO.ISBN;

            return Task.FromResult<Book?>(existingBook);
        }

        /// <inheritdoc />
        public Task<bool> DeleteAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return Task.FromResult(false);
            }

            _books.Remove(book);

            return Task.FromResult(true);
        }

        private Book CreateBookFromAddBookDTO(AddBookDTO bookDTO)
        {
            return new Book
            {
                Id = _nextId++,
                Title = bookDTO.Title,
                AuthorId = bookDTO.AuthorId,
                PublishedYear = bookDTO.PublishedYear,
                ISBN = bookDTO.ISBN
            };
        }
    }
}
