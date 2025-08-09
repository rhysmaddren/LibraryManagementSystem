namespace LibraryManagementSystem.Models
{
    public class Book
    {
        /// <summary>
        /// The book identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the book.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The author identifier for the book.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// The year the book was published.
        /// </summary>
        public int PublishedYear { get; set; }

        /// <summary>
        /// The International Standard Book Number (ISBN) of the book.
        /// </summary>
        public string ISBN { get; set; } = string.Empty;
    }
}
