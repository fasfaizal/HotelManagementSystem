using HotelManagementSystem.Common.Configs;
using HotelManagementSystem.Common.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotelManagementSystem.DataAccess.DbContext
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoCollection<Category> Categories { get; set; }
        public IMongoCollection<Room> Rooms { get; set; }

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Categories = database.GetCollection<Category>("Categories");
            Rooms = database.GetCollection<Room>("Rooms");
        }
    }
}
