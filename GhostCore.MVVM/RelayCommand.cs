using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GhostCore.MVVM
{
    public class RelayCommand : ICommand, IDisposable
    {
        #region Events

        public event EventHandler CanExecuteChanged;
       
        #endregion

        #region Fields

        private Action<object> _executeHandler;
        private Func<object, bool> _canExecuteHandler;

        #endregion

        public RelayCommand(Action<object> executeHandler, Func<object, bool> canExecute = null)
        {
            _executeHandler = executeHandler;
            _canExecuteHandler = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_executeHandler == null)
                return false;

            if (_canExecuteHandler == null)
                return true;

            return _canExecuteHandler(parameter);
        }

        public void Execute(object parameter)
        {
            _executeHandler?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            if (CanExecuteChanged == null)
                return;

            CanExecuteChanged(this, EventArgs.Empty);
        }

        public virtual void Dispose()
        {
            _executeHandler = null;
            _canExecuteHandler = null;
        }
    }
}
