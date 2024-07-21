using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;

        public BookingsController(IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="bookingRequest">The request object containing the booking details.</param>
        /// <returns>
        /// A 200 OK response if the booking is successfully created.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post(BookingRequest bookingRequest)
        {
            await _bookingsService.CreateAsync(bookingRequest);
            return Ok();
        }

        /// <summary>
        /// Checks the availability of rooms in a specific category within a given date range.
        /// </summary>
        /// <param name="categoryId">The ID of the category to check availability for.</param>
        /// <param name="startDate">The start date of the availability check. Defaults to today if not provided.</param>
        /// <param name="endDate">The end date of the availability check. Defaults to today if not provided.</param>
        /// <returns>
        /// A 200 OK response with the availability status.
        /// </returns>
        [HttpGet("availability/category/{categoryId}")]
        public async Task<IActionResult> GetAvailability(string categoryId, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
            {
                startDate = DateTime.Today;
            }
            if (endDate == null)
            {
                endDate = DateTime.Today;
            }
            var isAvailable = await _bookingsService.IsAvailable(categoryId, startDate.Value, endDate.Value);
            return Ok(isAvailable);
        }
    }
}
