using GhostCore.Animations.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostCore.Animations.Data.Layers
{
    public class SceneReferenceLayer : LayerBase, ICompositeLayer, ISimpleAsyncInitializable
    {
        public string Source { get; set; }
        public SceneReferenceLayerSourceType SourceType { get; set; }
        public IList<ILayer> Children { get; set; }

        public Task InitializeAsync()
        {
            // TODO
            return Task.CompletedTask;
        }

        public IEnumerable<IResourceDependentLayer> ExtractResourceDependentLayers()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum SceneReferenceLayerSourceType
    {
        SerializedString,
        RelativeFilePath
    }
}
