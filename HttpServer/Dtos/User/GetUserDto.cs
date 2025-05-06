namespace Backend.Dtos.User;

public class GetUserDto
{
    public string Id { get; set; }
    
    public string Email { get; set; }

    public static GetUserDto Of(Core.Models.User user)
    {
        return new GetUserDto
        {
            Id = user.Id,
            Email = user.Email,
        };
    }
}