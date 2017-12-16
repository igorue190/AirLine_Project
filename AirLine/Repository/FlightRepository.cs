using System;
using System.Configuration;
using System.Data.SqlClient;
using AirLine.LB.Enum;
using AirLine.LB.Model;

namespace AirLine.Repository
{
    class FlightRepository: IFlightRepository
    {
        StartPage startPage = new StartPage();
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void Add()
        {
            var addedFlight = new Flight();

            Console.WriteLine("Please, enter flightNumber: ");       //Add flight number
            var inputFlightNumber = Console.ReadLine();            
            int flightNumber;
            var resultFlightNumber = int.TryParse(inputFlightNumber, out flightNumber);
            while (resultFlightNumber != true)
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                inputFlightNumber = Console.ReadLine();
                resultFlightNumber = int.TryParse(inputFlightNumber, out flightNumber);
            }
            addedFlight.FlightNumber = flightNumber;

            Console.WriteLine("Please, enter city of departure: ");         //add departure city
            var cityOfDeparture = Console.ReadLine();
            cityOfDeparture.WhileForOneString();
            addedFlight.CityOfDeparture = cityOfDeparture;

            Console.WriteLine("Please, enter city of arrival: ");          //add arrival city
            var cityOfArrival = Console.ReadLine();
            cityOfArrival.WhileForOneString();
            addedFlight.CityOfArrival = cityOfArrival;            

            Console.WriteLine("Please, enter departure date (yyyy-mm-dd): ");    //add date                      
            addedFlight.DepartureDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Please, enter arrival date (yyyy-mm-dd): ");                
            addedFlight.ArrivalDate = DateTime.Parse(Console.ReadLine()); 

            Console.WriteLine("Please, enter terminal ({0},{1},{2},{3}): ", Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());       //add terminal
            var terminal = Console.ReadLine();
            terminal.WhileForFourString(Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());           
            foreach (var itemValue in Enum.GetValues(typeof(Terminal)))
            {
                if (terminal == itemValue.ToString())
                {
                    addedFlight.Terminal = (Terminal)itemValue;
                    break;
                }
            }

            Console.WriteLine("Please, enter gate ({0},{1},{2},{3}): ", Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());      //add gate
            var gate = Console.ReadLine();
            gate.WhileForFourString(Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());         
            foreach (var itemValue in Enum.GetValues(typeof(Terminal)))
            {
                if (gate == itemValue.ToString())
                {
                    addedFlight.Gate = (Terminal)itemValue;
                    break;
                }
            }

            Console.WriteLine("Please, enter flight status ({0},{1},{2},{3},{4}, {5},{6}, {7}, {8}): ", FlightStatus.Arrived.ToString(),             //add status
                FlightStatus.Canceled.ToString(), FlightStatus.CheckIn.ToString(), FlightStatus.Delayed.ToString(), FlightStatus.DepartedAt.ToString(), 
                FlightStatus.ExpectedAt.ToString(), FlightStatus.GateClosed.ToString(), FlightStatus.InFlight.ToString(), FlightStatus.Unknown.ToString());
            var flightStatus = Console.ReadLine();
            flightStatus.WhileForEightString(FlightStatus.Arrived.ToString(), FlightStatus.Canceled.ToString(), FlightStatus.CheckIn.ToString(),
                FlightStatus.Delayed.ToString(), FlightStatus.DepartedAt.ToString(), FlightStatus.ExpectedAt.ToString(), FlightStatus.GateClosed.ToString(), 
                FlightStatus.InFlight.ToString(), FlightStatus.Unknown.ToString());
            foreach (var itemValue in Enum.GetValues(typeof(FlightStatus)))
            {
                if (flightStatus == itemValue.ToString())
                {
                    addedFlight.FlightStatus = (FlightStatus)itemValue;
                    break;
                }
            }

            Console.WriteLine("Enter price for business class: ");                          //Add price for business
            addedFlight.PriceForBusinessClass = CheckInputDecimal();

            Console.WriteLine("Enter price for econome class: ");                            //Add price for econome
            addedFlight.PriceForEconomeClass = CheckInputDecimal();

