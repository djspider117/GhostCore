using System.Threading;

namespace GhostCore.Pipelines
{
    public class PipelineProcessData
    {
        public bool Cancel { get; set; }
        public IPipeline Pipeline { get; set; }
        public object SourceObject { get; set; }
        public object ProcessedObject { get; set; }
        public object[] PipelineArguments { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public T SourceAs<T>() => (T)SourceObject;
        public T DataAs<T>() => (T)ProcessedObject;
    }
}
