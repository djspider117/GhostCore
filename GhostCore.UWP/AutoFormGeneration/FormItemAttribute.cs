using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FormItemAttribute : Attribute
    {
        public string Label { get; set; }
        public int Order { get; set; }
        public BindingMode BindingMode { get; set; } = BindingMode.TwoWay;
        public IValueConverter Converter { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ReadOnlyIf : Attribute
    {
        public string BooleanSourceProperty { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class VisibleIf : ReadOnlyIf
    {
    }
}
