using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GhostCore.MVVM
{
    public class RelayCommand : ICommand
    {
        #region Events

        #region CanExecuteChanged
        public event EventHandler CanExecuteChanged;
        protected void OnCanExecuteChanged()
        {
            if (CanExecuteChanged == null)
                return;

            CanExecuteChanged(this, EventArgs.Empty);
        }
        #endregion

        #endregion

        #region Fields

        private Action<object> _executeHandler;
        private Func<object, bool> _canExecuteHandler;

        #endregion

        public RelayCommand(Action<object> executeHandler, Func<object, bool> canExecute = null)
        {
            _executeHandler = executeHandler ?? throw new ArgumentException("Execute handler cannot be null");
            _canExecuteHandler = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteHandler == null)
                return true;

            return _canExecuteHandler(parameter);
        }

        public void Execute(object parameter)
        {
            _executeHandler(parameter);
        }
    }
}
