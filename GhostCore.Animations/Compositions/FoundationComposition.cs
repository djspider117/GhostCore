using GhostCore.Animations.Layers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace GhostCore.Animations
{
    public class FoundationComposition : IFoundationComposition, IFoundationComposition2, IFoundationComposition3, IFoundationComposition4, IRuntimeFoundationComposition
    {
        public IList<ILayer> Layers { get; set; }
        public Size RenderSize { get; set; }
        public float RenderScale { get; set; }
        public float FrameRate { get; set; }
        public float Duration { get; set; }
        public Color BackgroundColor { get; set; }
        public CompositionWrapMode WrapMode { get; set; }

        public IList<ILayer> ExplodedLayers { get; set; }
        public string Name { get; set; }

        public IEnumerable<ImageLayer> GetAllImageLayers()
        {
            var lst = new List<ImageLayer>();
            foreach (var x in Layers)
            {
                if (x is ImageLayer il)
                    lst.Add(il);

                if (x is CompositeLayer cl)
                {
                    cl.GetImageLayers(lst);
                }
            }

            return lst;

            //return Layers.Where(x => x is ImageLayer).Cast<ImageLayer>();
        }

        public async Task<IList<ILayer>> ExplodePrecomps()
        {
            var newLayerList = new List<ILayer>();
            foreach (var layer in Layers)
            {
                if (layer is FoundationCompositionLayer precomp)
                {
                    var rd = FoundationCompositionReader.GetDefault();
                    switch (precomp.SourceType)
                    {
                        case FoundationCompositionLayerSourceType.XmlString:
                            var comp = await rd.ReadXmlString(precomp.Source);
                            newLayerList.AddRange(await comp.ExplodePrecomps());
                            break;
                        case FoundationCompositionLayerSourceType.RelativeFilePath:
                            var filecomp = await rd.ReadFile(precomp.Source);
                            newLayerList.AddRange(await filecomp.ExplodePrecomps());
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    newLayerList.Add(layer);
                }
            }

            return newLayerList;
        }
    }
}
