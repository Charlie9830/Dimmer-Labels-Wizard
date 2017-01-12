using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Dimmer_Labels_Wizard_WPF.Repositories;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;
using System.Windows.Controls;
using System.Diagnostics;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DatabaseManagerViewModel : ViewModelBase
    {
        public DatabaseManagerViewModel()
        {
            // Event Handlers.
            _Units.CollectionChanged += Units_CollectionChanged;

            // Commands.
            _OkCommand = new RelayCommand(OkCommandExecute);
            _AddCommand = new RelayCommand(AddCommandExecute);
            _RemoveCommand = new RelayCommand(RemoveCommandExecute, RemoveCommandCanExecute);
            _ImportCommand = new RelayCommand(ImportCommandExecute);
            _ToggleFindPopupCommand = new RelayCommand(ToggleFindPopupCommandExecute);
            _BreakCommand = new RelayCommand(BreakCommandExecute);
            _FindPopupEnterPressCommand = new RelayCommand(FindPopupEnterPressCommandExecute);
            _SearchCommand = new RelayCommand(SearchCommandExecute);
            _FindNextCommand = new RelayCommand(FindNextCommandExecute);
            _FindPreviousCommand = new RelayCommand(FindPreviousCommandExecute);
            _ReplaceAllCommand = new RelayCommand(ReplaceAllCommandExecute);
            _CloseFindPopupCommand = new RelayCommand(CloseFindPopupCommandExecute);

            // Setup.
            _SearchView = new CollectionView(Units);
            SetupSearchFields();
            SetSearchPredicate(SearchField.ChannelNumber);
            _SelectedUnitGroupBy = UnitGroupBys.First();
            _SelectedLabelField = LabelFields.Find(item => item.LabelField == LabelField.InstrumentName);

            var context = new PrimaryDB();
            _UnitRepository = new UnitRepository(context);

            RefreshUnits();
        }

        protected UnitRepository _UnitRepository;


        protected bool _ChangingUnitValues = false;
        protected UnitGroupFactory _UnitGroupFactory = new UnitGroupFactory();

        #region Binding Sources.
        protected IEnumerable<FriendlyUnitGroupBy> _UnitGroupBys = FriendlyEnumCollections.FriendlyUnitGroupBys;

        public IEnumerable<FriendlyUnitGroupBy> UnitGroupBys
        {
            get
            {
                return _UnitGroupBys;
            }
        }


        protected FriendlyUnitGroupBy _SelectedUnitGroupBy;

        public FriendlyUnitGroupBy SelectedUnitGroupBy
        {
            get { return _SelectedUnitGroupBy; }
            set
            {
                if (_SelectedUnitGroupBy != value)
                {
                    _SelectedUnitGroupBy = value;

                    // Notify Refresh Unit Groups.
                    RefreshUnitGroups();

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUnitGroupBy));
                }
            }
        }



        protected List<FriendlyLabelField> _LabelFields = FriendlyEnumCollections.FriendlyLabelFieldsDisplayedOnly;

        public List<FriendlyLabelField> LabelFields
        {
            get
            {
                return _LabelFields;
            }
        }

        protected FriendlyLabelField _SelectedLabelField;

        public FriendlyLabelField SelectedLabelField
        {
            get { return _SelectedLabelField; }
            set
            {
                if (_SelectedLabelField != value)
                {
                    _SelectedLabelField = value;

                    // Notify Refresh Unit Groups.
                    RefreshUnitGroups();

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLabelField));
                }
            }
        }


        protected ObservableCollection<UnitGroup> _UnitGroups = new ObservableCollection<UnitGroup>();

        public ObservableCollection<UnitGroup> UnitGroups
        {
            get { return _UnitGroups; }
            set
            {
                if (_UnitGroups != value)
                {
                    _UnitGroups = value;

                    // Notify.
                    OnPropertyChanged(nameof(UnitGroups));
                }
            }
        }

        public IEnumerable<DimmerDistroUnit> UnitsLoadingFallbackValue
        {
            get
            {
                return new List<DimmerDistroUnit>() { new DimmerDistroUnit() { Position = "Loading..." } };
            }
        }

        protected ObservableCollection<DimmerDistroUnit> _Units = new ObservableCollection<DimmerDistroUnit>();

        public ObservableCollection<DimmerDistroUnit> Units
        {
            get { return _Units; }
            set
            {
                if (_Units != value)
                {
                    _Units = value;

                    // Notify.
                    OnPropertyChanged(nameof(Units));
                }
            }
        }

        protected string _SearchProgressMessage = string.Empty;

        public string SearchProgressMessage
        {
            get { return _SearchProgressMessage; }
            set
            {
                if (_SearchProgressMessage != value)
                {
                    _SearchProgressMessage = value;

                    // Notify.
                    OnPropertyChanged(nameof(SearchProgressMessage));
                }
            }
        }

        protected bool _IsFindPopupOpen;

        public bool IsFindPopupOpen
        {
            get { return _IsFindPopupOpen; }
            set
            {
                if (_IsFindPopupOpen != value)
                {
                    _IsFindPopupOpen = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsFindPopupOpen));
                }
            }
        }

        protected ValidationError _SelectedValidationError;

        public ValidationError SelectedValidationError
        {
            get { return _SelectedValidationError; }
            set
            {
                if (_SelectedValidationError != value)
                {
                    _SelectedValidationError = value;
                    
                    if (value != null)
                    {
                        SelectedUnit = value.Unit;
                    }
                    
                    // Notify.
                    OnPropertyChanged(nameof(SelectedValidationError));
                }
            }
        }

        protected ObservableCollection<ValidationError> _ValidationErrors = new ObservableCollection<ValidationError>();

        public ObservableCollection<ValidationError> ValidationErrors
        {
            get { return _ValidationErrors; }
            set
            {
                if (_ValidationErrors != value)
                {
                    _ValidationErrors = value;

                    // Notify.
                    OnPropertyChanged(nameof(ValidationErrors));
                }
            }
        }


        protected DimmerDistroUnit _SelectedUnit;

        public DimmerDistroUnit SelectedUnit
        {
            get { return _SelectedUnit; }
            set
            {
                if (_SelectedUnit != value)
                {
                    _SelectedUnit = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUnit));
                    _RemoveCommand.CheckCanExecute();
                }
            }
        }


        protected string _FindInput = string.Empty;

        public string FindInput
        {
            get { return _FindInput; }
            set
            {
                if (_FindInput != value)
                {
                    _FindInput = value;

                    if (_IsSearchActive)
                    {
                        CancelSearch();
                    }

                    // Notify.
                    OnPropertyChanged(nameof(FindInput));
                }
            }
        }


        protected string _ReplaceInput = string.Empty;

        public string ReplaceInput
        {
            get { return _ReplaceInput; }
            set
            {
                if (_ReplaceInput != value)
                {
                    _ReplaceInput = value;

                    // Notify.
                    OnPropertyChanged(nameof(ReplaceInput));
                }
            }
        }


        protected bool _IsReplaceModeOn;

        public bool IsReplaceModeOn
        {
            get { return _IsReplaceModeOn; }
            set
            {
                if (_IsReplaceModeOn != value)
                {
                    _IsReplaceModeOn = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsReplaceModeOn));
                }
            }
        }


        protected bool _IsFindCaseSensitive = false;

        public bool IsFindCaseSensitive
        {
            get { return _IsFindCaseSensitive; }
            set
            {
                if (_IsFindCaseSensitive != value)
                {
                    _IsFindCaseSensitive = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsFindCaseSensitive));
                }
            }
        }

        protected IEnumerable<FriendlySearchField> _SearchFields;

        public IEnumerable<FriendlySearchField> SearchFields
        {
            get
            {
                return _SearchFields;
            }

            protected set
            {
                _SearchFields = value;

                OnPropertyChanged(nameof(SearchFields));
            }
        }


        protected FriendlySearchField _SelectedSearchField = null;

        public FriendlySearchField SelectedSearchField
        {
            get { return _SelectedSearchField; }
            set
            {
                if (_SelectedSearchField != value)
                {
                    _SelectedSearchField = value;

                    // Set the correct Predicate to the SearchSpace CollectionView.
                    SetSearchPredicate(value.SearchField);

                    // Notify.
                    OnPropertyChanged(nameof(SelectedSearchField));
                }
            }
        }

        #endregion

        #region Commands.


        protected RelayCommand _CloseFindPopupCommand;
        public ICommand CloseFindPopupCommand
        {
            get
            {
                return _CloseFindPopupCommand;
            }
        }

        protected void CloseFindPopupCommandExecute(object parameter)
        {
            CancelSearch();
            IsFindPopupOpen = false;
        }

        protected RelayCommand _ReplaceAllCommand;
        public ICommand ReplaceAllCommand
        {
            get
            {
                return _ReplaceAllCommand;
            }
        }

        protected void ReplaceAllCommandExecute(object parameter)
        {
            if (_IsSearchActive)
            {
                foreach (var unit in _SearchResults)
                {
                    ReplaceData(_ReplaceInput, _FindInput, unit, SelectedSearchField.SearchField);
                }
            }
        }


        protected RelayCommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                return _AddCommand;
            }
        }

        protected void AddCommandExecute(object parameter)
        {
            var dialog = new AddNewUnits();
            var viewModel = dialog.DataContext as AddNewUnitsViewModel;

            if (dialog.ShowDialog() == true)
            {
                // Units have been Created. Add them to Collections.
                foreach (var element in viewModel.Units)
                {
                    _Units.Add(element);
                    _AddedUnits.Add(element);

                    CheckUnitValidation(element);

                    // Select Incoming Unit.
                    element.IsSelected = true;
                }
            }
        }


        protected RelayCommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _RemoveCommand;
            }
        }

        protected void RemoveCommandExecute(object parameter)
        {
            var selectedUnits = SelectedUnits.ToList();

            if (selectedUnits.Count > 0)
            {
                foreach (var unit in selectedUnits)
                {
                    _Units.Remove(unit);
                    _RemovedUnits.Add(unit);
                    unit.IsSelected = false;
                }

                foreach (var unit in Units)
                {
                    CheckUnitValidation(unit);
                }

                // Validate Removed Units.
                foreach (var unit in _RemovedUnits)
                {
                    ClearValidationError(unit);
                }
            }
        }

        protected bool RemoveCommandCanExecute(object parameter)
        {
            if (SelectedUnits.Count() > 0)
            {
                return true;
            }

            else
            {
                return false;
            }
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
            var window = parameter as Window;

            if (ValidationErrors.Count > 0)
            {
                MessageBox.Show("Please correct all Validation Errors before returning to the Editor.");
            }

            else
            {
                UpdateDatabase();

                window.Close();
            }
        }


        protected RelayCommand _ImportCommand;
        public ICommand ImportCommand
        {
            get
            {
                return _ImportCommand;
            }
        }

        protected void ImportCommandExecute(object parameter)
        {
            if (ValidationErrors.Count > 0)
            {
                MessageBox.Show("Import cannot start until all Errors have been cleared.");
            }

            else
            {
                // Clear Current Selections.
                ClearSelections();

                // Commit current State to Database.
                _UnitRepository.Save();

                var dialog = new ImportUnitsWindow();
                dialog.ShowDialog();

                // New Units may have been added to the DB. The UnitRepository needs to be Disposed and Re-Initialzed
                // in order to force the DbContext to Refresh and find the new Units.
                _UnitRepository.Dispose();
                _UnitRepository = new UnitRepository(new PrimaryDB());

                // Re-query DB and populate Local Collections.
                RefreshUnits();
            }
        }


        protected RelayCommand _BreakCommand;
        public ICommand BreakCommand
        {
            get
            {
                return _BreakCommand;
            }
        }

        protected void BreakCommandExecute(object parameter)
        {
            Console.WriteLine("Break");
        }


        protected RelayCommand _ToggleFindPopupCommand;
        public ICommand ToggleFindPopupCommand
        {
            get
            {
                return _ToggleFindPopupCommand;
            }
        }

        protected void ToggleFindPopupCommandExecute(object parameter)
        {
            IsFindPopupOpen = !IsFindPopupOpen;
        }


        protected RelayCommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand;
            }
        }

        protected void SearchCommandExecute(object parameter)
        {
            if (_FindInput != string.Empty)
            {
                var dataGrid = parameter as DataGrid;

                BeginSearch();

                // Executes the Search
                _SearchView.Refresh();

                // Populate Search Results.
                foreach (var element in _SearchView)
                {
                    _SearchResults.Add(element as DimmerDistroUnit);
                }

                // Update Progress Indicator.
                SearchProgressMessage = _SearchResults.Count.ToString() + " Units found.";

                // Select First Result.
                ClearSelections();

                if (_SearchResults.Count > 0)
                {
                    _CurrentSearchSelection = _SearchResults.First();
                    SelectedUnit = _CurrentSearchSelection;
                    dataGrid.ScrollIntoView(SelectedUnit);
                }
            }
        }


        protected RelayCommand _FindNextCommand;
        public ICommand FindNextCommand
        {
            get
            {
                return _FindNextCommand;
            }
        }

        protected void FindNextCommandExecute(object parameter)
        {
            if (_IsSearchActive == false || _SearchResults.Count == 0)
            {
                return;
            }

            var dataGrid = parameter as DataGrid;

            if (_IsReplaceModeOn)
            {
                ReplaceData(_ReplaceInput, _FindInput, _CurrentSearchSelection, _SelectedSearchField.SearchField);

                EnumerateToNextFoundUnit(dataGrid);
            }

            else
            {
                EnumerateToNextFoundUnit(dataGrid);
            }
        }

        
        protected RelayCommand _FindPreviousCommand;
        public ICommand FindPreviousCommand
        {
            get
            {
                return _FindPreviousCommand;
            }
        }

        protected void FindPreviousCommandExecute(object parameter)
        {
            if (_IsSearchActive == false || _SearchResults.Count == 0)
            {
                return;
            }

            var dataGrid = parameter as DataGrid;
            EnumerateToPreviousFoundUnit(dataGrid);
        }


        protected RelayCommand _FindPopupEnterPressCommand;
        public ICommand FindPopupEnterPressCommand
        {
            get
            {
                return _FindPopupEnterPressCommand;
            }
        }

        protected void FindPopupEnterPressCommandExecute(object parameter)
        {
            if (_IsSearchActive)
            {
                // A Search is already in Progress.
                // Forward Command onto FindNextCommand.
                FindNextCommandExecute(parameter);
            }

            else
            {
                // No search in progress, Forward command onto Search Command.
                SearchCommandExecute(parameter);
            }
        }

        #endregion

        #region Non Binding Properties.
        protected IEnumerable<DimmerDistroUnit> SelectedUnits
        {
            get
            {
                return from unit in _Units
                       where unit.IsSelected == true
                       select unit;
            }
        }
        protected List<DimmerDistroUnit> _AddedUnits { get; set; } = new List<DimmerDistroUnit>();
        protected List<DimmerDistroUnit> _RemovedUnits { get; set; } = new List<DimmerDistroUnit>();
        protected ICollectionView _SearchView { get; set; }
        protected List<DimmerDistroUnit> _SearchResults { get; set; } = new List<DimmerDistroUnit>();
        protected bool _IsSearchActive { get; set; } = false;
        protected DimmerDistroUnit _CurrentSearchSelection { get; set; }
        #endregion

        #region Event Handlers.
        private void Units_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var unit = element as DimmerDistroUnit;
                    unit.PropertyChanged -= Unit_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var unit = element as DimmerDistroUnit;
                    unit.PropertyChanged += Unit_PropertyChanged;
                }
            }
        }

        private void Unit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ChangingUnitValues != true)
            {
                // Prevent Rentrency issues.
                _ChangingUnitValues = true;

                // A Unit's Value has Changed. Pass Value to other Selected Units.
                var changedUnit = sender as DimmerDistroUnit;
                bool validationCheckRequired = false;

                // RackUnitType.
                if (e.PropertyName == nameof(DimmerDistroUnit.RackUnitType))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.RackUnitType = changedUnit.RackUnitType;
                    }

                    validationCheckRequired = true;
                }

                // UniverseNumber
                else if (e.PropertyName == nameof(DimmerDistroUnit.UniverseNumber))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.UniverseNumber = changedUnit.UniverseNumber;
                    }

                    validationCheckRequired = true;
                }

                // DimmerNumber
                else if (e.PropertyName == nameof(DimmerDistroUnit.DimmerNumber))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.DimmerNumber = changedUnit.DimmerNumber;
                    }

                    validationCheckRequired = true;
                }

                // Channel Number
                else if (e.PropertyName == nameof(DimmerDistroUnit.ChannelNumber))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.ChannelNumber = changedUnit.ChannelNumber;
                    }
                }

                // Instrument Name
                else if (e.PropertyName == nameof(DimmerDistroUnit.InstrumentName))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.InstrumentName = changedUnit.InstrumentName;
                    }
                }

                // Position.
                else if (e.PropertyName == nameof(DimmerDistroUnit.Position))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.Position = changedUnit.Position;
                    }
                }

                // MulticoreName.
                else if (e.PropertyName == nameof(DimmerDistroUnit.MulticoreName))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.MulticoreName = changedUnit.MulticoreName;
                    }
                }

                // UserField1.
                else if (e.PropertyName == nameof(DimmerDistroUnit.UserField1))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.UserField1 = changedUnit.UserField1;
                    }
                }

                // UserField2.
                else if (e.PropertyName == nameof(DimmerDistroUnit.UserField2))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.UserField2 = changedUnit.UserField2;
                    }
                }

                // UserField3.
                else if (e.PropertyName == nameof(DimmerDistroUnit.UserField3))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.UserField3 = changedUnit.UserField3;
                    }
                }

                // UserField1.
                else if (e.PropertyName == nameof(DimmerDistroUnit.UserField4))
                {
                    foreach (var unit in SelectedUnits)
                    {
                        unit.UserField4 = changedUnit.UserField4;
                    }
                }

                // Check validation of Units if Key Values have been Modified.
                if (validationCheckRequired == true)
                {
                    // Check every Unit for Validation Errors.
                    foreach (var element in Units)
                    {
                        CheckUnitValidation(element);
                    }
                }

                // Reset Re Entrancy Block.
                _ChangingUnitValues = false;
            }
        }
        #endregion

        #region Methods.
        protected void RefreshUnitGroups()
        {
            while (UnitGroups.Count > 0)
            {
                UnitGroups.RemoveAt(UnitGroups.Count - 1);
            }


            foreach (var element in _UnitGroupFactory.CreateNameUnitGroups(Units, SelectedLabelField.LabelField,
                SelectedUnitGroupBy.UnitGroupBy))
            {
                UnitGroups.Add(element);
            }
        }

        protected void SetupSearchFields()
        {
            SearchFields = FriendlyEnumCollections.FriendlySearchFields;

            SelectedSearchField = SearchFields.First();
        }


        private void ReplaceData(string newData, string currentData, DimmerDistroUnit targetUnit, SearchField searchField)
        {
            switch (searchField)
            {
                case SearchField.All:
                    // Enumerate through the labelFields, Replace matching Values.
                    foreach (var element in Enum.GetValues(typeof(LabelField)))
                    {
                        var labelField = (LabelField)element;

                        if (labelField != LabelField.NoAssignment &&
                            labelField != LabelField.Custom)
                        {
                            if (targetUnit.GetData(labelField) == currentData)
                            {
                                targetUnit.SetData(newData, currentData);
                            }
                        }
                    }
                    break;
                case SearchField.ChannelNumber:
                    targetUnit.ChannelNumber = newData;
                    break;
                case SearchField.InstrumentName:
                    targetUnit.InstrumentName = newData;
                    break;
                case SearchField.MulticoreName:
                    targetUnit.MulticoreName = newData;
                    break;
                case SearchField.Position:
                    targetUnit.Position = newData;
                    break;
                case SearchField.UserField1:
                    targetUnit.UserField1 = newData;
                    break;
                case SearchField.UserField2:
                    targetUnit.UserField2 = newData;
                    break;
                case SearchField.UserField3:
                    targetUnit.UserField3 = newData;
                    break;
                case SearchField.UserField4:
                    targetUnit.UserField4 = newData;
                    break;
                default:
                    break;
            }
        }

        private void EnumerateToNextFoundUnit(DataGrid dataGrid)
        {
            if (_CurrentSearchSelection == null)
            {
                // User has not Enumerated Search Results yet.
                _CurrentSearchSelection = _SearchResults.First();
                SelectedUnit = _CurrentSearchSelection;

                dataGrid.ScrollIntoView(SelectedUnit);
            }

            else
            {
                // Enumerate to next Result if Possible.
                int currentIndex = _SearchResults.IndexOf(_CurrentSearchSelection);

                if (currentIndex == -1)
                {
                    throw new NotSupportedException("currentIndex has become -1");
                }

                if (currentIndex + 1 != _SearchResults.Count - 1)
                {
                    _CurrentSearchSelection = _SearchResults[currentIndex + 1];
                    SelectedUnit = _CurrentSearchSelection;
                    dataGrid.ScrollIntoView(SelectedUnit);
                }
            }
        }

        private void EnumerateToPreviousFoundUnit(DataGrid dataGrid)
        {
            int currentIndex = _SearchResults.IndexOf(_CurrentSearchSelection);

            if (currentIndex == -1)
            {
                throw new NotSupportedException("currentIndex has become -1");
            }

            if (currentIndex != 0)
            {
                _CurrentSearchSelection = _SearchResults[currentIndex - 1];
                SelectedUnit = _CurrentSearchSelection;
                dataGrid.ScrollIntoView(SelectedUnit);
            }
        }

        protected void BeginSearch()
        {
            _SearchResults.Clear();
            SearchProgressMessage = "Searching...";
            _IsSearchActive = true;
        }

        protected void CancelSearch()
        {
            EndSearch();
        }

        protected void EndSearch()
        {
            SearchProgressMessage = string.Empty;
            _IsSearchActive = false;
        }

        protected void ClearSelections()
        {
            foreach (var element in _Units)
            {
                element.IsSelected = false;
            }

            SelectedUnit = null;
        }

        protected void CheckUnitValidation(DimmerDistroUnit unit)
        {
            if (ValidateUnitKeyValues(unit.RackUnitType, unit.UniverseNumber, unit.DimmerNumber) == true)
            {
                ClearValidationError(unit);
            }

            else
            {
                GenerateValidationError(unit);
            }
        }

        protected void GenerateValidationError(DimmerDistroUnit unit)
        {
            ValidationErrors.Add(new ValidationError(unit, BuildErrorMessage(unit)));
        }

        protected void ClearValidationError(DimmerDistroUnit unit)
        {
            var query = (from error in ValidationErrors
                        where error.Unit == unit
                        select error).ToList();
            
            if (query.Count == 1)
            {
                ValidationErrors.Remove(query.First());
            }

            else
            {
                // Multiple Validation Errors involving this Unit have been Found.
                foreach (var element in query)
                {
                    ValidationErrors.Remove(element);
                }
            }

        }

        protected string BuildErrorMessage(DimmerDistroUnit unit)
        {
            // Build String.
            var builder = new StringBuilder();
            
            if (unit.RackUnitType == RackType.Distro)
            {
                builder.AppendFormat("DISTRO - Dimmer Number {0} conflicts with an existing unit, ",unit.DimmerNumber);
                builder.AppendFormat("The combination of Rack Type and Dimmer Number must be unique for each Unit");

                return builder.ToString();
            }

            if (unit.RackUnitType == RackType.Dimmer)
            {
                builder.AppendFormat("DIMMER - Universe Number {0} - Dimmer Number {1} ", unit.UniverseNumber, unit.DimmerNumber);
                builder.AppendFormat("conflicts with an existing Unit. ");
                builder.AppendFormat("The combination of Rack Type, Universe and Dimmer Number must be Unique for each unit.");

                return builder.ToString();    
            }

            return "General Error";
        }

        protected bool ValidateUnitKeyValues(RackType rackUnitType, int universeNumber, int dimmerNumber)
        {
            var query = from unit in _Units
                        where unit.DimmerNumber == dimmerNumber
                        where unit.UniverseNumber == universeNumber
                        where unit.RackUnitType == rackUnitType
                        select unit;

            if (query.Count() >= 2)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        protected void RefreshUnits()
        {
            // Clear Units with Event Notifications.
            while (_Units.Count > 0)
            {
                _Units.RemoveAt(_Units.Count - 1);
            }

            foreach (var element in _UnitRepository.GetUnitsSorted())
            {
                _Units.Add(element);
            }
        }

        protected void UpdateDatabase()
        {
            // Prepare Local Collections.
            foreach (var element in _RemovedUnits)
            {
                _UnitRepository.RemoveUnit(element);
            }

            foreach (var element in _AddedUnits)
            {
                _UnitRepository.InsertUnit(element);
            }

            _UnitRepository.Save();
        }


        protected void SetSearchPredicate(SearchField field)
        {
            switch (field)
            {
                case SearchField.All:
                    _SearchView.Filter = new Predicate<object>(MatchAll);
                    break;
                case SearchField.ChannelNumber:
                    _SearchView.Filter = new Predicate<object>(MatchChannelNumber);
                    break;
                case SearchField.InstrumentName:
                    _SearchView.Filter = new Predicate<object>(MatchInstrumentName);
                    break;
                case SearchField.MulticoreName:
                    _SearchView.Filter = new Predicate<object>(MatchMulticoreName);
                    break;
                case SearchField.Position:
                    _SearchView.Filter = new Predicate<object>(MatchPosition);
                    break;
                case SearchField.UserField1:
                    _SearchView.Filter = new Predicate<object>(MatchUserField1);
                    break;
                case SearchField.UserField2:
                    _SearchView.Filter = new Predicate<object>(MatchUserField2);
                    break;
                case SearchField.UserField3:
                    _SearchView.Filter = new Predicate<object>(MatchUserField3);
                    break;
                case SearchField.UserField4:
                    _SearchView.Filter = new Predicate<object>(MatchUserField4);
                    break;
                default:
                    break;
            }
        }

        protected bool MatchAll(object item)
        {
            if (item == null)
            {
                return false;
            }

            var unit = item as DimmerDistroUnit;
            // Attempt to Match all Fields.
            // Instrument Name.
            if (MatchString(unit.InstrumentName, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // Channel Number.
            if (MatchString(unit.ChannelNumber, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // Position.
            if (MatchString(unit.Position, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // Multicore Name.
            if (MatchString(unit.MulticoreName, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // User Field 1.
            if (MatchString(unit.UserField1, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // User Field 2.
            if (MatchString(unit.UserField2, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // User Field 3.
            if (MatchString(unit.UserField3, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // User Field 4.
            if (MatchString(unit.UserField4, _FindInput, _IsFindCaseSensitive))
            {
                return true;
            }

            // No Match Found.
            return false;
        }
        
        protected bool MatchChannelNumber(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;

                return MatchString(unit.ChannelNumber, _FindInput, _IsFindCaseSensitive);
            } 
        }

        protected bool MatchInstrumentName(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;

                return MatchString(unit?.InstrumentName, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchPosition(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;
                return MatchString(unit.Position, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchMulticoreName(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;
                return MatchString(unit.MulticoreName, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchUserField1(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;
                return MatchString(unit.UserField1, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchUserField2(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;
                return MatchString(unit.UserField2, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchUserField3(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;
                return MatchString(unit.UserField3, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchUserField4(object item)
        {
            if (item == null)
            {
                return false;
            }

            else
            {
                var unit = item as DimmerDistroUnit;
                return MatchString(unit.UserField4, _FindInput, _IsFindCaseSensitive);
            }
        }

        protected bool MatchString(string a, string b, bool caseSensitive)
        {
            if (a == null)
            {
                a = string.Empty;
            }

            if (b == null)
            {
                b = string.Empty;
            }


            if (caseSensitive)
            {
                return a == b;
            }

            else
            {
                return a.ToLower() == b.ToLower();
            }
        }
        #endregion
    }
}
