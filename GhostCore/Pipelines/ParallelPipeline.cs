using GhostCore.Foundation;
using GhostCore.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostCore.Pipelines
{
    /// <summary>
    /// This pipeline will execute the processors in parallel (more than one at a time).
    /// Internall this uses <see cref="Parallel.ForEach{TSource}(System.Collections.Generic.IEnumerable{TSource}, Action{TSource})"/>.
    /// </summary>
    /// <remarks>
    /// This pipeline type does not support rollbacks.
    /// </remarks>
    public class ParallelPipeline : PipelineBase
    {
        public ParallelPipelineFinishOptions FinishOptions { get; set; }
        public ParallelPipelineTaskFailHandling TaskFailHandling { get; set; }

        public ParallelPipeline(string name = null) : base(name)
        {
        }

        public override async void Start(object sender, object sourceObject, params object[] pipelineArguments)
        {
            if (TaskFailHandling == ParallelPipelineTaskFailHandling.SucceedAll && FinishOptions != ParallelPipelineFinishOptions.FinishWhenAll)
            {
                var msg = $@"Failed to start pipeline due to invalid combination of {nameof(FinishOptions)} and {nameof(TaskFailHandling)}. 
                                    {nameof(ParallelPipelineTaskFailHandling.SucceedAll)} and {nameof(ParallelPipelineFinishOptions.FinishWhenAll)} is not a valid combination.";
                LogPipelineMessage(msg, LoggingLevel.Error);

                throw new InvalidOperationException(msg);
            }

            LogPipelineMessage("Starting pipeline.");
            _isRunning = true;

            LogPipelineMessage("Initializing cancelletion token", LoggingLevel.Verbose);
            InitializeCancellationToken();


            LogPipelineMessage("Starting processors...", LoggingLevel.Verbose);

            var tasks = new List<Task<ISafeTaskResult>>();
            var pdatas = new Dictionary<Task, PipelineProcessData>();
            foreach (var ipp in _processors)
            {
                var pdata = CreatePipelineProcessData(sourceObject, pipelineArguments);
                var task = Process(ipp, pdata);
                pdatas.Add(task, pdata);
                tasks.Add(task);
            }

            try
            {
                switch (FinishOptions)
                {
                    case ParallelPipelineFinishOptions.FinishWhenAll:
                        var taskResults = await Task.WhenAll(tasks);

                        if (TaskFailHandling == ParallelPipelineTaskFailHandling.SucceedAll && taskResults.Any(x => x.IsFaulted) ||
                            TaskFailHandling == ParallelPipelineTaskFailHandling.SucceedAny && taskResults.All(x => x.IsFaulted))
                        {
                            LogPipelineMessage("Processors failed when executing in parallel.", LoggingLevel.Warning);

                            FinishPipeline(sourceObject, Array.Empty<object>(), pipelineArguments, isSuccess: false);
                            return;
                        }

                        var validResults = from task in taskResults
                                           from pdata in pdatas
                                           where !task.IsFaulted && task == pdata.Key
                                           select pdata.Value.ProcessedObject;

                        var endObj = CreatePipelineProcessData(sourceObject, pipelineArguments);
                        endObj.ProcessedObject = validResults.ToArray();

                        await ProcessEndpoint(endObj);
                        FinishPipeline(endObj, isSuccess: true);

                        break;
                    case ParallelPipelineFinishOptions.FinishWhenAny:

                        var finishedTask = await Task.WhenAny(tasks);

                        do
                        {
                            if (finishedTask.Result.IsFaulted)
                            {
                                tasks.Remove(finishedTask);
                                finishedTask = await Task.WhenAny(tasks);
                            }
                        }
                        while (tasks.Any(x => !x.IsCompleted) && finishedTask.Result.IsFaulted);

                        var taskResult = finishedTask.Result;

                        if (TaskFailHandling == ParallelPipelineTaskFailHandling.SucceedAny && taskResult.IsFaulted)
                        {
                            LogPipelineMessage("Processors failed when executing in parallel.", LoggingLevel.Warning);

                            FinishPipeline(sourceObject, Array.Empty<object>(), pipelineArguments, isSuccess: false);
                            return;
                        }

                        var pdataObj = pdatas[finishedTask];

                        await ProcessEndpoint(pdataObj);
                        FinishPipeline(sourceObject, pdatas[finishedTask].ProcessedObject, pipelineArguments, isSuccess: true);

                        break;
                    default:
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                LogPipelineMessage($"The pipeline task was canceled and halted.", LoggingLevel.Information);

                FinishPipeline(sourceObject, Array.Empty<object>(), pipelineArguments, isSuccess: false);
                return;
            }
            catch (Exception ex)
            {
                LogPipelineMessage($"Critical pipeline failure! Details: {ex}", LoggingLevel.Critical);

                FinishPipeline(sourceObject, Array.Empty<object>(), pipelineArguments, isSuccess: false);
                return;
            }
        }

        public override Task Stop(object sender, bool rollback = false)
        {
            throw new NotSupportedException();
        }
    }

    public enum ParallelPipelineFinishOptions
    {
        FinishWhenAll,
        FinishWhenAny
    }

    public enum ParallelPipelineTaskFailHandling
    {
        SucceedAll,
        SucceedAny
    }
}
