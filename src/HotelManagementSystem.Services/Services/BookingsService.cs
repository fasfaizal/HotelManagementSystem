using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Exceptions;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using System.Net;

namespace HotelManagementSystem.Services.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IBookingsRepo _bookingsRepo;
        private readonly ICategoriesRepo _categoriesRepo;
        private readonly IRoomsRepo _roomsRepo;

        public BookingsService(IBookingsRepo bookingsRepo, ICategoriesRepo categoriesRepo, IRoomsRepo roomsRepo)
        {
            _bookingsRepo = bookingsRepo;
            _categoriesRepo = categoriesRepo;
            _roomsRepo = roomsRepo;
        }

        public async Task CreateAsync(BookingRequest bookingRequest)
        {
            if (bookingRequest == null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            // Validate the category
            var category = await _categoriesRepo.GetByIdAsync(bookingRequest.CategoryId);
            if (category == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid category");
            }

            // Validate the number of guests
            if (bookingRequest.NoOfGuests > category.Capacity)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Allowed guests :{category.Capacity}");
            }

            // Check the availability
            var availableRooms = await GetAvailableRooms(bookingRequest.CategoryId, bookingRequest.StartDate, bookingRequest.EndDate);
            if (availableRooms == null || availableRooms.Count == 0)
            {
                throw new ApiException(HttpStatusCode.UnprocessableEntity, "No rooms available at this time");
            }

            // Create new booking
            var booking = new Booking
            {
                CategoryId = category.Id,
                StartDate = bookingRequest.StartDate,
                EndDate = bookingRequest.EndDate,
                RoomId = availableRooms[0].Id,
                NoOfGuests = bookingRequest.NoOfGuests
            };
            await _bookingsRepo.CreateAsync(booking);
        }

        private async Task<List<Room>> GetAvailableRooms(string categoryId, DateTime startDate, DateTime endDate)
        {
            var rooms = await _roomsRepo.GetByCategoryAsync(categoryId);
            // Check if rooms are added in this category
            if (rooms == null || rooms.Count == 0)
            {
                throw new ApiException(HttpStatusCode.UnprocessableEntity, "No rooms added for the category");
            }

            var bookings = await _bookingsRepo.GetBookingsForRange(categoryId, startDate, endDate);
            if (bookings == null || bookings.Count == 0)
            {
                return rooms;
            }

            var bookedRoomIds = bookings.Select(b => b.RoomId).ToList();
            return rooms.Where(r => !bookedRoomIds.Contains(r.Id)).ToList();
        }
    }
}
