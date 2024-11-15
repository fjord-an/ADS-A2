using Task2_Library.Data;
using Task2_Library.Models;

namespace Library;

public class Menu()
{
    public static void Input(User borrower)
    {
        while (true)
        {
            Console.WriteLine(MainMenu());
            
            // if the input is not a number, print an error message and continue the loop
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
                    LibraryFunctions.BorrowBook(borrower, true);
                    break;
                case 5:
                    // logout the user and return to the login screen
                    // also update the borrower object to the new user
                    borrower = Authentication.Logout(borrower);
                    break;
                case 6:
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
        3) Search the Library Catalogue
        4) Borrow a book
        5) Logout
        6) Exit the Program
        """;
}
