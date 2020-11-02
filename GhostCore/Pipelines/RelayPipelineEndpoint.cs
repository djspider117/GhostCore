using System;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public class RelayPipelineEndpoint : IPipelineEndpoint
    {
        private Func<PipelineProcessData, Task> _action;

        public RelayPipelineEndpoint(Func<PipelineProcessData, Task> action)
        {
            _action = action;
        }

        public Task FinishPipeline(PipelineProcessData pipelineProcessData)
        {
            return _action(pipelineProcessData);
        }
    }
}
