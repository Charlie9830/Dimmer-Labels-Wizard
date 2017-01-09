using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Dimmer_Labels_Wizard_WPF.Repositories;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Data.Entity;

namespace Dimmer_Labels_Wizard_WPF
{
    public class EditorViewModel : ViewModelBase, INotifyModification
    {
        public EditorViewModel()
        {
            // Database Repositories.
            RefreshContext();
            LoadRespositories();

            SelectedCells.CollectionChanged += SelectedCells_CollectionChanged;
            SelectedRows.CollectionChanged += SelectedRows_CollectionChanged;
            Units.CollectionChanged += Units_CollectionChanged;
            SelectedUnits.CollectionChanged += SelectedUnits_CollectionChanged;
            Mergers.CollectionChanged += Mergers_CollectionChanged;

            // Global Event Subscriptions.
            Strips.CollectionChanged += Strips_CollectionChanged;

            // Commands.
            _MergeSelectedCellsCommand = new RelayCommand(MergeSelectedCellsExecute, MergeSelectedCellsCanExecute);
            _SplitSelectedCellsCommand = new RelayCommand(SplitSelectedCellsExecute, SplitSelectedCellsCanExecute);
            _UndoCommand = new RelayCommand(UndoCommandExecute, UndoCommandCanExecute);
            _RedoCommand = new RelayCommand(RedoCommandExecute, RedoCommandCanExecute);
            _CreateNewTemplateCommand = new RelayCommand(CreateNewTemplateCommandExecute);
            _EditTemplateCommand = new RelayCommand(EditTemplateCommandExecute, EditTemplateCommandCanExecute);
            _MakeUniqueTemplateCommand = new RelayCommand(MakeUniqueTemplateCommandExecute);
            _EditUniqueTemplateCommand = new RelayCommand(EditUniqueTemplateCommandExecute, EditUniqueTemplateCommandCanExecute);
            _OpenTemplateSettings = new RelayCommand(OpenTemplateSettingsExecute);
            _ShowLabelManagerCommand = new RelayCommand(ShowLabelManagerCommandExecute);
            _ShowLabelColorManagerCommand = new RelayCommand(ShowLabelColorManagerCommandExecute);
            _ResetZoomCommand = new RelayCommand(ResetZoomCommandExecute);
            _ZoomInCommand = new RelayCommand(ZoomInCommandExecute);
            _ZoomOutCommand = new RelayCommand(ZoomOutCommandExecute);
            _SetZoomPercentageCommand = new RelayCommand(SetZoomPercentageCommandExecute);
            _OpenDatabaseManagerCommand = new RelayCommand(OpenDatabaseManagerCommandExecute);
            _OpenPrintDialogCommand = new RelayCommand(OpenPrintDialogExecute);
            _AutoMergeCellsCommand = new RelayCommand(AutoMergeCellsCommandExecute);
            _SaveProgramStateCommand = new RelayCommand(SaveProgramStateCommandExecute, SaveProgramStateCommandCanExecute);
            _SaveAsProgramStateCommand = new RelayCommand(SaveAsProgramStateCommandExecute);
            _LoadProgramStateCommand = new RelayCommand(LoadProgramStateCommandExecute);
            _SaveShortcutCommand = new RelayCommand(SaveShortcutCommandExecute);

            // Initialize UndoRedoManager.
            UndoRedoManager = new UndoRedoManager(_UnitRepository);

            LastUsedFilePath = string.Empty;
        }

        #region Fields
        // Database and Repositories.
        protected PrimaryDB _Context;
        protected UnitRepository _UnitRepository;
        protected TemplateRepository _TemplateRepository;
        protected StripRepository _StripRepository;
        protected UniqueCellTemplateRepository _UniqueCellTemplateRepository;

        protected UndoRedoManager UndoRedoManager;

        private static double unitConversionRatio = 96d / 25.4d;

        private const string _NonEqualData = "***";

        private LabelField[] _LabelFields =
        {
            LabelField.ChannelNumber, LabelField.Custom, LabelField.InstrumentName,
            LabelField.MulticoreName, LabelField.NoAssignment, LabelField.Position, LabelField.UserField1,
            LabelField.UserField2, LabelField.UserField3, LabelField.UserField4
        };

        private LabelStripMode[] _LabelStripModes =
        {
            LabelStripMode.Single, LabelStripMode.Dual
        };

        protected bool InPresentStripDataOperation = false;

        protected const string _FileExt = "lab";
        protected const string _ExtFilter = "Dimmer Labels Wizard Label Files (.lab) |*.lab";

        protected LabelCellTemplate _UniqueTemplateStandardSelection = new LabelCellTemplate()
        {
            IsSpecialSelectionHandlingTemplate = true,
            UniqueCellName = "Standard Cell Template"
        };

        #endregion

        #region CLR Properties - Binding Target.

        public bool AreAnyCellsSelected
        {
            get { return SelectedCells.Count > 0; }
        }

        protected LabelCellTemplate _SelectedUniqueCellTemplate;

        public LabelCellTemplate SelectedUniqueCellTemplate
        {
            get { return _SelectedUniqueCellTemplate; }
            set
            {
                if (_SelectedUniqueCellTemplate != value)
                {
                    LabelCellTemplate oldValue = _SelectedUniqueCellTemplate;
                    _SelectedUniqueCellTemplate = value;

                    if (value != null &&
                        SelectedStrip != null &&
                        SelectedCells.Count > 0)
                    {
                        // Get Addresses of Selected Cells.
                        var selectedCellAddresses = GetCellAddresses(SelectedCells);

                        if (value != _UniqueTemplateStandardSelection)
                        {
                            // Push Selected Cell Address Assignments to Unique Cell Template.
                            foreach (var address in selectedCellAddresses)
                            {
                                if (_SelectedUniqueCellTemplate.StripAddresses.Contains(address, new StripAddressComparer()) == false)
                                {
                                    _SelectedUniqueCellTemplate.StripAddresses.Add(address);
                                }
                            }

                            // Push to Strip.
                            if (SelectedStrip.UniqueCellTemplates.Contains(_SelectedUniqueCellTemplate) == false)
                            {
                                SelectedStrip.UniqueCellTemplates.Add(_SelectedUniqueCellTemplate);
                            }
                        }

                        else
                        {
                            if (oldValue != null)
                            {
                                // Query ExistingUniqueLabelCellTemplates for Templates that Contain the Addresses of Currently Selected Cells.
                                // and remove those Addresses.
                                foreach (var element in selectedCellAddresses)
                                {
                                    var matchedTemplates = ExistingUniqueTemplates.FindAll(item => item.StripAddresses.Contains(element, new StripAddressComparer()));

                                    foreach (var template in matchedTemplates)
                                    {
                                        template.StripAddresses.RemoveAll(item => new StripAddressComparer().Equals(element, item));
                                    }
                                }

                                // Purge Strip.UniqueCellTemplate collections of LabelCellTemplates that no longer have any
                                // StripAddress Assignments.
                                var purgeQuery = (from strip in Strips
                                                 let uniqueCellTemplates = strip.UniqueCellTemplates
                                                 from uniqueCellTemplate in uniqueCellTemplates
                                                 where uniqueCellTemplate.StripAddresses.Count == 0
                                                 select new { Strip = strip, UniqueCellTemplate = uniqueCellTemplate }).ToList();

                                foreach (var element in purgeQuery)
                                {
                                    element.Strip.UniqueCellTemplates.Remove(element.UniqueCellTemplate);
                                }
                                
                            }
                        }
                        
                        OnPropertyChanged(nameof(FilteredUniqueCellTemplates));
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUniqueCellTemplate));

                    // Executes.
                    _EditUniqueTemplateCommand.CheckCanExecute();
                }
            }
        }

