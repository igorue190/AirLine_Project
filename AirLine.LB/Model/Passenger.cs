using System;
using System.Data.SqlTypes;
using AirLine.LB.Enum;

namespace AirLine.LB.Model
{
   public class Passenger
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Nationality { get; set; }

        public int PassportSeries { get; set; }

        public DateTime DateOfBith { get; set; }

        public Sex Sex { get; set; }

        public ClassesOfService ClassesOfService { get; set; }

        public int FlightNumber { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return "Name: " + Name + "\nSurname: " + Surname + "\nNationality: " + Nationality + "\nPassport serias: " + PassportSeries + "\nDate of birth: " +
                 DateOfBith + "\nSex: " + Sex + "\nClass: " + ClassesOfService + "\nFlights number: " + FlightNumber + "\nPrice: " + Price + "$\n";
        }
    }
}
