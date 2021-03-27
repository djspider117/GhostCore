using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace GhostCore.Animations
{
    public enum WeightedMode : byte
    {
        None,
        In,
        Out,
        Both
    }

    public enum AnimationWrapMode
    {
        PlayOnce,
        Loop,
        PingPong
    }

    public enum CompositionWrapMode
    {
        PlayAndStop,
        PlayAndReset,
        Loop
    }
}
