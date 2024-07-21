using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.DataAccess.DbContext;

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
    }
}
