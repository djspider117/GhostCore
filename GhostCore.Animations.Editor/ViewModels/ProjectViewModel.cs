using GhostCore.Animations.Data;
using GhostCore.MVVM;
using System.Collections.Generic;
using System.Linq;

namespace GhostCore.Animations.Editor.ViewModels
{

    public class ProjectViewModel : ViewModelBase<Project>
    {
        private SceneViewModel _selectedScene;

        public ViewModelCollection<SceneViewModel, Scene> Scenes { get; set; }
        public ViewModelCollection<ProjectAssetViewModel, ProjectAsset> Assets { get; set; }

        public SceneViewModel SelectedScene
        {
            get { return _selectedScene; }
            set { _selectedScene = value; OnPropertyChanged(nameof(SelectedScene)); }
        }

        public ProjectViewModel(Project model)
            : base(model)
        {
            Assets = new ViewModelCollection<ProjectAssetViewModel, ProjectAsset>(model.Assets);
            Scenes = new ViewModelCollection<SceneViewModel, Scene>(model.Scenes);
            _selectedScene = Scenes[0];
        }
    }

}
