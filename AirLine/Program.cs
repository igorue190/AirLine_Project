using System;

namespace AirLine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 84);            
            StartPage startPage = new StartPage();
            startPage.MainMenu();
            
            Console.ReadKey();
        }
    }
}
