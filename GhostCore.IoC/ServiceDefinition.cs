using System;
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public class ServiceDefinition : IServiceDefinition
    {
        private Type _mockType;
        private object _mockObject;

        public Type RegisteredServiceType { get; set; }
        public ServiceScope Scope { get; set; }
        public object ConcreteInstance { get; set; }
        public Func<IServiceCollection, object> SyncFactoryMethod { get; set; }
        public Func<IServiceCollection, Task<object>> AsyncFactoryMethod { get; set; }

        public object GetValue(IServiceCollection svcCol, bool mock = false)
        {
            switch (Scope)
            {
                case ServiceScope.Transient:
                    if (SyncFactoryMethod == null)
                        throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(SyncFactoryMethod)} defined. Did you register it as async?");

                    if (mock)
                        return GetMock(svcCol);

                    return SyncFactoryMethod(svcCol);
                case ServiceScope.LazySingleton:
                    if (mock)
                    {
                        if (_mockObject == null)
                            _mockObject = GetMock(svcCol);

                        return _mockObject;
                    }

                    if (ConcreteInstance == null)
                    {
                        if (SyncFactoryMethod == null)
                            throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(SyncFactoryMethod)} defined. Did you register it as async?");

                        ConcreteInstance = SyncFactoryMethod(svcCol);
                    }

                    return ConcreteInstance;
                case ServiceScope.Singleton:
                    if (mock)
                        return _mockObject;

                    return ConcreteInstance;
                default:
                    return null;
            }
        }

        public async Task<object> GetValueAsync(IServiceCollection svcCol, bool mock = false)
        {
            switch (Scope)
            {
                case ServiceScope.Transient:
                    if (AsyncFactoryMethod == null)
                        throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(AsyncFactoryMethod)} defined. Did you register it as normal (sync)?");

                    if (mock)
                        return await GetMockAsync(svcCol);

                    return await AsyncFactoryMethod(svcCol);
                case ServiceScope.LazySingleton:
                    if (mock)
                    {
                        if (_mockObject == null)
                            _mockObject = await GetMockAsync(svcCol);

                        return _mockObject;
                    }

                    if (ConcreteInstance == null)
                    {
                        if (AsyncFactoryMethod == null)
                            throw new ServiceNotRegisteredException($"The service of registered type {RegisteredServiceType} does not have a {nameof(AsyncFactoryMethod)} defined. Did you register it as normal (sync)?");

                        ConcreteInstance = await AsyncFactoryMethod(svcCol);
                    }

                    return ConcreteInstance;
                case ServiceScope.Singleton:
                    if (mock)
                        return _mockObject;

                    return ConcreteInstance;
                default:
                    return null;
            }
        }

        private object GetMock(IServiceCollection svcCol)
        {
            var mockObj = _mockType.GetConstructor(Type.EmptyTypes).Invoke(null);
            if (mockObj is IMockProvider mockProvider)
                mockProvider.Initialize(svcCol);

            return mockObj;
        }

        private async Task<object> GetMockAsync(IServiceCollection svcCol)
        {
            var mockObj = _mockType.GetConstructor(Type.EmptyTypes).Invoke(null);
            if (mockObj is IAsyncMockProvider mockProvider)
                await mockProvider.InitializeAsync(svcCol);

            return mockObj;
        }

        public void SetMockProvider(Type mockProvider)
        {
            _mockType = mockProvider;
        }

        public void SetMockObject(object obj)
        {
            _mockObject = obj;
        }
    }
}
