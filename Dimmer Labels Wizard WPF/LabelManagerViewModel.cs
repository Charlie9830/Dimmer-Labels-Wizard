using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelManagerViewModel : ViewModelBase
    {
        public LabelManagerViewModel()
        {
            // Commands.
            _AddNewStripCommand = new RelayCommand(AddNewStripCommandExecute);

            ExistingStrips.CollectionChanged += ExistingStrips_CollectionChanged;
        }

        #region Binding Source Properties.
        public RackType[] RackTypes
        {
            get
            {
                return new RackType[] { RackType.Dimmer, RackType.Distro };
            }
        }


        protected RackType _SelectedSingleRackType = RackType.Dimmer;

        public RackType SelectedSingleRackType
        {
            get { return _SelectedSingleRackType; }
            set
            {
                if (_SelectedSingleRackType != value)
                {
                    _SelectedSingleRackType = value;

                    // Update Model.
                    foreach (var element in SelectedStrips)
                    {
                        element.RackType = value;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedSingleRackType));
                    OnPropertyChanged(nameof(ShowUniverse));
                }
            }
        }


        protected int _SelectedSingleUniverse;

        public int SelectedSingleUniverse
        {
            get { return _SelectedSingleUniverse; }
            set
            {
                if (_SelectedSingleUniverse != value)
                {
                    _SelectedSingleUniverse = value;

                    // Update Model.
                    foreach (var element in SelectedStrips)
                    {
                        element.Universe = value;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedSingleUniverse));
                }
            }
        }


        protected int _SelectedSingleFirstDimmer;

        public int SelectedSingleFirstDimmer
        {
            get { return _SelectedSingleFirstDimmer; }
            set
            {
                if (_SelectedSingleFirstDimmer != value)
                {
                    _SelectedSingleFirstDimmer = value;

                    // Update Model.
                    foreach (var element in SelectedStrips)
                    {
                        element.FirstDimmer = value;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedSingleFirstDimmer));
                }
            }
        }


        protected int _SelectedSingleLastDimmer;

        public int SelectedSingleLastDimmer
        {
            get { return _SelectedSingleLastDimmer; }
            set
            {
                if (_SelectedSingleLastDimmer != value)
                {
                    _SelectedSingleLastDimmer = value;

                    // Update Model.
                    foreach (var element in SelectedStrips)
                    {
                        element.LastDimmer = value;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedSingleLastDimmer));
                }
            }
        }

        public IEnumerable<LabelStripTemplate> ExistingTemplates
        {
            get
            {
                return Globals.Templates as IEnumerable<LabelStripTemplate>;
            }
        }


        protected LabelStripTemplate _SelectedSingleTemplate;

        public LabelStripTemplate SelectedSingleTemplate
        {
            get { return _SelectedSingleTemplate; }
            set
            {
                if (_SelectedSingleTemplate != value)
                {
                    _SelectedSingleTemplate = value;

                    // Update Model.
                    foreach (var element in SelectedStrips)
                    {
                        element.AssignedTemplate = value;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedSingleTemplate));
                }
            }
        }

        public ObservableCollection<Strip> ExistingStrips
        {
            get
            {
                return Globals.Strips;
            }
        }


        public bool ShowUniverse
        {
            get { return SelectedSingleRackType == RackType.Dimmer; }
        }

        #endregion

        #region Standard Properties.
        public IEnumerable<Strip> SelectedStrips
        {
            get
            {
                return (from strip in ExistingStrips
                       where strip.IsSelected == true
                       select strip).ToList();
            }
        }
        #endregion

        #region Commands.

        protected RelayCommand _AddNewStripCommand;
        public ICommand AddNewStripCommand
        {
            get
            {
                return _AddNewStripCommand;
            }
        }

        protected void AddNewStripCommandExecute(object parameter)
        {
            var newStrip = new Strip();

            DeselectAllStrips();

            ExistingStrips.Add(newStrip);

            newStrip.IsSelected = true;
        }
        #endregion

        #region Methods.
        protected void DeselectAllStrips()
        {
            foreach (var element in SelectedStrips)
            {
                element.IsSelected = false;
            }
        }

        private void OnSelectionChanged()
        {
            var selectedStrips = SelectedStrips.ToList();

            if (selectedStrips.Count > 0)
            {
                var firstStrip = selectedStrips.First();

                // Multi Selection.
                _SelectedSingleRackType = firstStrip.RackType;
                _SelectedSingleFirstDimmer = firstStrip.FirstDimmer;
                _SelectedSingleLastDimmer = firstStrip.LastDimmer;
                _SelectedSingleTemplate = firstStrip.AssignedTemplate;

                // Notify.
                OnPropertyChanged(nameof(SelectedSingleRackType));
                OnPropertyChanged(nameof(SelectedSingleFirstDimmer));
                OnPropertyChanged(nameof(SelectedSingleLastDimmer));
                OnPropertyChanged(nameof(SelectedSingleTemplate));
            }

        }
        #endregion

        #region Event Handlers.
        private void ExistingStrips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var item = element as INotifyPropertyChanged;
                    item.PropertyChanged += ExistingStrip_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var item = element as INotifyPropertyChanged;
                    item.PropertyChanged -= ExistingStrip_PropertyChanged;
                }
            }
        }

        private void ExistingStrip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                OnSelectionChanged();
            }
        }
        #endregion
    }
}
