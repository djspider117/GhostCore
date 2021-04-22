using GhostCore.Animations.Core;
using System.Collections.Generic;

namespace GhostCore.Animations.Rendering
{
    internal static class LayerExtensions
    {
        public static IEnumerable<IGeometryMask> GetGeometryMasks(this ILayer layer)
        {
            if (layer.Masks != null)
            {
                foreach (var mask in layer.Masks)
                {
                    var geomMask = GeometryMaskFactory.TryConvert(mask);
                    if (geomMask == null)
                        continue;

                    yield return geomMask;
                }
            }
        }
    }

}
