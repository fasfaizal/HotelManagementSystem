using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomsService;

        public RoomsController(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        /// <summary>
        /// Creates a new room.
        /// </summary>
        /// <param name="roomRequest">The request object containing the room details.</param>
        /// <returns>
        /// A 200 OK response with the created room object.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post(RoomRequest roomRequest)
        {
            var room = await _roomsService.CreateAsync(roomRequest);
            return Ok(room);
        }

        /// <summary>
        /// Deletes a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <returns>
        /// A 200 OK response if the deletion is successful.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _roomsService.DeleteAsync(id);
            return Ok();
        }
    }
}
