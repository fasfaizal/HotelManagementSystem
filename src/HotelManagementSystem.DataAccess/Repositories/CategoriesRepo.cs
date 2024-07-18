using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.DataAccess.DbContext;

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
    }
}
