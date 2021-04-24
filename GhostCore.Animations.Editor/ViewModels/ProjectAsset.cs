using GhostCore.MVVM;
using System.Collections.Generic;

namespace GhostCore.Animations.Editor.ViewModels
{
    public enum ColorGroup
    {
        Red,
        Green,
        Blue,
        Violet,
        Yellow,
        Orange,
        White,
        Gray,
        Brown
    }

    public class ProjectAsset : ViewModelBase
    {
        private bool _isExpanded;

        public string Name { get; set; } // TODO make inpc
        public string Type { get; set; } 
        public List<ProjectAsset> SubItems { get; set; } // TODO make obscol
        public ColorGroup Group { get; set; }
        public string Path { get; set; }
        public string FriendlyType
        {
            get
            {
                switch (Type)
                {
                    case nameof(FolderAsset): return "Folder";
                    case nameof(SvgAsset): return "SVG Asset";
                    case nameof(ImageAsset): return "Image Asset";
                    case nameof(SceneAsset): return "Scene";
                    default: return null;
                }
            }
        }

        public bool CanNestChildren { get; protected set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }

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
