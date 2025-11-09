namespace GhostCore.Data.Evaluation.SourceGen;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class EvaluatableAttribute : Attribute
{
    public EvaluatableAttribute() { }
    public EvaluatableAttribute(string customName) { }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class InputAttribute : Attribute
{
    public InputAttribute() { }
    public InputAttribute(string customName) { }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class OutputAttribute : Attribute
{
    public OutputAttribute() { }
    public OutputAttribute(string customName) { }
}
