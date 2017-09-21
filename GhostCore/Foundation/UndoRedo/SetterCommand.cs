using System;
using System.Reflection;

namespace GhostCore.Foundation.UndoRedo
{
    public class SetterCommand : ICommand
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
