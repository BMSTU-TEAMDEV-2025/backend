using Database;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo;

public class MongoRepository<TV>(IMongoCollection<TV> collection) : IRepository<string, TV> where TV : IModel<string>
{
    public void Put(TV model)
    {
        collection.InsertOne(model);
    }

    public void Put(IEnumerable<TV> models)
    {
        collection.InsertMany(models);
    }

    public void Update(TV model)
    {
        collection.ReplaceOne(m => m.Id == model.Id, model);
    }

    public TV? Find(string key)
    {
        if (!ObjectId.TryParse(key, out var id))
        {
            return default;
        }
        var filter = Builders<TV>.Filter.Eq("_id", id);
        return FindFirst(filter);
    }

    public IEnumerable<TV> FindAll(IEnumerable<string> keys)
    {
        return collection.Find(Combine(keys)).ToCursor().ToEnumerable();
    }

    public bool Delete(string key)
    {
        if (!ObjectId.TryParse(key, out var id))
        {
            return false;
        }
        return collection.DeleteOne(Builders<TV>.Filter.Eq("_id", id)).DeletedCount == 1;
    }

    public bool Contains(string key)
    {
        if (!ObjectId.TryParse(key, out var id))
        {
            return false;
        }
        return Count(Builders<TV>.Filter.Eq("_id", id)) == 1;
    }

    public bool Contains(IEnumerable<string> keys)
    {
        var count = keys.Count();
        if (count == 0) return true;
        return Count(Combine(keys)) == count;
    }

    public long DeleteAll(IEnumerable<string> keys)
    {
        return collection.DeleteMany(Combine(keys)).DeletedCount;
    }

    private static FilterDefinition<TV> Combine(IEnumerable<string> keys)
    {
        FilterDefinition<TV>? ret = null;
        foreach (var key in keys)
        {
            if (!ObjectId.TryParse(key, out var id))
            {
                continue;
            }
            var current = Builders<TV>.Filter.Eq("_id", id);
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
