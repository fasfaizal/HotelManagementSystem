using HotelManagementSystem.Common.Entities;

namespace HotelManagementSystem.Common.Interfaces.DataAccess
{
    public interface IRoomsRepo
    {
        Task CreateAsync(Room room);
        Task DeleteAsync(string id);
        Task DeleteByCategoryAsync(string categoryId);
        Task<List<Room>> GetByCategoryAsync(string categoryId);
    }
}