            const string sqlExpression = "INSERT INTO Flights (FlightNumber, CityOfDeparture, CityOfArrival, DepartureDate, ArrivalDate, Terminal, Gate, FlightStatus, PriceForBusinessClass, PriceForEconomeClass) VALUES " +
                                         "(@FlightNumber, @CityOfDeparture, @CityOfArrival, @DepartureDate, @ArrivalDate, @Terminal, @Gate, @FlightStatus, @PriceForBusinessClass, @PriceForEconomeClass)";         
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("FlightNumber", addedFlight.FlightNumber);
                command.Parameters.AddWithValue("CityOfDeparture", addedFlight.CityOfDeparture);
                command.Parameters.AddWithValue("CityOfArrival", addedFlight.CityOfArrival);
                command.Parameters.AddWithValue("DepartureDate", addedFlight.DepartureDate);
                command.Parameters.AddWithValue("ArrivalDate", addedFlight.ArrivalDate);
                command.Parameters.AddWithValue("Terminal", addedFlight.Terminal.ToString());
                command.Parameters.AddWithValue("Gate", addedFlight.Gate.ToString());
                command.Parameters.AddWithValue("FlightStatus", addedFlight.FlightStatus.ToString());
                command.Parameters.AddWithValue("PriceForBusinessClass", addedFlight.PriceForBusinessClass);
                command.Parameters.AddWithValue("PriceForEconomeClass", addedFlight.PriceForEconomeClass);
                var countAdded = command.ExecuteNonQuery();
                Console.WriteLine("Added object: {0}\n", countAdded);
            }
            Console.WriteLine(addedFlight.ToString());
            SelectNextStep();
        }

        public void Delete(int flightNumber)
        {
            const string sqlExpression = "DELETE  FROM Flights WHERE FlightNumber = @FlightNumber";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("FlightNumber", flightNumber);
                var number = command.ExecuteNonQuery();
                Console.WriteLine("Deleted object: {0}\nPress Enter", number);
                Console.ReadKey();
                Console.Clear();
                startPage.MainMenu();
            }
        }

        public void Edit(int flightNumber)
        {
            const string sqlExpression = "SELECT FlightNumber FROM Flights WHERE FlightNumber = @FlightNumber";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("FlightNumber", flightNumber);
                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("You input incorrect data. Press Enter\n");
                    Console.ReadKey();
                    Console.Clear();
                    startPage.MainMenu();
                }
            }
            Console.WriteLine("Select the field you want to change:");
            Console.WriteLine("\nCity of departure - 1\nCity of arrival - 2\nDeparture date - 3\nArrival date - 4\nTerminal - 5\nGate - 6\nFlight status - 7");
            int item;
            var result = int.TryParse(Console.ReadLine(), out item);
            while (!(result && (item == 1 || item == 2 || item == 3 || item == 4 || item == 5 || item == 6 || item == 7)))
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                result = int.TryParse(Console.ReadLine(), out item);
            }
            Console.Clear();            
            switch (item)
            {
                case 1:
                    Console.WriteLine("Please, enter city of departure: ");
                    var cityOfDeparture = Console.ReadLine();
                    cityOfDeparture.WhileForOneString();
                    const string sqlExpressionForCityOfDeparture = "UPDATE Flights SET CityOfDeparture = @CityOfDeparture WHERE FlightNumber = @FlightNumber";
                    sqlExpressionForCityOfDeparture.SqlRequestForEditCases("FlightNumber", flightNumber, "CityOfDeparture", cityOfDeparture);                                       
                    break;
                case 2:
                    Console.WriteLine("Please, enter city of arrival: ");
                    var cityOfArrival = Console.ReadLine();
                    cityOfArrival.WhileForOneString();
                    const string sqlExpressionForCityOfArrival = "UPDATE Flights SET CityOfArrival = @CityOfArrival WHERE FlightNumber = @FlightNumber";
                    sqlExpressionForCityOfArrival.SqlRequestForEditCases("FlightNumber", flightNumber, "CityOfArrival", cityOfArrival);                                       
                    break;
                case 3:
                    Console.WriteLine("Please, enter departure date(yyyy-mm-dd): ");
                    var departureDate = DateTime.Parse(Console.ReadLine());                  
                    const string sqlExpressionForDepartureDate = "UPDATE Flights SET DepartureDate = @DepartureDate WHERE FlightNumber = @FlightNumber";
                    sqlExpressionForDepartureDate.SqlRequestForData("FlightNumber", flightNumber, "DepartureDate", departureDate);                                    
                    break;
                case 4:
                    Console.WriteLine("Please, enter arrival date(yyyy-mm-dd): ");
                    var arrivalDate = DateTime.Parse(Console.ReadLine());
                    const string sqlExpressionForArrivalDate = "UPDATE Flights SET ArrivalDate = @ArrivalDate WHERE FlightNumber = @FlightNumber";
                    sqlExpressionForArrivalDate.SqlRequestForData("FlightNumber", flightNumber, "ArrivalDate", arrivalDate);                                       
                    break;
                case 5:
                    Console.WriteLine("Please, enter terminal ({0},{1},{2},{3}): ", Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());
                    var terminal = Console.ReadLine();
                    terminal.WhileForFourString(Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());           
                    foreach (var itemValue in Enum.GetValues(typeof(Terminal)))
                    {
                        if (terminal == itemValue.ToString())
                        {
                            const string sqlExpressionForTerminal = "UPDATE Flights SET Terminal = @Terminal WHERE FlightNumber = @FlightNumber";
                            sqlExpressionForTerminal.SqlRequestForEditCases("FlightNumber", flightNumber, "Terminal", itemValue.ToString());
                            break;
                        }
                    }                            
                    break;
                case 6:
                    Console.WriteLine("Please, enter gate ({0},{1},{2},{3}): ", Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());
                    var gate = Console.ReadLine();
                    gate.WhileForFourString(Terminal.A.ToString(), Terminal.B.ToString(), Terminal.C.ToString(), Terminal.D.ToString());           
                    foreach (var itemValue in Enum.GetValues(typeof(Terminal)))
                    {
                        if (gate == itemValue.ToString())
                        {
                            const string sqlExpressionForGate = "UPDATE Flights SET Gate = @Gate WHERE FlightNumber = @FlightNumber";
                            sqlExpressionForGate.SqlRequestForEditCases("FlightNumber", flightNumber, "Gate", itemValue.ToString());
                            break;
                        }
                    }                            
                    break;
                case 7:
                    Console.WriteLine("Please, enter flight status ({0},{1},{2},{3},{4}, {5},{6}, {7}, {8}): ", FlightStatus.Arrived.ToString(), 
                        FlightStatus.Canceled.ToString(), FlightStatus.CheckIn.ToString(), FlightStatus.Delayed.ToString(), FlightStatus.DepartedAt.ToString(),
                        FlightStatus.ExpectedAt.ToString(), FlightStatus.GateClosed.ToString(), FlightStatus.InFlight.ToString(), FlightStatus.Unknown.ToString());
                    var status = Console.ReadLine();
                    status.WhileForEightString(FlightStatus.Arrived.ToString(), FlightStatus.Canceled.ToString(), FlightStatus.CheckIn.ToString(), FlightStatus.Delayed.ToString(), 
                        FlightStatus.DepartedAt.ToString(), FlightStatus.ExpectedAt.ToString(), FlightStatus.GateClosed.ToString(), FlightStatus.InFlight.ToString(), 
                        FlightStatus.Unknown.ToString());           
                    foreach (var itemValue in Enum.GetValues(typeof(FlightStatus)))
                    {
                        if (status == itemValue.ToString())
                        {
                            const string sqlExpressionForFlightStatus = "UPDATE Flights SET FlightStatus = @FlightStatus WHERE FlightNumber = @FlightNumber";
                            sqlExpressionForFlightStatus.SqlRequestForEditCases("FlightNumber", flightNumber, "FlightStatus", itemValue.ToString());
                            break;
                        }
                    }                            
                    break;
            }
            Console.ReadKey();
            startPage.MainMenu();
        }        

        public void Details()
        {
            const string sqlExpression = "SELECT * FROM Flights";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var flightNumber = reader.GetValue(1);
                        var cityOfDeparture = reader.GetValue(2);
                        var cityOfArrival = reader.GetValue(3);
                        var departureDate = reader.GetValue(4);
                        var arrivalDate = reader.GetValue(5);
                        var terminal = reader.GetValue(6);
                        var gate = reader.GetValue(7);
                        var flightStatus = reader.GetValue(8);                        
                        
                        Console.WriteLine("Flight number: " + flightNumber + "\nCity of departure: " + cityOfDeparture + "\nCity of arrival: " + cityOfArrival
                            + "\nDeparture date: " + departureDate + "\nArrival date: "+ arrivalDate + "\nTerminal: " + terminal + "\nGate: " + gate + "\nFlight status: " + flightStatus + "\n");
                    }
                }
                reader.Close();
            }
        }       

        private void SelectNextStep()
        {
            Console.WriteLine("Add ones, press - 1\t\tReturn to main menu, press  - 2");                                  
            int item;
            var result = int.TryParse(Console.ReadLine(), out item);
            while (!(result && (item == 1 || item == 2)))
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");               
                result = int.TryParse(Console.ReadLine(), out item);
            }         
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

        private decimal CheckInputDecimal()
        {                     
            decimal item;
            var result = decimal.TryParse(Console.ReadLine(), out item);
            while (!result)
            {
                Console.WriteLine("Incorrectly entered data (string, symbols are invalid). Try again");
                result = decimal.TryParse(Console.ReadLine(), out item);
            }
            return item;
        }
    }
}
