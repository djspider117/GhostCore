using GhostCore.Utility;
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

            var typeAttributeMapping = AssemblyReflectionParser.GetTypesForAttributes<ServiceImplementationAttribute>();

            foreach (var pair in typeAttributeMapping)
            {
                var ctor = pair.Type.GetConstructor(Type.EmptyTypes);

                foreach (var attribute in pair.Attributes)
                {
                    ServiceLocator.Instance.Register(attribute.ServiceType, ctor.Invoke(null), ServiceLocator.NoFactory);
                }
            }
        }
    }

}
