using HotelManagementSystem.Common.Entities;

namespace HotelManagementSystem.Common.Interfaces.DataAccess
{
    public interface IBookingsRepo
    {
        Task CreateAsync(Booking booking);
        Task<List<Booking>> GetBookingsForRange(string categoryId, DateTime startDate, DateTime endDate);
    }
}
