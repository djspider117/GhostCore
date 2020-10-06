using GhostCore.Foundation;
using GhostCore.Services.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    public abstract class PipelineBase : IPipeline
    {
        public event EventHandler<PipelineFinishedEventArgs> Finished;
        public event EventHandler<double> Progress;


        protected CancellationTokenSource _cancellationSource;
        protected CancellationToken _cancellationToken;
        protected string _name;
        protected volatile bool _isRunning;
        protected List<IPipelineProcessor> _processors;

        public string Name => _name;
        public IReadOnlyCollection<IPipelineProcessor> Processors => _processors;
        public IPipelineEndpoint Endpoint { get; set; }

        protected PipelineBase(string name = null)
        {
            _name = name ?? GetType().Name;
            _processors = new List<IPipelineProcessor>();
        }

        public void AddProcessor(IPipelineProcessor processor)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), "Added processor cannot be null");

            if (_isRunning)
                throw new InvalidOperationException("Cannot add processor while the pipeline is running");

            _processors.Add(processor);
        }
        public bool RemoveProcessor(IPipelineProcessor pipelineProcessor) => _processors.Remove(pipelineProcessor);
        public RelayPipelineProcessor AddProcessor(Func<PipelineProcessData, Task<ISafeTaskResult>> processMethod) => AddProcessor(processMethod, null);
        public RelayPipelineProcessor AddProcessor(Func<PipelineProcessData, Task<ISafeTaskResult>> processMethod, Func<PipelineProcessData, Task<ISafeTaskResult>> rollbackMethod)
        {
            var rp = new RelayPipelineProcessor(processMethod, rollbackMethod);
            AddProcessor(rp);
            return rp;
        }

        public void Start(object sender) => Start(sender, null, null);

        public void Start(object sender, object sourceObject) => Start(sender, sourceObject, null);

        public abstract void Start(object sender, object sourceObject, params object[] pipelineArguments);
        public abstract Task Stop(object sender, bool rollback = false);

        protected virtual PipelineProcessData CreatePipelineProcessData(object sourceObject, object[] pipelineArguments)
        {
            LogPipelineMessage("Creating pipeline process data.", LoggingLevel.Verbose);
            return new PipelineProcessData
            {
                CancellationToken = _cancellationToken,
                Pipeline = this,
                PipelineArguments = pipelineArguments,
                SourceObject = sourceObject,
                Cancel = false,
                ProcessedObject = null
            };
        }

        protected virtual async Task<ISafeTaskResult> Process(IPipelineProcessor ipp, PipelineProcessData pdata)
        {
            ISafeTaskResult result = default;

            try
            {
                LogPipelineMessage($"Starting processor {_name}", LoggingLevel.Verbose);
                result = await ipp.ProcessAsync(pdata);
            }
            catch (TaskCanceledException)
            {
                var rv = new SafeTaskResult($"The pipeline task was canceled and {ipp.Name} was halted.");
                LogPipelineMessage(rv.FailReason, LoggingLevel.Information);
                FinishPipeline(pdata, isSuccess: false);
                return rv;
            }
            catch (Exception ex)
            {
                var rv = new SafeTaskResult($"Critical pipeline failure! Details: {ex}");
                LogPipelineMessage(rv.FailReason, LoggingLevel.Critical);

                FinishPipeline(pdata, isSuccess: false);
                return rv;
            }

            if (result.IsFaulted)
            {
                var rv = new SafeTaskResult($"Processor {ipp.Name} has failed because: {result.FailReason}.", result.DetailedException);
                LogPipelineMessage($"{rv.FailReason}. For more details, change the logging mode to {nameof(LoggingLevel.Information)}.", LoggingLevel.Warning);

                if (result.DetailedException != null)
                    LogPipelineMessage($"Details: {result.DetailedException}", LoggingLevel.Information);

                FinishPipeline(pdata, isSuccess: false);
                return rv;
            }

            return result;
        }


        protected virtual async Task ProcessEndpoint(PipelineProcessData pdata)
        {
            if (Endpoint != null)
            {
                try
                {
                    await Endpoint.FinishPipeline(pdata);
                }
                catch (TaskCanceledException)
                {
                    LogPipelineMessage($"The pipeline task was canceled and the endpoint was halted.", LoggingLevel.Information);

                    FinishPipeline(pdata, isSuccess: false);
                    return;
                }
                catch (Exception ex)
                {
                    LogPipelineMessage($"Critical pipeline failure! Details: {ex}", LoggingLevel.Critical);

                    FinishPipeline(pdata, isSuccess: false);
                    return;
                }
            }
        }


        protected void InitializeCancellationToken()
        {
            if (_cancellationSource != null)
            {
                _cancellationSource.Dispose();
                _cancellationSource = null;
            }

            _cancellationSource = new CancellationTokenSource();
            _cancellationToken = _cancellationSource.Token;
        }

        protected void OnFinished(PipelineFinishedEventArgs e) => Finished?.Invoke(this, e);

        protected virtual void LogPipelineMessage(string msg, LoggingLevel level = LoggingLevel.Information)
        {
            if (string.IsNullOrWhiteSpace(_name))
                Logger.LogInfo($"[{nameof(SerialPipeline)}] Starting pipeline.");
            else
                Logger.LogInfo($"[{nameof(SerialPipeline)}, Name = {_name}] Starting pipeline.");
        }

        protected void FinishPipeline(PipelineProcessData pdata, bool isSuccess) => FinishPipeline(pdata.SourceObject, pdata.ProcessedObject, pdata.PipelineArguments, isSuccess);
        protected virtual void FinishPipeline(object sourceObj, object procObj, object[] pipelineArgs, bool isSuccess)
        {
            _isRunning = false;
            _cancellationSource.Dispose();

            LogPipelineMessage($"Invoking the {nameof(Finished)} event with the args being the current pipeline process data.", LoggingLevel.Verbose);
            OnFinished(new PipelineFinishedEventArgs()
            {
                IsSuccess = isSuccess,
                PipelineArguments = pipelineArgs,
                SourceObject = sourceObj,
                FinalObject = procObj
            });
        }
    }
}
