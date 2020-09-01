using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GhostCore.ComponentModel;
using Newtonsoft.Json;

namespace GhostCore.MVVM
{
    namespace Internal
    {
        internal static class __Incrementor
        {
            public static int Value = 0;
        }
    }
    public class ViewModelBase<T> : NotifyPropertyChangedImpl
    {
        #region Fields

#pragma warning disable IDE1006 // Naming Styles
        private int _OBJECT_ID_;
#pragma warning restore IDE1006 // Naming Styles
        private T _model;

        private object _parent;

        #endregion

        #region Properties

        public T Model
        {
            get { return _model; }
            set { _model = value; OnPropertyChanged("Model"); }
        }

        public int InternalObjectId
        {
            get { return _OBJECT_ID_; }
        }

        [JsonIgnore]
        public object Parent
        {
            get { return _parent; }
            set { _parent = value; OnPropertyChanged("Parent"); }
        }

        #endregion

        #region Constructors and Initialization

        public ViewModelBase()
        {
            _OBJECT_ID_ = Internal.__Incrementor.Value++;
            _model = default(T);
        }

        public ViewModelBase(T model)
        {
            _OBJECT_ID_ = Internal.__Incrementor.Value++;
            if (model == null)
            {
#pragma warning disable IDE0016 // Use 'throw' expression: ?? operator and throw don't go along well
                throw new ArgumentNullException("model");
#pragma warning restore IDE0016 // Use 'throw' expression
            }

            _model = model;
        }

        #endregion


        public TType ModelAs<TType>() where TType : class
        {
            return Model as TType;
        }
    }

}
