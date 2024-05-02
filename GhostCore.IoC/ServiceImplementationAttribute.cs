using System;

namespace GhostCore.IoC
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ServiceImplementationAttribute : Attribute
    {
        public Type ServiceType { get; set; }
        public ServiceScope Scope { get; set; }
        public Type MockProviderType { get; set; }

        public ServiceImplementationAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public ServiceImplementationAttribute(Type serviceType, ServiceScope scope, Type mockProviderType = null)
        {
            Scope = scope;
            ServiceType = serviceType;
            MockProviderType = mockProviderType;
        }
    }


}
