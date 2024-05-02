using System.Collections;
using System.Collections.Specialized;

namespace GhostCore.UndoRedo
{
    public class CollectionAlterCommand : IUndoRedoCommand
    {
        public IList Context { get; set; }
        public NotifyCollectionChangedAction Action { get; set; }
        public object OldValue { get; set; }

        public bool CanExecute
        {
            get
            {
                if (Action == NotifyCollectionChangedAction.Add)
                    return Context != null && Context.Count > 0;

                if (Action == NotifyCollectionChangedAction.Remove)
                    return Context != null;

                return Context != null && Context.Count > 0 && OldValue != null;
            }
        }

        public void Execute()
        {
            switch (Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Context.Remove(OldValue);
                    Action = NotifyCollectionChangedAction.Remove;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Context.Add(OldValue);
                    Action = NotifyCollectionChangedAction.Add;
                    break;
                default:
                    break;
            }
        }
    }

}
