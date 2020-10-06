using GhostCore.Foundation;
using GhostCore.Services.Logging;
using System;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public class RelayPipelineProcessor : IPipelineProcessor
    {
        /// <inheritdoc />
        public event PipelineProgressEventHandler ProgressChanged;

        protected readonly Func<PipelineProcessData, Task<ISafeTaskResult>> _action;
        protected readonly Func<PipelineProcessData, Task<ISafeTaskResult>> _rollback;
        protected readonly string _name;

        /// <inheritdoc />
        public string Name => _name;

        public RelayPipelineProcessor(Func<PipelineProcessData, Task<ISafeTaskResult>> action, Func<PipelineProcessData, Task<ISafeTaskResult>> rollback = null)
        {
            if (action == null)
            {
                Logger.LogVerbose($"Unable to create {nameof(RelayPipelineProcessor)} because given {nameof(ProcessAsync)} handler was null.");
                throw new ArgumentNullException(nameof(action), "Action cannot be null");
            }

            _action = action;
            _rollback = rollback;
            _name = GetType().Name;
        }

        /// <inheritdoc />
        public Task<ISafeTaskResult> ProcessAsync(PipelineProcessData data)
        {
            Logger.LogVerbose($"[{_name}] Invoking {nameof(ProcessAsync)}.");
            var rv = _action(data);
            Logger.LogVerbose($"[{_name}] {nameof(ProcessAsync)} invoked.");
            return rv;
        }

        /// <inheritdoc />
        /// <remarks>
        /// If the rollback callback was set in the ctor, it will be executed, if not, passthrough.
        /// </remarks>
        public Task<ISafeTaskResult> Rollback(PipelineProcessData data)
        {
            Logger.LogVerbose($"[{_name}] Invoking {nameof(Rollback)}.");
            if (_rollback == null)
            {
                Logger.LogVerbose($"[{_name}] No {nameof(Rollback)} handler detected, skipping with OK task result");
                return Task.FromResult(SafeTaskResult.Ok);
            }

            var rv = _rollback(data);
            Logger.LogVerbose($"[{_name}] {nameof(Rollback)} invoked.");
            return rv;
        }
    }
}
