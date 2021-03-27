using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GhostCore.ComponentModel;

namespace GhostCore.MVVM
{
    public abstract class ViewModelController<T> where T : NotifyPropertyChangedImpl
    {
        protected T _vm;

        public T DataContext
        {
            get { return _vm; }
            set { _vm = value; }
        }

        private ViewModelController()
        {
            //do not let the user instantiate a view model controller without using a view model
            throw new InvalidOperationException("ViewModelControllers must be used with a viewModel");
        }

        public ViewModelController(T viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            DataContext = viewModel;
            DataContext.PropertyChanged += DataContext_PropertyChanged;
        }

        public void ChangeDataContext(T viewModel)
        {

            DataContext.PropertyChanged -= DataContext_PropertyChanged;
            DataContext = viewModel;
            DataContext.PropertyChanged += DataContext_PropertyChanged;
        }

        protected virtual void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}
