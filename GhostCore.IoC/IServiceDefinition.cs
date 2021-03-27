using System;
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public interface IServiceDefinition
    {
        Func<IServiceCollection, Task<object>> AsyncFactoryMethod { get; }
        object ConcreteInstance { get; }
        Type RegisteredServiceType { get; }
        ServiceScope Scope { get; }
        Func<IServiceCollection, object> SyncFactoryMethod { get; }

        object GetValue(IServiceCollection svcCol);
        Task<object> GetValueAsync(IServiceCollection svcCol);
    }
}