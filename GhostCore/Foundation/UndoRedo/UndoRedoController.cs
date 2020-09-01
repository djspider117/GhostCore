using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Foundation.UndoRedo
{
    public class UndoRedoController : IUndoRedo
    {
        #region Singleton

        private static volatile UndoRedoController _instance;
        private static object _syncRoot = new object();


        public static UndoRedoController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new UndoRedoController();
                    }
                }

                return _instance;
            }
        }

        public bool BulkCommandStarted { get; private set; }

        private UndoRedoController()
        {
            Initialize();
        }

        #endregion

        #region Fields

        private Dictionary<object, UndoRedoContext> _contextMapping;
        private UndoRedoContext _activeContext;

        #endregion

        private void Initialize()
        {
            _contextMapping = new Dictionary<object, UndoRedoContext>();
        }
        public void Cleanup()
        {
            _activeContext = null;

            foreach (var item in _contextMapping.Values)
            {
                item?.Cleanup();
            }

            _contextMapping.Clear();
        }
        public void SetActiveContext(object contextSource)
        {
            if (!_contextMapping.ContainsKey(contextSource))
                _contextMapping.Add(contextSource, new UndoRedoContext());

            _activeContext = _contextMapping[contextSource];
        }

        #region Undo-Redo API

        public void BeginBulkCommand()
        {
            BulkCommandStarted = true;
            CheckActiveContext();
            _activeContext.BeginBulkCommand();
        }

        public void EndBulkCommand()
        {
            BulkCommandStarted = false;
            CheckActiveContext();
            _activeContext.EndBulkCommand();
        }

        public void CancelBulkCommand()
        {
            BulkCommandStarted = false;
            CheckActiveContext();
            _activeContext.CancelBulkCommand();
        }

        public void RegisterCommand(IList collectionContext, NotifyCollectionChangedAction action, object @object)
        {
            CheckActiveContext();
            _activeContext.RegisterCommand(collectionContext, action, @object);
        }

        public void RegisterCommand(object context, string propName, object oldValue)
        {
            CheckActiveContext();
            _activeContext.RegisterCommand(context, propName, oldValue);
        }

        public void RegisterCommand(ICommand command)
        {
            CheckActiveContext();
            _activeContext.RegisterCommand(command);
        }


        public void AddCommandToLastBulk(IList collectionContext, NotifyCollectionChangedAction action, object @object)
        {
            CheckActiveContext();
            _activeContext.AddCommandToLastBulk(collectionContext, action, @object);
        }

        public void AddCommandToLastBulk(object context, string propName, object oldValue)
        {
            CheckActiveContext();
            _activeContext.AddCommandToLastBulk(context, propName, oldValue);
        }

        public void AddCommandToLastBulk(ICommand command)
        {
            CheckActiveContext();
            _activeContext.AddCommandToLastBulk(command);
        }

        public void Undo()
        {
            CheckActiveContext();
            _activeContext.Undo();
        }

        public void Redo()
        {
            CheckActiveContext();
            _activeContext.Redo();
        }

        #endregion

        private void CheckActiveContext()
        {
            if (_activeContext == null)
                throw new InvalidOperationException("Active Context cannot be null");
        }
    }
}
