using HotelManagementSystem.Common.Entities;
using MongoDB.Driver;

namespace HotelManagementSystem.DataAccess.DbContext
{
    public interface IMongoDbContext
    {
        public IMongoCollection<Category> Categories { get; set; }
        public IMongoCollection<Room> Rooms { get; set; }
    }
}
