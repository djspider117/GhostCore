using GhostCore.MVVM;
using System.Collections.Generic;

namespace GhostCore.Animations.Editor.ViewModels
{

    public class ProjectAsset : ViewModelBase
    {
        public string Name { get; set; } 
        public string Type { get; set; } 
        public List<ProjectAsset> SubItems { get; set; } 
        public ColorGroup Group { get; set; }
        public string Path { get; set; }

        public bool CanNestChildren { get; protected set; }

        public bool IsExpanded { get; set; }

        public ProjectAsset()
        {
            SubItems = new List<ProjectAsset>();
            Type = GetType().Name;
            Group = ColorGroup.White;
        }
    }

    public class FolderAsset : ProjectAsset
    {
        public FolderAsset()
        {
            Group = ColorGroup.Yellow;
            CanNestChildren = true;
        }
    }

    public class SvgAsset : ProjectAsset
    {
        public SvgAsset()
        {
            Group = ColorGroup.Green;
        }
    }

    public class ImageAsset : ProjectAsset
    {
        public ImageAsset()
        {
            Group = ColorGroup.White;
        }
    }

    public class SceneAsset : ProjectAsset
    {
        public SceneAsset()
        {
            Group = ColorGroup.Brown;
        }
    }
}
