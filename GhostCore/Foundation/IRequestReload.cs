using System;

namespace GhostCore.Foundation
{
    public interface IRequestReload
    {
        event EventHandler ReloadRequested;

        bool LoadRequestPending { get; }

        void RequestReload();
        void FulfillReloadRequest();
    }
}
