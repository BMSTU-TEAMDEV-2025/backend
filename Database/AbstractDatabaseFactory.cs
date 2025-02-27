﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public abstract class AbstractDatabaseFactory<TK> : IDatabaseFactory<TK>
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;

    private readonly Type _type = typeof(AbstractDatabaseFactory<TK>);

    public abstract IDatabase<TK> Create(string name, IDictionary<string, Type> types);

    protected abstract void Register<TKey, TValue>(IDatabase<TKey> database, string name, IServiceCollection services);

    public IDatabase<TK> CreateAndRegisterTypes(string name, Assembly assembly, IServiceCollection services)
    {
        var types = new Dictionary<string, Type>();
        // Find all annotated models
        foreach (var type in assembly.GetTypes())
        {
            var found = type.GetCustomAttributes(typeof(ModelAttribute), false);
            if (found.Length == 0) continue;

            var collection = ((ModelAttribute) found[0]).Collection;
            types[collection] = type;
        }

        var ret = Create(name, types);
        foreach (var entry in types)
        {
            var method = _type.GetMethod("Register", Flags);
            var generic = method!.MakeGenericMethod(typeof(TK), entry.Value);
            generic.Invoke(this, [ret, entry.Key, services]);
        }

        return ret;
    }
}
