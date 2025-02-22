using Database;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo;

public class MongoRepository<TV>(IMongoCollection<TV> collection) : IRepository<ObjectId, TV>
{
    public void Put(TV model)
    {
        collection.InsertOne(model);
    }

    public void Put(IEnumerable<TV> entities)
    {
        collection.InsertMany(entities);
    }

    public void Update(ObjectId key, TV model)
    {
        collection.ReplaceOne(Builders<TV>.Filter.Eq("_id", key), model);
    }

    public TV? Find(ObjectId key)
    {
        var filter = Builders<TV>.Filter.Eq("_id", key);
        return FindFirst(filter);
    }

    public IEnumerable<TV> FindAll(IEnumerable<ObjectId> keys)
    {
        return collection.Find(Combine(keys)).ToCursor().ToEnumerable();
    }

    public bool Delete(ObjectId key)
    {
        return collection.DeleteOne(Builders<TV>.Filter.Eq("_id", key)).DeletedCount == 1;
    }

    public bool Contains(ObjectId key)
    {
        return Count(Builders<TV>.Filter.Eq("_id", key)) == 1;
    }

    public bool Contains(IEnumerable<ObjectId> keys)
    {
        var count = keys.Count();
        if (count == 0) return true;
        return Count(Combine(keys)) == count;
    }

    public long DeleteAll(IEnumerable<ObjectId> keys)
    {
        return collection.DeleteMany(Combine(keys)).DeletedCount;
    }

    private static FilterDefinition<TV> Combine(IEnumerable<ObjectId> keys)
    {
        FilterDefinition<TV>? ret = null;
        foreach (var key in keys)
        {
            var current = Builders<TV>.Filter.Eq("_id", key);
            ret = ret == null ? current : Builders<TV>.Filter.Or(ret, current);
        }

        return ret ?? Builders<TV>.Filter.Empty;
    }

    public IEnumerable<TV> Filter(FilterDefinition<TV> filter)
    {
        return collection.Find(filter).ToCursor().ToEnumerable();
    }

    public TV? FindFirst(FilterDefinition<TV> filter)
    {
        using var found = Filter(filter).GetEnumerator();
        return !found.MoveNext() ? default : found.Current;
    }

    public long Count(FilterDefinition<TV> filter)
    {
        return collection.CountDocuments(filter);
    }

    public bool DeleteFirst(FilterDefinition<TV> filter)
    {
        return collection.DeleteOne(filter).DeletedCount == 1;
    }

    public long DeleteAll(FilterDefinition<TV> filter)
    {
        return collection.DeleteMany(filter).DeletedCount;
    }
}
