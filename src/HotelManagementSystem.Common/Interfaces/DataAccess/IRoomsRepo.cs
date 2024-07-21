using HotelManagementSystem.Common.Entities;

namespace HotelManagementSystem.Common.Interfaces.DataAccess
{
    public interface IRoomsRepo
    {
        Task CreateAsync(Room room);
    }
}
