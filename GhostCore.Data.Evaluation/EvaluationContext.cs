using System.Collections.Generic;

namespace GhostCore.Data.Evaluation;

public sealed class EvaluationContext
{
    public static readonly EvaluationContext Default = new("Default");

    public uint Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Variables { get; } = [];

    public EvaluationContext(string name)
    {
        Name = name;
    }
}
