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

        [HttpPost]
        public async Task<IActionResult> Post(BookingRequest bookingRequest)
        {
            await _bookingsService.CreateAsync(bookingRequest);
            return Ok();
        }

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
            return Ok(new { isAvailable });
        }
    }
}
