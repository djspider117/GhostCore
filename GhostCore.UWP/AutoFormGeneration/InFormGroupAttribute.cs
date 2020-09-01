using System;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InFormGroupAttribute : Attribute
    {
        public string GroupId { get; set; }
    }
}
