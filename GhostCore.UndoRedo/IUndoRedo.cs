using System.Collections;
using System.Collections.Specialized;

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
