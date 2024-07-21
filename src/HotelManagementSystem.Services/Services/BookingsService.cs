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

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="bookingRequest">The request object containing the booking details.</param>
        /// <exception cref="ArgumentNullException">Thrown when the bookingRequest is null.</exception>
        /// <exception cref="ApiException">
        /// Thrown when the category ID in the bookingRequest is invalid,
        /// the number of guests exceeds the category's capacity,
        /// or no rooms are available in the specified date range.
        /// </exception>
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

        /// <summary>
        /// Retrieves a list of available rooms in a specific category within a given date range asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the category to check for available rooms.</param>
        /// <param name="startDate">The start date of the availability check.</param>
        /// <param name="endDate">The end date of the availability check.</param>
        /// <returns>
        /// The task result contains a list of available rooms.
        /// </returns>
        /// <exception cref="ApiException">
        /// Thrown when no rooms are added in the specified category,
        /// or no rooms are available in the specified date range.
        /// </exception>
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

        /// <summary>
        /// Checks if there are available rooms in a specific category within a given date range asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the category to check for availability.</param>
        /// <param name="startDate">The start date of the availability check.</param>
        /// <param name="endDate">The end date of the availability check.</param>
        /// <returns>
        /// The task result contains a boolean indicating the availability of rooms.
        /// </returns>
        public async Task<bool> IsAvailable(string categoryId, DateTime startDate, DateTime endDate)
        {
            var availableRooms = await GetAvailableRooms(categoryId, startDate, endDate);
            if (availableRooms == null || availableRooms.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
