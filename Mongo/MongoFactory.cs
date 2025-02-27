using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Database;
using Microsoft.Extensions.DependencyInjection;

namespace Mongo;

public class MongoFactory(IMongoClient client) : AbstractDatabaseFactory<ObjectId>
{
    public override IDatabase<ObjectId> Create(string name, IDictionary<string, Type> types)
    {
        var database = client.GetDatabase(name);
        // Ping database
        database.RunCommand((Command<BsonDocument>) "{ping:1}");
        var ret = new MongoDatabase(database);
        foreach (var type in types.Values)
        {
            var map = new BsonClassMap(type);
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            BsonClassMap.RegisterClassMap(map);
        }
        return ret;
    }

    protected override void Register<TKey, TValue>(IDatabase<TKey> database, string name, IServiceCollection services)
    {
        var repository = database.Create<TValue>(name);
        services.AddSingleton(repository);
    }
}
