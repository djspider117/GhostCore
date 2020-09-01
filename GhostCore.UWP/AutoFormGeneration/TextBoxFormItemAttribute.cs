using System;
using Windows.UI.Xaml.Input;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class TextBoxFormItemAttribute : FormItemAttribute
    {
        public string Placeholder { get; set; }
        public InputScope InputScope { get; set; }
    }
}
