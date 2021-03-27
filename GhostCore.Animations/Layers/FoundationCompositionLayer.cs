namespace GhostCore.Animations.Layers
{
    public class FoundationCompositionLayer : LayerBase
    {
        public string Source { get; set; }
        public FoundationCompositionLayerSourceType SourceType { get; set; }
    }

    public enum FoundationCompositionLayerSourceType
    {
        XmlString,
        RelativeFilePath
    }
}
