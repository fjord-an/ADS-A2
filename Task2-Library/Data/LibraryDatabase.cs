using Task2_Library.Models;

namespace Task2_Library.Data;

public sealed class LibraryDatabase
// the class is sealed to prevent inheritance, as it is a singleton class
// using the singleton pattern to ensure only one instance of the database is created
{
    // lock object to ensure only one instance of the database is created
    // and data integrity is maintained at all times:
    private static LibraryDatabase _instance;

    // an instance is created
    private static readonly object _lock = new object();

    public List<Book> Books { get; private set; }
    public static Dictionary<int, Book> BookCatalogue { get; private set; }
    public static int[] BooksOnLoan { get; private set; }

    // the instance is created in a private constructor, so it cannot be
    //  instantiated from outside the class
    private LibraryDatabase()
    {
        // create a list of books to store the books in the library (books that are present in the library)
        // this list is used to keep track of the books present in the library,
        // the dictionary BookCatalogue is used to keep track of books on loan
        Books = new List<Book>()
        {
            new Book("Dune", "Frank Herbert", "Sci-Fi"),
            new Book("The Silmarillion", "J.R.R. Tolkien", "Fantasy"),
            new Book("1984", "George Orwell", "Dystopian"),
            new Book("The Hobbit", "J.R.R. Tolkien", "Fantasy"),
            new Book("The Lord of the Rings", "J.R.R. Tolkien", "Fantasy"),
            new Book("The Da Vinci Code", "Dan Brown", "Mystery"),
            new Book("Moby Dick", "Herman Melville", "Adventure"),
            new Book("Harry Potter and the Philosophers Stone", "J.K rowling", "Fantasy"),
            new Book("Brave New World", "Aldous Huxley", "Sci-Fi"),
            new Book("Lord of the Flies", "William Golding", "Psychological"),
            new Book("To Kill a Mockingbird", "Harper Lee", "Fiction"),
            new Book("Pride and Prejudice", "Jane Austen", "Romance"),
            new Book("The Great Gatsby", "F. Scott Fitzgerald", "Fiction"),
            new Book("War and Peace", "Leo Tolstoy", "Historical"),
            new Book("Crime and Punishment", "Fyodor Dostoevsky", "Psychological"),
            new Book("The Catcher in the Rye", "J.D. Salinger", "Fiction"),
            new Book("The Odyssey", "Homer", "Epic"),
            new Book("Ulysses", "James Joyce", "Epic"),
        };

        BookCatalogue = new Dictionary<int, Book>();
        // create a dictionary and assign an id to each book to reference for the catalogue.
        // the catalogue will keep track of all books belonging to the library, whether it is
        // present in the library or on loan.
        for (int i = 1; i < Books.Count; i++)
        {
            Book book = Books[i - 1]; // calibrate zero-index
            BookCatalogue.Add(i, book); //assign an id to the book for the library catalogue
            // the book id will now be the same across the lists
        }

        // the array of id's that keep track of books that are on loan
        BooksOnLoan = new int[Books.Count];
    }

    // Create a static property to access the instance of the database.
    // using method-level locking to ensure thread safety and prevent multiple instances of the database
    // (third example in the article):
    // Implementing the Singleton Pattern in C#. (n.d.). Retrieved November 10, 2024, from https://csharpindepth.com/articles/singleton
    public static LibraryDatabase Instance
        // *this method is used to access the instance of this class
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new LibraryDatabase();
                }

                return _instance;
            }
        }
    }
    
    // method to return the books in the library
    public Dictionary<int, Book> GetBooks()
    {
        return BookCatalogue;
    }


    public Book? SearchById(int id)
    {
        if (BookCatalogue.TryGetValue(id, out var book))
        {
            return book;
        }

        return null;
    }

    public void LoanBook(Book book, User borrower)
    {
        if (BookCatalogue.ContainsValue(book))
        {
            // find the key according to the value (book) of the dictionary with a LINQ query. this will be used
            // to keep track of borrowed books in the borrowed array as per the requirements
            // of task 2 step 2.2
            int id = BookCatalogue.FirstOrDefault(x => x.Value.Equals(book)).Key;
            
            // stop the user from borrowing the same book twice by checking if the book is already on loan
            if (BooksOnLoan.Contains(id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This book is already on loan.");
                Console.ResetColor();
                return;
            }
            
            Books.Remove(book);
            // assign the book to the name of the borrower
            book.Borrower = borrower;
            // Add the book id to the array of loaned books, the index/number will correspond to the book
            // in the library catalogue to keep track of the loan status of the book
            BooksOnLoan[id] = id;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Successfully borrowed: {book.Title}");
        Console.ResetColor();
            
        }
    }

    public void ReturnBook(Book book)
    {
        int id = BookCatalogue.FirstOrDefault(x => x.Value.Equals(book)).Key;

        //todo need to set the book id to unique id not index, and compare id to the id in the dictionary:
        if (BookCatalogue.ContainsValue(book) && BooksOnLoan.Contains(id))
        {
            // get the database id of the book to return using the dictionary and LINQ query:
            BooksOnLoan[id] = 0;
            Books.Add(book);
        }
    }

    public void ListBooksOnLoan(User user)
    {
        foreach (int id in BooksOnLoan.Where(id => id != 0))
            // todo check implementation
            // foreach (int id in BooksOnLoan), the corresponding id number is
            // identified in the library catalogue to print it's information to the user
        {
            if (id != 0 && BookCatalogue[id].Borrower.Username == user.Username)
                // check if the book is on loan and also belongs to the logged in user
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("You have Borrowed:\n" + BookCatalogue[id]);
                Console.ResetColor();
            }
        }
    }

    public Book SearchByIndex(string index)
    {
        if (int.TryParse(index, out int id))
        {
            foreach (KeyValuePair<int, Book> book in BookCatalogue)
            {
                Console.WriteLine($"{book.Key}: {book.Value}");
            }

            if (BookCatalogue.TryGetValue(id, out var byIndex))
            {
                return byIndex;
            }
        }

        return new Book("Book not Found", "null", "null");
    }

    public IEnumerable<(int, Book)> SearchByKeyword(string keyword)
    {
        // return a collection of books that contain the keyword in the title, author or genre
        // using Where to filter the library catalogue for books that contain the keyword
        // with StringComparison.OrdinalIgnoreCase to ignore case sensitivity for flexible searching.
        // Select is used to return the book id and access the book information using the id as the key
        return BookCatalogue
            .Where(kvp =>
                kvp.Value.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                kvp.Value.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                kvp.Value.Genre.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => (kvp.Key, kvp.Value));
    }

    public IEnumerable<(int, Book)> SearchByAuthor(string author)
    {
        // filter the library catalogue for books by author using LINQ
        return BookCatalogue
            .Where(kvp => kvp.Value.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => (kvp.Key, kvp.Value));
    }

    public IEnumerable<(int, Book)> SearchByGenre(string genre)
    {
        // filter the library catalogue for books by genre using LINQ
        return BookCatalogue
            .Where(kvp => kvp.Value.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => (kvp.Key, kvp.Value));
    }

    public IEnumerable<(int, Book)> GetBooksOnLoan(User user)
    {
        // IEnumerable interface to return a collection of books on loan
        // that belong to the user. IEnumberable is more flexible for iterating over collections.
        // the method uses LINQ to query the BooksOnLoan array,  to find books that are on loan
        // and belong to the user (by referencing the LibraryDatabase BookCatalogue dictionary, which
        // contains the status of the books in the library). using Where to filter the books that are on loan
        // and belong to the user, and Select to return the book id and access its information using the id as the key
        return BooksOnLoan
            .Where(id => id != 0 && BookCatalogue[id].Borrower.Username == user.Username)
            .Select(id => (id, BookCatalogue[id]));
    }
}