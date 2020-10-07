using System;
using System.Linq;
using System.Reflection;

namespace GhostCore
{
    public static class ServiceLocatorAutoRegister
    {
        public static void AutoRegisterServices()
        {
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
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
