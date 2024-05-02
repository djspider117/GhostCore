using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GhostCore.IoC
{
    public class ServiceLocator : ServiceCollectionBase, INamedServiceCollection
    {
        private ConcurrentDictionary<string, INamedServiceCollection> _serviceCollections;

        #region Singleton

        private static readonly Lazy<ServiceLocator> _lazyInstance = new Lazy<ServiceLocator>(() => new ServiceLocator(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static ServiceLocator Instance => _lazyInstance.Value;

        private ServiceLocator()
        {
            Initialize();
        }

        #endregion

        internal void Initialize()
        {
            Name = "default";

            _serviceCollections = new ConcurrentDictionary<string, INamedServiceCollection>();
            _serviceCollections.TryAdd(Name, this);
        }

        public INamedServiceCollection GetOrCreateServiceCollection(string name)
        {
            if (_serviceCollections.ContainsKey(name))
                return _serviceCollections[name];

            var col = new ServiceCollectionBase { Name = name };
            _serviceCollections.TryAdd(name, col);

            return col;
        }

        public bool RemoveCollection(string name, bool disposeItems = true)
        {
            if (!_serviceCollections.ContainsKey(name))
                return false;

            var col = _serviceCollections[name];

            if (disposeItems)
                col.Dispose();

            return true;
        }
    }
}
