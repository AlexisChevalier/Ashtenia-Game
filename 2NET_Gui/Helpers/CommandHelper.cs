using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _2NET_Gui.Helpers
{
    class CommandHelper : ICommand
    {
        public CommandHelper(Action<object> execute) : this(execute, null)
        {
            
        }

        public CommandHelper(Action<object> execute, Predicate<object> canExecute)
        {
            if(execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
    }
}
