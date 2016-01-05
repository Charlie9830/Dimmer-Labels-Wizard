using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class RelayCommand : ICommand
    {
        private Action<object> _Action;
        private Func<object, bool> _CanExecute;

        public RelayCommand(Action<object> action)
        {
            _Action = action;
        }

        public RelayCommand(Action<object> action, Func<object, bool> canExecute)
        {
            _Action = action;
            _CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _Action(parameter);
        }
    }
}
