using GhostCore.MVVM;
using System;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class ProjectAssetViewModel : ViewModelBase<ProjectAsset>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string Path
        {
            get { return Model.Path; }
            set { Model.Path = value; OnPropertyChanged(nameof(Path)); }
        }
        public bool IsExpanded
        {
            get { return Model.IsExpanded; }
            set { Model.IsExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }
        public ColorGroup Group
        {
            get { return Model.Group; }
            set { Model.Group = value; OnPropertyChanged(nameof(Group)); }
        }
        public string Type
        {
            get { return Model.Type; }
            set { Model.Type = value; OnPropertyChanged(nameof(Type)); }
        }

        public string FriendlyType
        {
            get
            {
                switch (Model.Type)
                {
                    case nameof(FolderAsset): return "Folder";
                    case nameof(SvgAsset): return "SVG Asset";
                    case nameof(ImageAsset): return "Image Asset";
                    case nameof(SceneAsset): return "Scene";
                    default: return null;
                }
            }
        }
        public bool CanNestChildren => Model.CanNestChildren;

        public ViewModelCollection<ProjectAssetViewModel, ProjectAsset> SubItems { get; set; }

        public ProjectAssetViewModel(ProjectAsset model)
            : base(model)
        {
            if (model.SubItems == null)
                throw new ArgumentNullException(nameof(model.SubItems));

            SubItems = new ViewModelCollection<ProjectAssetViewModel, ProjectAsset>(model.SubItems);
        }
    }


    public class FolderAssetViewModel : ProjectAssetViewModel
    {
        public FolderAssetViewModel(FolderAsset model)
            : base(model)
        {
        }
    }

    public class SvgAssetViewModel : ProjectAssetViewModel
    {
        public SvgAssetViewModel(SvgAsset model)
            : base(model)
        {
        }
    }

    public class ImageAssetViewModel : ProjectAssetViewModel
    {
        public ImageAssetViewModel(ImageAsset model)
            : base(model)
        {
        }
    }
    public class SceneAssetViewModel : ProjectAssetViewModel
    {
        public SceneAssetViewModel(SceneAsset model)
            : base(model)
        {
        }
    }
}
