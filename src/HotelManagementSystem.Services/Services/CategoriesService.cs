using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;

namespace HotelManagementSystem.Services.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepo _categoriesRepo;

        public CategoriesService(ICategoriesRepo categoriesRepo)
        {
            _categoriesRepo = categoriesRepo;
        }

        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="categoryRequest">The request object containing the category details.</param>
        /// <returns>
        /// The created category object.
        /// </returns>
        public async Task<Category> CreateAsync(CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
            {
                throw new ArgumentNullException(nameof(categoryRequest));
            }

            // Create new category and save
            var category = new Category
            {
                BedType = categoryRequest.BedType,
                Capacity = categoryRequest.Capacity,
                CategoryName = categoryRequest.CategoryName,
                Description = categoryRequest.Description,
                PricePerNight = categoryRequest.PricePerNight
            };
            await _categoriesRepo.CreateAsync(category);
            return category;
        }
    }
}
