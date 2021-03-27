using System;
using System.Reflection;

namespace GhostCore.UndoRedo
{
    public class SetterCommand : IUndoRedoCommand
    {
        public object Context { get; set; }
        public string PropertyName { get; set; }
        public object OldValue { get; set; }

        public bool CanExecute
        {
            get
            {
                return Context != null && PropertyName != null;
            }
        }

        public void Execute()
        {
            var ctype = Context.GetType();
            var prop = ctype.GetProperty(PropertyName);

            var curVal = prop.GetValue(Context);
            prop.SetValue(Context, OldValue);

            OldValue = curVal;
        }
    }

}
