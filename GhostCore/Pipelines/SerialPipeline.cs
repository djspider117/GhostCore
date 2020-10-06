using GhostCore.Foundation;
using GhostCore.Services.Logging;
using System;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    /// <summary>
    /// This pipeline will execute the processors in serial (one after the other).
    /// </summary>
    public class SerialPipeline : PipelineBase, IDisposable
    {
        private PipelineProcessData _pdata;

        public SerialPipeline(string name = null) : base(name)
        {
        }

        /// <inheritdoc />
        public override async void Start(object sender, object sourceObject, params object[] pipelineArguments)
        {
            LogPipelineMessage("Starting pipeline.");
            _isRunning = true;

            LogPipelineMessage("Initializing cancellation token", LoggingLevel.Verbose);
            InitializeCancellationToken();

            _pdata = CreatePipelineProcessData(sourceObject, pipelineArguments);

            LogPipelineMessage("Starting processors...", LoggingLevel.Verbose);
            foreach (var ipp in _processors)
            {
                if (_pdata.CancellationToken.IsCancellationRequested)
                {
                    LogPipelineMessage($"Cancellation was requested. Ending pipeline.", LoggingLevel.Information);
                    FinishPipeline(_pdata, isSuccess: false);
                    return;
                }

                var procResult = await Process(ipp, _pdata);
                if (procResult.IsFaulted)
                    return;
            }

            if (_pdata.CancellationToken.IsCancellationRequested)
            {
                LogPipelineMessage($"Cancellation was requested. Ending pipeline.", LoggingLevel.Information);
                FinishPipeline(_pdata, isSuccess: false);
                return;
            }

            await ProcessEndpoint(_pdata);

            FinishPipeline(_pdata, isSuccess: true);
        }

        /// <inheritdoc />
        public override async Task Stop(object sender, bool rollback = false)
        {
            if (!_isRunning)
            {
                LogPipelineMessage("Tried calling stop when the pipeline was not running, ignoring.", LoggingLevel.Verbose);
                return;
            }

            LogPipelineMessage("Stopping pipeline.");
            _cancellationSource.Cancel();

            if (rollback)
            {
                LogPipelineMessage($"Starting rollback...");
                foreach (var ipp in _processors)
                {
                    ISafeTaskResult result = default;

                    try
                    {
                        LogPipelineMessage($"Starting rollback for processor {ipp.Name}.");
                        result = await ipp.Rollback(_pdata);
                        LogPipelineMessage($"Rollback complete for processor {ipp.Name}.");
                    }
                    catch (Exception ex)
                    {
                        LogPipelineMessage($"Critical rollback pipeline failure! Details: {ex}", LoggingLevel.Critical);

                        FinishPipeline(_pdata, isSuccess: false);
                        return;
                    }

                    if (result.IsFaulted)
                    {
                        LogPipelineMessage($"Rollback for processor {ipp.Name} has failed because: {result.FailReason}. For more details, change the logging mode to {nameof(LoggingLevel.Information)}.", LoggingLevel.Warning);

                        if (result.DetailedException != null)
                            LogPipelineMessage($"Details: {result.DetailedException}", LoggingLevel.Information);

                        FinishPipeline(_pdata, isSuccess: false);
                        return;
                    }
                }
            }

            LogPipelineMessage($"Stop complete.", LoggingLevel.Verbose);
        }

        public void Dispose()
        {
            _cancellationSource?.Dispose();
        }
    }
}
