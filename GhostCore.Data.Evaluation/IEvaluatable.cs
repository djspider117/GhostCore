using System.Collections.Generic;

namespace GhostCore.Data.Evaluation;

public interface IEvaluatable
{
    DynamicTypeDefinition TypeDefinition { get; }

    object?[] GetOutputValues(EvaluationContext context);
    object? GetOutputValue(PortInfo port, EvaluationContext context);
}
