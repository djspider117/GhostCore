using GhostCore.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private void Initialize()
        {
            _pipelines = new List<IPipeline>();
        }

        public IPipeline GetOrCreatePipeline(string name, DefaultPipelineType type = DefaultPipelineType.Serial)
        {
            var existing = _pipelines.FirstOrDefault(x => x.Name == name);
            if (existing != null)
                return existing;

            if (type == DefaultPipelineType.Serial)
                return CreateSerialPipeline(name);
            else
                return CreateParallelPipeline(name);
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
            return pipe;
        }

        public ParallelPipeline CreateParallelPipeline(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Pipeline must have a valid name.");

            var pipe = new ParallelPipeline(name);
            Logger.LogInfo($"[PipelineManager] Adding parallel pipeline {name}");
            _pipelines.Add(pipe);
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
