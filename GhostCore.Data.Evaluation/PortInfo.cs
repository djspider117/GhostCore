using System;

namespace GhostCore.Data.Evaluation;

// TODO type converters

public readonly record struct PortInfo(uint Id, uint TypeId, string Name) : IEquatable<PortInfo>
{
    public bool Equals(PortInfo other) => Id == other.Id;
    public override int GetHashCode() => Id.GetHashCode();
}
