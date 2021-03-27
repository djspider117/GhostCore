using System;
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public interface IServiceCollection : IDisposable
    {
        void Add<TService>(ServiceScope scope);
        void Add<TServiceContract, TServiceImplementation>(ServiceScope scope) where TServiceImplementation : TServiceContract;

        void Add<TService>(ServiceScope scope, Func<IServiceCollection, TService> factory);
        Task AddAsync<TService>(ServiceScope scope, Func<IServiceCollection, Task<TService>> asyncFactory);

        void Add(Type serviceType, ServiceScope scope);

        void Add(Type serviceType, Type implementationType, ServiceScope scope, Func<IServiceCollection, object> factory);
        Task AddAsync(Type serviceType, ServiceScope scope, Func<IServiceCollection, Task<object>> asyncFactory);

        bool RemoveService<TService>();
        bool RemoveService(Type serviceType);

        TService Get<TService>(bool tryInitialize = true);
        TService GetDisposable<TService>(bool tryInitialize = true) where TService : IDisposable;

        Task<TService> GetAsync<TService>(bool tryInitialize = true);
        Task<TService> GetDisposableAsync<TService>(bool tryInitialize = true) where TService : IDisposable;

        bool TryGet<TService>(out TService svc, bool tryInitialize = true);
        bool TryGetDisposable<TService>(out TService svc, bool tryInitialize = true) where TService : IDisposable;

        Task<(bool, TService)> TryGetAsync<TService>(bool tryInitialize = true);
        Task<(bool, TService)> TryGetDisposableAsync<TService>(bool tryInitialize = true) where TService : IDisposable;
    }
}
