namespace Library;

public class Menu()
{
    public static void Input(User borrower)
    {
        while (true)
        {
            Console.WriteLine(MainMenu());
            
            if (!(double.TryParse(Console.ReadLine(), out double input)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input, enter the number corresponding to the desired action:");
                Console.ResetColor();
                continue;
            }

            switch (input)
            {
                case 1:
                    LibraryFunctions.ListUserBooks(borrower);
                    break;
                case 2:
                    LibraryFunctions.ReturnBook(borrower);
                    break;
                case 3:
                    LibraryFunctions.SearchForBook();
                    break;
                case 4:
                    LibraryFunctions.BorrowBook(borrower);
                    break;
                case 5:
                    Environment.Exit(69);
                    break;
                default:
                    Console.WriteLine(MainMenu());
                    break;
            }
        }
    }

    static string MainMenu() =>
        """
        1) List all books you have on loan
        2) Return a book
        3) List all books in the library
        4) Borrow a book
        5) Exit 
        """;
}
