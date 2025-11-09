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

    public void ConnectToPort(string portName, IEvaluatable source)
    {
        if (source is not EvaluatableBase evb)
            throw new ArgumentException($"To use this override of {nameof(ConnectToPort)} the provided source must have a base type of {nameof(EvaluatableBase)}. Consider using ConnectToPort(PortInfo, IEvaluatable, PortInfo).", nameof(source));

        ConnectToPort(GetInputPort(portName), source, evb.GetOutputPort("Output"));
    }
    public void ConnectToPort(string portName, IEvaluatable source, string sourcePortName)
    {
        if (source is not EvaluatableBase evb)
            throw new ArgumentException($"To use this override of {nameof(ConnectToPort)} the provided source must have a base type of {nameof(EvaluatableBase)}. Consider using ConnectToPort(PortInfo, IEvaluatable, PortInfo)", nameof(source));

        ConnectToPort(GetInputPort(portName), source, evb.GetOutputPort(sourcePortName));
    }

    public void ConnectToPort(PortInfo targetPort, IEvaluatable sourceObject, PortInfo sourcePort)
    {
        var pc = new PortConnection(sourcePort, sourceObject);
        if (_inputConnections.ContainsKey(targetPort))
            _inputConnections[targetPort] = pc;
        else
            _inputConnections.Add(targetPort, pc);
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

    protected abstract void UpdateInputs(EvaluationContext context);

    public abstract object?[] GetOutputValuesInternal(EvaluationContext context);
    public abstract object? GetOutputValueInternal(PortInfo port, EvaluationContext context);

    public PortInfo GetInputPort(string portName) => MetadataCache.GetInputPort(TypeDefinition.TypeId, portName);
    public PortInfo GetOutputPort(string portName) => MetadataCache.GetOutputPort(TypeDefinition.TypeId, portName);
}
