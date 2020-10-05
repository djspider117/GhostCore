using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public interface IPipelineEndpoint
    {
        Task FinishPipeline(PipelineProcessData pipelineProcessData);
    }
}
