using Database;

namespace Core.Models;

[Model("hellos")]
public class HelloModel : IModel<string>
{
    public string? Id { get; set; }
    
    public string Message { get; set; }
}
