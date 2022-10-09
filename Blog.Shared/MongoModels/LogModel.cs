using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blog.Shared.MongoModels
{
    public class LogModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string IpAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BsonDateTime CreatedAt { get; set; }
    }
}
