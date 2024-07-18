using HotelManagementSystem.Common.Entities;

namespace HotelManagementSystem.Common.Interfaces.DataAccess
{
    public interface ICategoriesRepo
    {
        Task CreateAsync(Category category);
        Task<Category> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}
