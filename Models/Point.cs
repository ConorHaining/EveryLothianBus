using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EveryBus.Models
{
    public class Point
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int stop_id;
        public long latitude;
        public long longitude;
    }
}