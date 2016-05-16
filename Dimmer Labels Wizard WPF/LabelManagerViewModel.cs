using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dimmer_Labels_Wizard_WPF.Repositories;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelManagerViewModel : ViewModelBase
    {
        public LabelManagerViewModel()
        {
            // Repositories.
            PrimaryDB context = new PrimaryDB();
            _TemplateRepository = new TemplateRepository(context);
            _StripRepository = new StripRepository(context);

            // Commands.
            _AddNewStripCommand = new RelayCommand(AddNewStripCommandExecute);
            _RemoveSelectedTemplatesCommand = new RelayCommand(RemoveSelectedCommandExecute, RemoveSelectedCommandsCanExecute);
            _GenerateLabelsCommand = new RelayCommand(GenerateLabelsCommandExecute);
            _OkCommand = new RelayCommand(OkCommandExecute);

            ExistingStrips.CollectionChanged += ExistingStrips_CollectionChanged;

            // Populate Existing Strips.
            BeginExistingStripLoad();

            foreach (var element in _StripRepository.GetStrips())
            {
                ExistingStrips.Add(element);
            }

            EndExistingStripLoad();

        }

        // Database Repositories.
        TemplateRepository _TemplateRepository;
        StripRepository _StripRepository;

        public bool InSetup = false;
        protected bool _LoadingExistingStrips = false;

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
                    OnPropertyChanged(nameof(SingleShowUniverse));
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
                return _TemplateRepository.GetTemplates();
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

        protected ObservableCollection<Strip> _ExistingStrips = new ObservableCollection<Strip>();
        public ObservableCollection<Strip> ExistingStrips
        {
            get
            {
                return _ExistingStrips;
            }
        }


        public bool SingleShowUniverse
        {
            get { return SelectedSingleRackType == RackType.Dimmer; }
        }


        protected RackType _SelectedRangeRackType;

        public RackType SelectedRangeRackType
        {
            get { return _SelectedRangeRackType; }
            set
            {
                if (_SelectedRangeRackType != value)
                {
                    _SelectedRangeRackType = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedRangeRackType));
                    OnPropertyChanged(nameof(RangeShowUniverse));
                }
            }
        }


        protected int _SelectedRangeUniverse;

        public int SelectedRangeUniverse
        {
            get { return _SelectedRangeUniverse; }
            set
            {
                if (_SelectedRangeUniverse != value)
                {
                    _SelectedRangeUniverse = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedRangeUniverse));
                }
            }
        }


        protected int _SelectedRangeFirstDimmer;

        public int SelectedRangeFirstDimmer
        {
            get { return _SelectedRangeFirstDimmer; }
            set
            {
                if (_SelectedRangeFirstDimmer != value)
                {
                    _SelectedRangeFirstDimmer = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedRangeFirstDimmer));
                }
            }
        }


        protected int _SelectedRangeRackMultiplier = 1;

        public int SelectedRangeRackMultiplier
        {
            get { return _SelectedRangeRackMultiplier; }
            set
            {
                if (_SelectedRangeRackMultiplier != value)
                {
                    // Coerce.
                    if (value < 1)
                    {
                        value = 1;
                    }

                    _SelectedRangeRackMultiplier = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedRangeRackMultiplier));
                }
            }
        }


        protected int _SelectedRangeUnitsPerRack;

        public int SelectedRangeUnitsPerRack
        {
            get { return _SelectedRangeUnitsPerRack; }
            set
            {
                if (_SelectedRangeUnitsPerRack != value)
                {
                    _SelectedRangeUnitsPerRack = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedRangeUnitsPerRack));
                }
            }
        }


        protected LabelStripTemplate _SelectedRangeTemplate = null;

        public LabelStripTemplate SelectedRangeTemplate
        {
            get { return _SelectedRangeTemplate; }
            set
            {
                if (_SelectedRangeTemplate != value)
                {
                    _SelectedRangeTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedRangeTemplate));
                }
            }
        }

        public bool RangeShowUniverse
        {
            get
            {
                return SelectedRangeRackType == RackType.Dimmer;
            }
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

        protected RelayCommand _GenerateLabelsCommand;
        public ICommand GenerateLabelsCommand
        {
            get
            {
                return _GenerateLabelsCommand;
            }
        }

        protected void GenerateLabelsCommandExecute(object parameter)
        {
            int rangeStart = SelectedRangeFirstDimmer;
            int unitsPerRack = SelectedRangeUnitsPerRack;
            int rackMultiplier = SelectedRangeRackMultiplier;
            int universe = SelectedRangeUniverse;
            RackType rackType = SelectedRangeRackType;
            LabelStripTemplate template = SelectedRangeTemplate;

            int firstDimmer = rangeStart;
            int lastDimmer = firstDimmer + (unitsPerRack - 1);

            for (int count = 1; count <= rackMultiplier; count++)
            {
                var strip = new Strip()
                {
                    FirstDimmer = firstDimmer,
                    LastDimmer = lastDimmer,
                    Universe = universe,
                    RackType = rackType,
                    AssignedTemplate = template
                };

                ExistingStrips.Add(strip);

                firstDimmer += unitsPerRack;
                lastDimmer += unitsPerRack;

            }

        }

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


        protected RelayCommand _RemoveSelectedTemplatesCommand;
        public ICommand RemoveSelectedTemplatesCommand
        {
            get
            {
                return _RemoveSelectedTemplatesCommand;
            }
        }

        protected void RemoveSelectedCommandExecute(object parameter)
        {
            foreach (var element in SelectedStrips)
            {
                ExistingStrips.Remove(element);
                _StripRepository.Remove(element);
            }

            if (ExistingStrips.Count > 0)
            {
                // Select Last Strip if available.
                ExistingStrips.Last().IsSelected = true;
            }
        }

        protected bool RemoveSelectedCommandsCanExecute(object parameter)
        {
            return SelectedStrips.Count() > 0;
        }


        protected RelayCommand _OkCommand;
        public ICommand OkCommand
        {
            get
            {
                return _OkCommand;
            }
        }

        protected void OkCommandExecute(object parameter)
        {
            var window = parameter as LabelManager;

            _TemplateRepository.Save();
            _StripRepository.Save();

            if (InSetup)
            {
                var Editor = new Editor();
                var viewModel = Editor.DataContext as EditorViewModel;

                window.Close();
                Editor.Show();
            }

            else
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        #endregion

        #region Methods.
        protected void BeginExistingStripLoad()
        {
            _LoadingExistingStrips = true;
        }

        protected void EndExistingStripLoad()
        {
            _LoadingExistingStrips = false;
        }

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

                // Selection.
                _SelectedSingleRackType = firstStrip.RackType;
                _SelectedSingleFirstDimmer = firstStrip.FirstDimmer;
                _SelectedSingleLastDimmer = firstStrip.LastDimmer;
                _SelectedSingleTemplate = firstStrip.AssignedTemplate;
            }

            // Notify.
            OnPropertyChanged(nameof(SelectedSingleRackType));
            OnPropertyChanged(nameof(SelectedSingleFirstDimmer));
            OnPropertyChanged(nameof(SelectedSingleLastDimmer));
            OnPropertyChanged(nameof(SelectedSingleTemplate));

            // Executes
            _RemoveSelectedTemplatesCommand.CheckCanExecute();
        }
        #endregion

        #region Event Handlers.
        private void ExistingStrips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                // Hookup Property Changed Event.
                foreach (var element in e.NewItems)
                {
                    var item = element as INotifyPropertyChanged;
                    item.PropertyChanged += ExistingStrip_PropertyChanged;
                }

                if (_LoadingExistingStrips == false)
                {
                    // Push to DB.
                    foreach (var element in e.NewItems)
                    {
                        var item = element as Strip;
                        _StripRepository.Insert(item);
                    }
                }
            }

            if (e.OldItems != null)
            {
                // Hookup Property Changed Event.
                foreach (var element in e.OldItems)
                {
                    var item = element as INotifyPropertyChanged;
                    item.PropertyChanged -= ExistingStrip_PropertyChanged;
                }

                if (_LoadingExistingStrips == false)
                {
                    // Push Change to DB.
                    foreach (var element in e.OldItems)
                    {
                        var item = element as Strip;
                        _StripRepository.Remove(item);
                    }
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
