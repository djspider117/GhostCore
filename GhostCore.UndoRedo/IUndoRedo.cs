﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.UndoRedo
{
    public interface IUndoRedo
    {
        void BeginBulkCommand();
        void EndBulkCommand();

        void RegisterCommand(IList collectionContext, NotifyCollectionChangedAction action, object @object);
        void RegisterCommand(object context, string propName, object oldValue);
        void RegisterCommand(IUndoRedoCommand command);

        void Undo();
        void Redo();
    }

}
