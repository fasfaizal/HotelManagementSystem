using HotelManagementSystem.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelManagementSystem.Common.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public BedType BedType { get; set; }

    }
}
