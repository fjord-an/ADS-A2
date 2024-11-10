namespace Library;

public static class Authentication
{
    public static List<User> Users = new List<User>
    {
        new User("user", "password")
    };
    
    public static User Login()
    {
        string username = GetUsername();
        string password = GetPassword();
        User user = Users.Find(u => u.Username == username);
        if (VerifyUser(user))
        {
            Console.WriteLine("Login successful");
            return user;
        }
        else
        {
            Console.WriteLine("Login failed. Please try again. (Use 'user' and 'password' as default)");
            return Login();
        }
    }
    
    public static string GetUsername()
    {
        Console.WriteLine("Enter your username:");
        return Console.ReadLine();
    }

    public static string GetPassword()
    {
        Console.WriteLine("Enter your password:");
        return Console.ReadLine();
    }

    public static bool VerifyUser(User user)
    {
        if (Users.Contains(user))
        {
            if (user.Password == Users.Find(u => u.Username == user.Username).Password)
            {
                // if the linq expression finds a user with a mathing username and password, return true
                // password is just a string for now, but in a real application, it would be hashed
                user.SignedIn = true;
                return true;
            }
            // if the linq expression finds a user with a matching username but not password, return false
            return false;
        }
        return false;
    }
}