using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpacModels.Shared.Models.Core;

public class Error
{ 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Code { get; set; }
        public string Section { get; set; }
        public string Message { get; set; }
}