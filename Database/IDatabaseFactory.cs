using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public interface IDatabaseFactory<TK>
{
    IDatabase<TK> Create(string name, IDictionary<string, Type> types);

    IDatabase<TK> CreateAndRegisterTypes(string name, Assembly assembly, IServiceCollection services);
}
