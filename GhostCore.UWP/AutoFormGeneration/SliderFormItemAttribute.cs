using System;

namespace GhostCore.UWP.AutoFormGeneration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class SliderFormItemAttribute : FormItemAttribute
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
