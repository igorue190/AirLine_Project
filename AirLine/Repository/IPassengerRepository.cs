
namespace AirLine
{
    interface IPassengerRepository
    {
        void Add();

        void Delete(int passportSeries);

        void Edit(int passportSeries);

        void Details();       
    }
}
