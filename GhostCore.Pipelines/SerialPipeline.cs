
using GhostCore.Logging;
using System;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    /// <summary>
    /// This pipeline will execute the processors in serial (one after the other).
    /// </summary>
    public class SerialPipeline : PipelineBase, IDisposable
    {
        private const string E_PIPELINE_CANCEL = "Cancellation was requested. Ending pipeline.";
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

            // BIG HACK pls fix
            int retryCount = 100;
            while (_pdata != null && retryCount > 0)
            {
                await Task.Delay(25);
                retryCount--;
            }

            _pdata = CreatePipelineProcessData(sourceObject, pipelineArguments);
            _pdata.PipelineStarter = sender;

            LogPipelineMessage("Starting processors...", LoggingLevel.Verbose);
            foreach (var ipp in _processors)
            {
                if (_pdata.CancellationToken.IsCancellationRequested)
                {
                    LogPipelineMessage(E_PIPELINE_CANCEL, LoggingLevel.Information);
                    FinishPipeline(_pdata, isSuccess: false, E_PIPELINE_CANCEL);
                    return;
                }

                var procResult = await Process(ipp, _pdata);
                if (procResult.IsFaulted)
                    return;

            }

            if (_processors.Count == 0 && _pdata.ProcessedObject == null)
            {
                _pdata.ProcessedObject = _pdata.SourceObject;
            }

            if (_pdata.CancellationToken.IsCancellationRequested)
            {
                LogPipelineMessage(E_PIPELINE_CANCEL, LoggingLevel.Information);
                FinishPipeline(_pdata, isSuccess: false, E_PIPELINE_CANCEL);
                return;
            }


            FinishPipeline(_pdata);
            _pdata = null;
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
                        string msg = $"Critical rollback pipeline failure! Details: {ex}";
                        LogPipelineMessage(msg, LoggingLevel.Critical);

                        FinishPipeline(_pdata, isSuccess: false, msg);
                        return;
                    }

                    if (result.IsFaulted)
                    {
                        var msg = $"Rollback for processor {ipp.Name} has failed because: {result.FailReason}.";
                        LogPipelineMessage($"{msg} For more details, change the logging mode to {nameof(LoggingLevel.Information)}.", LoggingLevel.Warning);

                        if (result.DetailedException != null)
                            LogPipelineMessage($"Details: {result.DetailedException}", LoggingLevel.Information);

                        FinishPipeline(_pdata, isSuccess: false, msg);
                        return;
                    }
                }
            }

            LogPipelineMessage($"Stop complete.", LoggingLevel.Verbose);
        }

        public override void Dispose()
        {
            base.Dispose();
            _cancellationSource?.Dispose();
        }
    }
}
