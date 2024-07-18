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

        /// <summary>
        /// Retrieves a category asynchronously from the repository based on its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the category object
        /// matching the provided ID, or null if no category is found.
        /// </returns>
        public async Task<Category> GetByIdAsync(string id)
        {
            return await _categoriesRepo.GetByIdAsync(id);
        }
    }
}
