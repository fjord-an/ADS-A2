namespace Library;

public class BookList
{
    public static List<Book> Books { get; set; }
    public static Dictionary<int, Book> BookCatalogue { get; set; } = new Dictionary<int, Book>();
    public static int[] BooksOnLoan { get; set; }
    public BookList()
    {
        Books = new List<Book>()
        {
            new Book("Dune", "Frank Herbert", "Sci-Fi"),
            new Book("The Silmarilon", "J.R.R. Tolkien", "Fantasy"),
            new Book("1984", "George Orwell", "Dystopian"),
            new Book("The Hobbit", "J.R.R. Tolkien", "Fantasy"),
            new Book("The Lord of the Rings", "J.R.R. Tolkien", "Fantasy"),
            new Book("The Da Vinci Code", "Dan Brown", "Mystery"),
            new Book("Moby Dick", "Herman Melville", "Adventure"),
            new Book("Harry Potter and the Philosophers Stone", "J.K rowling", "Fantasy"),
            new Book("Brave New World", "Aldous Huxley", "Sci-Fi"),
            new Book("Lord of the Flies", "William Golding", "Psychological"),
        };
        // create a dictionary and assign an id to each book to reference for the catalogue
        for(int i = 1; i < Books.Count; i++)
        {
            Book book = Books[i];
            BookCatalogue.Add(i, book);
        }
        BooksOnLoan = new int[Books.Count];
    }
    
    
    public void LoanBook(Book book, string borrower)
    {
        if (BookCatalogue.ContainsValue(book))
        {
            // find the key according to the value (book) of the dictionary with a LINQ query. this will be used
            // to keep track of borrowed books in the borrowed array as per the requirements
            // of task 2 step 2.2
            int id = BookCatalogue.FirstOrDefault(x => x.Value.Equals(book)).Key;
            Books.Remove(book);
            // assign the book to the name of the borrower
            book.Borrower = borrower;
            // Add the book id to the array of loaned books
            BooksOnLoan[id] = id;
        }
    }

    public void ReturnBook(Book book)
    {
        int id = BookCatalogue.FirstOrDefault(x => x.Value.Equals(book)).Key;
        
        //todo need to set the book id to unique id not index, and compare id to the id in the dictionary:
        if (BookCatalogue.ContainsValue(book) && BooksOnLoan[id] == BookCatalogue[id])
        {
            // get the database id of the book to return using the dictionary and LINQ query:
            BooksOnLoan[id] = 0;
            Books.Add(book);
        }
    }
}