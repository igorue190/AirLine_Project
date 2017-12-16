using System;
using System.Configuration;
using System.Data.SqlClient;
using AirLine.Repository;

namespace AirLine.BusinessLogic
{
    class PassengerManager
    {      
        IPassengerRepository passengerRepository = new PassengerRepository();
        StartPage startPage = new StartPage();
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Menu()
        {
            Console.WriteLine("\nShow information about all passengers, press - 1\n");
            Console.WriteLine("Add new passenger, press - 2\n");
            Console.WriteLine("Edit information about passenger, press - 3\n");
            Console.WriteLine("Delete information about passenger, press - 4\n");
            Console.WriteLine("Find information about concret passenger, press - 5\n");

            var result = int.Parse(Console.ReadLine());
            while (!(result == 1 || result == 2 || result == 3 || result == 4 || result == 5))
            {
                Console.WriteLine("Input data incorrect, try again!");
                result = int.Parse(Console.ReadLine());
            }
            switch(result)
            {
                case 1:
                    passengerRepository.Details();
                    break;
                case 2:
                    passengerRepository.Add();
                break;
                case 3:
                    Console.WriteLine("Please enter serias of passport you want edit");
                    var passportSeriesForEdit = CheckInputedNumbers();
                    passengerRepository.Edit(passportSeriesForEdit);
                break;
                case 4:
                    Console.WriteLine("Please enter serias of passport you want delete");
                    var passportSeriesForDelete = CheckInputedNumbers();
                    passengerRepository.Delete(passportSeriesForDelete);                    
                break;
                case 5:
                Console.WriteLine("\nFind information by passenger name, press - 1\n");
                Console.WriteLine("Find information by passport, press - 2\n");
                Console.WriteLine("Find information by price, press - 3\n");
                var resultForFind = int.Parse(Console.ReadLine());
                while (!(resultForFind == 1 || resultForFind == 2 || resultForFind == 3))
                {
                    Console.WriteLine("Input data incorrect, try again!");
                    resultForFind = int.Parse(Console.ReadLine());
                }
                switch (resultForFind)
                {
                    case 1:
                        Console.WriteLine("Enter passenger name");
                        var stringResult = Console.ReadLine();                       
                        Find(stringResult);
                        break;
                    case 2:
                        Console.WriteLine("Enter passport or flights number");
                        var resultInt = CheckInputedNumbers();
                        Find(resultInt);
                        break;
                    case 3:
                        Console.WriteLine("Enter price");
                        var inputString = Console.ReadLine();
                        decimal item;
                        var resultDecimal = decimal.TryParse(inputString, out item);
                        while (resultDecimal != true)
                        {
                            Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                            inputString = Console.ReadLine();
                            resultDecimal = decimal.TryParse(inputString, out item);
                        }
                        Find(item);
                        break;
                }
                break;                  
            }
        }

        public void Find(string info)
        {
            const string sqlExpressionName = "SELECT * FROM Passengers WHERE Name = @Name";     
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpressionName, connection);
                command.Parameters.AddWithValue("Name", info);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var name = reader.GetValue(1);
                        var surname = reader.GetValue(2);
                        var nationality = reader.GetValue(3);
                        var passportSeries = reader.GetValue(4);
                        var dateOfBith = reader.GetValue(5);
                        var sex = reader.GetValue(6);
                        var classesOfService = reader.GetValue(7);
                        var flight = reader.GetValue(8);
                        var price = reader.GetValue(9);

                        Console.WriteLine("Name: " + name + "\nSurname: " + surname + "\nNationality: " + nationality + "\nPassport series: " + passportSeries + "\nDate of birth: " +
                                          dateOfBith + "\nSex: " + sex + "\nClass: " + classesOfService + "\nFlights number: " + flight + "\nPrice: " + price + "$\n");
                    }
                }
                reader.Close();
            }    
            Console.WriteLine("Return to main menu, press - 1\n");
            ReturnToMainMenu();
        }

        public void Find(int? info)
        {
            const string sqlExpressionPassport = "SELECT * FROM Passengers WHERE PassportSeries = @PassportSeries";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpressionPassport, connection);
                command.Parameters.AddWithValue("PassportSeries", info);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var name = reader.GetValue(1);
                        var surname = reader.GetValue(2);
                        var nationality = reader.GetValue(3);
                        var passportSeries = reader.GetValue(4);
                        var dateOfBith = reader.GetValue(5);
                        var sex = reader.GetValue(6);
                        var classesOfService = reader.GetValue(7);
                        var flight = reader.GetValue(8);
                        var price = reader.GetValue(9);

                        Console.WriteLine("Name: " + name + "\nSurname: " + surname + "\nNationality: " + nationality +
                                          "\nPassport series: " + passportSeries + "\nDate of birth: " +
                                          dateOfBith + "\nSex: " + sex + "\nClass: " + classesOfService +
                                          "\nFlights number: " + flight + "\nPrice: " + price + "$\n");
                    }
                }
                reader.Close();
            }
            Console.WriteLine("Return to main menu, press - 1\n");
            ReturnToMainMenu();
        }

        public void Find(decimal? info)
        {
            const string sqlExpressionPrice = "SELECT * FROM Passengers WHERE Price = @Price";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpressionPrice, connection);
                command.Parameters.AddWithValue("Price", info);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var name = reader.GetValue(1);
                        var surname = reader.GetValue(2);
                        var nationality = reader.GetValue(3);
                        var passportSeries = reader.GetValue(4);
                        var dateOfBith = reader.GetValue(5);
                        var sex = reader.GetValue(6);
                        var classesOfService = reader.GetValue(7);
                        var flight = reader.GetValue(8);
                        var price = reader.GetValue(9);

                        Console.WriteLine("Name: " + name + "\nSurname: " + surname + "\nNationality: " + nationality +
                                          "\nPassport series: " + passportSeries + "\nDate of birth: " +
                                          dateOfBith + "\nSex: " + sex + "\nClass: " + classesOfService +
                                          "\nFlights number: " + flight + "\nPrice: " + price + "$\n");
                    }
                }
                reader.Close();
            }
            Console.WriteLine("Return to main menu, press - 1\n");
            ReturnToMainMenu();
        }     

        private int CheckInputedNumbers()
        {           
            var inputString = Console.ReadLine();
            int item;
            var result = int.TryParse(inputString, out item);
            while (!result)
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                inputString = Console.ReadLine();
                result = int.TryParse(inputString, out item);
            }
            return item;
        }

        private void ReturnToMainMenu()
        {
            var inputString = Console.ReadLine();
            int item;
            var result = int.TryParse(inputString, out item);
            while (!(result && item == 1))
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                inputString = Console.ReadLine();
                result = int.TryParse(inputString, out item);
            }
            Console.Clear();
            startPage.MainMenu();
        }
    }
}