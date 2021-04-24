using GhostCore.MVVM;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ProjectViewModel _currentProject;

        public ToolbarViewModel ToolbarViewModel { get; private set; }

        public ProjectViewModel CurrentProject
        {
            get { return _currentProject; }
            set { _currentProject = value; OnPropertyChanged(nameof(CurrentProject)); }
        }

        public MainPageViewModel()
        {
            ToolbarViewModel = new ToolbarViewModel();
            _currentProject = new ProjectViewModel();
        }
    }

}
