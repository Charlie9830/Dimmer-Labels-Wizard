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

namespace Dimmer_Labels_Wizard_WPF
{
    public class EditorViewModel : ViewModelBase, INotifyModification
    {
        public EditorViewModel()
        {
            SelectedCells.CollectionChanged += SelectedCells_CollectionChanged;
            SelectedRows.CollectionChanged += SelectedRows_CollectionChanged;
            Units.CollectionChanged += Units_CollectionChanged;
            StripTemplates.CollectionChanged += StripTemplates_CollectionChanged;
            SelectedUnits.CollectionChanged += SelectedUnits_CollectionChanged;

            // Global Event Subscriptions.
            Globals.Strips.CollectionChanged += Strips_CollectionChanged;

            // Commands.
            _MergeSelectedCellsCommand = new RelayCommand(MergeSelectedCellsExecute, MergeSelectedCellsCanExecute);
            _SplitSelectedCellsCommand = new RelayCommand(SplitSelectedCellsExecute, SplitSelectedCellsCanExecute);
            _UndoCommand = new RelayCommand(UndoCommandExecute, UndoCommandCanExecute);
            _RedoCommand = new RelayCommand(RedoCommandExecute, RedoCommandCanExecute);
            _ApplyTemplateChangesCommand = new RelayCommand(ApplyTemplateChangesCommandExecute);
            _DiscardTemplateChangesCommand = new RelayCommand(DiscardTemplateChangesCommandExecute);
            _CreateNewTemplateCommand = new RelayCommand(CreateNewTemplateCommandExecute);
            
            #region Testing Code
            // Testing
            Strip strip1 = new Strip();
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "1", Position = "LX5", MulticoreName = "LX5A", InstrumentName = "Beam", DimmerNumber = 1 });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "2", Position = "LX5", MulticoreName = "LX5A", InstrumentName = "Beam", DimmerNumber = 2 });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "3", Position = "LX5", MulticoreName = "LX5A", InstrumentName = "Beam", DimmerNumber = 3  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "4", Position = "LX5", MulticoreName = "LX5A", InstrumentName = "Beam", DimmerNumber = 4  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "5", Position = "LX5", MulticoreName = "LX5B", InstrumentName = "Wash", DimmerNumber = 5  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "6", Position = "LX5", MulticoreName = "LX5B", InstrumentName = "Wash", DimmerNumber = 6  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "7", Position = "LX5", MulticoreName = "LX5B", InstrumentName = "Wash", DimmerNumber = 7  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "8", Position = "LX5", MulticoreName = "LX5B", InstrumentName = "Wash", DimmerNumber = 8  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "9", Position = "LX5", MulticoreName = "LX5C", InstrumentName = "Spot", DimmerNumber = 9  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "10", Position = "LX5", MulticoreName = "LX5C", InstrumentName = "Spot", DimmerNumber = 10  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "11", Position = "LX5", MulticoreName = "LX5C", InstrumentName = "Spot", DimmerNumber = 11  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "12", Position = "LX5", MulticoreName = "LX5C", InstrumentName = "Spot", DimmerNumber = 12 });

            Strip strip2 = new Strip();
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "81", Position = "LX3", MulticoreName = "LX3A", InstrumentName = "VL1k", DimmerNumber = 13 });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "82", Position = "LX3", MulticoreName = "LX3A", InstrumentName = "VL1k", DimmerNumber = 14  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "83", Position = "LX3", MulticoreName = "LX3A", InstrumentName = "VL1k", DimmerNumber = 15  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "84", Position = "LX3", MulticoreName = "LX3A", InstrumentName = "VL1k", DimmerNumber = 16  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "85", Position = "LX3", MulticoreName = "LX3B", InstrumentName = "VL1k", DimmerNumber = 17  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "86", Position = "LX3", MulticoreName = "LX3B", InstrumentName = "VL1k", DimmerNumber = 18  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "87", Position = "LX3", MulticoreName = "LX3B", InstrumentName = "VL1k", DimmerNumber = 19  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "88", Position = "LX3", MulticoreName = "LX3B", InstrumentName = "VL1k", DimmerNumber = 20  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "89", Position = "LX3", MulticoreName = "LX3C", InstrumentName = "VL1k", DimmerNumber = 21  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "90", Position = "LX3", MulticoreName = "LX3C", InstrumentName = "VL1k", DimmerNumber = 22  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "91", Position = "LX3", MulticoreName = "LX3C", InstrumentName = "VL1k", DimmerNumber = 23  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "92", Position = "LX3", MulticoreName = "LX3C", InstrumentName = "VL1k", DimmerNumber = 24  });

            Globals.DimmerDistroUnits.AddRange(strip1.Units);
            Globals.DimmerDistroUnits.AddRange(strip2.Units);

            var rowTemplatesA = new List<CellRowTemplate>();
            rowTemplatesA.Add(new CellRowTemplate() { DataField = LabelField.ChannelNumber});
            rowTemplatesA.Add(new CellRowTemplate() { DataField = LabelField.InstrumentName });

            var rowTemplatesB = new List<CellRowTemplate>();
            rowTemplatesB.Add(new CellRowTemplate() { DataField = LabelField.InstrumentName });
            rowTemplatesB.Add(new CellRowTemplate() { DataField = LabelField.ChannelNumber });

            var cellTemplateA = new LabelCellTemplate(Globals.BaseLabelCellTemplate)
            {
                CellDataMode = CellDataMode.MixedField,
                CellRowTemplates = rowTemplatesA,
            };

            var cellTemplateB = new LabelCellTemplate(Globals.BaseLabelCellTemplate)
            {
                CellDataMode = CellDataMode.MixedField,
                CellRowTemplates = rowTemplatesB
            };

            var template1 = new LabelStripTemplate(Globals.BaseLabelStripTemplate);
            template1.Name = "Template A Standard";
            template1.StripMode = LabelStripMode.Dual;
            template1.UpperCellTemplate = cellTemplateA;
            template1.LowerCellTemplate = cellTemplateA;

            var template2 = new LabelStripTemplate(template1);
            template2.Name = "Template B Flipped";
            template2.UpperCellTemplate = cellTemplateB;
            template2.LowerCellTemplate = cellTemplateB;

            strip1.AssignedTemplate = Globals.DefaultTemplate;
            strip2.AssignedTemplate = template2;

            template1.AssignedToStrips.Add(strip1);
            template2.AssignedToStrips.Add(strip2);

            Globals.Templates.Add(template1);
            Globals.Templates.Add(template2);
            Globals.Strips.Add(strip1);
            Globals.Strips.Add(strip2);

            #endregion

            // Initialize UndoRedoManager.
            UndoRedoManager = new UndoRedoManager(Globals.DimmerDistroUnits, this);
        }

        #region Fields
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

        private List<string> _TemplateChangesRegister = new List<string>();

        #endregion

        #region CLR Properties - Binding Target.
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



        private ObservableCollection<Merge> _Mergers = new ObservableCollection<Merge>();

        public ObservableCollection<Merge> Mergers
        {
            get { return _Mergers; }
            set { _Mergers = value; }
        }


        private double _StripWidthmm;

        public double StripWidthmm
        {
            get { return Math.Round(_StripWidthmm,2); }
            set
            {
                if (_StripWidthmm != value)
                {
                    // Refresh Display Template.
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        StripWidth = value * unitConversionRatio
                    };

                    // Register
                    RegisterTemplateChange(nameof(StripWidthmm));

                    _StripWidthmm = value;

                    // Notify.
                    OnPropertyChanged(nameof(StripWidthmm));
                }
            }
        }

        private double _StripHeightmm;

        public double StripHeightmm
        {
            get
            { return Math.Round(_StripHeightmm,2); }

            set
            {
                if (_StripHeightmm != value)
                {
                    // Refresh Display Template.
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        StripHeight = value * unitConversionRatio
                    };

                    // Register
                    RegisterTemplateChange(nameof(StripHeightmm));

                    _StripHeightmm = value;

                    // Notify.
                    OnPropertyChanged(nameof(StripHeightmm));
                }
            }
        }

        public string TemplateStatusText
        {
            get
            {
                if (_TemplateChangesPending)
                { return "Template Changes Pending"; }

                else
                { return "Template up to date."; }
            }
        }

        private bool _TemplateChangesPending = false;

        public bool TemplateChangesPending
        {
            get { return _TemplateChangesPending; }
            set
            {
                if (value != _TemplateChangesPending)
                {
                    _TemplateChangesPending = value;
                    OnPropertyChanged(nameof(TemplateChangesPending));
                    OnPropertyChanged(nameof(TemplateStatusText));
                }
            }
        }

        private LabelStripMode _SelectedLabelStripMode;

        public LabelStripMode SelectedLabelStripMode
        {
            get
            {
                return _SelectedLabelStripMode;
            }
            set
            {
                if (value != _SelectedLabelStripMode)
                {
                    // Refresh Displayed Template.
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        StripMode = value
                    };

                    // Register.
                    RegisterTemplateChange(nameof(LabelStripTemplate.StripMode));

                    _SelectedLabelStripMode = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLabelStripMode));
                }
            }
        }


        private LabelStripTemplate _DisplayedTemplate;

        public LabelStripTemplate DisplayedTemplate
        {
            get { return _DisplayedTemplate; }
            set
            {
                if (value != _DisplayedTemplate)
                {
                    _DisplayedTemplate = value;
                    OnPropertyChanged(nameof(DisplayedTemplate));
                }
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
                    DisplayedTemplate = _SelectedStripTemplate;

                    // Push Change to Selected Strip.
                    SelectedStrip.AssignedTemplate = _SelectedStripTemplate;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedStripTemplate));
                }
            }
        }

        public ObservableCollection<LabelStripTemplate> StripTemplates
        {
            get { return Globals.Templates; }
            set { Globals.Templates = value; }
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
                return Globals.Strips;
            }
            set
            {
                Globals.Strips = value;
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
        #endregion


        #region CLR Properties.
        private ObservableCollection<CellRow> _SelectedRows = new ObservableCollection<CellRow>();

        public ObservableCollection<CellRow> SelectedRows
        {
            get { return _SelectedRows; }
            set { _SelectedRows = value; }
        }


        #endregion

        #region Commands
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
            var test = new TemplateEditor();
            test.Show();
        }

        private RelayCommand _ApplyTemplateChangesCommand;

        public ICommand ApplyTemplateChangesCommand
        {
            get
            {
                return _ApplyTemplateChangesCommand;
            }
        }

        protected void ApplyTemplateChangesCommandExecute(object parameter)
        {
            ApplyTemplateChanges();

            
        }


        private RelayCommand _DiscardTemplateChangesCommand;

        public ICommand DiscardTemplateChangesCommand
        {
            get
            {
                return _DiscardTemplateChangesCommand;
            }
        }

        protected void DiscardTemplateChangesCommandExecute(object parameter)
        {
            DiscardTemplateChanges();
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
        private void StripTemplates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
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

                    // Update DataBasePanel.
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
            SelectedData = GetSelectedData();
            OnPropertyChanged(nameof(SelectedCells));
            DatabasePanelVisibility = collection.Count > 1 ? Visibility.Hidden : Visibility.Visible;

            // Command CanExecute Notifications.
            _MergeSelectedCellsCommand.CheckCanExecute();
            _SplitSelectedCellsCommand.CheckCanExecute();

            //Console.WriteLine("======");
            //foreach (var element in collection)
            //{
            //    Console.WriteLine("Rows Count {0}, GridRows Count {1}", element.Rows.Count, element.Grid.RowDefinitions.Count);
            //}
            //Console.WriteLine("======");
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
        protected void ApplyTemplateChanges()
        {
            if (TemplateChangesPending)
            {
                // Modify existing Template.
                TemplateHelper.ModifyExistingTemplate(SelectedStrip.AssignedTemplate, DisplayedTemplate);

                LoadTemplate(SelectedStrip);
                TemplateChangesPending = false;
            }
        }

        protected void DiscardTemplateChanges()
        {
            if (TemplateChangesPending)
            {
                DisplayedTemplate = SelectedStrip.AssignedTemplate;

                TemplateChangesPending = false;
            }
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

        private void PresentStripData(Strip value)
        {
            // Clear Selections.
            ClearSelections();

            // Clear Current Unit Collection.
            while (Units.Count > 0)
            {
                Units.RemoveAt(Units.Count - 1);
            }

            // Clear Mergers Collection.
            while (Mergers.Count > 0)
            {
                Mergers.RemoveAt(Mergers.Count - 1);
            }

            // Load new StripData.
            foreach (var element in _SelectedStrip.Units)
            {
                Units.Add(element);
            }

            // Retrieve and Load Template.
            LoadTemplate(value);

            // Notify Listeners.
            OnPropertyChanged(nameof(Units));
            OnPropertyChanged(nameof(SelectedCells));
        }

        private void LoadTemplate(Strip strip)
        {
            DisplayedTemplate = strip.AssignedTemplate;

            // Appearance Controls
            _SelectedLabelStripMode = DisplayedTemplate.StripMode;
            _StripWidthmm = DisplayedTemplate.StripWidth / unitConversionRatio;
            _StripHeightmm = DisplayedTemplate.StripHeight / unitConversionRatio;
            _SelectedStripTemplate = DisplayedTemplate;

            // Notify.
            OnPropertyChanged(nameof(SelectedLabelStripMode));
            OnPropertyChanged(nameof(StripWidthmm));
            OnPropertyChanged(nameof(StripHeightmm));
            OnPropertyChanged(nameof(SelectedStripTemplate));
        }

        private void RegisterTemplateChange(string propertyName)
        {
            if (_TemplateChangesRegister.Contains(propertyName) == false)
            {
                _TemplateChangesRegister.Add(propertyName);
            }

            if (TemplateChangesPending == false)
            {
                TemplateChangesPending = true;
            }
        }

        private void ClearTemplateChangeRegister()
        {
            _TemplateChangesRegister.Clear();

            if (TemplateChangesPending == true)
            {
                TemplateChangesPending = false;
            }
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
                    // Mixed Field.
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
