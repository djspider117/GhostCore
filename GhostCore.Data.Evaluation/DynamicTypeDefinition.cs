using System.Collections.Generic;

namespace GhostCore.Data.Evaluation;

public class DynamicTypeDefinition
{
    public uint TypeId { get; set; }
    public string? TypeName { get; set; }

    public List<PortInfo> Inputs { get; } = [];
    public List<PortInfo> Outputs { get; } = [];
  
    public DynamicTypeDefinition(uint id, string? name)
    {
        TypeId = id;
        TypeName = name;
    }
}
