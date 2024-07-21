using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Exceptions;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;

namespace HotelManagementSystem.Services.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepo _categoriesRepo;
        private IRoomsRepo _roomsRepo;

        public CategoriesService(ICategoriesRepo categoriesRepo, IRoomsRepo roomsRepo)
        {
            _categoriesRepo = categoriesRepo;
            _roomsRepo = roomsRepo;
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


        /// <summary>
        /// Deletes a category asynchronously from the repository based on its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteAsync(string id)
        {
            // Delete rooms mapped to a category
            await _roomsRepo.DeleteByCategoryAsync(id);
            // Delete category
            await _categoriesRepo.DeleteAsync(id);
        }


        /// <summary>
        /// Retrieves a paginated list of categories asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Must be a positive integer.</param>
        /// <param name="pageSize">The number of categories per page. Must be a positive integer.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of categories.
        /// </returns>
        /// <exception cref="ApiException">
        /// Thrown when the pageNumber or pageSize is less than 1.
        /// </exception>
        public async Task<List<Category>> GetCategoriesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Parameters should be positive");
            }
            return await _categoriesRepo.GetCategoriesAsync(pageNumber, pageSize);
        }
    }
}