        public List<LabelCellTemplate> ExistingUniqueTemplates
        {
            get
            {
                // Return all the Existing Unique Templates with Injected Special Selection Handling Template.
                var uniqueTemplates = _UniqueCellTemplateRepository.GetUniqueCellTemplates().ToList();
                uniqueTemplates.Insert(0, _UniqueTemplateStandardSelection);

                return uniqueTemplates;
            }
        }

        protected double _LabelScale = 1d;

        public double LabelScale
        {
            get { return _LabelScale; }
            set
            {
                if (_LabelScale != value)
                {
                    _LabelScale = value;

                    // Notify.
                    OnPropertyChanged(nameof(LabelScale));
                }
            }
        }

        private Visibility _DatabasePanelVisibility = Visibility.Visible;

        public Visibility DatabasePanelVisibility
        {
            get { return _DatabasePanelVisibility; }
            set
            {
                if (value != _DatabasePanelVisibility)
                {
                    _DatabasePanelVisibility = value;
                    OnPropertyChanged(nameof(DatabasePanelVisibility));
                    OnPropertyChanged(nameof(DatabaseMultiSelectVisibility));
                }
            }
        }

        public Visibility DatabaseMultiSelectVisibility
        {
            get
            {
                // Reverse value of whatever DatabasePanelVisibility is doing.
                if (DatabasePanelVisibility == Visibility.Visible)
                {
                    return Visibility.Hidden;
                }

                else
                {
                    return Visibility.Visible;
                }
            }
        }

        private List<LabelCell> _Cells = new List<LabelCell>();

        public List<LabelCell> Cells
        {
            get { return _Cells; }
            set { _Cells = value; }
        }


        private bool _DatabaseChannelNumberEnable = true;

        public bool DatabaseChannelNumberEnable
        {
            get { return _DatabaseChannelNumberEnable; }
            protected set
            {
                if (_DatabaseChannelNumberEnable != value)
                {
                    _DatabaseChannelNumberEnable = value;
                    OnPropertyChanged(nameof(DatabaseChannelNumberEnable));
                }
            }
        }

        private bool _DatabaseInstrumentNameEnable = true;

        public bool DatabaseInstrumentNameEnable
        {
            get { return _DatabaseInstrumentNameEnable; }
            protected set
            {
                if (_DatabaseInstrumentNameEnable != value)
                {
                    _DatabaseInstrumentNameEnable = value;
                    OnPropertyChanged(nameof(DatabaseInstrumentNameEnable));
                }
            }
        }


        private bool _DatabasePositionEnable = true;

        public bool DatabasePositionEnable
        {
            get { return _DatabasePositionEnable; }
            protected set
            {
                if (_DatabasePositionEnable != value)
                {
                    _DatabasePositionEnable = value;
                    OnPropertyChanged(nameof(DatabasePositionEnable));
                }
            }
        }

        private bool _DatabaseMulticoreNameEnable = true;

        public bool DatabaseMulticoreNameEnable
        {
            get { return _DatabaseMulticoreNameEnable; }
            protected set
            {
                if (_DatabaseMulticoreNameEnable != value)
                {
                    _DatabaseMulticoreNameEnable = value;
                    OnPropertyChanged(nameof(DatabaseMulticoreNameEnable));
                }
            }
        }


        private bool _DatabaseUserField1Enable = true;

        public bool DatabaseUserField1Enable
        {
            get { return _DatabaseUserField1Enable; }
            protected set
            {
                if (_DatabaseUserField1Enable != value)
                {
                    _DatabaseUserField1Enable = value;
                    OnPropertyChanged(nameof(DatabaseUserField1Enable));
                }
            }
        }

        private bool _DatabaseUserField2Enable = true;

        public bool DatabaseUserField2Enable
        {
            get { return _DatabaseUserField2Enable; }
            protected set
            {
                if (_DatabaseUserField2Enable != value)
                {
                    _DatabaseUserField2Enable = value;
                    OnPropertyChanged(nameof(DatabaseUserField2Enable));
                }
            }
        }

        private bool _DatabaseUserField3Enable = true;

        public bool DatabaseUserField3Enable
        {
            get { return _DatabaseUserField3Enable; }
            protected set
            {
                if (_DatabaseUserField3Enable != value)
                {
                    _DatabaseUserField3Enable = value;
                    OnPropertyChanged(nameof(DatabaseUserField3Enable));
                }
            }
        }


        private bool _DatabaseUserField4Enable = true;

        public bool DatabaseUserField4Enable
        {
            get { return _DatabaseUserField4Enable; }
            protected set
            {
                if (_DatabaseUserField4Enable != value)
                {
                    _DatabaseUserField4Enable = value;
                    OnPropertyChanged(nameof(DatabaseUserField4Enable));
                }
            }
        }

        public int DatabaseDimmerNumber
        {
            get
            {
                if (SelectedDatabaseUnit != null)
                {
                    return SelectedDatabaseUnit.DimmerNumber;
                }

                else
                {
                    return 0;
                }
            }

            set
            {
                if (SelectedDatabaseUnit != null)
                {
                    SelectedDatabaseUnit.DimmerNumber = value;
                    OnPropertyChanged(nameof(DatabaseDimmerNumber));
                }
            }
        }

        public string DatabaseChannelNumber
        {
            get
            {
                return GetDatabaseData(LabelField.ChannelNumber);
            }
            set
            {
                SetDatabaseData(LabelField.ChannelNumber, value);
            }
        }

        public string DatabaseInstrumentName
        {
            get
            {
                return GetDatabaseData(LabelField.InstrumentName);
            }
            set
            {
                SetDatabaseData(LabelField.InstrumentName, value);
            }
        }

        public string DatabasePosition
        {
            get
            {
                return GetDatabaseData(LabelField.Position);
            }
            set
            {
                SetDatabaseData(LabelField.Position, value);
            }
        }

        public string DatabaseMulticoreName
        {
            get
            {
                return GetDatabaseData(LabelField.MulticoreName);
            }
            set
            {
                SetDatabaseData(LabelField.MulticoreName, value);
            }
        }

        public string DatabaseUserField1
        {
            get
            {
                return GetDatabaseData(LabelField.UserField1);
            }
            set
            {
                SetDatabaseData(LabelField.UserField1, value);
            }
        }

        public string DatabaseUserField2
        {
            get
            {
                return GetDatabaseData(LabelField.UserField2);
            }
            set
            {
                SetDatabaseData(LabelField.UserField2, value);
            }
        }

        public string DatabaseUserField3
        {
            get
            {
                return GetDatabaseData(LabelField.UserField3);
            }
            set
            {
                SetDatabaseData(LabelField.UserField3, value);
            }
        }

        public string DatabaseUserField4
        {
            get
            {
                return GetDatabaseData(LabelField.UserField4);
            }
            set
            {
                SetDatabaseData(LabelField.UserField4, value);
            }
        }

        private DimmerDistroUnit _SelectedDatabaseUnit = new DimmerDistroUnit();

        public DimmerDistroUnit SelectedDatabaseUnit
        {
            get { return _SelectedDatabaseUnit; }
            set
            {
                if (_SelectedDatabaseUnit != value)
                {
                    _SelectedDatabaseUnit = value;
                    OnPropertyChanged(nameof(SelectedDatabaseUnit));

                    // Set Property Grid Enables.
                    SetPropertyGridEnables(SelectedCells.FirstOrDefault(), value);

                    // Notify Property Grid
                    NotifyPropertyGridUI();
                }
            }
        }

        private ObservableCollection<DimmerDistroUnit> _SelectedUnits = new ObservableCollection<DimmerDistroUnit>();

        public ObservableCollection<DimmerDistroUnit> SelectedUnits
        {
            get { return _SelectedUnits; }
            set { _SelectedUnits = value; }
        }


