using System;

namespace GhostCore.Threading
{
    public interface IPromiseService
    {
        Promise GetExistingPromiseForId(int id);
        Promise GetOrCreatePromiseForId(int id);
        Promise GetOrCreatePromiseForId(int id, Action<object> handler);
        void ReleasePromiseForId(int id);
    }
}
