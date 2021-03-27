using System.Collections.Generic;

namespace GhostCore.Pipelines
{
    public interface IPipelineRegistry
    {
        IReadOnlyList<IPipeline> Pipelines { get; }

        void Register(IPipeline pipeline);
        void Remove(string pipelineName);
        IPipeline Get(string pipelineName);
    }
}
