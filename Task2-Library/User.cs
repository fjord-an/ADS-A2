namespace Library;

public class User
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public bool SignedIn { get; set; }
    
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}