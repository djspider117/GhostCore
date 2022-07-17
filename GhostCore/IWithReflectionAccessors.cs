namespace GhostCore
{
    /// <summary>
    /// Provides an interface to allow arbitrary getting or setting of property values using either property names or property hash values in either a safe or fast way.
    /// </summary>
    public interface IWithReflectionAccessors : IClonableEx
    {
        void SetProperty(string propName, object value);
        void SetProperty(long propHash, object value);

        T GetProperty<T>(string propName);
        T GetProperty<T>(long propHash);
    }

}
