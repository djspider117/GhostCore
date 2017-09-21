using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.DebugUtils
{
    public sealed class DebugLogger
    {
        private IDebugOutput _output;

        public static DebugLogger Default { get; private set; }

        static DebugLogger()
        {
            Default = new DebugLogger();
        }

        public DebugLogger()
        {
            _output = new StandardDebugOutput();
        }

        public DebugLogger(IDebugOutput output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            _output = output;
        }

        public void Log(object item)
        {
            _output.Log(item);
        }

        /// <summary>
        /// Prints a visual representation of the memory in the given buffer on the current debug output
        /// </summary>
        /// <param name="buffer">Data to be represented</param>
        /// <param name="maxNumberOfBytes">The maximum value that a represented buffer can have. Default (-1) is equal to the size of the buffer</param>
        /// <param name="noiseTolerance">The value at which the algorithm decides to put black character instead of ocupied character.</param>
        /// <param name="displayLenght">Numbers of characters that the visual representation should be. Default 20 characters</param>
        /// <param name="trailWhitespaces">Paramater that indicates if the visual representation should be followed up with 5 trailing whitespaces. Default true.</param>
        /// <param name="blank">The string used for blank/empty/thresholded memory</param>
        /// <param name="full">The string used for ocupied/non-empty/thresholded memory</param>
        public void PrintMemory(byte[] buffer, int maxNumberOfBytes = -1, byte noiseTolerance = 0, int displayLenght = 20, bool trailWhitespaces = true, string blank = "-", string full = "|")
        {
            if (maxNumberOfBytes == -1)
                maxNumberOfBytes = buffer.Length;

            double factor = (double)displayLenght / (double)maxNumberOfBytes;
            for (int i = 0; i < displayLenght; i++)
            {
                int projecttion = 0;
                if (i != 0)
                    projecttion = (int)(i / factor);

                bool hasMem = false;

                for (int o = 0; o < maxNumberOfBytes / displayLenght; o++)
                {
                    if (buffer[o + projecttion] >= noiseTolerance)
                    {
                        hasMem = true;
                        break;
                    }
                }

                if (!hasMem)
                    _output.Write(blank);
                else
                    _output.Write(full);
            }

            if (trailWhitespaces)
                for (int i = 0; i < 5; i++)
                {
                    System.Diagnostics.Debug.Write(" ");
                }
        }

    }
}
