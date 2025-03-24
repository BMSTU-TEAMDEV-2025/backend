using Core.Models;

namespace Core.Services;

public interface IHelloService
{
    public HelloModel? GetHello(string id);
    
    public string PutHello(HelloModel model);
}
