using GhostCore.Animations.Editor.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GhostCore.Animations.Editor.Controls
{
    public class EditorUserControl : UserControl
    {
        public static readonly DependencyProperty CurrentProjectProperty =
            DependencyProperty.Register(nameof(CurrentProject), typeof(ProjectViewModel), typeof(EditorUserControl), new PropertyMetadata(null));

        public ProjectViewModel CurrentProject
        {
            get { return (ProjectViewModel)GetValue(CurrentProjectProperty); }
            set { SetValue(CurrentProjectProperty, value); }
        }
    }
}
