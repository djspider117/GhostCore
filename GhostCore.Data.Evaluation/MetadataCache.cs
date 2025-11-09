using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Threading;

namespace GhostCore.Data.Evaluation;

public static class MetadataCache
{
    private static IDictionary<uint, PortInfo> _portCache = new Dictionary<uint, PortInfo>();
    private static IDictionary<uint, DynamicTypeDefinition> _typeCache = new Dictionary<uint, DynamicTypeDefinition>();
    private static IDictionary<string, DynamicTypeDefinition> _nameTypeCache = new Dictionary<string, DynamicTypeDefinition>();
    private static IDictionary<Type, uint> _typeIdMapping = new Dictionary<Type, uint>();

    private static readonly object _lock = new();
    private static long _globalIdCounter;

    public static void Freeze()
    {
        lock (_lock)
        {
            if (_portCache != null)
                _portCache = _portCache.ToFrozenDictionary(x => x.Key, x => x.Value);

            if (_typeCache != null)
                _typeCache = _typeCache.ToFrozenDictionary(x => x.Key, x => x.Value);
        }
    }

    public static DynamicTypeDefinition GetOrRegisterTypeDefinition(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        if (_nameTypeCache.TryGetValue(name, out var existing))
            return existing;

        var id = GetNextId();
        var node = new DynamicTypeDefinition(id, name);
        _typeCache.Add(node.TypeId, node);
        _nameTypeCache.Add(name, node);
        return node;
    }

    public static PortInfo RegisterInputPortForType(uint nodeType, string name, uint propTypeId)
    {
        if (!_typeCache.TryGetValue(nodeType, out var existing))
            throw new InvalidOperationException("Not found");

        var prop = new PortInfo(GetNextId(), propTypeId, name);
        existing.Inputs.Add(prop);

        _portCache.Add(prop.Id, prop);

        return prop;
    }
    public static PortInfo RegisterOutputPortForType(uint nodeType, string name, uint propTypeId)
    {
        if (!_typeCache.TryGetValue(nodeType, out var existing))
            throw new InvalidOperationException("Not found");

        var prop = new PortInfo(GetNextId(), propTypeId, name);
        existing.Outputs.Add(prop);

        _portCache.Add(prop.Id, prop);

        return prop;
    }

    public static uint GetNextId() => (uint)Interlocked.Increment(ref _globalIdCounter);

    public static PortInfo GetPort(uint propType)
    {
        if (_portCache.TryGetValue(propType, out var existing))
            return existing;

        throw new InvalidOperationException("Not found");
    }
    public static PortInfo GetInputPort(uint nodeType, string propName)
    {
        if (!_typeCache.TryGetValue(nodeType, out var node))
            throw new InvalidOperationException("Not found");

        return node.Inputs.Find(x => x.Name == propName);
    }
    public static PortInfo GetInputPort(string nodeTypeName, string propName)
    {
        if (!_nameTypeCache.TryGetValue(nodeTypeName, out var node))
            throw new InvalidOperationException("Not found");

        return node.Inputs.Find(x => x.Name == propName);
    }

    public static PortInfo GetOutputPort(uint nodeType, string propName)
    {
        if (!_typeCache.TryGetValue(nodeType, out var node))
            throw new InvalidOperationException("Not found");

        return node.Outputs.Find(x => x.Name == propName);
    }
    public static PortInfo GetOutputPort(string nodeTypeName, string propName)
    {
        if (!_nameTypeCache.TryGetValue(nodeTypeName, out var node))
            throw new InvalidOperationException("Not found");

        return node.Outputs.Find(x => x.Name == propName);
    }

    public static uint GetOrRegisterTypeId(Type type)
    {
        if (_typeIdMapping.TryGetValue(type, out var id))
            return id;

        id = GetNextId();
        _typeIdMapping.Add(type, id);
        return id;
    }
}
