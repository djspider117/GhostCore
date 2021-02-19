using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.Foundation
{
    public abstract class DictionaryFactory<TFactoryKey, TBaseClass>
    {
        private Dictionary<TFactoryKey, Func<TBaseClass>> _factoryMap;

        public DictionaryFactory()
        {
            _factoryMap = new Dictionary<TFactoryKey, Func<TBaseClass>>();
            Configure(_factoryMap);
        }

        protected abstract void Configure(Dictionary<TFactoryKey, Func<TBaseClass>> factoryMap);

        public TBaseClass Create(TFactoryKey factoryKey)
        {
            if (!_factoryMap.ContainsKey(factoryKey))
                return default;

            return _factoryMap[factoryKey]();
        }
    }
}
