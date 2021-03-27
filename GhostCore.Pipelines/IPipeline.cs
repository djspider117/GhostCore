using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    /// <summary>
    /// Manages the flow and execution of multiple tasks (<see cref="IPipelineProcessor" />) that work together to do something more complicated.
    /// </summary>
    /// <remarks>
    /// TODO: add remarks and examples
    /// </remarks>
    public interface IPipeline
    {
        /// <summary>
        /// Event that is triggered when the pipeline has finished execution.
        /// </summary>
        event EventHandler<PipelineFinishedEventArgs> Finished;

        /// <summary>
        /// Event that is triggered when the pipeline status has changed.
        /// </summary>
        event EventHandler<string> StatusChanged;

        /// <summary>
        /// Event that is triggered when the progress status of the pipeline has changed.
        /// </summary>
        event EventHandler<double> Progress;

        /// <summary>
        /// Gets the name of the pipeline.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IReadOnlyCollection{IPipelineProcessor}"/> of processors in the pipeline.
        /// </summary>
        IReadOnlyCollection<IPipelineProcessor> Processors { get; }

        /// <summary>
        /// Gets or sets the <see cref="IPipelineEndpoint"/> object that can handle the ending of the pipeline.
        /// </summary>
        /// <remarks>
        /// Setting this object does not affect the behaviour of the <see cref="Finished"/> event. It will still trigger.
        /// This property is also optional. If the endpoint is null when the pipeline executes, this final step of the 
        /// pipeline will be skipped, and <see cref="Finished"/> will be invoked imediatly
        /// </remarks>
        IPipelineEndpoint Endpoint { get; set; }

        /// <summary>
        /// Starts the pipeline.
        /// </summary>
        /// <param name="sender">The object that started the pipeline. Should usually be <see langword="this"/> on the caller.</param>
        void Start(object sender);

        /// <inheritdoc cref="Start(object)"/>
        /// <param name="sourceObject">The main data object that will be passed in the pipeline to be processed.</param>
        void Start(object sender, object sourceObject);

        /// <inheritdoc cref="Start(object, object)"/>
        /// <param name="pipelineArguments">An optional parameter list with different arguments that the pipeline can make use of. Think of this like ENV_VARs</param>
        void Start(object sender, object sourceObject, params object[] pipelineArguments);

        /// <summary>
        /// Stops the pipeline from running. Stopping the pipeline always results in a 
        /// <see cref="PipelineFinishedEventArgs.IsSuccess"/> to be false when the <see cref="Finished"/> event is triggered.
        /// </summary>
        /// <param name="sender">The object that started the pipeline. Should usually be <see langword="this"/> on the caller.</param>
        /// <param name="rollback">True if the pipeline should rollback changes, false otherwise. 
        /// A pipeline does not guarrentee that it can be rolled back. It depends on each pipeline processor implementation.
        /// </param>
        /// <returns>A task representing the async task.</returns>
        Task Stop(object sender, bool rollback = false);

        /// <summary>
        /// Adds a <see cref="IPipelineProcessor"/> to the current list of pipeline processors.
        /// </summary>
        /// <remarks>
        /// If the pipeline is currently running, a <see cref="InvalidOperationException"/> will be thrown.
        /// You can add the same <see cref="IPipelineProcessor"/> multiple times.
        /// </remarks>
        /// <param name="processor">The processor to be added.</param>
        void AddProcessor(IPipelineProcessor processor);

        /// <summary>
        /// Removes a <see cref="IPipelineProcessor"/> instance from the current pipeline.
        /// </summary>
        /// <remarks>
        /// If the pipeline is currently running, a <see cref="InvalidOperationException"/> will be thrown.
        /// </remarks>
        /// <param name="pipelineProcessor">The processor to be removed.</param>
        bool RemoveProcessor(IPipelineProcessor pipelineProcessor);

        /// <summary>
        /// Adds a <see cref="RelayPipelineProcessor"/> based on the <paramref name="processMethod"/> func provided to the current list of pipeline processors.
        /// </summary>
        /// <remarks>
        /// If the pipeline is currently running, a <see cref="InvalidOperationException"/> will be thrown.
        /// You can add the same <see cref="IPipelineProcessor"/> multiple times.
        /// </remarks>
        /// <param name="processMethod">A callback to a process method.</param>
        RelayPipelineProcessor AddProcessor(Func<PipelineProcessData, Task<ISafeTaskResult>> processMethod);

        /// <inheritdoc cref="AddProcessor(Func{PipelineProcessData, Task{ISafeTaskResult}})"/>
        /// <param name="rollbackMethod">A callback to a rollback method.</param>
        RelayPipelineProcessor AddProcessor(Func<PipelineProcessData, Task<ISafeTaskResult>> processMethod, Func<PipelineProcessData, Task<ISafeTaskResult>> rollbackMethod);
    }

    public enum DefaultPipelineType
    {
        Serial,
        Parallel
    }
}
