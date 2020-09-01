using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.MVVM.Dynamic
{

    public interface IViewModelFactory<T, K> where T : ViewModelBase<K>
    {
        T Create(K parameter);
    }
}
