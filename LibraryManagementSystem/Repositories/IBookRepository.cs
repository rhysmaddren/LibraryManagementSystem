using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public interface IBookRepository
    {
        /// <summary>
        /// Asynchronously retrieves all books from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable{T} of <see cref="Book"/> objects representing all books.</returns>
        Task<IEnumerable<Book>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Book"/> if found.</returns>
        Task<Book?> GetByIdAsync(int id);

        /// <summary>
        /// Asynchronously adds a new book to the collection.
        /// </summary>
        /// <param name="book">The book to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Book book);

        /// <summary>
        /// Updates the specified book in the data store asynchronously.
        /// </summary>
        /// <remarks>This method updates the details of an existing book in the data store. Ensure that
        /// the <see cref="Book.Id"/> property of the provided <paramref name="book"/> matches an existing book. If no
        /// matching book is found, an exception is thrown.</remarks>
        /// <param name="book">The book to update. The <see cref="Book.Id"/> property must be set to identify the book to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(Book book);

        /// <summary>
        /// Deletes the entity with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the entity
        /// was successfully deleted; otherwise, false. </returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Determines whether a book with the specified ISBN exists in the database.
        /// </summary>
        /// <param name="isbn">The ISBN of the book to check. Cannot be null or empty.</param>
        /// <returns> true if a book with the specified ISBN exists.</returns>
        Task<bool> IsISBNUniqueAsync(string isbn);
    }
}
