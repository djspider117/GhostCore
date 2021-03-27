using System;

namespace GhostCore.Pipelines
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PipelineStageAttribute : Attribute
    {
        public string PipelineName { get; set; }
        public int StageOrder { get; set; }
    }
}
