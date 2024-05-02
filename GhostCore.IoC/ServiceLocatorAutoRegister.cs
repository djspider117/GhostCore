using GhostCore.Utility;
using System;
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
                    if (pair.Type.GetInterface(nameof(IAsyncServiceInitializer)) != null)
                    {
                        await ServiceLocator.Instance.AddAsync(attribute.ServiceType, attribute.Scope, (_) => Task.Run(() => ctor.Invoke(null)));

                        if (attribute.MockProviderType != null)
                        {
                            if (attribute.MockProviderType.GetInterface(nameof(IAsyncMockProvider)) != null)
                                await ServiceLocator.Instance.AssignMockProviderAsync(attribute.ServiceType, attribute.MockProviderType, attribute.Scope);
                        }
                    }
                    else
                    {
                        ServiceLocator.Instance.Add(attribute.ServiceType, pair.Type, attribute.Scope, (_) => ctor.Invoke(null));

                        if (attribute.MockProviderType != null)
                        {
                            if (attribute.MockProviderType.GetInterface(nameof(IMockProvider)) != null)
                                ServiceLocator.Instance.AssignMockProvider(attribute.ServiceType, attribute.MockProviderType, attribute.Scope);
                        }
                    }


                }

            }
        }
    }
}

