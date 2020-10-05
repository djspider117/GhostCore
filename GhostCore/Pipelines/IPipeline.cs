using System;
using System.Collections.Generic;

namespace GhostCore.Pipelines
{
    public interface IPipeline
    {
        event EventHandler<PipelineFinishedEventArgs> Finished;
        event EventHandler<double> Progress;

        string Name { get; }
        IReadOnlyCollection<IPipelineProcessor> Processors { get; }

        void Start(object sender);
        void Start(object sender, object sourceObject);
        void Start(object sender, object sourceObject, params object[] pipelineArguments);

        /// Stopping the pipeline ALWAYS results in a IsSuccess == false on Finished
        void Stop(object sender, bool rollback = false);

        void AddProcessor(IPipelineProcessor processor);
        void AddProcessor(Action<PipelineProcessData> processMethod);
    }
}
