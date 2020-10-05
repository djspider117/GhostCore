using GhostCore.Foundation;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public delegate void PipelineProgressEventHandler(IPipelineProcessor sender, double value);

    public interface IPipelineProcessor
    {
        event PipelineProgressEventHandler ProgressChanged;

        string Name { get; }
        Task<ISafeTaskResult> ProcessAsync(PipelineProcessData data);
        Task<ISafeTaskResult> Rollback(PipelineProcessData data);
    }
}
