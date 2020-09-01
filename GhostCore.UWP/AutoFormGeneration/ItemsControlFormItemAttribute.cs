using System;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ItemsControlFormItemAttribute : FormItemAttribute
    {
        public string DataSourceProperty { get; set; }
        public ItemsControlType Type { get; set; }
    }
}
