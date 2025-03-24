using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Database;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Mongo;

public class MongoFactory(IMongoClient client) : AbstractDatabaseFactory<string>
{
    public override IDatabase<string> Create(string name, IDictionary<string, Type> types)
    {
        var database = client.GetDatabase(name);
        // Ping database
        database.RunCommand((Command<BsonDocument>) "{ping:1}");
        var ret = new MongoDatabase(database);
        foreach (var type in types.Values)
        {
            var map = new BsonClassMap(type);
            map.AutoMap();
            map.MapIdProperty("Id")
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            map.SetIgnoreExtraElements(true);
            BsonClassMap.RegisterClassMap(map);
        }
        return ret;
    }

    protected override void Register<TValue>(IDatabase<string> database, string name, IServiceCollection services)
    {
        var repository = database.Create<TValue>(name);
        services.AddSingleton(repository);
    }
}
