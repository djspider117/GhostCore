using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Foundation
{

    public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);
}
