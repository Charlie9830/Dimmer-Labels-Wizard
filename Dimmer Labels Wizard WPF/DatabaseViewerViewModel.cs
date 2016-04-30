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
            _ExecuteSQLCommand = new RelayCommand(ExecuteSQLCommandExecute);

            Status = "Loading";
            _Context.Units.Load();
            _Context.Strips.Load();
            _Context.Templates.Load();
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

        public ObservableCollection<LabelStripTemplate> Templates
        {
            get
            {
                return _Context.Templates.Local;
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


        protected string _SQLCommand;

        public string SQLCommand
        {
            get { return _SQLCommand; }
            set
            {
                if (_SQLCommand != value)
                {
                    _SQLCommand = value;

                    // Notify.
                    OnPropertyChanged(nameof(SQLCommand));
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
            _Context.Templates.Load();
            Status = "Idle";
        }


        protected RelayCommand _ExecuteSQLCommand;
        public ICommand ExecuteSQLCommand
        {
            get
            {
                return _ExecuteSQLCommand;
            }
        }

        protected void ExecuteSQLCommandExecute(object parameter)
        {
            Status = "Executing SQL Command";
            _Context.Database.ExecuteSqlCommand(SQLCommand);

            ReloadCommandExecute(new object());

        }
    }
}
