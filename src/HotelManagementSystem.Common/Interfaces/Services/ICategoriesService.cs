using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Models.Request;

namespace HotelManagementSystem.Common.Interfaces.Services
{
    public interface ICategoriesService
    {
        Task<Category> CreateAsync(CategoryRequest categoryRequest);
        Task<Category> GetByIdAsync(string id);
        Task DeleteAsync(string id);
        Task<List<Category>> GetCategoriesAsync(int pageNumber, int pageSize);
    }
}
