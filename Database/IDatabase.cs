namespace Database;

public interface IDatabase<TK>
{
    IRepository<TK, TV> Create<TV>(string name) where TV : IModel<TK>;
}