using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EveryBus.Models
{
    public class Services
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name;
        public string description;
        public string service_type;

        public Route[] routes;
    }
}