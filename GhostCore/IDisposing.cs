﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore
{
    public interface IDisposing
    {
        event EventHandler Disposing;
    }
}
