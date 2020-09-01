using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GhostCore.Foundation.UndoRedo
{

    public class UndoRedoContext : IUndoRedo
    {
        #region Constants

        public const int DEFAULT_CAPACITY = 30;

        #endregion

        #region Fields

        private int _capacity;
        private LinkedList<ICommand> _undoStack;
        private LinkedList<ICommand> _redoStack;
        private bool _isBulking;
        private BulkCommand _currentBulkCommand;

        #endregion

        #region Properties

        public bool IsBulking
        {
            get
            {
                return _isBulking;
            }
            set
            {
                _isBulking = value;
            }
        }

        #endregion

        #region Initialization

        public UndoRedoContext() : this(DEFAULT_CAPACITY)
        {

        }

        public UndoRedoContext(int capacity)
        {
            _capacity = capacity;
            _undoStack = new LinkedList<ICommand>();
            _redoStack = new LinkedList<ICommand>();
        }

        #endregion

        #region API

        public void BeginBulkCommand()
        {
            IsBulking = true;
            _currentBulkCommand = new BulkCommand();
        }
        public void EndBulkCommand()
        {
            IsBulking = false;
            if (_currentBulkCommand.Count == 0)
                return;
            RegisterCommand(_currentBulkCommand);
        }


        public void CancelBulkCommand()
        {
            IsBulking = false;
            _currentBulkCommand = null;
        }

        public void RegisterCommand(IList collectionContext, NotifyCollectionChangedAction action, object @object)
        {
            var cmd = new CollectionAlterCommand()
            {
                Context = collectionContext,
                Action = action,
                OldValue = @object
            };

            RegisterCommand(cmd);
        }
        public void RegisterCommand(object context, string propName, object oldValue)
        {
            var cmd = new SetterCommand()
            {
                Context = context,
                PropertyName = propName,
                OldValue = oldValue
            };

            RegisterCommand(cmd);
        }
        public void RegisterCommand(ICommand command)
        {
            if (IsBulking)
            {
                _currentBulkCommand.Add(command);
                return;
            }

            if (_undoStack.Count == _capacity)
                _undoStack.RemoveLast();
            _undoStack.AddFirst(command);
            _redoStack.Clear();
        }

        public void AddCommandToLastBulk(IList collectionContext, NotifyCollectionChangedAction action, object @object)
        {
            var cmd = new CollectionAlterCommand()
            {
                Context = collectionContext,
                Action = action,
                OldValue = @object
            };

            AddCommandToLastBulk(cmd);
        }
        public void AddCommandToLastBulk(object context, string propName, object oldValue)
        {
            var cmd = new SetterCommand()
            {
                Context = context,
                PropertyName = propName,
                OldValue = oldValue
            };

            AddCommandToLastBulk(cmd);
        }
        public void AddCommandToLastBulk(ICommand cmd)
        {
            bool cmdFound = false;

            foreach (var x in _undoStack)
            {
                if (x is BulkCommand)
                {
                    (x as BulkCommand).Add(cmd);
                    cmdFound = true;
                    break;
                }
            }

            if (!cmdFound)
                throw new InvalidOperationException("You must have at least one BulkCommand registered in order to use this method.");
        }

        public void Undo()
        {
            if (_undoStack.Count == 0)
                return;

            var x = _undoStack.First.Value;
            if (!x.CanExecute)
            {
                _undoStack.RemoveFirst();
                Undo(); // recursive call to cleanup stack
                return;
            }

            x.Execute();
            if (_redoStack.Count == _capacity)
                _redoStack.RemoveLast();

            _redoStack.AddFirst(x);
            _undoStack.RemoveFirst();
            return;
        }
        public void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            var x = _redoStack.First.Value;
            if (!x.CanExecute)
            {
                _redoStack.RemoveFirst();
                Redo(); // recursive call to cleanup stack
                return;
            }

            x.Execute();
            if (_undoStack.Count == _capacity)
                _undoStack.RemoveLast();

            _undoStack.AddFirst(x);
            _redoStack.RemoveFirst();
            return;
        }

        #endregion

        public void Cleanup()
        {
            _undoStack.Clear();
            _redoStack.Clear();

            _undoStack = null;
            _redoStack = null;
        }
    }
}
