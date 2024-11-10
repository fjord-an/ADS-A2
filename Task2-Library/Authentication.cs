using Task2_Library.Models;

namespace Library;

public static class Authentication
{
    public static List<User> Users = new List<User>
    {
        new User("user")
    };
    
    public static User Login()
    {
        Console.WriteLine("1) Login");
        Console.WriteLine("2) Sign Up");
        Console.Write("Choose an option: ");
        
        string choice = Console.ReadLine();
        
        if (choice == "2")
        {
            return SignUp();
        }
        
        string username = GetUsername();
        User user = Users.Find(u => u.Username == username);
        if (VerifyUser(user))
        {
            Console.WriteLine("Login successful");
            return user;
        }
        else
        {
            Console.WriteLine("User not found. Please try again. (enter 'user' for the default account)");
            return Login();
        }
    }
    
    public static User Logout(User user)
    {
        user.SignedIn = false;
        Console.WriteLine("You have been logged out.");
        return Login();
    }
    
    public static string GetUsername()
    {
        Console.WriteLine("Enter your username:");
        return Console.ReadLine();
    }

    public static bool VerifyUser(User user)
    {
        if (Users.Contains(user))
        {
            // if the linq expression finds a user with a mathing username and password, return true
            // password is just a string for now, but in a real application, it would be hashed
            user.SignedIn = true;
            return true;
        }
        return false;
    }

    public static User SignUp()
    {
        Console.WriteLine("=== Sign Up ===");
        string username;
        do
        {
            Console.WriteLine("Enter a new username:");
            username = Console.ReadLine();
            
            if (Users.Any(u => u.Username == username))
            {
                Console.WriteLine("Username already exists. Please choose another.");
                continue;
            }
            break;
        } while (true);


        User newUser = new User(username);
        Users.Add(newUser);
        
        Console.WriteLine("Sign up successful! Please log in.");
        return Login();
    }
}