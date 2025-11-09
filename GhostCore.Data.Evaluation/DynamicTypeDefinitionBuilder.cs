using System;

namespace GhostCore.Data.Evaluation;

public class DynamicTypeDefinitionBuilder
{
    private readonly DynamicTypeDefinition _curNode;
    private bool _built = false;

    private DynamicTypeDefinitionBuilder(uint id, string name)
    {
        _curNode = new DynamicTypeDefinition(id, name);
    }
    private DynamicTypeDefinitionBuilder(DynamicTypeDefinition node) => _curNode = node;

    public static DynamicTypeDefinitionBuilder Create(uint id, string name)
    {
        return new DynamicTypeDefinitionBuilder(id, name);
    }

    public static DynamicTypeDefinitionBuilder Create(string name)
    {
        var node = MetadataCache.GetOrRegisterTypeDefinition(name);
        return new DynamicTypeDefinitionBuilder(node);
    }

    public DynamicTypeDefinitionBuilder WithInputProperty(string propName, uint typeId)
    {
        MetadataCache.RegisterInputPortForType(_curNode.TypeId, propName, typeId);
        return this;
    }
    public DynamicTypeDefinitionBuilder WithOutputProperty(string propName, uint typeId)
    {
        MetadataCache.RegisterOutputPortForType(_curNode.TypeId, propName, typeId);
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
