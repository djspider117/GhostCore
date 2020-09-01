using System;


namespace GhostCore
{
    public interface IClonableEx : ICloneable
    {
        T CloneAs<T>();
    }

    public interface IClonableEx<T> : IClonableEx
    {
        T CloneAs();
    }

    public interface IDeepClone<T>
    {
        T DeepClone();
    }
    public interface IDeepClone : IDeepClone<object>
    {
    }
}

