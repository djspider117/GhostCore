using System;
using System.Collections.Generic;

namespace GhostCore
{
    public abstract class DictionaryFactory<TFactoryKey, TBaseClass>
    {
        private Dictionary<TFactoryKey, Func<TBaseClass>> _factoryMap;
        protected IEqualityComparer<TFactoryKey> _customKeyComparer;

        public DictionaryFactory()
        {
            _factoryMap = new Dictionary<TFactoryKey, Func<TBaseClass>>();
            Configure(_factoryMap);
        }

        protected abstract void Configure(Dictionary<TFactoryKey, Func<TBaseClass>> factoryMap);

        public TBaseClass Create(TFactoryKey factoryKey)
        {
            if (_customKeyComparer != null)
            {
                foreach (var item in _factoryMap)
                {
                    if (_customKeyComparer.Equals(item.Key, factoryKey))
                        return item.Value();
                }

                return default;
            }
            else
            {
                if (!_factoryMap.ContainsKey(factoryKey))
                    return default;

                return _factoryMap[factoryKey]();
            }
        }
    }
}
