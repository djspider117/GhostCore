using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GhostCore.Animations.Editor.Controls
{
    public sealed partial class ProjectEditor : EditorUserControl
    {
        private List<Grid> _treeViewItemRootGrids = new List<Grid>();
        public ProjectEditor()
        {
            InitializeComponent();
        }

        #region Tree Column Resizing

        private void TreeViewItemContentPresenterGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
            grid.Unloaded += TreeViewItemContentPresenterGrid_Unloaded;

            _treeViewItemRootGrids.Add(grid);

            int i = 0;
            foreach (var item in grid.ColumnDefinitions)
            {
                item.Width = pnlTreeColumnIndicator.ColumnDefinitions[i].Width;
                i++;
            }
        }
        private void TreeViewItemContentPresenterGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
            grid.Unloaded -= TreeViewItemContentPresenterGrid_Unloaded;

            _treeViewItemRootGrids.Remove(grid);
        }

        private void pnlTreeColumnIndicator_Loaded(object sender, RoutedEventArgs e)
        {
            pnlTreeColumnIndicator.ColumnDefinitions[0].RegisterPropertyChangedCallback(ColumnDefinition.WidthProperty, NameColumnWidthPropertyChanged);
            pnlTreeColumnIndicator.ColumnDefinitions[2].RegisterPropertyChangedCallback(ColumnDefinition.WidthProperty, TypeColumnWidthPropertyChanged);
        }

        private void TypeColumnWidthPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            foreach (var grid in _treeViewItemRootGrids)
            {
                grid.ColumnDefinitions[2].Width = (GridLength)sender.GetValue(dp);
            }
        }
        private void NameColumnWidthPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            foreach (var grid in _treeViewItemRootGrids)
            {
                grid.ColumnDefinitions[0].Width = (GridLength)sender.GetValue(dp);
            }
        }

        #endregion

        private void TreeViewItem_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
        }
    }
}
