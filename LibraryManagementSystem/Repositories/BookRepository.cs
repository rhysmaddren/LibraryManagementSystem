using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class with an optional list of initial books
        /// and a starting ID for new books (for testing purposes).
        /// </summary>
        /// <remarks>If <paramref name="initialBooks"/> is provided, the repository will use the highest
        /// ID from the  provided books to determine the next ID for new book. If no books are provided, the repository
        /// will initialize with demo data and use <paramref name="startingId"/> as the starting ID.</remarks>
        /// <param name="initialBooks">An optional list of <see cref="Book"/> objects to initialize the repository with. If null, the
        /// repository will use demo data.</param>
        /// <param name="startingId">The starting ID to use for new books if no books are provided in <paramref name="initialBooks"/>.  Defaults
        /// to 1.</param>
        public BookRepository(List<Book>? initialBooks = null, int startingId = 1)
        {
            _books = initialBooks ?? ProvideDemoBooks();

            if (_books.Any())
            {
                _nextId = _books.Max(b => b.Id) + 1;
            }
            else
            {
                _nextId = startingId;
            }
        }

        /// Inheritdoc />
        public Task<IEnumerable<Book>> GetAllAsync() => Task.FromResult<IEnumerable<Book>>(_books);

        /// Inheritdoc />
        public Task<Book?> GetByIdAsync(int id) => Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

        /// Inheritdoc />
        public Task AddAsync(Book book)
        {
            book.Id = _nextId++;

            _books.Add(book);

            return Task.CompletedTask;
        }

        /// Inheritdoc />
        public Task UpdateAsync(Book book)
        {
            var index = _books.FindIndex(b => b.Id == book.Id);

            if (index >= 0)
            {
                _books[index] = book;
            }
            
            return Task.CompletedTask;
        }

        /// Inheritdoc />
        public Task<bool> DeleteAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                _books.Remove(book);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// Inheritdoc />
        public Task<bool> IsISBNUniqueAsync(string isbn)
        {
            var isISBNUnique = _books.Any(b => b.ISBN == isbn);

            return Task.FromResult(isISBNUnique);
        }

        private List<Book> ProvideDemoBooks()
        {
            return new List<Book>
            {
                
                new Book { Id = _nextId++, Title = "The Fellowship of the Ring", AuthorId = 1, PublishedYear = 1954, ISBN = "978-0547928210" },
                new Book { Id = _nextId++, Title = "The Two Towers", AuthorId = 1, PublishedYear = 1954, ISBN = "978-0547928203" },
                new Book { Id = _nextId++, Title = "The Return of the King", AuthorId = 1, PublishedYear = 1955, ISBN = "978-0547928197" },
                new Book { Id = _nextId++, Title = "Harry Potter and the Sorcerer's Stone", AuthorId = 2, PublishedYear = 1997, ISBN = "978-0590353427" },
                new Book { Id = _nextId++, Title = "Harry Potter and the Chamber of Secrets", AuthorId = 2, PublishedYear = 1998, ISBN = "978-0439064873" },
                new Book { Id = _nextId++, Title = "Harry Potter and the Prisoner of Azkaban", AuthorId = 2, PublishedYear = 1999, ISBN = "978-0439136365" },
                new Book { Id = _nextId++, Title = "Harry Potter and the Goblet of Fire", AuthorId = 2, PublishedYear = 2000, ISBN = "978-0439139601" },
                new Book { Id = _nextId++, Title = "1984", AuthorId = 3, PublishedYear = 1949, ISBN = "978-0451524935" },
                new Book { Id = _nextId++, Title = "To Kill a Mockingbird", AuthorId = 4, PublishedYear = 1960, ISBN = "978-0061120084" },
                new Book { Id = _nextId++, Title = "Pride and Prejudice", AuthorId = 5, PublishedYear = 1813, ISBN = "978-1503290563" },
                new Book { Id = _nextId++, Title = "Moby-Dick", AuthorId = 6, PublishedYear = 1851, ISBN = "978-1503280786" },
                new Book { Id = _nextId++, Title = "The Martian", AuthorId = 7, PublishedYear = 2011, ISBN = "978-0553418026" },
                new Book { Id = _nextId++, Title = "The Da Vinci Code", AuthorId = 8, PublishedYear = 2003, ISBN = "978-0307474278" },
                new Book { Id = _nextId++, Title = "The Hunger Games", AuthorId = 9, PublishedYear = 2008, ISBN = "978-0439023481" },
                new Book { Id = _nextId++, Title = "Catching Fire", AuthorId = 9, PublishedYear = 2009, ISBN = "978-0439023498" },
                new Book { Id = _nextId++, Title = "Dune", AuthorId = 10, PublishedYear = 1965, ISBN = "978-0441172719" },
                new Book { Id = _nextId++, Title = "Foundation", AuthorId = 11, PublishedYear = 1951, ISBN = "978-0553293357" },
                new Book { Id = _nextId++, Title = "Neuromancer", AuthorId = 12, PublishedYear = 1984, ISBN = "978-0441569595" },
                new Book { Id = _nextId++, Title = "Gone Girl", AuthorId = 13, PublishedYear = 2012, ISBN = "978-0307588371" },
                new Book { Id = _nextId++, Title = "The Girl with the Dragon Tattoo", AuthorId = 14, PublishedYear = 2005, ISBN = "978-0307454546" }
            };
        }
    }
}
