using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace delegateCommand.Model
{
    public class DelegateCommand : ICommand
    {
        private Action action;
        private Func<bool> canExecute;

        public DelegateCommand(Action action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute (object parameter)
        {
            return canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.action();
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged(this, new EventArgs());
        }

    }
}
