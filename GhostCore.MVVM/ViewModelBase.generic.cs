using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GhostCore.ComponentModel;
using GhostCore.MVVM.Internal;
using Newtonsoft.Json;

namespace GhostCore.MVVM
{
    public class ViewModelBase<T> : NotifyPropertyChangedImpl
    {
        #region Fields

        private readonly int _internalObjectId;
        private T _model;

        private object _parent;

        #endregion

        #region Properties

        public virtual T Model
        {
            get { return _model; }
            set { _model = value; OnPropertyChanged(nameof(Model)); }
        }

        public int InternalObjectId => _internalObjectId;

        [JsonIgnore]
        public object Parent
        {
            get { return _parent; }
            set { _parent = value; OnPropertyChanged(nameof(Parent)); }
        }

        #endregion

        #region Constructors and Initialization

        public ViewModelBase()
        {
            _internalObjectId = __Incrementor.GetIncrementedValue();
        }

        public ViewModelBase(T model)
        {
            _internalObjectId = __Incrementor.GetIncrementedValue();
            _model = model ?? throw new ArgumentNullException("Model must not be null.You can use default ctor if you do not want to pass in a model.", nameof(model));
        }

        #endregion

        public TType ModelAs<TType>() where TType : class => Model as TType;
    }

}
