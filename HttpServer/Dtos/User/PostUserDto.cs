using Core.Models;
using Database;

namespace Backend.Dtos;

public class PostUserDto
{
    public string Email { get; set; }
    
    public string Password { get; set; }

    public static GetUserDto Of(User user)
    {
        return new GetUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password
        };
    }
}