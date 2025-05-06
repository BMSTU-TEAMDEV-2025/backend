using MongoDB.Driver;

namespace Database;

public interface IRepository<in TK, TV> where TV : IModel<TK>
{
    void Put(TV model);

    void Put(IEnumerable<TV> entities);

    void Update(TV model);

    TV? Find(TK key);

    // public IEnumerable<TV> Filter(FilterDefinition<TV> filter);

    IEnumerable<TV> FindAll(IEnumerable<TK> keys);

    bool Delete(TK key);

    bool Contains(TK key);

    bool Contains(IEnumerable<TK> keys);

    long DeleteAll(IEnumerable<TK> keys);
}
