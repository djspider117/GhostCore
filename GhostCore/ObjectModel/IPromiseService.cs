using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GhostCore.ObjectModel
{
    public interface IPromiseService
    {
        Promise GetExistingPromiseForId(int id);
        Promise GetOrCreatePromiseForId(int id);
        Promise GetOrCreatePromiseForId(int id, Action<object> handler);
        void ReleasePromiseForId(int id);
    }
}
