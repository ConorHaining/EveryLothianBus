using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EveryBus.Models
{
    public class Route
    {   
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string destination;
        public Point[] points;
        public int[] stops;
    }
}