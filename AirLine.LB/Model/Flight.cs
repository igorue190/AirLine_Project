using System;
using AirLine.LB.Enum;

namespace AirLine.LB.Model
{
    public class Flight
    {
        public int FlightNumber { get; set; }

        public string CityOfDeparture { get; set; }

        public string CityOfArrival { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public Terminal Terminal { get; set; }

        public Terminal Gate { get; set; }

        public FlightStatus FlightStatus { get; set; }

        public decimal PriceForBusinessClass { get; set; }

        public decimal PriceForEconomeClass { get; set; }

        public override string ToString()
        {
            return "Flight number: " + FlightNumber + "\nCity of departure: " + CityOfDeparture + "\nCity of arrival: " + CityOfArrival + "\nDeparture date: " + DepartureDate + "\nArrival date: " +
                 ArrivalDate + "\nTerminal: " + Terminal + "\nGate: " + Gate + "\nFlight status: " + FlightStatus + "\n";
        }
    }
}
