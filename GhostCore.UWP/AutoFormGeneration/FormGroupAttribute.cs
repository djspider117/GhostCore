using System;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class FormGroupAttribute : Attribute
    {
        public string GroupId { get; set; }
        public string GroupLabel { get; set; }
    }
}