        protected ObservableCollection<Merge> _Merges = new ObservableCollection<Merge>();
        public ObservableCollection<Merge> Mergers
        {
            get
            {
                return _Merges;
            }
        }


        private double _StripWidthmm;

        public double StripWidthmm
        {
            get { return Math.Round(_StripWidthmm, 2); }
        }

        private double _StripHeightmm;

        public double StripHeightmm
        {
            get
            { return Math.Round(_StripHeightmm, 2); }
        }

        private LabelStripMode _SelectedLabelStripMode;

        public LabelStripMode SelectedLabelStripMode
        {
            get
            {
                return _SelectedLabelStripMode;
            }
        }


        public Style DisplayedStyle
        {
            get
            {
                return SelectedStripTemplate?.Style;
            }
        }

        private LabelStripTemplate _SelectedStripTemplate;

        public LabelStripTemplate SelectedStripTemplate
        {
            get
            {
                return _SelectedStripTemplate;
            }
            set
            {
                if (value != _SelectedStripTemplate)
                {
                    _SelectedStripTemplate = value;

                    // Push Change to Selected Strip.
                    if (SelectedStrip != null)
                    {
                        SelectedStrip.AssignedTemplate = _SelectedStripTemplate;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedStripTemplate));
                    OnPropertyChanged(nameof(DisplayedStyle));

                    // Executes.
                    _EditTemplateCommand.CheckCanExecute();
                }
            }
        }


        protected IList<LabelStripTemplate> _StripTemplates;

        public IList<LabelStripTemplate> StripTemplates
        {
            get { return _StripTemplates; }
            set
            {
                if (_StripTemplates != value)
                {
                    _StripTemplates = value;

                    // Notify.
                    OnPropertyChanged(nameof(StripTemplates));
                }
            }
        }


        public LabelStripMode[] LabelStripModes
        {
            get
            {
                return _LabelStripModes;
            }
        }

        public LabelField[] LabelFields
        {
            get
            {
                return _LabelFields;
            }
        }

        public ObservableCollection<Strip> Strips
        {
            get
            {
                return _StripRepository.Local;
            }

        }

        private Strip _SelectedStrip;

        public Strip SelectedStrip
        {
            get
            {
                return _SelectedStrip;
            }

            set
            {
                if (_SelectedStrip != value)
                {
                    _SelectedStrip = value;
                    PresentStripData(value);
                    OnPropertyChanged(nameof(SelectedStrip));
                }
            }
        }

        private string _DebugOutput = "Debug Output";

        public string DebugOutput
        {
            get { return _DebugOutput; }
            set
            {
                _DebugOutput = value;
                OnPropertyChanged(nameof(DebugOutput));
            }
        }


        private ObservableCollection<DimmerDistroUnit> _Units = new ObservableCollection<DimmerDistroUnit>();

        public ObservableCollection<DimmerDistroUnit> Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        private ObservableCollection<LabelCell> _SelectedCells = new ObservableCollection<LabelCell>();

        public ObservableCollection<LabelCell> SelectedCells
        {
            get { return _SelectedCells; }
            set { _SelectedCells = value; }
        }

        protected string _SelectedData;

        public string SelectedData
        {
            get
            {
                return _SelectedData;
            }

            set
            {
                if (_SelectedData != value)
                {
                    SetSelectedData(value);
                    OnPropertyChanged(nameof(SelectedData));
                }
            }
        }

        public IEnumerable<LabelCellTemplateWrapper> FilteredUniqueCellTemplates
        {
            get
            {
                if (SelectedStrip == null)
                {
                    return null;
                }

                else
                {
                    return SelectedStrip.FilteredUniqueCellTemplates;
                }
            }
        }

        #endregion


        #region CLR Properties.
        public string LastUsedFilePath
        {
            get
            {
                return Properties.Settings.Default.LastUsedFilePath;
            }
            set
            {
                Properties.Settings.Default.LastUsedFilePath = value;
            }
        }

        private ObservableCollection<CellRow> _SelectedRows = new ObservableCollection<CellRow>();

        public ObservableCollection<CellRow> SelectedRows
        {
            get { return _SelectedRows; }
            set { _SelectedRows = value; }
        }


        #endregion

        #region Commands
        protected RelayCommand _SaveShortcutCommand;
        public ICommand SaveShortcutCommand
        {
            get
            {
                return _SaveShortcutCommand;
            }
        }

        protected void SaveShortcutCommandExecute(object parameter)
        {
            if (SaveProgramStateCommandCanExecute(new object()) == true)
            {
                SaveProgramState();
            }

            else
            {
                SaveAsProgramState();
            }
        }


        protected RelayCommand _SaveProgramStateCommand;
        public ICommand SaveProgramStateCommand
        {
            get
            {
                return _SaveProgramStateCommand;
            }
        }

        protected void SaveProgramStateCommandExecute(object parameter)
        {
            // Save or Save As.
            if (LastUsedFilePath == string.Empty)
            {
                SaveAsProgramState();
            }

            else
            {
                SaveProgramState();
            }

        }

        protected void SaveProgramState()
        {
            if (LastUsedFilePath != string.Empty)
            {
                var programState = GetProgramState();

                // Initialize Serializer.
                var serializer = new GlobalPersistanceManager(LastUsedFilePath);
                serializer.SerializeProgramState(programState);
            }
        }

        private ProgramState GetProgramState()
        {
            // Save progress to Database.
            PersistData();

            // Initialize new Serializer Friendly DB Context.
            using (var context = new PrimaryDB(true))
            {
                var unitRepo = new UnitRepository(context);
                var stripRepo = new StripRepository(context);
                var templateRepo = new TemplateRepository(context);
                var colorRepo = new ColorDictionaryRepository(context);

                // Package up Program State.
                var programState = new ProgramState();

                programState.Units.AddRange(unitRepo.GetUnits());
                programState.Strips.AddRange(stripRepo.GetStrips());
                programState.Templates.AddRange(templateRepo.GetTemplates());
                programState.ColorDictionaries.Add(colorRepo.DimmerColorDictionary);
                programState.ColorDictionaries.Add(colorRepo.DistroColorDictionary);

                return programState;
            }
        }

        protected bool SaveProgramStateCommandCanExecute(object parameter)
        {
            return LastUsedFilePath != string.Empty;
        }


        protected RelayCommand _SaveAsProgramStateCommand;
        public ICommand SaveAsProgramStateCommand
        {
            get
            {
                return _SaveAsProgramStateCommand;
            }
        }

        protected void SaveAsProgramStateCommandExecute(object parameter)
        {
            SaveAsProgramState();
        }

        protected void SaveAsProgramState()
        {
            var programState = GetProgramState();

            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = _ExtFilter;
            saveDialog.DefaultExt = _FileExt;

            if (saveDialog.ShowDialog() == true)
            {
                string filePath = saveDialog.FileName;

                // Initialize Serializer.
                var serializer = new GlobalPersistanceManager(filePath);
                serializer.SerializeProgramState(programState);

                // Store File Path.
                LastUsedFilePath = filePath;

                // Executes.
                _SaveProgramStateCommand.CheckCanExecute();
            }
        }

        protected RelayCommand _LoadProgramStateCommand;
        public ICommand LoadProgramStateCommand
        {
            get
            {
                return _LoadProgramStateCommand;
            }
        }

        protected void LoadProgramStateCommandExecute(object parameter)
        {
            var window = parameter as Window;

            string messageBoxText = "Changes will be lost, would you like to save changes to the current Labels?";
            string caption = "Open Labels";

            switch (MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    if (LastUsedFilePath == string.Empty)
                    {
                        SaveAsProgramState();
                        LoadProgramState(window);
                        break;
                    }

                    else
                    {
                        SaveProgramState();
                        LoadProgramState(window);
                        break;
                    }

                case MessageBoxResult.No:
                    LoadProgramState(window);
                    break;

                case MessageBoxResult.Cancel:
                    return;

                default:
                    break;
            }
        }

        protected void LoadProgramState(Window window)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = _ExtFilter;
            dialog.DefaultExt = _FileExt;

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                var serializer = new GlobalPersistanceManager(filePath);
                ProgramState incomingProgramingState = serializer.DeserializeProgramState();

                if (incomingProgramingState == null)
                {
                    MessageBox.Show("Something went wrong, Please check the file type and try again.",
                        "File Format Error", MessageBoxButton.OK);
                    return;
                }

                // You Should Somehow Vet the incoming Program State here before you Wipe the Database.

                // Clear the Database.
                var colorRepo = new ColorDictionaryRepository(_Context);

                _UnitRepository.RemoveAllUnits();
                _StripRepository.RemoveAll();
                _TemplateRepository.RemoveAllTemplates();
                colorRepo.RemoveAll();

                // The Repo Remove functions directly fire SQL commands to the Database, therefore, a new Context needs to
                // be created to ensure it is in Sync with the DB.
                using (var context = new PrimaryDB())
                {
                    var unitRepository = new UnitRepository(context);
                    var templateRepository = new TemplateRepository(context);
                    var stripRepository = new StripRepository(context);
                    colorRepo = new ColorDictionaryRepository(context);


                    // Insert Incoming Data.
                    foreach (var unit in incomingProgramingState.Units)
                    {
                        unitRepository.InsertUnit(unit);
                    }

                    foreach (var template in incomingProgramingState.Templates)
                    {
                        templateRepository.InsertTemplate(template);
                    }

                    foreach (var strip in incomingProgramingState.Strips)
                    {
                        stripRepository.Insert(strip);
                    }

                    foreach (var dictionary in incomingProgramingState.ColorDictionaries)
                    {
                        if (dictionary.EntriesRackType == RackType.Dimmer)
                        {
                            foreach (var entry in dictionary)
                            {
                                colorRepo.DimmerColorDictionary.Add(entry.UniverseKey, entry.DimmerNumberKey, entry.Value);
                            }
                        }

                        if (dictionary.EntriesRackType == RackType.Distro)
                        {
                            foreach (var entry in dictionary)
                            {
                                colorRepo.DistroColorDictionary.Add(entry.UniverseKey, entry.DimmerNumberKey, entry.Value);
                            }
                        }
                    }

                    unitRepository.Save();
                    templateRepository.Save();
                    stripRepository.Save();
                    colorRepo.Save();
                }

                // Persist FilePath.
                LastUsedFilePath = filePath;

                // Reload the Window.
                window.Close();
                var newEditor = new Editor();

                // Set LastUsedFilePath value of new ViewModel.
                var viewModel = newEditor.DataContext as EditorViewModel;
                viewModel.LastUsedFilePath = filePath;

                newEditor.Show();
                _Context.Dispose();
            }
        }

        protected RelayCommand _AutoMergeCellsCommand;
        public ICommand AutoMergeCellsCommand
        {
            get
            {
                return _AutoMergeCellsCommand;
            }
        }

        protected void AutoMergeCellsCommandExecute(object parameter)
        {
            ClearSelections();
            var merges = new List<Merge>();

            foreach (var element in Strips)
            {
                // Select Strip.
                SelectedStrip = element;

                // Get Upper Cells.
                var upperCells = (from cell in Cells
                                  where cell.CellVerticalPosition == CellVerticalPosition.Upper
                                  select cell).ToList();

                if (upperCells.Count > 0)
                {
                    DimmerDistroUnit primaryUnit = new DimmerDistroUnit();
                    var consumedUnits = new List<DimmerDistroUnit>();
                    LabelField displayedDataField = upperCells.First().DisplayedDataFields.First();
                    bool mergeBuilding = false;

                    // Merge Building Loop.
                    for (int index = 0; index < upperCells.Count; index++)
                    {
                        if (index == upperCells.Count - 2)
                        {
                            break;
                        }

                        DimmerDistroUnit currentReference = upperCells[index].DataReference;
                        DimmerDistroUnit nextReference = upperCells[index + 1].DataReference;
                        string currentData = currentReference.GetData(displayedDataField);
                        string nextData = nextReference.GetData(displayedDataField);

                        if (currentData == nextData)
                        {
                            if (mergeBuilding == false)
                            {
                                // Start building a new Merge
                                mergeBuilding = true;
                                primaryUnit = currentReference;
                                consumedUnits.Add(nextReference);
                            }

                            else
                            {
                                // Merge is already being built, add it to that one.
                                consumedUnits.Add(currentReference);
                            }
                        }

                        else
                        {
                            if (mergeBuilding == true)
                            {
                                // Complete Merge.
                                merges.Add(new Merge(CellVerticalPosition.Upper, primaryUnit, consumedUnits));

                                mergeBuilding = false;
                                primaryUnit = null;
                                consumedUnits.Clear();
                            }
                        }
                    }
                }

                element.Mergers.AddRange(merges);
            }
        }

        protected RelayCommand _OpenPrintDialogCommand;
        public ICommand OpenPrintDialogCommand
        {
            get
            {
                return _OpenPrintDialogCommand;
            }
        }

        protected void OpenPrintDialogExecute(object parameter)
        {
            // Push to Database.
            PersistData();

            var dialog = new PrintWindow();

            dialog.ShowDialog();
        }

        protected RelayCommand _OpenDatabaseManagerCommand;
        public ICommand OpenDatabaseManagerCommand
        {
            get
            {
                return _OpenDatabaseManagerCommand;
            }
        }

        protected void OpenDatabaseManagerCommandExecute(object parameter)
        {
            // Persist Progress.
            PersistData();

            var dialog = new DatabaseManager();

            dialog.Show();

            // Re Load Repositories.
            RefreshContext();
            LoadRespositories();
        }



        protected RelayCommand _SetZoomPercentageCommand;
        public ICommand SetZoomPercentageCommand
        {
            get
            {
                return _SetZoomPercentageCommand;
            }
        }

        protected void SetZoomPercentageCommandExecute(object parameter)
        {
            int percentage;

            if (int.TryParse((string)parameter, out percentage))
            {
                LabelScale = (double)percentage / 100d;
            }
        }

        protected RelayCommand _ResetZoomCommand;
        public ICommand ResetZoomCommand
        {
            get
            {
                return _ResetZoomCommand;
            }
        }

        protected void ResetZoomCommandExecute(object parameter)
        {
            LabelScale = 1d;
        }


        protected RelayCommand _ZoomInCommand;
        public ICommand ZoomInCommand
        {
            get
            {
                return _ZoomInCommand;
            }
        }

        protected void ZoomInCommandExecute(object parameter)
        {
            LabelScale += 0.1d;
        }


        protected RelayCommand _ZoomOutCommand;
        public ICommand ZoomOutCommand
        {
            get
            {
                return _ZoomOutCommand;
            }
        }

        protected void ZoomOutCommandExecute(object parameter)
        {
            LabelScale -= 0.1d;
        }

        protected RelayCommand _ShowLabelColorManagerCommand;
        public ICommand ShowLabelColorManagerCommand
        {
            get
            {
                return _ShowLabelColorManagerCommand;
            }
        }

        protected void ShowLabelColorManagerCommandExecute(object parameter)
        {
            // Persist Progress.
            PersistData();

            var dialog = new LabelColorManager();

            if (dialog.ShowDialog() == true)
            {
                // Changes to Coloring have been Made, Invalidate LabelColors.
                foreach (var element in Units)
                {
                    element.InvalidateLabelColor();
                }
            }
        }

        protected RelayCommand _ShowLabelManagerCommand;
        public ICommand ShowLabelManagerCommand
        {
            get
            {
                return _ShowLabelManagerCommand;
            }
        }

        protected void ShowLabelManagerCommandExecute(object parameter)
        {
            // Persist Progress.
            PersistData();

            var dialog = new LabelManager();
            var viewModel = dialog.DataContext as LabelManagerViewModel;

            if (SelectedStrip != null)
            {
                // Pre Select a Strip for LabelManager.
                SelectedStrip.IsSelected = true;
            }

            dialog.ShowDialog();

            LoadRespositories();

            PresentStripData(_SelectedStrip);
        }

        protected RelayCommand _MakeUniqueTemplateCommand;
        public ICommand MakeUniqueTemplateCommand
        {
            get
            {
                return _MakeUniqueTemplateCommand;
            }
        }

        protected void MakeUniqueTemplateCommandExecute(object parameter)
        {
            // Generate a new UniqueTemplateManager Dialog.
            ShowUniqueTemplateManager(null);
        }

        protected RelayCommand _EditUniqueTemplateCommand;
        public ICommand EditUniqueTemplateCommand
        {
            get
            {
                return _EditUniqueTemplateCommand;
            }
        }

        protected void EditUniqueTemplateCommandExecute(object parameter)
        {
            if (SelectedUniqueCellTemplate != null)
            {
                ShowUniqueTemplateManager(SelectedUniqueCellTemplate);
            }
        }

        protected bool EditUniqueTemplateCommandCanExecute(object parameter)
        {
            return SelectedUniqueCellTemplate != null && SelectedUniqueCellTemplate != _UniqueTemplateStandardSelection;
        }

      
        protected RelayCommand _EditTemplateCommand;

        public ICommand EditTemplateCommand
        {
            get
            {
                return _EditTemplateCommand;
            }
        }

        protected void EditTemplateCommandExecute(object parameter)
        {
            // Persist Progess.
            PersistData();

            var templateEditor = new TemplateEditor();
            var viewModel = templateEditor.DataContext as TemplateEditorViewModel;

            // Preselect Template.
            viewModel.SelectedTemplateName = SelectedStripTemplate.Name;

            templateEditor.ShowDialog();

            LoadRespositories();

            // Assert Displayed Template.
            if (SelectedStrip != null)
            {
                _SelectedStripTemplate = SelectedStrip.AssignedTemplate;
                OnPropertyChanged(nameof(SelectedStripTemplate));
            }

            OnPropertyChanged(nameof(DisplayedStyle));
        }

        protected bool EditTemplateCommandCanExecute(object parameter)
        {
            return !(SelectedStripTemplate == null);
        }

        private RelayCommand _CreateNewTemplateCommand;

        public ICommand CreateNewTemplateCommand
        {
            get
            {
                return _CreateNewTemplateCommand;
            }
        }

        protected void CreateNewTemplateCommandExecute(object parameter)
        {
            // Persist Progress.
            PersistData();

            // Show the NewTemplate Dialog first. If user chooses to continue with New Template creation. Show
            // the Template Editor with the new Template pre selected.
            var newTemplateDialog = new NewTemplate();
            var newTemplateViewModel = newTemplateDialog.DataContext as NewTemplateViewModel;

            if (newTemplateDialog.ShowDialog() == true)
            {
                // User wants to continue with Template Creation.
                var templateEditor = new TemplateEditor();
                var templateEditorViewModel = templateEditor.DataContext as TemplateEditorViewModel;

                // Pre Select the new Template.
                templateEditorViewModel.SelectedTemplateName = newTemplateViewModel.TemplateName;

                templateEditor.ShowDialog();

                _TemplateRepository.Load();

                if (SelectedStrip != null)
                {
                    // Re Assert Displayed Template in case user also edited the currently Displayed Template.
                    OnPropertyChanged(nameof(DisplayedStyle));
                }
            }

            LoadRespositories();
        }

        protected RelayCommand _OpenTemplateSettings;
        public ICommand OpenTemplateSettings
        {
            get
            {
                return _OpenTemplateSettings;
            }
        }

        protected void OpenTemplateSettingsExecute(object parameter)
        {
            // Persist Progress.
            PersistData();

            if (parameter.GetType() != typeof(string))
            {
                return;
            }

            int tabIndex = 0;

            // Try Parse. Otherwise tabIndex Remains 0.
            int.TryParse((string)parameter, out tabIndex);

            var templateEditor = new TemplateEditor();
            var viewModel = templateEditor.DataContext as TemplateEditorViewModel;

            viewModel.SelectedTabIndex = tabIndex;

            templateEditor.ShowDialog();

            if (SelectedStrip != null)
            {
                // Re Assert Displayed Template in case user also edited the Currently displayed Template.
                OnPropertyChanged(nameof(DisplayedStyle));



            }

            // Re Load Repositories.
            RefreshContext();
            LoadRespositories();
        }

        // Undo.
        private RelayCommand _UndoCommand;

        public ICommand UndoCommand
        {
            get
            {
                return _UndoCommand;
            }
        }

        protected void UndoCommandExecute(object parameter)
        {
            UndoRedoManager.Undo();
        }

        protected bool UndoCommandCanExecute(object parameter)
        {
            return UndoRedoManager.CanUndo;
        }

        // Redo
        private RelayCommand _RedoCommand;

        public ICommand RedoCommand
        {
            get
            {
                return _RedoCommand;
            }
        }

        protected void RedoCommandExecute(object parameter)
        {
            UndoRedoManager.Redo();
        }

        protected bool RedoCommandCanExecute(object parameter)
        {
            return UndoRedoManager.CanRedo;
        }

        // Merge Cells
        private RelayCommand _MergeSelectedCellsCommand;

        public ICommand MergeSelectedCellsCommand
        {
            get
            {
                return _MergeSelectedCellsCommand;
            }
        }

        protected void MergeSelectedCellsExecute(object parameter)
        {
            MergeSelections();
        }

        protected bool MergeSelectedCellsCanExecute(object parameter)
        {
            if (SelectedCells.Count <= 1)
            {
                // No Cells Selected.
                return false;
            }

            if (SelectedCells.All(item => item.CellVerticalPosition == CellVerticalPosition.Upper) ^
            SelectedCells.All(item => item.CellVerticalPosition == CellVerticalPosition.Lower))
            {
                // Cell selection does not span between Upper and Lower Cells.

                // Determine if Indexes are consequtive, and Thus if Cells are adjacent to eachother.
                if (AreCellsAdjacent(SelectedCells))
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            return false;
        }

        // Split Cells / DeMerge Cells.
        private RelayCommand _SplitSelectedCellsCommand;

        public ICommand SplitSelectedCellsCommand
        {
            get
            {
                return _SplitSelectedCellsCommand;
            }
        }

        protected void SplitSelectedCellsExecute(object parameter)
        {
            DeMergeSelections();
        }

        protected bool SplitSelectedCellsCanExecute(object parameter)
        {
            if (SelectedCells.Count == 0)
            {
                return false;
            }

            if (SelectedCells.All(item => item.IsMerged == true))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        #endregion

        #region Event Handlers
        private void Mergers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (InPresentStripDataOperation == false && SelectedStrip != null)
            {
                if (e.NewItems != null)
                {
                    foreach (var element in e.NewItems)
                    {
                        var merge = element as Merge;

                        if (SelectedStrip.Mergers.Contains(merge) == false)
                        {
                            SelectedStrip.Mergers.Add(merge);
                        }
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (var element in e.OldItems)
                    {
                        SelectedStrip.Mergers.Remove(element as Merge);
                    }
                }
            }
        }

        private void Strips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Strip>;
            OnPropertyChanged(nameof(Strips));

            if (collection.Count > 0)
            {
                SelectedStrip = collection.First();
            }

            else
            {
                SelectedStrip = null;
            }
        }

        private void Units_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    // Disconnect outgoing Event.
                    var unit = element as DimmerDistroUnit;
                    unit.PropertyChanged -= Unit_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    // Connect incoming Event.
                    var unit = element as DimmerDistroUnit;
                    unit.PropertyChanged += Unit_PropertyChanged;
                }
            }
        }

        private void Unit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // A DimmerDistroUnit DataReference has signalled a Modified property.
            // check if any View Model Objects need to update.

            var unit = sender as DimmerDistroUnit;
            string propertyName = e.PropertyName;
            LabelField dataField;

            // Map PropertyName to LabelField.
            if (Enum.TryParse(propertyName, out dataField) == false)
            {
                // Could not find Match.
                return;
            }

            if (SelectedCells.Count > 0)
            {
                bool cellMatchFound = SelectedCells.Where(item => item.DataReference == unit).Any();

                if (cellMatchFound)
                {
                    // Pre Set selected Data.
                    _SelectedData = GetSelectedData(unit, dataField);

                    // Notify UI.
                    OnPropertyChanged(nameof(SelectedData));
                    NotifyPropertyGridUI();
                }
            }
        }

        private void SelectedUnits_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<DimmerDistroUnit>;

            if (collection.Count > 0)
            {
                SelectedDatabaseUnit = collection.First();
            }

            else
            {
                SelectedDatabaseUnit = null;
            }
        }

        private void SelectedCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<LabelCell>;

            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cell = element as LabelCell;

                    // Sync ViewModel SelectedRows Collection with current Cell Row Selection State.
                    foreach (var row in cell.SelectedRows)
                    {
                        SelectedRows.Add(row);
                    }

                    // Connect Event handler for future Row Selection changes.
                    cell.SelectedRows.CollectionChanged += SelectedCells_SelectedRows_CollectionChanged;

                    // Update DatabasePanel.
                    if (SelectedUnits.Contains(cell.DataReference) == false)
                    {
                        SelectedUnits.Add(cell.DataReference);
                    }

                    if (cell.IsMerged == true)
                    {
                        foreach (var unit in cell.ConsumedReferences)
                        {
                            if (SelectedUnits.Contains(unit) == false)
                            {
                                SelectedUnits.Add(unit);
                            }
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;

                    // Disconnect Event Handler.
                    cell.SelectedRows.CollectionChanged -= SelectedCells_SelectedRows_CollectionChanged;

                    // Update SelectedUnits.
                    var totalMergedCells = SelectedCells.SelectMany(item => item.ConsumedReferences);

                    if (totalMergedCells.Contains(cell.DataReference) == false)
                    {
                        // Cell has been actually deselected by User, as opposed to being consumed in a Merge operation.

                        // Remove Unit from Selected Units.
                        SelectedUnits.Remove(cell.DataReference);

                        // Remove Consumed References.
                        if (cell.IsMerged)
                        {
                            foreach (var unit in cell.ConsumedReferences)
                            {
                                SelectedUnits.Remove(unit);
                            }
                        }
                    }
                }
            }

            // UI Updates.
            SelectedUniqueCellTemplate = CollectUniqueCellTemplateSelection(SelectedCells);
            SelectedData = GetSelectedData();
            OnPropertyChanged(nameof(SelectedCells));
            OnPropertyChanged(nameof(AreAnyCellsSelected));
            DatabasePanelVisibility = collection.Count > 1 ? Visibility.Hidden : Visibility.Visible;

            // Command CanExecute Notifications.
            _MergeSelectedCellsCommand.CheckCanExecute();
            _SplitSelectedCellsCommand.CheckCanExecute();
            _MakeUniqueTemplateCommand.CheckCanExecute();
            _EditUniqueTemplateCommand.CheckCanExecute();
        }

        protected LabelCellTemplate CollectUniqueCellTemplateSelection(IEnumerable<LabelCell> currentSelectedCells)
        {
            // Checks for Equalty of currently Selected Cell Templates. If all the same Unique, return that Unique,
            // if all Standard return the Standard Special LabelCellTemplate, if mixed, return null.

            if (currentSelectedCells.Count() == 0)
            {
                return null;
            }

            var cellTemplates = new List<LabelCellTemplate>();
            var isUniqueFlags = new List<bool>();

            foreach (var cell in currentSelectedCells)
            {
                bool isUnique;
                cellTemplates.Add(GetCellTemplate(cell, out isUnique));
                isUniqueFlags.Add(isUnique);
            }

            if (isUniqueFlags.All(item => item == false))
            {
                // All isUniqueFlags are false, therefore return the Special Standard Template.
                return _UniqueTemplateStandardSelection;
            }

            else if (isUniqueFlags.All(item => item))
            {
               if (cellTemplates.Count == cellTemplates.Distinct().Count())
                {
                    // All the Same Template.
                    return cellTemplates.First();
                }

               else
                {
                    // Differing Templates.
                    return null;
                }
            }

            else
            {
                // Mix of Unique and other Uniques and Standards.
                return null;
            }
        }

        private void SelectedRows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedRows));
            SelectedData = GetSelectedData();
        }

        /// <summary>
        /// Connected to the SelectedRows property of SelectedCells. To Track changes in Row Selection that do not invoke a
        /// cell selection Change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedCells_SelectedRows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                // Add to ViewModel SelectedRows collection.
                foreach (var element in e.NewItems)
                {
                    var row = element as CellRow;
                    if (SelectedRows.Contains(row) == false)
                    {
                        SelectedRows.Add(row);
                    }
                }
            }

            if (e.OldItems != null)
            {
                // Remove from ViewModel SelectedRows collection.
                foreach (var element in e.OldItems)
                {
                    var row = element as CellRow;
                    SelectedRows.Remove(row);
                }
            }
        }

        #endregion

        #region Methods
        protected void ShowUniqueTemplateManager(LabelCellTemplate templateToEdit)
        {
            // Persist Data.
            PersistData();

            var dialog = new UniqueCellTemplateEditor();
            var viewModel = dialog.DataContext as UniqueCellTemplateEditorViewModel;

            if (templateToEdit != null)
            {
                viewModel.SelectedExistingTemplate = templateToEdit;
            }

            else
            {
                viewModel.CreateNewUniqueTemplate(null);
            }

            // Load the DataReference if any Cells are selected.
            if (AreAnyCellsSelected == true)
            {
                viewModel.DataReference = SelectedCells.Last().DataReference;
            }

            if (dialog.ShowDialog() == true)
            {
                // Notify.
                OnPropertyChanged(nameof(ExistingUniqueTemplates));

                // Deselect currently selected Unique Cell Template if it has been deleted from DB.
                if (SelectedUniqueCellTemplate != null)
                {
                    EntityState? entityState = _Context.Entry(SelectedUniqueCellTemplate)?.State;

                    if (entityState != null && entityState == EntityState.Deleted)
                    {
                        SelectedUniqueCellTemplate = null;
                    }
                }
            }

            // Invalidate Unique Templates as they may have been Modified.
            OnPropertyChanged(nameof(FilteredUniqueCellTemplates));
        }

        protected List<StripAddress> GetCellAddresses(IEnumerable<LabelCell> cellCollection)
        {
            var returnList = new List<StripAddress>();

            foreach (var cell in cellCollection)
            {
                var address = new StripAddress()
                {
                    Strip = SelectedStrip,
                    VerticalPosition = cell.CellVerticalPosition,
                    HorizontalIndex = cell.HorizontalIndex,
                };

                returnList.Add(address);
            }

            return returnList;
        }

    

        protected LabelCellTemplate GetCellTemplate(LabelCell cell, out bool foundUniqueTemplate)
        {
            if (cell == null)
            {
                foundUniqueTemplate = false;
                return null;
            }

            // Query for an existing Unique Template.
            Predicate<StripAddress> matchAddressToCell = addr => addr.Strip == SelectedStrip &&
                                                         addr.VerticalPosition == cell.CellVerticalPosition &&
                                                         addr.HorizontalIndex == cell.HorizontalIndex;

            var query = from uniqueTemplate in FilteredUniqueCellTemplates
                        where uniqueTemplate.FilteredStripAddresses.Find(matchAddressToCell) != null
                        select uniqueTemplate;

            if (query.Count() > 0)
            {
                // Return Unique Template
                foundUniqueTemplate = true;
                return query.First().CellTemplate;
            }

            else
            {
                // Return the Template from the Assigned Strip Template.
                foundUniqueTemplate = false;
                return cell.CellVerticalPosition == CellVerticalPosition.Upper ?
                    SelectedStrip.AssignedTemplate.UpperCellTemplate :
                    SelectedStrip.AssignedTemplate.LowerCellTemplate;
            }
        }
         
        public void PersistData()
        {
            _TemplateRepository.Save();
            _StripRepository.Save();
            _UnitRepository.Save();
        }

        protected void RefreshContext()
        {
            // Load/Reload Context.
            if (_Context != null)
            {
                _Context.Dispose();
            }

            _Context = new PrimaryDB();

            // Load Repositories.
            _UnitRepository = new UnitRepository(_Context);
            _TemplateRepository = new TemplateRepository(_Context);
            _StripRepository = new StripRepository(_Context);
            _UniqueCellTemplateRepository = new UniqueCellTemplateRepository(_Context);
        }

        protected void LoadRespositories()
        {

            StripTemplates = _TemplateRepository.GetTemplates();
            _StripRepository.Load();
            _TemplateRepository.Load();
            _UnitRepository.Load();
            _UniqueCellTemplateRepository.Load();

            // Notify.
            OnPropertyChanged(nameof(Strips));
            OnPropertyChanged(nameof(StripTemplates));
            OnPropertyChanged(nameof(SelectedStrip));
            OnPropertyChanged(nameof(SelectedStripTemplate));
        }

        private void NotifyPropertyGridUI()
        {
            OnPropertyChanged(nameof(DatabaseDimmerNumber));
            OnPropertyChanged(nameof(DatabaseChannelNumber));
            OnPropertyChanged(nameof(DatabaseInstrumentName));
            OnPropertyChanged(nameof(DatabasePosition));
            OnPropertyChanged(nameof(DatabaseMulticoreName));
            OnPropertyChanged(nameof(DatabaseUserField1));
            OnPropertyChanged(nameof(DatabaseUserField2));
            OnPropertyChanged(nameof(DatabaseUserField3));
            OnPropertyChanged(nameof(DatabaseUserField4));
        }

        protected void BeginPresentStripDataOperation()
        {
            InPresentStripDataOperation = true;
        }

        protected void EndPresentStripDataOperation()
        {
            InPresentStripDataOperation = false;
        }

        private void PresentStripData(Strip value)
        {
            BeginPresentStripDataOperation();

            // Clear Selections.
            ClearSelections();

            // Clear Current Unit Collection.
            while (Units.Count > 0)
            {
                Units.RemoveAt(Units.Count - 1);
            }

            // Clear Mergers.
            while (Mergers.Count > 0)
            {
                Mergers.RemoveAt(Mergers.Count - 1);
            }

            // Load new StripData.
            if (_SelectedStrip != null)
            {
                foreach (var element in _SelectedStrip.GetUnits(_UnitRepository))
                {
                    Units.Add(element);
                }

                foreach (var element in _SelectedStrip.Mergers)
                {
                    Mergers.Add(element);
                }

                // Retrieve and Load Template.
                LoadTemplate(value);
            }

            // UI Updates.
            SelectedUniqueCellTemplate = null;

            // Notify.
            OnPropertyChanged(nameof(Units));
            OnPropertyChanged(nameof(SelectedCells));;
            OnPropertyChanged(nameof(FilteredUniqueCellTemplates));

            EndPresentStripDataOperation();
        }

        private void LoadTemplate(Strip strip)
        {
            if (strip == null)
            {
                return;
            }

            // Displayed Template.
            SelectedStripTemplate = strip.AssignedTemplate;

            // Appearance Controls
            _SelectedLabelStripMode = strip.AssignedTemplate.StripMode;
            _StripWidthmm = strip.AssignedTemplate.StripWidth / unitConversionRatio;
            _StripHeightmm = strip.AssignedTemplate.StripHeight / unitConversionRatio;
            _SelectedStripTemplate = strip.AssignedTemplate;

            // Notify.
            OnPropertyChanged(nameof(DisplayedStyle));
            OnPropertyChanged(nameof(SelectedLabelStripMode));
            OnPropertyChanged(nameof(StripWidthmm));
            OnPropertyChanged(nameof(StripHeightmm));
            OnPropertyChanged(nameof(SelectedStripTemplate));
        }


        private void WriteDebugOutput(string message)
        {
            DebugOutput = message;
            OnPropertyChanged(nameof(DebugOutput));
        }

        private List<CellRow> ExtractSelectedRows(LabelCell targetCell)
        {
            // Return a list of Cells that are flagged as IsSelected.
            return targetCell.SelectedRows.Where(item => item.IsSelected == true).ToList();
        }

        /// <summary>
        /// Returns a string representing the currently selected Data.
        /// </summary>
        /// <returns></returns>
        private string GetSelectedData()
        {
            var dataElements = new List<string>();

            // Collect all Row.Data elements.
            foreach (var element in SelectedRows)
            {
                if (dataElements.Contains(element.Data) == false)
                {
                    dataElements.Add(element.Data);
                }
            }

            // Collect all SingleFieldMode Cell.Data elements.
            foreach (var element in SelectedCells)
            {
                if (element.CellDataMode == CellDataMode.SingleField)
                {
                    dataElements.Add(element.SingleFieldData);
                }
            }

            if (dataElements.Count > 0)
            {
                string reference = dataElements.First();

                if (dataElements.TrueForAll(item => item == reference) == true)
                {
                    return reference;
                }

                else
                {
                    return _NonEqualData;
                }
            }

            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns a string representing the currently Selected Data modified by injected Source Unit and DataField.
        /// </summary>
        /// <param name="sourceUnit"></param>
        /// <param name="injectionDataField"></param>
        /// <returns></returns>
        private string GetSelectedData(DimmerDistroUnit sourceUnit, LabelField injectionDataField)
        {
            var dataElements = new List<string>();

            // Add injectionData.
            dataElements.Add(sourceUnit.GetData(injectionDataField));

            // Collect all Row.Data elements.
            foreach (var element in SelectedRows)
            {
                if (dataElements.Contains(element.Data) == false &&
                    (element.CellParent.DataReference != sourceUnit && element.DataField != injectionDataField))
                {
                    // If dataElement String has not already been added and it does 
                    // not reference the sourceUnit and the injectionDataField. Add it to dataElements.
                    dataElements.Add(element.Data);
                }
            }

            // Collect all SingleFieldMode Cell.Data elements.
            foreach (var element in SelectedCells)
            {
                if (element.CellDataMode == CellDataMode.SingleField &&
                    (element.DataReference != sourceUnit && element.SingleFieldDataField != injectionDataField))
                {
                    // If dataElement String has not already been added and it does 
                    // not reference the sourceUnit and the injectionDataField. Add it to dataElements.
                    dataElements.Add(element.SingleFieldData);
                }
            }

            if (dataElements.Count > 0)
            {
                string reference = dataElements.First();

                if (dataElements.TrueForAll(item => item == reference) == true)
                {
                    return reference;
                }

                else
                {
                    return _NonEqualData;
                }
            }

            else
            {
                return string.Empty;
            }
        }

        private void SetSelectedData(string data)
        {
            if (data == _NonEqualData)
            {
                _SelectedData = data;
                OnPropertyChanged(nameof(SelectedData));
                return;
            }

            // Set Row Data
            foreach (var element in SelectedRows)
            {
                element.Data = data;
            }

            // Set SingleField Data Mode cells.
            foreach (var element in SelectedCells)
            {
                if (element.CellDataMode == CellDataMode.SingleField)
                {
                    element.SingleFieldData = data;
                }
            }

            // Set SelectedData Backing Field.
            _SelectedData = data;
        }

        public void ClearSelections()
        {
            while (SelectedCells.Count > 0)
            {
                SelectedCells.RemoveAt(SelectedCells.Count - 1);
            }
        }

        public void MergeSelections()
        {
            if (SelectedCells.Count > 1)
            {
                CellVerticalPosition verticalPosition;
                var consumedUnits = new List<DimmerDistroUnit>();

                // Sort Selected Cells into Horizontal Index.
                var orderedSelectedCells = SelectedCells.OrderBy(item => item.HorizontalIndex);

                DimmerDistroUnit primaryUnit = orderedSelectedCells.First().DataReference;

                if (SelectedCells.All(item => item.CellVerticalPosition == CellVerticalPosition.Upper))
                {
                    // All UpperCells.
                    verticalPosition = CellVerticalPosition.Upper;
                }
                 
                else if (SelectedCells.All(item => item.CellVerticalPosition == CellVerticalPosition.Lower))
                {
                    // All LowerCells
                    verticalPosition = CellVerticalPosition.Lower;
                }   

                else
                {
                    // Illegal operation. Only cells residing within the same Strips can be merged.
                    return;
                }


                // Collect Data references of soon to be Consumed Cells.
                bool onFirstCell = true;
                foreach (var element in orderedSelectedCells)
                {
                    if (onFirstCell)
                    {
                        // Skip first Cell.
                        onFirstCell = false;
                    }

                    else
                    {
                        consumedUnits.Add(element.DataReference);
                    }
                }

                // Add Merge object to Collection.
                Mergers.Add(new Merge(verticalPosition, primaryUnit, consumedUnits));
            }
        }

        public void DeMergeSelections()
        {
            // Generate a collection of Merge objects to be removed.
            var deMergingCells = SelectedCells.Where(item => item.IsMerged == true);
            var primaryUnits = deMergingCells.Select(item => item.DataReference);
            var merges = Mergers.Where(item => primaryUnits.Contains(item.PrimaryUnit)).ToList();

            foreach (var element in merges)
            {
                Mergers.Remove(element);
            }

            // Cleanse SelectedUnits Collection.
            CleanseSelectedUnits();
        }

        protected bool AreCellsAdjacent(IEnumerable<LabelCell> cellCollection)
        {
            var indexes = cellCollection.Select(item => item.HorizontalIndex).OrderBy(item => item);
            var range = Enumerable.Range(indexes.First(), indexes.Count());

            if (indexes.SequenceEqual(range))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        protected string GetDatabaseData(LabelField dataField)
        {
            if (SelectedDatabaseUnit == null)
            {
                return string.Empty;
            }

            return SelectedDatabaseUnit.GetData(dataField);
        }

        protected void SetDatabaseData(LabelField dataField, string value)
        {
            if (SelectedDatabaseUnit == null)
            {
                return;
            }

            SelectedDatabaseUnit.SetData(value, dataField);
        }

        /// <summary>
        /// Removes any Leftover Units from DeMerge operations.
        /// </summary>
        private void CleanseSelectedUnits()
        {
            // Clean up SelectedUnits Collection.
            // Remove any Units that have been left behind from a Split Cell operation.

            // Collect all references.
            var selectedReferences = new List<DimmerDistroUnit>();

            foreach (var element in SelectedCells)
            {
                if (selectedReferences.Contains(element.DataReference) == false)
                {
                    selectedReferences.Add(element.DataReference);
                }

                if (element.IsMerged)
                {
                    foreach (var consumedReference in element.ConsumedReferences)
                    {
                        if (selectedReferences.Contains(consumedReference) == false)
                        {
                            selectedReferences.Add(consumedReference);
                        }
                    }
                }
            }

            // Remove references that are not currently selected.
            var removalReferences = SelectedUnits.Except(selectedReferences).ToList();
            foreach (var element in removalReferences)
            {
                SelectedUnits.Remove(element);
            }
        }

        protected void SetPropertyGridEnables(LabelCell selectedCell, DimmerDistroUnit selectedDatabaseUnit)
        {
            if (selectedCell == null || selectedCell.IsMerged == false)
            {
                // No Selected Cell or Selected Cell is Not Merged.

                // Reset
                DatabaseChannelNumberEnable = true;
                DatabaseInstrumentNameEnable = true;
                DatabasePositionEnable = true;
                DatabaseMulticoreNameEnable = true;
                DatabaseUserField1Enable = true;
                DatabaseUserField2Enable = true;
                DatabaseUserField3Enable = true;
                DatabaseUserField4Enable = true;

            }

            else if (selectedCell.IsMerged == true &&
                selectedCell.ConsumedReferences.Contains(selectedDatabaseUnit))
            {
                // Selected Cell is Merged and Selected Database Unit is consumed.

                // Generate a list of Displayed Rows.
                List<LabelField> displayedFields = new List<LabelField>();

                if (selectedCell.CellDataMode == CellDataMode.SingleField)
                {
                    // Single Field.
                    displayedFields.Add(selectedCell.SingleFieldDataField);
                }

                else
                {
                    // Multi Field.
                    foreach (var row in selectedCell.Rows)
                    {
                        if (displayedFields.Contains(row.DataField) == false)
                        {
                            displayedFields.Add(row.DataField);
                        }
                    }
                }

                // Set Enabled States.
                DatabaseChannelNumberEnable = displayedFields.Contains(LabelField.ChannelNumber) ? false : true;
                DatabaseInstrumentNameEnable = displayedFields.Contains(LabelField.InstrumentName) ? false : true;
                DatabasePositionEnable = displayedFields.Contains(LabelField.Position) ? false : true;
                DatabaseMulticoreNameEnable = displayedFields.Contains(LabelField.MulticoreName) ? false : true;
                DatabaseUserField1Enable = displayedFields.Contains(LabelField.UserField1) ? false : true;
                DatabaseUserField2Enable = displayedFields.Contains(LabelField.UserField2) ? false : true;
                DatabaseUserField3Enable = displayedFields.Contains(LabelField.UserField3) ? false : true;
                DatabaseUserField4Enable = displayedFields.Contains(LabelField.UserField4) ? false : true;
            }

            else
            {
                // Selected DatabaseUnit is primary Reference of selected Cell.

                DatabaseChannelNumberEnable = true;
                DatabaseInstrumentNameEnable = true;
                DatabasePositionEnable = true;
                DatabaseMulticoreNameEnable = true;
                DatabaseUserField1Enable = true;
                DatabaseUserField2Enable = true;
                DatabaseUserField3Enable = true;
                DatabaseUserField4Enable = true;
            }
        }
        #endregion

        #region Interface Implementations
        public event NotifyModificationEventHandler NotifyModification;

        protected void OnNotifyModification(object sender, string propertyName, LabelStripTemplate oldValue)
        {
            if (NotifyModification != null)
            {
                NotifyModification(sender, new NotifyModificationEventArgs(sender, propertyName, oldValue));
            }
        }
        #endregion
    }
}
