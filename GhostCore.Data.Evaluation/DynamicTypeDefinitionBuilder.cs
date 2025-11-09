using System;

namespace GhostCore.Data.Evaluation;

internal class ExistingTypeDefinitionBuilder : DynamicTypeDefinitionBuilder
{
    internal ExistingTypeDefinitionBuilder(DynamicTypeDefinition node) : base(node) {}

    public override DynamicTypeDefinitionBuilder WithInputProperty(string propName, uint typeId) => this;
    public override DynamicTypeDefinitionBuilder WithOutputProperty(string propName, uint typeId) => this;
}

public class DynamicTypeDefinitionBuilder
{
    private readonly DynamicTypeDefinition _curNode;
    private bool _built = false;

    protected DynamicTypeDefinitionBuilder(uint id, string name)
    {
        _curNode = new DynamicTypeDefinition(id, name);
    }
    protected DynamicTypeDefinitionBuilder(DynamicTypeDefinition node) => _curNode = node;

    public static DynamicTypeDefinitionBuilder Create(uint id, string name)
    {
        return new DynamicTypeDefinitionBuilder(id, name);
    }

    public static DynamicTypeDefinitionBuilder Create(string name)
    {
        var node = MetadataCache.GetOrRegisterTypeDefinition(name, out var isNew);
        if (!isNew)
            return new ExistingTypeDefinitionBuilder(node);

        return new DynamicTypeDefinitionBuilder(node);
    }

    public virtual DynamicTypeDefinitionBuilder WithInputProperty(string propName, uint typeId)
    {
        _curNode.Inputs.Add(MetadataCache.RegisterInputPortForType(_curNode.TypeId, propName, typeId));
        return this;
    }
    public virtual DynamicTypeDefinitionBuilder WithOutputProperty(string propName, uint typeId)
    {
        _curNode.Outputs.Add(MetadataCache.RegisterOutputPortForType(_curNode.TypeId, propName, typeId));
        return this;
    }

    public DynamicTypeDefinition Build()
    {
        if (_built)
            throw new InvalidOperationException("Type definition already built");

        _built = true;
        return _curNode;
    }
}
