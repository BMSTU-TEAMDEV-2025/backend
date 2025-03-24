namespace Database;

public interface IModel<TK>
{
    public TK? Id {get; set;}
}
