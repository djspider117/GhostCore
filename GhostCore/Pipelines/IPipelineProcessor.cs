using GhostCore.Foundation;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public delegate void PipelineProgressEventHandler(IPipelineProcessor sender, double value);

    public interface IPipelineProcessor
    {
        /// <summary>
        /// Triggered when the progress of the processor changes.
        /// </summary>
        event PipelineProgressEventHandler ProgressChanged;

        /// <summary>
        /// Gets the name of the processor.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The entry point of a pipeline processor. It is called automatically by the pipeline. It should not be manually called outside of a pipeline.
        /// </summary>
        /// <param name="data">The interchange object between the pipeline processors.</param>
        /// <returns>A task representing the async operation.</returns>
        Task<ISafeTaskResult> ProcessAsync(PipelineProcessData data);


        /// <summary>
        /// This method is called automatically by the pipeline when it is forcefully stopped.
        /// </summary>
        /// <remarks>
        /// This method does not need an implementation unless it is deemed necessary by the implementor.</remarks>
        /// <param name="data">The interchange object between the pipeline processors.</param>
        /// <returns>A task representing the async operation.</returns>
        Task<ISafeTaskResult> Rollback(PipelineProcessData data);
    }
}
