using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EveryBus.Models
{
    public class VehicleLocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string vehicle_id { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public int last_gps_fix { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int? speed { get; set; }
        public int? heading { get; set; }
        public string service_name { get; set; }
        public string destination { get; set; }
        public string journey_id { get; set; }
        public string next_stop_id { get; set; }
        public string vehicle_type { get; set; }
    }
}