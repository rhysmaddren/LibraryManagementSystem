using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing books in the library, including retrieving, adding, updating, and deleting books.
    /// </summary>
    /// <param name="bookService"> The service instance used to perform book-related operations.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class BookController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        /// <summary>
        /// Retrieves all books, optionally sorted by a specified property.
        /// </summary>
        /// <remarks>The returned list of books is sorted in ascending order based on the specified
        /// property. If no sorting is specified, the books are sorted by title by default.</remarks>
        /// <param name="sortBy">The property by which to sort the books. Valid values are "Title" (default) and "PublishedYear". Sorting is
        /// case-insensitive. If an invalid value is provided, no sorting is applied.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="Book"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAll(
            [FromQuery] string sortBy = "Title",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var books = await _bookService.GetAllAsync();

            books = sortBy.ToLower() switch
            {
                "title" => books.OrderBy(b => b.Title),
                "publishedyear" => books.OrderBy(b => b.PublishedYear),
                _ => books
            };

            books = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return Ok(books);
        }

        /// <summary>
        /// Retrieves a single book by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>
        /// The requested book if found; otherwise, a 404 Not Found response.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="bookDTO">The bookDTO object containing new book details.</param>
        /// <returns>
        /// A 201 Created response containing the newly created book.
        /// Returns 400 Bad Request if validation fails.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Book>> Add(AddBookDTO bookDTO)
        {
            try
            {
                var createdBook = await _bookService.AddAsync(bookDTO);
                return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the details of an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="bookDTO">The updated book details.</param>
        /// <returns>
        /// The updated book if successful; otherwise, 404 Not Found or 400 Bad Request.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Update(int id, UpdateBookDTO bookDTO)
        {
            try
            {
                var updatedBook = await _bookService.UpdateAsync(id, bookDTO);

                if (updatedBook == null)
                {
                    return NotFound();
                }

                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a book.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>
        /// 204 No Content if the deletion is successful; otherwise, 404 Not Found.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _bookService.DeleteAsync(id))
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
