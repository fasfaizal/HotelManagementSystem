using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.DataAccess.DbContext;
using MongoDB.Driver;

namespace HotelManagementSystem.DataAccess.Repositories
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private readonly IMongoDbContext _dbContext;

        public CategoriesRepo(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        /// <summary>
        /// Inserts a new category into the database asynchronously.
        /// </summary>
        /// <param name="category">The category object to be inserted into the database.</param>
        public async Task CreateAsync(Category category)
        {
            await _dbContext.Categories.InsertOneAsync(category);
        }


        /// <summary>
        /// Retrieves a category based on its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the category object
        /// matching the provided ID, or null if no category is found.
        /// </returns>
        public async Task<Category> GetByIdAsync(string id)
        {
            return await _dbContext.Categories.Find(options => options.Id == id).FirstOrDefaultAsync();
        }
    }
}
