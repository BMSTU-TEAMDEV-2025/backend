namespace Database;

public interface IDatabase<in TK>
{
    IRepository<TK, TV> Create<TV>(string name);
}