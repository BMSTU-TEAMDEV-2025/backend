using Database;
using MongoDB.Driver;

namespace Mongo;

public class MongoDatabase(IMongoDatabase database) : IDatabase<string>
{
    public IRepository<string, TV> Create<TV>(string name) where TV : IModel<string>
    {
        var collection = database.GetCollection<TV>(name);
        return new MongoRepository<TV>(collection);
    }
}
