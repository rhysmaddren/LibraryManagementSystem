using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    /// <summary>
    /// Defines asynchronous operations for managing books in the library.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of all books.
        /// </returns>
        Task<IEnumerable<Book>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific book by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the matching book, or null if not found.
        /// </returns>
        Task<Book?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="bookDTO">The book to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the newly added book, including its generated ID.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the ISBN is not unique.</exception>
        Task<Book> AddAsync(AddBookDTO bookDTO);

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The unique ID of the book to update.</param>
        /// <param name="bookDTO">The updated book details.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the updated book, or null if not found.
        /// </returns>
        Task<Book?> UpdateAsync(int id, UpdateBookDTO bookDTO);

        /// <summary>
        /// Deletes a book.
        /// </summary>
        /// <param name="id">The identifier of the book to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing true if the book was deleted, false if not found.
        /// </returns>
        Task<bool> DeleteAsync(int id);
    }
}
