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
    }
}
