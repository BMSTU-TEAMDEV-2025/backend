using Core.Models;
using Core.Services;
using Database;
using MongoDB.Driver;

using Mongo.Extensions;

namespace Services;

public class UserService : IUserService
{
    private readonly IRepository<string, User> _repo;

    public UserService(IRepository<string, User> repo)
    {
        _repo = repo;
    }
    
    public User? GetUser(string id)
    {
        return _repo.Find(id);
    }

    public string PutUser(string email, string password)
    {
        var user = new User{
            Email = email,
            Password = password
        };
        _repo.Put(user);
        
        return user.Id!;
    }
    
    public User? FindUserByEmail(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
    
        return _repo.FindFirst(filter);
    }
}