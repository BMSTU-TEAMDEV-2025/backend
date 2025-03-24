using Core.Models;
using Core.Services;
using Database;

namespace Services;

public class HelloService : IHelloService
{
    private readonly IRepository<string, HelloModel> _repo;

    public HelloService(IRepository<string, HelloModel> repo)
    {
        _repo = repo;
    }
    
    public HelloModel? GetHello(string id)
    {
        return _repo.Find(id);
    }

    public string PutHello(HelloModel model)
    {
        _repo.Put(model);
        return model.Id!;
    }
}
