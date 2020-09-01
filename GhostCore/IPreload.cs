using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore
{
    public interface IPreload
    {
        string FriendlyDisplayName { get; }

        Task PreloadAsync();
    }
}
