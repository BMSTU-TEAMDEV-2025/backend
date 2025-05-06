using Database;

namespace Core.Models;

[Model("sessions")]
public class Session : IModel<string>
{
    public string? Id { get; set; }
    
    public string? Owner { get; set; }
    
    public bool Revealed { get; set; }
    
    public ISet<int> Points { get; set; }
    
    public IDictionary<string, int> Votes { get; set; }
}
