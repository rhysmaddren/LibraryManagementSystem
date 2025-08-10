using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        /// <summary>
        /// Initializes the <see cref="BookService"/> with demo data.
        /// </summary>
        public BookService()
        {
            // Add demo data
            _books.AddRange(new List<Book>
            {
                new Book
                {
                    Id = _nextId++,
                    Title = "The Fellowship of the Ring",
                    AuthorId = 1,
                    PublishedYear = 1954,
                    ISBN = "978-0547928210"
                },
                new Book
                {
                    Id = _nextId++,
                    Title = "The Two Towers",
                    AuthorId = 1,
                    PublishedYear = 1954,
                    ISBN = "978-0547928203"
                },
                new Book
                {
                    Id = _nextId++,
                    Title = "The Return of the King",
                    AuthorId = 1,
                    PublishedYear = 1955,
                    ISBN = "978-0547928197"
                }
            });
        }

        /// <inheritdoc />
        public IEnumerable<Book> GetAll() => _books;

        /// <inheritdoc />
        public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

        /// <inheritdoc />
        public Book Add(AddBookDTO bookDTO)
        {
            var book = new Book
            {
                Title = bookDTO.Title,
                AuthorId = bookDTO.AuthorId,
                PublishedYear = bookDTO.PublishedYear,
                ISBN = bookDTO.ISBN
            };

            if (_books.Any(b => b.ISBN == book.ISBN))
            {
                throw new InvalidOperationException("ISBN must be unique.");
            }

            if (book.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Published year cannot be in the future.");
            }

            book.Id = _nextId++;

            _books.Add(book);

            return book;
        }

        /// <inheritdoc />
        public Book? Update(int id, UpdateBookDTO bookDTO)
        {
            var book = new Book
            {
                Title = bookDTO.Title,
                AuthorId = bookDTO.AuthorId,
                PublishedYear = bookDTO.PublishedYear,
                ISBN = bookDTO.ISBN
            };

            var existingBook = GetById(id);

            if (existingBook == null)
            {
                return null;
            }

            if (_books.Any(b => b.ISBN == book.ISBN && b.Id != id))
            {
                throw new InvalidOperationException("ISBN must be unique.");
            }

            if (book.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Published year cannot be in the future.");
            }

            existingBook.Title = book.Title;
            existingBook.AuthorId = book.AuthorId;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.ISBN = book.ISBN;

            return existingBook;
        }

        /// <inheritdoc />
        public bool Delete(int id)
        {
            var book = GetById(id);

            if (book == null)
            {
                return false;
            } 

            _books.Remove(book);

            return true;
        }
    }
}
