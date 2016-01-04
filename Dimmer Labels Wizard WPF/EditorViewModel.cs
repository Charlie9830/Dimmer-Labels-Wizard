using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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

            #region Testing Code
            // Testing
            Strip strip1 = new Strip();
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "101", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 1 });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "102", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 2 });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "103", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 3  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "104", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 4  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "105", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 5  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "106", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 6  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "107", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 7  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "108", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 8  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "109", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 9  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "110", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 10  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "111", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 11  });
            strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "112", Position = "LX5", InstrumentName = "Alpha", DimmerNumber = 12 });

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

            var template1 = new LabelStripTemplate(Globals.BaseLabelStripTemplate);
            template1.Name = "template1, Based on BaseLabelStripTemplate";
            template1.StripMode = LabelStripMode.Dual;

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
            set { _DebugOutput = value; }
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

                OnPropertyChanged(nameof(SelectedCells));
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;

                    cell.SelectedRows.CollectionChanged -= SelectedCells_SelectedRows_CollectionChanged;
                }

                OnPropertyChanged(nameof(SelectedData));
                OnPropertyChanged(nameof(SelectedCells));
            }
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
        #endregion
    }
}
