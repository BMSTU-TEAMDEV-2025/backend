namespace Database;

public class ModelAttribute(string collection) : Attribute
{
    public ModelAttribute() : this("")
    {
    }

    public string Collection { get; } = collection;
}