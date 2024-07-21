using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.DataAccess.DbContext;
using MongoDB.Driver;

namespace HotelManagementSystem.DataAccess.Repositories
{
    public class BookingsRepo : IBookingsRepo
    {
        private readonly IMongoDbContext _dbContext;

        public BookingsRepo(IMongoDbContext mongoDbContext)
        {
            _dbContext = mongoDbContext;
        }

        public async Task CreateAsync(Booking booking)
        {
            await _dbContext.Bookings.InsertOneAsync(booking);
        }

        public async Task<List<Booking>> GetBookingsForRange(string categoryId, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(r => r.CategoryId, categoryId),
                Builders<Booking>.Filter.Or(
                    Builders<Booking>.Filter.And(
                        Builders<Booking>.Filter.Gte(r => r.StartDate, startDate),
                        Builders<Booking>.Filter.Lte(r => r.StartDate, endDate)
                    ),
                    Builders<Booking>.Filter.And(
                        Builders<Booking>.Filter.Gte(r => r.EndDate, startDate),
                        Builders<Booking>.Filter.Lte(r => r.EndDate, endDate)
                    )
                )
            );

            return await _dbContext.Bookings.Find(filter).ToListAsync();
        }
    }
}
