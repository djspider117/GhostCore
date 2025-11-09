using System;

namespace GhostCore.Data.Evaluation;

public interface IConverter
{
    object? Convert(object? data, Type? sourceType, Type targetType);
}