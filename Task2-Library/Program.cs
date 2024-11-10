using Task2_Library.Models;

namespace Library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            User user = Authentication.Login();
            Menu.Input(user);
        }
    }
}