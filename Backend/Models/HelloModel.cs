using Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models;

[Model("hello")]
public class HelloModel
{
    [BsonId] public ObjectId Id { get; set; }
    
    public string Message { get; set; }
}
