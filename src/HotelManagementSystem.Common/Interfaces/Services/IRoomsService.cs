using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Models.Request;

namespace HotelManagementSystem.Common.Interfaces.Services
{
    public interface IRoomsService
    {
        Task<Room> CreateAsync(RoomRequest roomRequest);
    }
}
