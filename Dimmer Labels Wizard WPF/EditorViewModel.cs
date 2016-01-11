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

namespace Dimmer_Labels_Wizard_WPF
{
    public class EditorViewModel : ViewModelBase
    {
        public EditorViewModel()
        {
            SelectedCells.CollectionChanged += SelectedCells_CollectionChanged;
            SelectedRows.CollectionChanged += SelectedRows_CollectionChanged;
            Units.CollectionChanged += Units_CollectionChanged;
            StripTemplates.CollectionChanged += StripTemplates_CollectionChanged;

            // Global Event Subscriptions.
            Globals.Strips.CollectionChanged += Strips_CollectionChanged;

            // Commands.
            _MergeSelectedCellsCommand = new RelayCommand(MergeSelectedCellsExecute, MergeSelectedCellsCanExecute);
            _SplitSelectedCellsCommand = new RelayCommand(SplitSelectedCellsExecute, SplitSelectedCellsCanExecute);

            #region Testing Code
            // Testing
            Strip strip1 = new Strip();
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "1", Position = "LX5", InstrumentName = "Beam", DimmerNumber = 1 });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "2", Position = "LX5", InstrumentName = "Beam", DimmerNumber = 2 });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "3", Position = "LX5", InstrumentName = "Beam", DimmerNumber = 3  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "4", Position = "LX5", InstrumentName = "Beam", DimmerNumber = 4  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "5", Position = "LX5", InstrumentName = "Wash", DimmerNumber = 5  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "6", Position = "LX5", InstrumentName = "Wash", DimmerNumber = 6  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "7", Position = "LX5", InstrumentName = "Wash", DimmerNumber = 7  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "8", Position = "LX5", InstrumentName = "Wash", DimmerNumber = 8  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "9", Position = "LX5", InstrumentName = "Spot", DimmerNumber = 9  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "10", Position = "LX5", InstrumentName = "Spot", DimmerNumber = 10  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "11", Position = "LX5", InstrumentName = "Spot", DimmerNumber = 11  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "12", Position = "LX5", InstrumentName = "Spot", DimmerNumber = 12 });

            Strip strip2 = new Strip();
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "81", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 13 });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "82", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 14  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "83", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 15  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "84", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 16  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "85", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 17  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "86", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 18  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "87", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 19  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "88", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 20  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "89", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 21  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "90", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 22  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "91", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 23  });
            strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "92", Position = "LX3", InstrumentName = "VL1k", DimmerNumber = 24  });

            var rowTemplates = new List<CellRowTemplate>();
            rowTemplates.Add(new CellRowTemplate() { DataField = LabelField.ChannelNumber});
            rowTemplates.Add(new CellRowTemplate() { DataField = LabelField.InstrumentName });

            var cellTemplate = new LabelCellTemplate(Globals.BaseLabelCellTemplate)
            {
                CellDataMode = CellDataMode.MixedField,
                RowCount = 2,
                CellRowTemplates = rowTemplates,
            };

            var cellTemplates = new List<LabelCellTemplate>();
            for (int count = 0; count < 12; count++)
            {
                cellTemplates.Add(cellTemplate);
            }

            var template1 = new LabelStripTemplate(Globals.BaseLabelStripTemplate);
            template1.Name = "template1, Based on BaseLabelStripTemplate";
            template1.StripMode = LabelStripMode.Dual;
            template1.UpperCellTemplates = cellTemplates;
            template1.LowerCellTemplates = cellTemplates;

            var template2 = new LabelStripTemplate(template1);
            template2.Name = "template2, Based on template1";
            template2.StripHeight = 70d;
            template2.StripMode = LabelStripMode.Single;

            strip1.AssignedTemplate = template1;
            strip2.AssignedTemplate = template2;

            template1.AssignedToStrips.Add(strip1);
            template2.AssignedToStrips.Add(strip2);

            Globals.Templates.Add(template1);
            Globals.Templates.Add(template2);
            Globals.Strips.Add(strip1);
            Globals.Strips.Add(strip2);

            #endregion

        }

        #region Fields
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

        #region CLR Properties - Binding Targets
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

        public string SelectedData
        {
            get
            {
                return GetSelectedData();
            }

            set
            {
                SetSelectedData(value);
                OnPropertyChanged(nameof(SelectedData));
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

        }

        private void SelectedCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

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
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;

                    cell.SelectedRows.CollectionChanged -= SelectedCells_SelectedRows_CollectionChanged;
                }
            }

            // Debug Output.
            DebugOutput = "VM Selected Cell Count = " + SelectedCells.Count.ToString();

            // Property Notifications
            OnPropertyChanged(nameof(SelectedData));
            OnPropertyChanged(nameof(SelectedCells));

            // Command CanExecute Notifications.
            _MergeSelectedCellsCommand.CheckCanExecute();
            _SplitSelectedCellsCommand.CheckCanExecute();
        }

        private void SelectedRows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedRows));
            OnPropertyChanged(nameof(SelectedData));
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

            // Notify.
            OnPropertyChanged(nameof(SelectedLabelStripMode));
            OnPropertyChanged(nameof(StripWidthmm));
            OnPropertyChanged(nameof(StripHeightmm));
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

        private string GetSelectedData()
        {
            List<string> dataElements = new List<string>();

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

        private void SetSelectedData(string data)
        {
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
        #endregion
    }
}
