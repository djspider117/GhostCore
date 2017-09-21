using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore
{
    public sealed class CoreConfiguration
    {
        public static int RETRY_COUNT { get; private set; }

        static CoreConfiguration()
        {
            InitializeDefaults();
        }

        private CoreConfiguration()
        {
        }

        public static void InitializeDefaults()
        {
            RETRY_COUNT = 3;
        }
    }
}
