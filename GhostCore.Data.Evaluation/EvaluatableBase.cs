using System;
using System.Collections.Generic;

namespace GhostCore.Data.Evaluation;

public abstract class EvaluatableBase : IEvaluatable
{
    private readonly Dictionary<PortInfo, PortConnection> _inputConnections = [];
    public IReadOnlyDictionary<PortInfo, PortConnection> InputConnections => _inputConnections;

    public DynamicTypeDefinition TypeDefinition { get; init; }

    protected EvaluatableBase(DynamicTypeDefinition typeDefinition)
    {
        TypeDefinition = typeDefinition;
    }

    public PortConnection ConnectToPort(string portName, IEvaluatable source, IConverter? converter = null)
    {
        if (source is not EvaluatableBase evb)
            throw new ArgumentException($"To use this override of {nameof(ConnectToPort)} the provided source must have a base type of {nameof(EvaluatableBase)}. Consider using ConnectToPort(PortInfo, IEvaluatable, PortInfo).", nameof(source));

        return ConnectToPort(GetInputPort(portName), source, evb.GetOutputPort("Output"), converter);
    }
    public PortConnection ConnectToPort(string portName, IEvaluatable source, string sourcePortName, IConverter? converter = null)
    {
        if (source is not EvaluatableBase evb)
            throw new ArgumentException($"To use this override of {nameof(ConnectToPort)} the provided source must have a base type of {nameof(EvaluatableBase)}. Consider using ConnectToPort(PortInfo, IEvaluatable, PortInfo)", nameof(source));

        return ConnectToPort(GetInputPort(portName), source, evb.GetOutputPort(sourcePortName), converter);
    }

    public PortConnection ConnectToPort(PortInfo targetPort, IEvaluatable sourceObject, PortInfo sourcePort, IConverter? converter = null)
    {
        var pc = new PortConnection(sourcePort, sourceObject, converter);
        if (_inputConnections.ContainsKey(targetPort))
            _inputConnections[targetPort] = pc;
        else
            _inputConnections.Add(targetPort, pc);

        return pc;
    }

    public object?[] GetOutputValues(EvaluationContext context)
    {
        UpdateInputs(context);
        return GetOutputValuesInternal(context);
    }

    public object? GetOutputValue(PortInfo port, EvaluationContext context)
    {
        UpdateInputs(context);
        return GetOutputValueInternal(port, context);
    }

    protected virtual void UpdateInputs(EvaluationContext context) { }

    public abstract object?[] GetOutputValuesInternal(EvaluationContext context);
    public abstract object? GetOutputValueInternal(PortInfo port, EvaluationContext context);

    public PortInfo GetInputPort(string portName) => MetadataCache.GetInputPort(TypeDefinition.TypeId, portName);
    public PortInfo GetOutputPort(string portName) => MetadataCache.GetOutputPort(TypeDefinition.TypeId, portName);
}
