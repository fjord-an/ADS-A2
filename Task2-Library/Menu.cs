namespace Library;

public class Menu()
{
    
    public static void Display(double option)
    {

        static void ListBooks()
        {
            //foreach
        }
    }

    public static void Input()
    {
        bool validInput;
        while (true)
        {
            // print the options before requesting input
            Console.WriteLine(MainMenu());
            
            // if the input is invalid (not a number), try again (restart the loop => continue)
            // else output the value to the variable: input
            if (!(validInput = double.TryParse(Console.ReadLine(), out double input)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input, enter the number corresponding to the desired action:");
                Console.ResetColor();
                continue;
            }

            double choice;
            switch (input)
            {
                case 1:
                        // list all books on loan
                    break;
                case 2:
                    // return a book
                    break;
                case 3:
                    // list all books in the library
                break;
                case 4:
                    //Borrow a book
                    break;
                case 5:
                    //exit
                    Environment.Exit(69);
                    break;
                default:
                    Console.WriteLine(MainMenu());
                    break;
            }
            
            return;
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
    
    //static !xx?IEnumerable<string> ListBooks(books) => foreach(book in books){List<string> }
    // what type?
}
