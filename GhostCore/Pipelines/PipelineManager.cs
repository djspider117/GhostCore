using GhostCore.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GhostCore.Pipelines
{
    public class PipelineManager
    {
        #region Singleton

        private static volatile PipelineManager _instance;
        private static object _syncRoot = new object();

        public static PipelineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new PipelineManager();
                    }
                }

                return _instance;
            }
        }
        private PipelineManager()
        {
            Initialize();
        }

        #endregion

        private List<IPipeline> _pipelines;
        private Dictionary<string, List<IPipelineProcessor>> _precachedStages;

        private void Initialize()
        {
            _pipelines = new List<IPipeline>();
            _precachedStages = new Dictionary<string, List<IPipelineProcessor>>();

            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = ass.GetTypes();
                var pipelineStages = types.Where(x => x.GetCustomAttribute<PipelineStageAttribute>() != null);


                foreach (var pipelineStageType in pipelineStages)
                {
                    var psas = pipelineStageType.GetCustomAttributes<PipelineStageAttribute>();

                    foreach (var psa in psas.OrderBy(x => x.StageOrder))
                    {
                        var defaultCtor = pipelineStageType.GetConstructor(Type.EmptyTypes);
                        var stage = (IPipelineProcessor)defaultCtor.Invoke(null);

                        if (_precachedStages.ContainsKey(psa.PipelineName))
                            _precachedStages[psa.PipelineName].Add(stage);
                        else
                            _precachedStages.Add(psa.PipelineName, new List<IPipelineProcessor> { stage });
                    }
                }
            }
        }

        public IPipeline GetOrCreatePipeline(string name, DefaultPipelineType type = DefaultPipelineType.Serial)
        {
            var existing = _pipelines.FirstOrDefault(x => x.Name == name);
            if (existing != null)
                return existing;

            IPipeline pipe = default;

            if (type == DefaultPipelineType.Serial)
                pipe = CreateSerialPipeline(name);
            else
                pipe = CreateParallelPipeline(name);

            return pipe;
        }

        public IPipeline GetPipeline(string name)
        {
            var existing = _pipelines.FirstOrDefault(x => x.Name == name);
            if (existing == null)
            {
                Logger.LogError($"[PipelineManager] No pipeline named {name} is registered");
                throw new InvalidOperationException($"No pipeline named {name} is registered.");
            }

            return existing;
        }

        public SerialPipeline CreateSerialPipeline(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Pipeline must have a valid name.");

            var pipe = new SerialPipeline(name);
            Logger.LogInfo($"[PipelineManager] Adding serial pipeline {name}");
            _pipelines.Add(pipe);


            if (_precachedStages.ContainsKey(name))
            {
                foreach (var proc in _precachedStages[name])
                    pipe.AddProcessor(proc);
            }


            return pipe;
        }

        public ParallelPipeline CreateParallelPipeline(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Pipeline must have a valid name.");

            var pipe = new ParallelPipeline(name);
            Logger.LogInfo($"[PipelineManager] Adding parallel pipeline {name}");
            _pipelines.Add(pipe);


            if (_precachedStages.ContainsKey(name))
            {
                foreach (var proc in _precachedStages[name])
                    pipe.AddProcessor(proc);
            }


            return pipe;
        }

        public void RegisterCustomPipeline(IPipeline pipeline)
        {
            if (pipeline == null)
                throw new ArgumentNullException(nameof(pipeline));

            if (string.IsNullOrWhiteSpace(pipeline.Name))
                throw new ArgumentException(nameof(pipeline), "Custom pipelines must have a name.");

            _pipelines.Add(pipeline);
        }

        public bool RemovePipeline(string name)
        {
            Logger.LogInfo($"[PipelineManager] Removing pipeline {name}");
            var pipe = _pipelines.FirstOrDefault(x => x.Name == name);
            if (pipe == null)
            {
                Logger.LogInfo("[PipelineManager] Tried removing pipeline that does not exist");
                return false;
            }

            var rv = _pipelines.Remove(pipe);

            if (pipe is IDisposable dsp)
                dsp.Dispose();

            return rv;
        }
    }
}
