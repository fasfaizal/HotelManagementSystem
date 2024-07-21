using HotelManagementSystem.Common.Models.Request;

namespace HotelManagementSystem.Common.Interfaces.Services
{
    public interface IBookingsService
    {
        Task CreateAsync(BookingRequest bookingRequest);
        Task<bool> IsAvailable(string categoryId, DateTime startDate, DateTime endDate);
    }
}
