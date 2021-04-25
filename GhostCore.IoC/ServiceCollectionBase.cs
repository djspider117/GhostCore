using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public class ServiceCollectionBase : INamedServiceCollection
    {
        #region Fields

        private readonly List<ServiceDefinition> _serviceDefinitions;

        #endregion

        public virtual string Name { get; set; }

        #region Ctor and Init

        public ServiceCollectionBase()
        {
            _serviceDefinitions = new List<ServiceDefinition>();
        }

        #endregion

        #region Add & Remove

        public void Add(Type serviceType, ServiceScope scope) => InternalAdd(scope, (_) => _.Invoke(null), (_) => Task.Run(() => _.Invoke(null)), serviceType, serviceType);

        public void Add(Type interfaceType, Type implType, ServiceScope scope, Func<IServiceCollection, object> factory) => InternalAdd(scope, (_) => _.Invoke(null), (_) => Task.Run(() => _.Invoke(null)), interfaceType, implType);

        public async Task AddAsync(Type serviceType, ServiceScope scope, Func<IServiceCollection, Task<object>> asyncFactory)
        {
            CheckDuplicate(serviceType);

            var svcDef = new ServiceDefinition
            {
                Scope = scope,
                RegisteredServiceType = serviceType
            };

            var ctor = serviceType.GetConstructor(Type.EmptyTypes);

            if (scope == ServiceScope.Singleton)
                svcDef.ConcreteInstance = await asyncFactory(this);
            else
                svcDef.AsyncFactoryMethod = async (_) => await asyncFactory(this);

            _serviceDefinitions.Add(svcDef);
        }

        public void Add<TService>(ServiceScope scope) => InternalAdd<TService, TService>(scope, (_) => _.Invoke(null), (_) => Task.Run(() => _.Invoke(null)));
        public void Add<TServiceContract, TServiceImplementation>(ServiceScope scope) where TServiceImplementation : TServiceContract => InternalAdd<TServiceContract, TServiceImplementation>(scope, (_) => _.Invoke(null), (_) => Task.Run(() => _.Invoke(null)));
        public virtual void Add<TService>(ServiceScope scope, Func<IServiceCollection, TService> factory)
        {
            var type = typeof(TService);
            CheckDuplicate(type);

            var svcDef = new ServiceDefinition
            {
                Scope = scope,
                RegisteredServiceType = type
            };

            var ctor = type.GetConstructor(Type.EmptyTypes);

            if (scope == ServiceScope.Singleton)
                svcDef.ConcreteInstance = factory(this);
            else
                svcDef.SyncFactoryMethod = (_) => factory(this);

            _serviceDefinitions.Add(svcDef);
        }

        public virtual async Task AddAsync<TService>(ServiceScope scope, Func<IServiceCollection, Task<TService>> asyncFactory)
        {
            var type = typeof(TService);
            CheckDuplicate(type);

            var svcDef = new ServiceDefinition
            {
                Scope = scope,
                RegisteredServiceType = type
            };

            var ctor = type.GetConstructor(Type.EmptyTypes);

            if (scope == ServiceScope.Singleton)
                svcDef.ConcreteInstance = await asyncFactory(this);
            else
                svcDef.AsyncFactoryMethod = async (_) => await asyncFactory(this);

            _serviceDefinitions.Add(svcDef);
        }
        public virtual bool RemoveService<TService>() => RemoveService(typeof(TService));

        public bool RemoveService(Type serviceType)
        {
            var idx = FindDefinitionIndex(serviceType);
            if (idx == -1)
                return false;

            _serviceDefinitions.RemoveAt(idx);

            return true;
        }

        #endregion

        #region Gets
        public TService GetDisposable<TService>(bool tryInitialize = true) where TService : IDisposable => Get<TService>(tryInitialize);
        public virtual TService Get<TService>(bool tryInitialize = true)
        {
            var item = FindDefinition(typeof(TService));
            var svc = (TService)item.GetValue(this);

            if (tryInitialize && svc is IInitializedService svcinit)
                svcinit.Initialize();

            return svc;
        }

        public Task<TService> GetDisposableAsync<TService>(bool tryInitialize = true) where TService : IDisposable => GetAsync<TService>(tryInitialize);
        public virtual async Task<TService> GetAsync<TService>(bool tryInitialize = true)
        {
            var item = FindDefinition(typeof(TService));
            var svc = (TService)(await item.GetValueAsync(this));

            if (tryInitialize && svc is IAsyncServiceInitializer svcinit)
                await svcinit.Initialize();

            return svc;
        }
        #endregion

        #region TryGets

        public bool TryGet<TService>(out TService svc, bool tryInitialize = true)
        {
            var val = InternalTryGet(() => Get<TService>(tryInitialize));
            svc = val.Item2;
            return val.Item1;
        }

        public bool TryGetDisposable<TService>(out TService svc, bool tryInitialize = true) where TService : IDisposable
        {
            var val = InternalTryGet(() => GetDisposable<TService>(tryInitialize));
            svc = val.Item2;
            return val.Item1;
        }

        public async Task<(bool, TService)> TryGetAsync<TService>(bool tryInitialize = true)
        {
            var val = InternalTryGet(() => GetAsync<TService>(tryInitialize));
            var svc = await val.Item2;
            return (val.Item1, svc);
        }

        public async Task<(bool, TService)> TryGetDisposableAsync<TService>(bool tryInitialize = true) where TService : IDisposable
        {
            var val = InternalTryGet(() => GetDisposableAsync<TService>(tryInitialize));
            var svc = await val.Item2;
            return (val.Item1, svc);
        }

        #endregion

        #region Helpers

        protected virtual (bool, TService) InternalTryGet<TService>(Func<TService> func)
        {
            try
            {
                return (true, func());
            }
            catch (Exception)
            {
                return (false, default);
            }
        }

        protected virtual void InternalAdd<TServiceContract, TServiceImplementation>(ServiceScope scope, Func<ConstructorInfo, object> syncFact, Func<ConstructorInfo, Task<object>> asyncFact)
        {
            var interfaceType = typeof(TServiceContract);
            var type = typeof(TServiceImplementation);
            InternalAdd(scope, syncFact, asyncFact, interfaceType, type);
        }

        protected virtual void InternalAdd(ServiceScope scope, Func<ConstructorInfo, object> syncFact, Func<ConstructorInfo, Task<object>> asyncFact, Type interfaceType, Type type)
        {
            CheckDuplicate(interfaceType);

            var svcDef = new ServiceDefinition
            {
                Scope = scope,
                RegisteredServiceType = interfaceType
            };

            var ctor = type.GetConstructor(Type.EmptyTypes);

            if (scope == ServiceScope.Singleton)
            {
                svcDef.ConcreteInstance = ctor.Invoke(null);
            }
            else
            {
                svcDef.SyncFactoryMethod = (_) => syncFact(ctor);
                svcDef.AsyncFactoryMethod = (_) => asyncFact(ctor);
            }

            _serviceDefinitions.Add(svcDef);
        }

        protected virtual int CheckDuplicate(Type svcType)
        {
            if (svcType == null)
                throw new ArgumentNullException(nameof(svcType));

            var idx = FindDefinitionIndex(svcType);
            if (idx == -1)
                throw new InvalidOperationException("Duplicate type {svcType} found.");

            return idx;
        }

        protected virtual int FindDefinitionIndex(Type svcType) => _serviceDefinitions.FindIndex(x => x.RegisteredServiceType == svcType);
        protected virtual IServiceDefinition FindDefinition(Type svcType)
        {
            var idx = FindDefinitionIndex(svcType);
            if (idx == -1)
                throw new ServiceNotRegisteredException($"Service of type {svcType} was not registered.");

            return _serviceDefinitions[idx];
        }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
            for (int i = 0; i < _serviceDefinitions.Count; i++)
            {
                var item = _serviceDefinitions[i];

                if (item is IDisposable dsp)
                    dsp.Dispose();
            }
        }

        #endregion
    }
}
