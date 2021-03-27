using System;

namespace GhostCore.MVVM.Dynamic
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ViewModelForAttribute : Attribute
    {
        public Type TargetType { get; set; }

        public ViewModelForAttribute(Type targetType)
        {
            TargetType = targetType;
        }

    }

}
