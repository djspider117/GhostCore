using System;
using System.Linq;
using System.Reflection;

namespace GhostCore
{
    public static class ServiceLocatorAutoRegister
    {
        private static bool _initialized;

        public static void AutoRegisterServices()
        {
            if (_initialized)
                return;

            _initialized = true;
            Assembly[] asses = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in asses)
            {
                var types = ass.GetTypes();
                var serviceImpls = types.Where(x => x.GetCustomAttribute<ServiceImplementationAttribute>() != null);

                foreach (var serviceImpl in serviceImpls)
                {
                    var sis = serviceImpl.GetCustomAttributes<ServiceImplementationAttribute>();

                    foreach (var si in sis)
                    {
                        var defaultCtor = serviceImpl.GetConstructor(Type.EmptyTypes);
                        ServiceLocator.Instance.Register(si.ServiceType, defaultCtor.Invoke(null), ServiceLocator.NoFactory);
                    }
                }
            }
        }
    }

}
