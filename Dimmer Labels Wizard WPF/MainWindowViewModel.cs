using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor.
        public MainWindowViewModel()
        {
            // Commands.
            _NewProjectCommand = new RelayCommand(NewProjectCommandExecute);
        }
        #endregion

        #region Commands.

        protected RelayCommand _NewProjectCommand;
        public ICommand NewProjectCommand
        {
            get
            {
                return _NewProjectCommand;
            }
        }

        protected void NewProjectCommandExecute(object parameter)
        {
            var ImportWindow = new ImportUnitsWindow();
            ImportWindow.Show();
        }
        #endregion
    }
}
