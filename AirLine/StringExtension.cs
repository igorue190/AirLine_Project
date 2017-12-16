using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AirLine
{
    static class StringExtension
    {
        static StartPage startPage = new StartPage();
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static bool StringCheck(string temp)
        {

            if (!Regex.IsMatch(temp, @"[a-zA-z\d/]"))
            {
                Console.WriteLine("Incorrectly entered data (numbers, symbols are invalid). Try again");
                return false;
            }
            return true;
        }

        public static bool WhileForOneString(this string item)
        {
            while (!StringCheck(item))
            {
                item = Console.ReadLine();
            }
            return true;
        }

        public static bool WhileForTwoString(this string item, string first, string second)
        {
            while (!(item == first || item == second))
            {
                Console.WriteLine("Incorrectly entered data (numbers, symbols are invalid). Try again");
                item = Console.ReadLine();
            }
            return true;
        }

        public static bool WhileForFourString(this string item, string first, string second, string third, string fourth)
        {
            while (!(item == first || item == second || item == third || item == fourth))
            {
                Console.WriteLine("Incorrectly entered data. Try again");
                item = Console.ReadLine();
            }
            return true;
        }

        public static bool WhileForEightString(this string item, string first, string second, string third, string fourth, string fifth, string sixth, string seventh, string eighth, string nineth)
        {
            while (!(item == first || item == second || item == third || item == fourth || item == fifth || item == sixth || item == seventh || item == eighth || item == nineth))
            {
                Console.WriteLine("Incorrectly entered data. Try again");
                item = Console.ReadLine();
            }
            return true;
        }

        public static void SqlRequestForEditCases(this string expression, string passportOld, int passportSearch, string changedParameter, string newParameter)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(expression, connection);
                command.Parameters.AddWithValue(passportOld, passportSearch);
                command.Parameters.AddWithValue(changedParameter, newParameter);
                var countUpdated = command.ExecuteNonQuery();
                Console.WriteLine("Updated object(s): {0}\n", countUpdated);
                Console.ReadKey();
                startPage.MainMenu();
            }
        }

        public static void SqlRequestForData(this string expression, string parametrOld, int parametrNew, string changedParameter, DateTime newDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(expression, connection);
                command.Parameters.AddWithValue(parametrOld, parametrNew);
                command.Parameters.AddWithValue(changedParameter, newDate);
                var countUpdated = command.ExecuteNonQuery();
                Console.WriteLine("Updated object(s): {0}\n", countUpdated);
                Console.ReadKey();
                startPage.MainMenu();
            }
        }

        public static void CreatFlightNumberList(this string expression, List<string> list)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(expression, connection);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var flightNumbers = reader.GetValue(0).ToString();
                        list.Add(flightNumbers);
                    }
                }
                reader.Close();
            }
        }

        public static decimal SelectPriceForPassenger(this string expression, int flightNumber)
        {
            decimal price = 0;
            using (var connection = new SqlConnection(connectionString))
            {                
                connection.Open();
                var command = new SqlCommand(expression, connection);
                command.Parameters.AddWithValue("FlightNumber", flightNumber);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        price = (decimal)reader.GetValue(0);                        
                    }                   
                }             
                reader.Close();
            }
            return price;
        } 
    }
}
