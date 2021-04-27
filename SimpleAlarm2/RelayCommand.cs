using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleAlarm2
{
    public class RelayCommand<T> : ICommand
    {
        private Action<T> execute;
        private Predicate<object> canExecute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        { }

        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
