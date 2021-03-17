using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Animations.Controls
{
    public delegate Task AsyncTypedEventHandler<TSender, TResult>(TSender sender, TResult args);
}
