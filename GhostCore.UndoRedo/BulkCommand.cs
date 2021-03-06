﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
