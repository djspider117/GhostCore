using GhostCore.Foundation;
using GhostCore.Services.Logging;
using System;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public class RelayPipelineProcessor : PipelineProcessorBase
    {
        protected readonly Func<PipelineProcessData, Task<ISafeTaskResult>> _action;
        protected readonly Func<PipelineProcessData, Task<ISafeTaskResult>> _rollback;

        /// <inheritdoc />
        public RelayPipelineProcessor(Func<PipelineProcessData, Task<ISafeTaskResult>> action, Func<PipelineProcessData, Task<ISafeTaskResult>> rollback = null)
        {
            if (action == null)
            {
                Logger.LogVerbose($"Unable to create {nameof(RelayPipelineProcessor)} because given {nameof(ProcessAsync)} handler was null.");
                throw new ArgumentNullException(nameof(action), "Action cannot be null");
            }

            _action = action;
            _rollback = rollback;
        }

        /// <inheritdoc />
        public override Task<ISafeTaskResult> ProcessAsync(PipelineProcessData data)
        {
            Logger.LogVerbose($"[{Name}] Invoking {nameof(ProcessAsync)}.");
            var rv = _action(data);
            Logger.LogVerbose($"[{Name}] {nameof(ProcessAsync)} invoked.");
            return rv;
        }

        /// <inheritdoc />
        /// <remarks>
        /// If the rollback callback was set in the ctor, it will be executed, if not, passthrough.
        /// </remarks>
        public override Task<ISafeTaskResult> Rollback(PipelineProcessData data)
        {
            Logger.LogVerbose($"[{Name}] Invoking {nameof(Rollback)}.");
            if (_rollback == null)
            {
                Logger.LogVerbose($"[{Name}] No {nameof(Rollback)} handler detected, skipping with OK task result");
                return Task.FromResult(SafeTaskResult.Ok);
            }

            var rv = _rollback(data);
            Logger.LogVerbose($"[{Name}] {nameof(Rollback)} invoked.");
            return rv;
        }
    }
}
