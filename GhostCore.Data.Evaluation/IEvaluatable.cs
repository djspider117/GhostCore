using System.Collections.Generic;

namespace GhostCore.Data.Evaluation;

public interface IEvaluatable
{
    DynamicTypeDefinition TypeDefinition { get; }

    object?[] GetOutputValues(EvaluationContext context);
    object? GetOutputValue(PortInfo port, EvaluationContext context);
    PortConnection ConnectToPort(string portName, IEvaluatable source, IConverter? converter = null);
    PortConnection ConnectToPort(string portName, IEvaluatable source, string sourcePortName, IConverter? converter = null);
    PortConnection ConnectToPort(PortInfo targetPort, IEvaluatable sourceObject, PortInfo sourcePort, IConverter? converter = null);
    PortInfo GetInputPort(string portName);
    PortInfo GetOutputPort(string portName);
}
