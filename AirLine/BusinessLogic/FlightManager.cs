using System;
using AirLine.Repository;


namespace AirLine.BusinessLogic
{
    class FlightManager
    {
        IFlightRepository flightRepository = new FlightRepository();

        public void Menu()
        {
            Console.WriteLine("\nShow information about all flights, press - 1\n");
            Console.WriteLine("Add new flight, press - 2\n");
            Console.WriteLine("Edit information about flight, press - 3\n");
            Console.WriteLine("Delete information about flight, press - 4\n");

            var result = int.Parse(Console.ReadLine());
            while (!(result == 1 || result == 2 || result == 3 || result == 4))
            {
                Console.WriteLine("Input data incorrect, try again!");
                result = int.Parse(Console.ReadLine());
            }
            switch (result)
            {
                case 1:
                    flightRepository.Details();
                    break;
                case 2:
                    flightRepository.Add();
                    break;
                case 3:
                    Console.WriteLine("Please enter flight number you want edit");
                    var passportSeriesForEdit = CheckInputedNumbers();
                    flightRepository.Edit(passportSeriesForEdit);
                    break;
                case 4:
                    Console.WriteLine("Please enter flight number you want delete");
                    var passportSeriesForDelete = CheckInputedNumbers();
                    flightRepository.Delete(passportSeriesForDelete);
                    break;               
            }
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
    }
}
