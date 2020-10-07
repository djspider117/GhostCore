using GhostCore.Foundation;
using System;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public abstract class PipelineProcessorBase : IPipelineProcessor
    {
        public event PipelineProgressEventHandler ProgressChanged;
        public event PipelineStatusEventHandler StatusChanged;

        public string Name { get; set; }

        public PipelineProcessorBase()
        {
            Name = GetType().Name;
        }

        public abstract Task<ISafeTaskResult> ProcessAsync(PipelineProcessData data);

        public virtual Task<ISafeTaskResult> Rollback(PipelineProcessData data)
        {
            throw new NotSupportedException();
        }

        protected virtual void OnProgressChanged(double value) => ProgressChanged?.Invoke(this, value);
        protected virtual void OnStatusChanged(string status) => StatusChanged?.Invoke(this, status);
    }
}
