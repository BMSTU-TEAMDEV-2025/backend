using Core.Models;

namespace Core.Services;

public interface IUserService
{
    User GetUser(string id);
    
    // List<User> GetUsersAsync();

    string PutUser(string email, string password);

    User? FindUserByEmail(string email);
}