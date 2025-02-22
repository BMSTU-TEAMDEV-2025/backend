using Database;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo;

public class MongoDatabase(IMongoDatabase database) : IDatabase<ObjectId>
{
    public IRepository<ObjectId, TV> Create<TV>(string name)
    {
        var collection = database.GetCollection<TV>(name);
        return new MongoRepository<TV>(collection);
    }
}
