using Database;

namespace Core.Models;


[Model("users")]
public class User : IModel<string>
{
    public string Id { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}