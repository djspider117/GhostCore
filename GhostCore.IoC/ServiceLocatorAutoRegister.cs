using GhostCore.Utility;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public static class ServiceLocatorAutoRegister
    {
        private static bool _initialized;

        public static async Task AutoRegisterServicesAsync()
        {
            if (_initialized)
                return;

            _initialized = true;

            var typeAttributeMapping = AssemblyReflectionParser.GetTypesForAttributes<ServiceImplementationAttribute>();

            foreach (var pair in typeAttributeMapping)
            {
                var ctor = pair.Type.GetConstructor(Type.EmptyTypes);

                foreach (var attribute in pair.Attributes)
                {
                    if (pair.Type.IsSubclassOf(typeof(IAsyncServiceInitializer)))
                    {
                        await ServiceLocator.Instance.AddAsync(attribute.ServiceType, attribute.Scope, (_) => Task.Run(() => ctor.Invoke(null)));
                    }
                    else
                    {
                        ServiceLocator.Instance.Add(attribute.ServiceType, pair.Type, attribute.Scope, (_) => ctor.Invoke(null));
                    }
                }
            }
        }
    }
}

