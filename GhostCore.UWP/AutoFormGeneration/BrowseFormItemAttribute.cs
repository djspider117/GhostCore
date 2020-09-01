using System;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class BrowseFormItemAttribute : FormItemAttribute
    {
        public bool BrowseForFolder { get; set; }
    }
}
