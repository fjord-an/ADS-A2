namespace Library;

public class User
{
    public string Username { get; private set; }
    public bool SignedIn { get; set; }
    
    public User(string username)
    {
        Username = username;
    }
}