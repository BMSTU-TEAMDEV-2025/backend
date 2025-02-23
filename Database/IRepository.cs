namespace Database;

public interface IRepository<in TK, TV>
{
    void Put(TV model);

    void Put(IEnumerable<TV> entities);

    void Update(TK key, TV model);

    TV? Find(TK key);

    IEnumerable<TV> FindAll(IEnumerable<TK> keys);

    bool Delete(TK key);

    bool Contains(TK key);

    bool Contains(IEnumerable<TK> keys);

    long DeleteAll(IEnumerable<TK> keys);
}
