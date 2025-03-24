using Database;
using MongoDB.Driver;

namespace Mongo.Extensions;

public static class RepositoryExtension
{
    public static IEnumerable<TV> Filter<TV>(this IRepository<string, TV> adapter, FilterDefinition<TV> filter) where TV : IModel<string>
    {
        return ((MongoRepository<TV>) adapter).Filter(filter);
    }

    public static TV? FindFirst<TV>(this IRepository<string, TV> adapter, FilterDefinition<TV> filter) where TV : IModel<string>
    {
        return ((MongoRepository<TV>) adapter).FindFirst(filter);
    }

    public static long Count<TV>(this IRepository<string, TV> adapter, FilterDefinition<TV> filter) where TV : IModel<string>
    {
        return ((MongoRepository<TV>) adapter).Count(filter);
    }

    public static bool DeleteFirst<TV>(this IRepository<string, TV> adapter, FilterDefinition<TV> filter) where TV : IModel<string>
    {
        return ((MongoRepository<TV>) adapter).DeleteFirst(filter);
    }

    public static long DeleteAll<TV>(this IRepository<string, TV> adapter, FilterDefinition<TV> filter) where TV : IModel<string>
    {
        return ((MongoRepository<TV>) adapter).DeleteAll(filter);
    }
}
