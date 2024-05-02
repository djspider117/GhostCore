namespace GhostCore.UndoRedo
{
    public interface IUndoRedoCommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}
