﻿using GhostCore.Animations.Editor.ViewModels;
using System;
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

        public EditorUserControl()
        {
            Loaded += EditorUserControl_Loaded;
            Unloaded += EditorUserControl_Unloaded;
        }

        private void EditorUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            OnLoadedInternal(sender, e);
        }

        private void EditorUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= EditorUserControl_Loaded;
            Unloaded -= EditorUserControl_Unloaded;
            OnUnloadedInternal(sender, e);
        }

        protected virtual void OnLoadedInternal(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnUnloadedInternal(object sender, RoutedEventArgs e)
        {
        }
    }
}
