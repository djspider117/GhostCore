/*
    This file is part of Project Emilie.
    Public redistribution is not permitted.
*/

using GhostCore.Numerics;
using System.Collections.Generic;
using Windows.UI.Composition;

namespace GhostCore.UWP.Media
{
    internal static class CompositionEasings
    {
        /// <summary>
        /// Be aware, easing functions are cached by default. When using make sure not to dispose cached functions.
        /// </summary>
        static Dictionary<int, CubicBezierEasingFunction> _cache { get; } = new Dictionary<int, CubicBezierEasingFunction>();

        internal static CubicBezierEasingFunction GetEasingFunction(this Compositor compositor, PennerType type, PennerVariation variation, bool cache = true)
        {
            int pennerType = (int)type;
            int pennerVariation = (int)variation;
            int sum = pennerType + pennerVariation;

            if (cache && _cache.TryGetValue(sum, out var cached))
                return cached;

            CubicBezierControlPoints controlPoints = CubicBezierControlPoints.CreatePenner(type, variation);
            CubicBezierEasingFunction function = compositor.CreateCubicBezierEasingFunction(controlPoints);

            if (cache)
                _cache.Add(sum, function);

            return function;
        }
    }
}
