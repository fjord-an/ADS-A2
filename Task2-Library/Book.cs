namespace Library;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public bool OnLoan { get; set; }
    public string Borrower { get; set; }
    public DateTime DueDate { get; set; }
    
    public Book(string title, string author, string genre)
    {
        Title = title;
        Author = author;
        Genre = genre;
        OnLoan = false;
        Borrower = null;
        DueDate = DateTime.Now + new TimeSpan(14, 0, 0, 0);
    }
    
    
    public override string ToString()
    {
        return $"Title: {Title}, Author: {Author}, Genre: {Genre}";
    }
    
    public string ToMetaDataString()
    {
        return $"{Title},{Author},{Genre},{OnLoan},{Borrower},{DueDate}";
    }
    
    public static Book FromFileString(string fileString)
    {
        string[] parts = fileString.Split(',');
        Book book = new Book(parts[0], parts[1], parts[2]);
        book.OnLoan = bool.Parse(parts[3]);
        book.Borrower = parts[4];
        book.DueDate = DateTime.Parse(parts[5]);
        return book;
    }
}