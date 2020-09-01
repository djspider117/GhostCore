using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GhostCore.Collections
{
    public class ModelUpdatingCollection<T> : ObservableCollection<T>
    {
        public IList<T> ModelCollection { get; internal set; }

        public ModelUpdatingCollection(IList<T> modelCollection)
        {
            ModelCollection = modelCollection;
        }

        public ModelUpdatingCollection(IList<T> modelCollection, IEnumerable<T> col) : base(col)
        {
            ModelCollection = modelCollection;
        }

        public ModelUpdatingCollection(IList<T> modelCollection, List<T> list) : base(list)
        {
            ModelCollection = modelCollection;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T x in e.NewItems)
                    {
                        ModelCollection.Add(x);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (T x in e.OldItems)
                    {
                        ModelCollection.Remove(x);
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
