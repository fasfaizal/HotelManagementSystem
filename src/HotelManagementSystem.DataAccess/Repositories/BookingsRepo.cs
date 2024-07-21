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

        /// <summary>
        /// Inserts a new booking into the database asynchronously.
        /// </summary>
        /// <param name="booking">The booking object to be inserted into the database.</param>
        public async Task CreateAsync(Booking booking)
        {
            await _dbContext.Bookings.InsertOneAsync(booking);
        }

        /// <summary>
        /// Retrieves a list of bookings for a specific category within a given date range asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve bookings for.</param>
        /// <param name="startDate">The start date of the range for which to retrieve bookings.</param>
        /// <param name="endDate">The end date of the range for which to retrieve bookings.</param>
        /// <returns>
        /// The task result contains a list of bookings that match the specified criteria.
        /// </returns>
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
