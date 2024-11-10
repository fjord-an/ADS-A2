using Task2_Library.Models;

namespace Task2_Library.Data;

public class LibraryFunctions
{
    public static void ListUserBooks(User borrower)
    {
        LibraryDatabase.Instance.ListBooksOnLoan(borrower);
    }

    public static void ReturnBook(User borrower)
    {
        var booksOnLoan = LibraryDatabase.Instance.GetBooksOnLoan(borrower);
        // int[] booksOnLoan;
        
        if (!booksOnLoan.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You don't have any books to return.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nYour borrowed books:");
        foreach (var (id, book) in booksOnLoan)
        {
            Console.WriteLine($"ID: {id} - {book}");
        }
        Console.ResetColor();

        Console.WriteLine("\nEnter the ID of the book you want to return (or press Enter to cancel):");
        string input = Console.ReadLine();
        
        if (string.IsNullOrEmpty(input)) return;
        
        if (int.TryParse(input, out int bookId))
        {
            if (booksOnLoan.Any(b => b.Item1 == bookId))
            {
                var book = LibraryDatabase.Instance.SearchById(bookId);
                if (book != null)
                {
                    LibraryDatabase.Instance.ReturnBook(book);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Successfully returned: {book.Title}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid book ID. Please try again.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please enter a valid book ID.");
            Console.ResetColor();
        }
    }

    public static void BorrowBook(User borrower, bool selectBook)
    {
        // Search for books to borrow, starting a serach with the option to select a book
        // to borrow with selectBook argument set to true
        Book? book = SearchForBook(selectBook);
        if (book != null)
        {
            LibraryDatabase.Instance.LoanBook(book, borrower);
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
        
        // Function to search for books based on a search term
        // using a delegate function to search the library catalogue
        // taking a string search term and returning a list of books
        // with its ID in a tuple (Func<param, return>). this simplifies
        // the search functions and allows for code reuse
        Func<string, IEnumerable<(int, Book)>> searchFunction,
        bool selectBook = false)
    {
        Console.WriteLine(prompt);
        string searchTerm = Console.ReadLine().ToLower();

        var matches = searchFunction(searchTerm);

        // Display the matching books found in the library catalogue from
        // the search term
        if (matches.Any())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nMatching books found:");
            foreach (var (id, book) in matches)
            {
                if (book.Borrower != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("On loan - ");
                }
                else
                    Console.ForegroundColor = ConsoleColor.Green;
                
                Console.WriteLine($"ID: {id} - {book}");
            }
            Console.ResetColor();

            if (selectBook)
            {
                Console.WriteLine("\nEnter the ID of the book you want to borrow (or press Enter to cancel):");
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
                if (book.Borrower != null)
                {
                    Console.Write("On loan - ");
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
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
        var books = LibraryDatabase.Instance.GetBooks();

        if (books.Any())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAll books in library:");
            // Display all books in the library catalogue and indicate if they are on loan
            foreach (var (id, book) in books)
            {
                if (book.Borrower != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("On loan - ");
                }
                else
                    Console.ForegroundColor = ConsoleColor.Green;
                
                Console.WriteLine($"ID: {id} - {book}");
                Console.ResetColor();
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

    // Sub-menu options for searching the library catalogue
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