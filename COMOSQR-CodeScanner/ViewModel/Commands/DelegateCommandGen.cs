using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace COMOSQR_CodeScanner.ViewModel.Commands
{
    internal class DelegateCommandGen<T> : ICommand
    {
        private Action<T> _execute;
        private Func<bool> _canExecute;

        public DelegateCommandGen(Action<T> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommandGen(Action<T> execute)
        {
            _execute = execute;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute();
        }

        void ICommand.Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
