using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.SimpleIoC
{
    public class ServiceLocator
    {
        public const bool REPLACE_EXISTING = true;

        #region Statics

        public static readonly Func<object> NoFactory = () => { return new object(); };

        #endregion

        #region Fields

        private Dictionary<Type, Func<object>> _objectFactories;
        private Dictionary<Type, object> _createdInstances;
        private Dictionary<Type, object> _defaults;

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
            _defaults = new Dictionary<Type, object>();
        }

        #endregion

        #region API

        public void AddDefault<T>(T value) where T : class, new()
        {
            var type = typeof(T);

            if (_defaults.ContainsKey(type))
            {
                Debug.WriteLine("Type {type} already configured.");
                if (REPLACE_EXISTING)
                    _defaults[type] = value;
            }
            else
            {
                _defaults.Add(type, value);
            }
        }

        public T GetDefaults<T>()
        {
            var type = typeof(T);
            if (_defaults.ContainsKey(type))
            {
                return (T)_defaults[type];
            }

            return default;
        }

        public void Register<T>(T data)
        {
            Register<T>(NoFactory, data);
        }

        public void Register(Type type, object initialValue, Func<object> factory = null)
        {
            if (factory == null)
                throw new ArgumentException("Factory is null.", "factory");

            if (_objectFactories.ContainsKey(type))
            {
                Debug.WriteLine("Type is already registered in service locator. Type : " + type.ToString());
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
                Debug.WriteLine("Type is already registered in service locator. Type : " + type.ToString());
                if (REPLACE_EXISTING)
                {
                    _createdInstances[type] = initialValue;
                }
                return;
            }

            if (initialValue != null)
                _createdInstances.Add(type, initialValue);
        }

        public void Register<T>(Func<object> factory, object initialValue = null)
        {
            var type = typeof(T);

            if (type == null)
                throw new ArgumentException("Type is null.", "type");

            Register(type, initialValue, factory);
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (_createdInstances.TryGetValue(type, out object instance))
                return (T)instance;

            if (!_objectFactories.ContainsKey(type))
                throw new ArgumentException($"Type {type.Name} is not registered.");

            var fact = _objectFactories[type];

            var obj = fact();
            _createdInstances.Add(type, obj);

            return (T)obj;
        }

        public bool TryResolve<T>(out T obj)
        {
            Type type = typeof(T);
            if (_createdInstances.TryGetValue(type, out object e))
            {
                obj = (T)e;
                return true;
            }

            if (!_objectFactories.ContainsKey(type))
            {
                obj = default;
                return false;
            }

            object instance = _objectFactories[type]();
            _createdInstances.Add(type, instance);

            obj = (T)instance;
            return true;
        }

        public void ClearRegisteredItems()
        {
            _objectFactories.Clear();
            _createdInstances.Clear();
        }

        #endregion
    }

}
