using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Properties;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    /// <summary>
    /// Provides an in-memory asynchronous implementation of <see cref="IBookService"/>.
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        /// <summary>
        /// Initializes the <see cref="BookService"/> with demo data.
        /// </summary>
        /// <param name="bookRepository">The repository to use for book operations.</param>
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        /// <inheritdoc />
        public Task<IEnumerable<Book>> GetAllAsync() => _bookRepository.GetAllAsync();

        /// <inheritdoc />
        public Task<Book?> GetByIdAsync(int id) => _bookRepository.GetByIdAsync(id);
    

        /// <inheritdoc />
        public async Task<Book> AddAsync(AddBookDTO bookDTO)
        {
            if (await _bookRepository.IsISBNUniqueAsync(bookDTO.ISBN))
            {
                throw new InvalidOperationException(Resource.ISBNNotUniqueMessage);
            }

            if (bookDTO.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException(Resource.PublishedYearInFutureMessage);
            }

            var book = CreateBookWithoutIDFromAddBookDTO(bookDTO);

            await _bookRepository.AddAsync(book);
            return book;
        }

        /// <inheritdoc />
        public async Task<Book?> UpdateAsync(int id, UpdateBookDTO bookDTO)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);

            if (existingBook == null)
            {
                return null;
            }

            if (await _bookRepository.IsISBNUniqueAsync(bookDTO.ISBN) && bookDTO.ISBN != existingBook.ISBN)
            {
                throw new InvalidOperationException(Resource.ISBNNotUniqueMessage);
            }

            if (bookDTO.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException(Resource.PublishedYearInFutureMessage);
            }
                
            existingBook.Title = bookDTO.Title;
            existingBook.AuthorId = bookDTO.AuthorId;
            existingBook.PublishedYear = bookDTO.PublishedYear;
            existingBook.ISBN = bookDTO.ISBN;

            await _bookRepository.UpdateAsync(existingBook);

            return existingBook;
        }

        /// <inheritdoc />
        public Task<bool> DeleteAsync(int id) => _bookRepository.DeleteAsync(id);

        private Book CreateBookWithoutIDFromAddBookDTO(AddBookDTO bookDTO)
        {
            return new Book
            {
                Title = bookDTO.Title,
                AuthorId = bookDTO.AuthorId,
                PublishedYear = bookDTO.PublishedYear,
                ISBN = bookDTO.ISBN
            };
        }
    }
}
