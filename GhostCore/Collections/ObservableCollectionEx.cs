using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GhostCore.Collections
{
    public interface IReflectionDetails
    {
        string PropertyName { get; set; }
    }

    public class ObservableCollectionEx<T> : ObservableCollection<T>, IReflectionDetails
    {
        public string PropertyName { get; set; }
    }
}
