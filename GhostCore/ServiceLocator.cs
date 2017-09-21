using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore
{
    public class ServiceLocator
    {
        private const bool REPLACE_EXISTING = true;

        #region Statics

        public static readonly Func<object> NoFactory = () => { return new object(); };

        #endregion

        #region Fields

        private Dictionary<Type, Func<object>> _objectFactories;
        private Dictionary<Type, object> _createdInstances;

        #endregion

        #region Singleton

        private static ServiceLocator _instance = null;
        private static readonly object _syncRoot = new object();

        public static ServiceLocator Instance
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new ServiceLocator();
                    return _instance;
                }
            }
            set
            {
                lock (_syncRoot)
                    _instance = value;
            }
        }

        public static T InstanceAs<T>() where T : ServiceLocator
        {
            return Instance as T;
        }

        protected ServiceLocator()
        {
            Initialize();
        }

        #endregion

        #region Initialization

        private void Initialize()
        {
            _objectFactories = new Dictionary<Type, Func<object>>();
            _createdInstances = new Dictionary<Type, object>();
        }

        #endregion

        #region API

        public void Register<T>(Func<object> factory, object initialValue = null)
        {
            var type = typeof(T);

            if (type == null)
                throw new ArgumentException("Type is null.", "type");

            if (factory == null)
                throw new ArgumentException("Factory is null.", "factory");

            if (_objectFactories.ContainsKey(type))
            {
                Debug.Log("Type is already registered in service locator. Type : " + type.ToString());
                if (REPLACE_EXISTING)
                {
                    _objectFactories[type] = factory;
                }
                return;
            }

            if (factory != NoFactory)
                _objectFactories.Add(type, factory);

            if (_createdInstances.ContainsKey(type))
            {
                Debug.Log("Type is already registered in service locator. Type : " + type.ToString());
                if (REPLACE_EXISTING)
                {
                    _createdInstances[type] = initialValue;
                }
                return;
            }

            if (initialValue != null)
                _createdInstances.Add(type, initialValue);
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (_createdInstances.ContainsKey(type))
                return (T)_createdInstances[type];

            if (!_objectFactories.ContainsKey(type))
                throw new ArgumentException("Type is not registered.");

            var fact = _objectFactories[type];

            var obj = fact();
            _createdInstances.Add(type, obj);

            return (T)obj;
        }

        public void ClearRegisteredItems()
        {
            _objectFactories.Clear();
            _createdInstances.Clear();
        }

        #endregion
    }

}
