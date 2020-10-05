using System;

namespace GhostCore.Pipelines
{
    public class PipelineFinishedEventArgs : EventArgs
    {
        public bool IsSuccess { get; set; }
        public object SourceObject { get; set; }
        public object FinalObject { get; set; }
        public object[] PipelineArguments { get; set; }
    }
}
