using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.DataAccess.DbContext;
using MongoDB.Driver;

namespace HotelManagementSystem.DataAccess.Repositories
{
    public class RoomsRepo : IRoomsRepo
    {
        private readonly IMongoDbContext _dbContext;
        public RoomsRepo(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Inserts a new room into the database asynchronously.
        /// </summary>
        /// <param name="room">The room object to be inserted into the database.</param>
        public async Task CreateAsync(Room room)
        {
            await _dbContext.Rooms.InsertOneAsync(room);
        }

        /// <summary>
        /// Deletes a room from the database asynchronously based on its ID.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        public async Task DeleteAsync(string id)
        {
            await _dbContext.Rooms.DeleteOneAsync(o => o.Id == id);
        }

        /// <summary>
        /// Deletes rooms from the database asynchronously based on their category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category whose rooms are to be deleted.</param>
        public async Task DeleteByCategoryAsync(string categoryId)
        {
            await _dbContext.Rooms.DeleteManyAsync(o => o.CategoryId == categoryId);
        }

        /// <summary>
        /// Retrieves a list of rooms from the database asynchronously based on their category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category whose rooms are to be retrieved.</param>
        /// <returns>
        /// The task result contains a list of rooms matching the provided category ID.
        /// </returns>
        public async Task<List<Room>> GetByCategoryAsync(string categoryId)
        {
            return await _dbContext.Rooms.Find(o => o.CategoryId == categoryId).ToListAsync();
        }
    }
}
