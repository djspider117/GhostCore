using GhostCore.Utility;
using System;

namespace GhostCore.SimpleIoC
{
    public static class ServiceLocatorAutoRegister
    {
        private static bool _initialized;

        public static void AutoRegisterServices()
        {
            if (_initialized)
                return;

            _initialized = true;

            var typeAttributeMapping = AssemblyReflectionParser.GetTypesForAttributes<IoC.ServiceImplementationAttribute>();

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
