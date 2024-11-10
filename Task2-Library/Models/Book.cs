namespace Task2_Library.Models;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public User Borrower { get; set; }
    public DateTime DueDate { get; set; }
    
    public Book(string title, string author, string genre)
    {
        Title = title;
        Author = author;
        Genre = genre;
        Borrower = null;
        DueDate = DateTime.Now + new TimeSpan(14, 0, 0, 0);
    }
    
    
    public override string ToString()
    // by overriding the ToString method, we can print the object in a more readable format
    // by default, instead of the default object reference string! ToString is called when
    // an object is printed to the console.
    {
        return $"Title: {Title}, Author: {Author}, Genre: {Genre}";
    }
    
    public string ToCatalogueString()
    {
        return $"{Title},{Author},{Genre},{Borrower},{DueDate}";
    }
}
