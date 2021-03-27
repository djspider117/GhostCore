using System;
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public class ServiceDefinition : IServiceDefinition
    {
        public Type RegisteredServiceType { get; set; }
        public ServiceScope Scope { get; set; }
        public object ConcreteInstance { get; set; }
        public Func<IServiceCollection, object> SyncFactoryMethod { get; set; }
        public Func<IServiceCollection, Task<object>> AsyncFactoryMethod { get; set; }

        public object GetValue(IServiceCollection svcCol)
        {
            switch (Scope)
            {
                case ServiceScope.Transient:
                    if (SyncFactoryMethod == null)
                        throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(SyncFactoryMethod)} defined. Did you register it as async?");

                    return SyncFactoryMethod(svcCol);
                case ServiceScope.LazySingleton:
                    if (ConcreteInstance == null)
                    {
                        if (SyncFactoryMethod == null)
                            throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(SyncFactoryMethod)} defined. Did you register it as async?");

                        ConcreteInstance = SyncFactoryMethod(svcCol);
                    }

                    return ConcreteInstance;
                case ServiceScope.Singleton:
                    return ConcreteInstance;
                default:
                    return null;
            }
        }
        public async Task<object> GetValueAsync(IServiceCollection svcCol)
        {
            switch (Scope)
            {
                case ServiceScope.Transient:
                    if (AsyncFactoryMethod == null)
                        throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(AsyncFactoryMethod)} defined. Did you register it as normal (sync)?");

                    return await AsyncFactoryMethod(svcCol);
                case ServiceScope.LazySingleton:
                    if (ConcreteInstance == null)
                    {
                        if (AsyncFactoryMethod == null)
                            throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(AsyncFactoryMethod)} defined. Did you register it as normal (sync)?");

                        ConcreteInstance = await AsyncFactoryMethod(svcCol);
                    }

                    return ConcreteInstance;
                case ServiceScope.Singleton:
                    return ConcreteInstance;
                default:
                    return null;
            }
        }
    }
}
