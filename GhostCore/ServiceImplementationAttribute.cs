using System;

namespace GhostCore
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ServiceImplementationAttribute : Attribute
    {
        public Type ServiceType { get; set; }

        public ServiceImplementationAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }

}
