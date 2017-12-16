using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using AirLine.LB.Model;
using AirLine.LB.Enum;

namespace AirLine.Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        StartPage startPage = new StartPage();
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Add()
        {
            var addedPassenger = new Passenger();

            Console.WriteLine("Please, enter name: "); // Add name
            var name = Console.ReadLine();
            name.WhileForOneString();
            addedPassenger.Name = name;

            Console.WriteLine("Please, enter surname: "); //Add surname
            var surname = Console.ReadLine();
            surname.WhileForOneString();
            addedPassenger.Surname = surname;

            Console.WriteLine("Please, enter nationality: "); //Add nationality
            var nationality = Console.ReadLine();
            nationality.WhileForOneString();
            addedPassenger.Nationality = nationality;

            Console.WriteLine("Please, enter passport series: "); //Add passport series
            var passport = Console.ReadLine();
            int passportSeries;
            var result = int.TryParse(passport, out passportSeries);
            while (result != true)
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                passport = Console.ReadLine();
                result = int.TryParse(passport, out passportSeries);
            }
            addedPassenger.PassportSeries = passportSeries;

            Console.WriteLine("Please, enter date of bith (yyyy-mm-dd): "); //Add date of bith
            addedPassenger.DateOfBith = DateTime.Parse(Console.ReadLine());                                

            Console.WriteLine("Please, enter you sex (male/famale): "); //Add sex
            var sex = Console.ReadLine();
            sex.WhileForTwoString("male", "female");
            addedPassenger.Sex = sex == "male"
                ? Sex.Male
                : Sex.Female;

            Console.WriteLine("Please, enter classes of service you want (business/economy): "); //Add service
            var classes = Console.ReadLine();
            classes.WhileForTwoString("business", "economy");
            addedPassenger.ClassesOfService = classes == "business"
                ? ClassesOfService.Business
                : ClassesOfService.Economy;

            const string sqlExpressionRead = "SELECT FlightNumber FROM Flights"; //Add flight number
            var flightNumberList = new List<string>();
            sqlExpressionRead.CreatFlightNumberList(flightNumberList);
            Console.WriteLine("Please, select direction:\n");
            foreach (var itemList in flightNumberList)
            {
                Console.WriteLine("Flight number - {0}\n", itemList);
            }
            addedPassenger.FlightNumber = int.Parse(Console.ReadLine());            

            if (addedPassenger.ClassesOfService == ClassesOfService.Business) //add price
            {
                const string sqlExpressionPrice =
                    "SELECT PriceForBusinessClass FROM Flights WHERE FlightNumber = @FlightNumber";
                addedPassenger.Price = sqlExpressionPrice.SelectPriceForPassenger(addedPassenger.FlightNumber);
            }
            else
            {
                const string sqlExpressionPrice =
                    "SELECT PriceForEconomeClass FROM Flights WHERE FlightNumber = @FlightNumber";
                addedPassenger.Price = sqlExpressionPrice.SelectPriceForPassenger(addedPassenger.FlightNumber);
            }

            const string sqlExpression =
                "INSERT INTO Passengers (Name, Surname, Nationality, PassportSeries, DateOfBith, Sex, ClassesOfService, Flights, Price) VALUES (@Name, @Surname, @Nationality, @PassportSeries, @DateOfBith, @Sex, @ClassesOfService, @Flights, @Price)";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("Name", addedPassenger.Name);
                command.Parameters.AddWithValue("Surname", addedPassenger.Surname);
                command.Parameters.AddWithValue("Nationality", addedPassenger.Nationality);
                command.Parameters.AddWithValue("PassportSeries", addedPassenger.PassportSeries);
                command.Parameters.AddWithValue("DateOfBith", addedPassenger.DateOfBith);
                command.Parameters.AddWithValue("Sex", addedPassenger.Sex.ToString());
                command.Parameters.AddWithValue("ClassesOfService", addedPassenger.ClassesOfService.ToString());
                command.Parameters.AddWithValue("Flights", addedPassenger.FlightNumber);
                command.Parameters.AddWithValue("Price", addedPassenger.Price);
                var countAdded = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}\n", countAdded);
            }
            Console.WriteLine(addedPassenger.ToString());
            Console.WriteLine("Add ones, press - 1\t\tReturn to main menu, press  - 2");
            var item = CheckInputNumbers();
            switch (item)
            {
                case 1:
                    Add();
                    break;
                case 2:
                    Console.Clear();
                    startPage.MainMenu();
                    break;
            }
        }

        public void Delete(int passportSeries)
        {
            const string sqlExpression = "DELETE  FROM Passengers WHERE PassportSeries = @PassportSeries";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("PassportSeries", passportSeries);
                var number = command.ExecuteNonQuery();
                Console.WriteLine("Deleted object: {0}\n.Press Enter", number);
                Console.ReadKey();
                Console.Clear();
                startPage.MainMenu();
            }
        }

        public void Edit(int passportSeries)
        {
            const string
                sqlExpression =
                    "SELECT PassportSeries FROM Passengers WHERE PassportSeries = @PassportSeries"; //Check passport series
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("PassportSeries", passportSeries);
                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("You input incorrect data. Press Enter\n");
                    Console.ReadKey();
                    Console.Clear();
                    startPage.MainMenu();
                }
            }
            Console.WriteLine("Select field you want change: ");
            Console.WriteLine(
                "Name - 1\nSurname - 2\nNationality - 3\nDateOfBith - 4\nSex - 5\nFlightNumber - 6\nClassesOfService - 7");
            var inputString = Console.ReadLine();
            int item;
            var result = int.TryParse(inputString, out item);
            while (!(result == true &&
                     (item == 1 || item == 2 || item == 3 || item == 4 || item == 5 || item == 6 || item == 7)))
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                inputString = Console.ReadLine();
                result = int.TryParse(inputString, out item);
            }
            Console.Clear();
            switch (item)                                  //Edit data
            {
                case 1:
                    Console.WriteLine("Please, enter name: ");
                    var name = Console.ReadLine();
                    name.WhileForOneString();
                    const string sqlExpressionForName =
                        "UPDATE Passengers SET Name = @Name WHERE PassportSeries = @PassportSeries";
                    sqlExpressionForName.SqlRequestForEditCases("PassportSeries", passportSeries, "Name", name);
                    break;
                case 2:
                    Console.WriteLine("Please, enter surname: ");
                    var surname = Console.ReadLine();
                    surname.WhileForOneString();
                    string sqlExpressionForSurname =
                        "UPDATE Passengers SET Surname = @Surname WHERE PassportSeries = @PassportSeries";
                    sqlExpressionForSurname.SqlRequestForEditCases("PassportSeries", passportSeries, "Surname",
                        surname);
                    break;
                case 3:
                    Console.WriteLine("Please, enter nationality: ");
                    var nationality = Console.ReadLine();
                    nationality.WhileForOneString();
                    const string sqlExpressionForNationality =
                        "UPDATE Passengers SET Nationality = @Nationality WHERE PassportSeries = @PassportSeries";
                    sqlExpressionForNationality.SqlRequestForEditCases("PassportSeries", passportSeries, "Nationality",
                        nationality);
                    break;
                case 4:
                    Console.WriteLine("Please, enter DateOfBith(yyyy-mm-dd): ");
                    var dateOfBith = DateTime.Parse(Console.ReadLine());                  
                    const string sqlExpressionForDateOfBith =
                        "UPDATE Passengers SET DateOfBith = @DateOfBith WHERE PassportSeries = @PassportSeries";  
                    sqlExpressionForDateOfBith.SqlRequestForData("PassportSeries", passportSeries, "DateOfBith", dateOfBith);               
                    break;
                case 5:
                    Console.WriteLine("Please, enter Sex(male/female): ");
                    var sex = Console.ReadLine();
                    sex.WhileForTwoString("male", "female");
                    const string sqlExpressionForSex =
                        "UPDATE Passengers SET Sex = @Sex WHERE PassportSeries = @PassportSeries";
                    sqlExpressionForSex.SqlRequestForEditCases("PassportSeries", passportSeries, "Sex",
                        sex == "male" ? Sex.Male.ToString() : Sex.Female.ToString());
                    break;
                case 6:
                    ChangeDataAfterEditedFlightNaumber(passportSeries);                    
                    Console.ReadKey();
                    startPage.MainMenu();
                    break;
                case 7:
                    Console.WriteLine("Please, enter ClassesOfService(businnes/economy): ");
                    var classesOfService = Console.ReadLine();
                    classesOfService.WhileForTwoString("businnes", "economy");
                    const string sqlExpressionForClassesOfService =
                        "UPDATE Passengers SET ClassesOfService = @ClassesOfService WHERE PassportSeries = @PassportSeries";
                    sqlExpressionForClassesOfService.SqlRequestForEditCases("PassportSeries", passportSeries,
                        "ClassesOfService",
                        classesOfService == "businnes"
                            ? ClassesOfService.Business.ToString()
                            : ClassesOfService.Economy.ToString());
                    break;
            }
        }

        public void Details()
        {
            const string sqlExpression = "SELECT * FROM Passengers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
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
            Console.ReadKey();
            startPage.MainMenu();
        }

        private int CheckInputNumbers()
        {
            var inputString = Console.ReadLine();
            int item;
            var result = int.TryParse(inputString, out item);
            while (!(result && (item == 1 || item == 2)))
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                inputString = Console.ReadLine();
                result = int.TryParse(inputString, out item);
            }
            Console.Clear();
            return item;
        }

        private void ChangeDataAfterEditedFlightNaumber(int passportSeries)
        {
            const string sqlExpressionRead = "SELECT FlightNumber FROM Flights";
            const string sqlExpressionReadInfo = "SELECT PriceForBusinessClass, PriceForEconomeClass FROM Flights WHERE FlightNumber = @FlightNumber";
            const string sqlExpressionForFlightNumber = "UPDATE Passengers SET Flights = @FlightNumber, Price = @Price WHERE PassportSeries = @PassportSeries";

            var flightNumberList = new List<string>();
            sqlExpressionRead.CreatFlightNumberList(flightNumberList);
            Console.WriteLine("Please, select direction:\n");
            foreach (var itemList in flightNumberList)
            {
                Console.WriteLine("Flight number - {0}\n", itemList);
            }
            var flightNumber = int.Parse(Console.ReadLine());
            
            using (var connection = new SqlConnection(connectionString))
            {            
                decimal priceForBusinessClass = 0;
                decimal priceForEconomeClass = 0;
                connection.Open();
                var commandRead = new SqlCommand(sqlExpressionReadInfo, connection);
                commandRead.Parameters.AddWithValue("FlightNumber", flightNumber);
                var reader = commandRead.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        priceForBusinessClass = (decimal) reader.GetValue(0);
                        priceForEconomeClass = (decimal) reader.GetValue(1);
                    }
                }
                reader.Close();
                var command = new SqlCommand(sqlExpressionForFlightNumber, connection);
                command.Parameters.AddWithValue("PassportSeries", passportSeries);
                command.Parameters.AddWithValue("FlightNumber", flightNumber);
                command.Parameters.AddWithValue("Price",
                    PassengerServiceClass(passportSeries) == ClassesOfService.Business.ToString()
                        ? priceForBusinessClass
                        : priceForEconomeClass);
                var countUpdated = command.ExecuteNonQuery();
                Console.WriteLine("Updated object: {0}\nPress enter", countUpdated);
            }
        }

        private string PassengerServiceClass(int passportSeries)
        {
            string classOfCervice = null;
            const string sqlExtenssion = "SELECT ClassesOfService FROM Passengers WHERE PassportSeries = @PassportSeries";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExtenssion, connection);
                command.Parameters.AddWithValue("PassportSeries", passportSeries);
                var reader = command.ExecuteReader();
                if (!reader.HasRows) 
                    return null;
                while (reader.Read())
                {
                    classOfCervice =  reader.GetValue(0).ToString();
                }
                reader.Close();
            }
            return classOfCervice;
        }
    }
}
