namespace Task2_Library.Models;

public class User
{
    public string Username { get; private set; }
    public bool SignedIn { get; set; }
    
    public User(string username)
    {
        Username = username;
    }
}