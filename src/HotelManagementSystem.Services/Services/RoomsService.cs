using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Exceptions;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using System.Net;

namespace HotelManagementSystem.Services.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IRoomsRepo _roomsRepo;
        private readonly ICategoriesRepo _categoriesRepo;

        public RoomsService(IRoomsRepo roomsRepo, ICategoriesRepo categories)
        {
            _roomsRepo = roomsRepo;
            _categoriesRepo = categories;
        }

        /// <summary>
        /// Creates a new room asynchronously.
        /// </summary>
        /// <param name="roomRequest">The request object containing the room details.</param>
        /// <returns>
        /// The task result contains the created room object.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the roomRequest is null.</exception>
        /// <exception cref="ApiException">Thrown when the category ID in the roomRequest is invalid.</exception>
        public async Task<Room> CreateAsync(RoomRequest roomRequest)
        {
            if (roomRequest == null)
            {
                throw new ArgumentNullException(nameof(roomRequest));
            }

            // Category validation
            var category = await _categoriesRepo.GetByIdAsync(roomRequest.CategoryId);
            if (category == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid category");
            }

            // Create a new room
            var room = new Room
            {
                CategoryId = roomRequest.CategoryId,
                RoomName = roomRequest.RoomName,
                SizeInSquareFeet = roomRequest.SizeInSquareFeet,
                FloorNumber = roomRequest.FloorNumber
            };
            await _roomsRepo.CreateAsync(room);
            return room;
        }
    }
}
