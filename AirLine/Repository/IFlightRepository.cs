
namespace AirLine.Repository
{
    interface IFlightRepository
    {
        void Add();

        void Delete(int flightNumber);

        void Edit(int flightNumber);

        void Details();
    }
}
