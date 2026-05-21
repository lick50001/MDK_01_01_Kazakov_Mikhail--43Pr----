using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskManager_Kazakov.Classes
{
    public class RealyCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;
        public RealyCommand(Action<object> execute, Func<object, bool> executeFunc = null)
        {
            this.execute = execute;
            canExecute = executeFunc;
        }

        public bool CanEcecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter) =>
            this.execute(parameter);
    }
}
