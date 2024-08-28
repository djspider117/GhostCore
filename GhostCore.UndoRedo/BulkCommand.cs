using System.Collections.Generic;

namespace GhostCore.UndoRedo
{
    public class BulkCommand : List<IUndoRedoCommand>, IUndoRedoCommand
    {
        public bool CanExecute
        {
            get
            {
                return TrueForAll((x) => x.CanExecute);
            }
        }

        public void Execute()
        {
            foreach (var x in this)
            {
                x.Execute();
            }
        }
    }

}
