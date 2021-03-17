using System;
using System.Collections.Generic;

namespace GhostCore.Animations.Layers
{
    public class CompositeLayer : LayerBase
    {
        public IList<ILayer> Children { get; set; }

        public CompositeLayer()
        {
            Children = new List<ILayer>();
        }

        internal void GetImageLayers(List<ImageLayer> lst)
        {
            if (Children == null)
                return;

            foreach (var x in Children)
            {
                if (x is ImageLayer il)
                    lst.Add(il);

                if (x is CompositeLayer cl)
                {
                    cl.GetImageLayers(lst);
                }
            }
        }
    }
}
