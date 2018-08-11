using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMaker_test_
{
    public static class UndoRedo
    {
        public static Stack<string> UndoActionsStack = new Stack<string>();
        public static Stack<string> RedoActionsStack = new Stack<string>();
    }
}
