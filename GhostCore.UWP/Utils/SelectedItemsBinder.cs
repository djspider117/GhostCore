using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Windows.UI.Xaml.Controls;
using System.Collections.Specialized;

namespace GhostCore.UWP.Utils
{
    public class SelectedItemsBinder
    {
        private ListView _listView;
        private IList _collection;

        public SelectedItemsBinder(ListView listView, IList collection)
        {
            _listView = listView;
            _collection = collection;
        }

        public void Bind()
        {
            _listView.Loaded += ListView_Loaded;
            _listView.SelectionChanged += ListView_SelectionChanged;

            if (_collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged += Collection_CollectionChanged;
            }
        }

        private void ListView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Sync();
        }

        private void Sync()
        {
            _listView.SelectedItems.Clear();

            foreach (var el in _collection)
                _listView.SelectedItems.Add(el);
        }

        public void UnBind()
        {
            if (_listView != null)
                _listView.SelectionChanged -= ListView_SelectionChanged;

            if (_collection != null && _collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged -= Collection_CollectionChanged;
            }

            _listView.Loaded -= ListView_Loaded;
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_listView == null || _listView.Items == null || _listView.Items.Count == 0)
                return;

            foreach (var item in e.NewItems ?? new object[0])
            {
                if (!_listView.SelectedItems.Contains(item))
                    _listView.SelectedItems.Add(item);
            }
            foreach (var item in e.OldItems ?? new object[0])
            {
                if (_listView.SelectedItems.Contains(item))
                {
                    _listView.SelectedItems.Remove(item);
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems ?? new object[0])
            {
                if (!_collection.Contains(item))
                    _collection.Add(item);
            }

            foreach (var item in e.RemovedItems ?? new object[0])
            {
                _collection.Remove(item);
            }
        }
    }
}

