using System;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class FormGroupBehaviourAttribute : Attribute
    {
        public FormGroupMode GroupInto { get; set; }
    }
}
