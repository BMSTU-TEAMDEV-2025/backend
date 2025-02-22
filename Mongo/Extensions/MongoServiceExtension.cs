using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Mongo.Configuration;

namespace Mongo.Extensions;

public static class MongoServiceExtension
{
    private const string MongoSectionKey = "Mongo";

    public static void AddMongoService(this IServiceCollection services, IConfiguration configuration, Assembly e)
    {
        var config = configuration.GetSection(MongoSectionKey).Get<MongoConfig>();
        if (config == null) throw new NullReferenceException("Cannot find mongo config");

        var settings = MongoClientSettings.FromConnectionString(config.Url);
        settings.ApplicationName = config.ApplicationName;
        settings.ConnectTimeout = config.ConnectTimeout;
        settings.SocketTimeout = config.SocketTimeout;
        settings.ServerSelectionTimeout = config.ServerSelectionTimeout;
        var client = new MongoClient(settings);
        var factory = new MongoFactory(client);
        var database = factory.CreateAndRegisterTypes(config.Database, e, services);
        services.AddSingleton(database);
    }
}
