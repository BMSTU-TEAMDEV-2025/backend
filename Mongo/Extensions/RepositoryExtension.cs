using Database;
using MongoDB.Driver;

namespace Mongo.Extensions;

public static class RepositoryExtension
{
    public static IEnumerable<TV> Filter<TK, TV>(this IRepository<TK, TV> adapter, FilterDefinition<TV> filter)
    {
        return ((MongoRepository<TV>) adapter).Filter(filter);
    }

    public static TV? FindFirst<TK, TV>(this IRepository<TK, TV> adapter, FilterDefinition<TV> filter)
    {
        return ((MongoRepository<TV>) adapter).FindFirst(filter);
    }

    public static long Count<TK, TV>(this IRepository<TK, TV> adapter, FilterDefinition<TV> filter)
    {
        return ((MongoRepository<TV>) adapter).Count(filter);
    }

    public static bool DeleteFirst<TK, TV>(this IRepository<TK, TV> adapter, FilterDefinition<TV> filter)
    {
        return ((MongoRepository<TV>) adapter).DeleteFirst(filter);
    }

    public static long DeleteAll<TK, TV>(this IRepository<TK, TV> adapter, FilterDefinition<TV> filter)
    {
        return ((MongoRepository<TV>) adapter).DeleteAll(filter);
    }
}
