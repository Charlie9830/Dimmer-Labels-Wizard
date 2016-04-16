using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class InvalidUnitsViewModel : ViewModelBase
    {
        #region Constructor.
        public InvalidUnitsViewModel()
        {
            // Commands.
            _ContinueCommand = new RelayCommand(ContinueCommandExecute);
            _BackCommand = new RelayCommand(BackCommandExecute);
        }
        #endregion

        #region Binding Properties.

        protected UnitImporter _UnitImporter = null;

        public UnitImporter UnitImporter
        {
            get { return _UnitImporter; }
            set
            {
                if (_UnitImporter != value)
                {
                    _UnitImporter = value;

                    // Notify.
                    OnPropertyChanged(nameof(UnitImporter));
                    OnPropertyChanged(nameof(InvalidUnits));
                }
            }
        }

        public List<DimmerDistroUnit> InvalidUnits
        {
            get
            {
                if (UnitImporter != null)
                {
                    return (from unit in UnitImporter.UnResolveableUnits
                           orderby unit.RackUnitType
                           orderby unit.DimmerNumberText
                           select unit).ToList();
                }

                else
                {
                    return null;
                }
            }
        }


        protected FriendlyImportFormat _DimmerImportFormat;

        public FriendlyImportFormat DimmerImportFormat
        {
            get { return _DimmerImportFormat; }
            set
            {
                if (_DimmerImportFormat != value)
                {
                    _DimmerImportFormat = value;

                    // Notify.
                    OnPropertyChanged(nameof(DimmerImportFormat));
                    OnPropertyChanged(nameof(DimmerImportFriendlyName));
                }
            }
        }

        public string DimmerImportFriendlyName
        {
            get
            {
                if (UnitImporter == null)
                {
                    return string.Empty;
                }

                if (UnitImporter.ImportConfiguration.DimmerRanges.Count > 0)
                {
                    return DimmerImportFormat.FriendlyName;
                }

                else
                {
                    return "No Dimmers Imported";
                }
            }
        }

        protected FriendlyImportFormat _DistroImportFormat;

        public FriendlyImportFormat DistroImportFormat
        {
            get { return _DistroImportFormat; }
            set
            {
                if (_DistroImportFormat != value)
                {
                    _DistroImportFormat = value;

                    // Notify.
                    OnPropertyChanged(nameof(DistroImportFormat));
                    OnPropertyChanged(nameof(DistroImportFriendlyName));
                }
            }
        }

        public string DistroImportFriendlyName
        {
            get
            {
                if (UnitImporter == null)
                {
                    return string.Empty;
                }

                if (UnitImporter.ImportConfiguration.DistroRanges.Count > 0)
                {
                    return DistroImportFormat.FriendlyName;
                }

                else
                {
                    return "No Distros Imported";
                }
            }
        }

        #endregion

        #region Commands.
        protected RelayCommand _ContinueCommand;
        public ICommand ContinueCommand
        {
            get
            {
                return _ContinueCommand;
            }
        }

        protected void ContinueCommandExecute(object parameter)
        {
            var window = parameter as InvalidUnits;

            // Attempt to Re Validate Data.
            UnitImporter.RetryValidation();

            if (UnitImporter.AllUnitsValid)
            {
                // Close Dialog.
                window.DialogResult = true;
                window.Close();
            }

            else
            {
                // Reload InvalidUnits collection.
                OnPropertyChanged(nameof(InvalidUnits));
            }

            
        }


        protected RelayCommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return _BackCommand;
            }
        }

        protected void BackCommandExecute(object parameter)
        {
            var window = parameter as InvalidUnits;

            window.DialogResult = false;
            window.Close();
        }
        #endregion
    }
}
