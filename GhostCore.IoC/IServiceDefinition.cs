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

        void SetMockProvider(Type mockProvider);
        void SetMockObject(object obj);

        object GetValue(IServiceCollection svcCol, bool mock = false);
        Task<object> GetValueAsync(IServiceCollection svcCol, bool mock = false);
    }
}