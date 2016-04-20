using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DatabaseViewerViewModel : ViewModelBase
    {
        public DatabaseViewerViewModel()
        {
            // Commands.
            _ReloadCommand = new RelayCommand(ReloadCommandExecute);

            Status = "Loading";
            _Context.Units.Load();
            Status = "Idle";
        }

        private PrimaryDB _Context = new PrimaryDB();

        public ObservableCollection<DimmerDistroUnit> Units
        {
            get
            {
                return _Context.Units.Local;
            }
        }

        public ObservableCollection<Strip> Strips
        {
            get
            {
                return _Context.Strips.Local;
            }
        }

        protected string _Status = "Idle";

        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;

                    // Notify.
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        protected RelayCommand _ReloadCommand;
        public ICommand ReloadCommand
        {
            get
            {
                return _ReloadCommand;
            }
        }

        protected void ReloadCommandExecute(object parameter)
        {
            Status = "Loading";
            _Context.Units.Load();
            _Context.Strips.Load();
            Status = "Idle";
        }
    }
}
