namespace Library;

public class LibraryFunctions
{
    public static void ListUserBooks(User borrower) =>
        LibraryDatabase.Instance.ListBooksOnLoan(borrower);

    public static void ReturnBook(User borrower) =>
        LibraryDatabase.Instance.ReturnBook(LibraryDatabase.Instance.SearchByIndex(Console.ReadLine()));

    public static void BorrowBook(User borrower) =>
        LibraryFunctions.BorrowBook(borrower, false);

    public static void BorrowBook(User borrower, bool selectBook)
    {
        Book? book = SearchForBook(selectBook);
        if (book != null)
        {
            LibraryDatabase.Instance.LoanBook(book, borrower);
        }
    }

    // public static void ReturnBook(User borrower)
    // {
    //     LibraryDatabase.Instance.ListBooks();
    // }

    public static void ListAllBooks(User borrower)
    {
        Book? book = SearchForBook(selectBook: true);
        if (book != null)
        {
            LibraryDatabase.Instance.LoanBook(book, borrower);
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }

    public static Book? SearchForBook(bool selectBook = false)
    {
        Console.WriteLine(GetSearchOptions());
        string input = Console.ReadLine();
        
        return input switch
        {
            "1" => SearchByKeyword(selectBook),
            "2" => SearchByAuthor(selectBook),
            "3" => SearchByGenre(selectBook),
            "4" => SearchById(selectBook),
            "5" => ListAllBooks(selectBook),
            _ => null
        };
    }

    private static Book? SearchBooks(
        string prompt,
        Func<string, IEnumerable<(int, Book)>> searchFunction,
        bool selectBook = false)
    {
        Console.WriteLine(prompt);
        string searchTerm = Console.ReadLine().ToLower();

        var matches = searchFunction(searchTerm);

        if (matches.Any())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nMatching books found:");
            foreach (var (id, book) in matches)
            {
                Console.WriteLine($"ID: {id} - {book}");
            }
            Console.ResetColor();

            if (selectBook)
            {
                Console.WriteLine("\nEnter the ID of the book you want to select (or press Enter to cancel):");
                string selection = Console.ReadLine();
                if (int.TryParse(selection, out int selectedId))
                {
                    return matches.FirstOrDefault(m => m.Item1 == selectedId).Item2;
                }
            }
            return null;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No matching books found");
            Console.ResetColor();
            return null;
        }
    }

    private static Book? SearchByKeyword(bool selectBook = false) =>
        SearchBooks("Enter keyword:", 
            keyword => LibraryDatabase.Instance.SearchByKeyword(keyword), 
            selectBook);

    private static Book? SearchByAuthor(bool selectBook = false) =>
        SearchBooks("Enter author name:", 
            author => LibraryDatabase.Instance.SearchByAuthor(author), 
            selectBook);

    private static Book? SearchByGenre(bool selectBook = false) =>
        SearchBooks("Enter genre:", 
            genre => LibraryDatabase.Instance.SearchByGenre(genre), 
            selectBook);

    private static Book? SearchById(bool selectBook = false)
    {
        Console.WriteLine("Enter book ID:");
        string input = Console.ReadLine();
        
        if (int.TryParse(input, out int id))
        {
            var book = LibraryDatabase.Instance.SearchById(id);
            if (book != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nBook found: ID: {id} - {book}");
                Console.ResetColor();
                return book;
            }
        }
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Book not found");
        Console.ResetColor();
        return null;
    }

    private static Book? ListAllBooks(bool selectBook = false)
    {
        var books = LibraryDatabase.GetBooks();

        if (books.Any())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAll books in library:");
            foreach (var (id, book) in books)
            {
                Console.WriteLine($"ID: {id} - {book}");
            }
            Console.ResetColor();

            if (selectBook)
            {
                Console.WriteLine("\nEnter the ID of the book you want to select (or press Enter to cancel):");
                string selection = Console.ReadLine();
                if (int.TryParse(selection, out int selectedId))
                {
                    return books.FirstOrDefault(m => m.Key == selectedId).Value;
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No books found in library");
            Console.ResetColor();
        }
        return null;
    }

    private static string GetSearchOptions() =>
        """
        1) search by keyword
        2) search by author
        3) search by genre
        4) search by ID
        5) list all books
        6) return to main menu
        """;
} 