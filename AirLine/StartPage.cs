using System;
using AirLine.BusinessLogic;
using AirLine.Repository;

namespace AirLine
{
    class StartPage
    {
        public void MainMenu()
        {
            var flightManager = new FlightManager();
            var passengerManager = new PassengerManager();
            IFlightRepository repository = new FlightRepository();
            var priceList = new PriceList();
           
            repository.Details();
            Console.WriteLine("Passenger menu, press - 1\t\tFlight menu, press - 2\t\tPricelist, press - 3");  
            
            var result = int.Parse(Console.ReadLine());
            while (!(result == 1 || result == 2 || result == 3))
            {
                Console.WriteLine("Input data incorrect, try again!");
                result = int.Parse(Console.ReadLine());
            }
            switch (result)
            {
                case 1:
                    passengerManager.Menu();
                    break;
                case 2:                  
                    flightManager.Menu();
                    break;
                case 3:
                    priceList.Details();
                    break;
            }
        }
    }
}
