using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GhostCore.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Add<T>(this ObservableCollection<T> collection, params T[] args)
        {
            foreach (var x in args)
            {
                collection.Add(x);
            }
        }
        public static void Remove<T>(this ObservableCollection<T> collection, params T[] args)
        {
            foreach (var x in args)
            {
                collection.Remove(x);
            }
        }
    }
}
