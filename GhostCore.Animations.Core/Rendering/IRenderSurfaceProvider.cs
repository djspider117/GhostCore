using System;
using System.Threading.Tasks;

namespace GhostCore.Animations.Core
{
    public interface IRenderSurfaceProvider : IDisposable
    {
        Task<ISafeTaskResult<IRenderSurface>> CreateSurface();
    }

}
