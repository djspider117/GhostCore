using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace GhostCore.MVVM
{
    public class ViewModelCollection<TViewModel, TModel> : ObservableCollection<TViewModel> where TViewModel : ViewModelBase<TModel>
    {
        private readonly IList<TModel> _modelCollection;

        private bool _disableCollectionAdd;

        public ViewModelCollection()
        {
            _modelCollection = new List<TModel>();
        }

        public ViewModelCollection(IList<TModel> modelCollection)
        {
            _modelCollection = modelCollection;
            InitializeItems();
        }

        public ViewModelCollection(IList<TModel> modelCollection, IEnumerable<TViewModel> col) : base(col)
        {
            _modelCollection = modelCollection;
            InitializeItems();
        }

        public ViewModelCollection(IList<TModel> modelCollection, List<TViewModel> list) : base(list)
        {
            _modelCollection = modelCollection;
            InitializeItems();
        }

        protected void InitializeItems()
        {
            if (_modelCollection == null)
                return;

            _disableCollectionAdd = true;

            var ctor = typeof(TViewModel).GetConstructor(new Type[] { typeof(TModel) });
            foreach (var item in _modelCollection)
            {
                var vmItem = (TViewModel)ctor.Invoke(new object[] { item });
                Add(vmItem);
            }
            _disableCollectionAdd = false;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_disableCollectionAdd)
                return;

            base.OnCollectionChanged(e);
            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TViewModel x in e.NewItems)
                    {
                        _modelCollection.Insert(e.NewStartingIndex, x.Model);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TViewModel x in e.OldItems)
                    {
                        _modelCollection.Remove(x.Model);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
    }
}
