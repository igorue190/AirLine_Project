using System;
using System.Configuration;
using System.Data.SqlClient;

namespace AirLine.BusinessLogic
{
    class PriceList
    {
        StartPage startPage = new StartPage();
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Details()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\tPRICE LIST\n");
            const string sqlExpression = "SELECT FlightNumber, CityOfDeparture, CityOfArrival, PriceForBusinessClass, PriceForEconomeClass FROM Flights";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var flightNumber = reader.GetValue(0);
                        var cityOfDeparture = reader.GetValue(1);
                        var cityOfArrival = reader.GetValue(2);
                        var priceForBusinessClass  = reader.GetValue(3);
                        var priceForEconomeClass = reader.GetValue(4);

                        Console.WriteLine("Flight number: {0} ({1} - {2})\n\tPrice for business class - {3}$" + "\n\tPrice for business class - {4}$\n", 
                            flightNumber, cityOfDeparture, cityOfArrival, priceForBusinessClass, priceForEconomeClass);
                    }
                }
                reader.Close();
            }
            Console.ReadKey();
            startPage.MainMenu();
        }      
    }
}
