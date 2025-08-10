using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    /// <summary>
    /// Defines operations for managing books in the library.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>A collection of all books</returns>
        IEnumerable<Book> GetAll();

        /// <summary>
        /// Retrieves a specific book by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>The matching book, or null if not found.</returns>
        Book? GetById(int id);

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="bookDTO">The book to add.</param>
        /// <returns>The newly added book, including its generated ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ISBN is not unique.</exception>
        Book Add(AddBookDTO bookDTO);

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The unique ID of the book to update.</param>
        /// <param name="bookDTO">The updated book details.</param>
        /// <returns>The updated book, or null if not found.</returns>
        Book? Update(int id, UpdateBookDTO bookDTO);

        /// <summary>
        /// Deletes a book.
        /// </summary>
        /// <param name="id">The identifier of the book to delete.</param>
        /// <returns>True if the book was deleted, false if not found.</returns>
        bool Delete(int id);
    }
}
