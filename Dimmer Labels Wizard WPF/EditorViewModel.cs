using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
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

            #region Testing Code
            // Testing
            //StripData strip1 = new StripData();
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "101", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "102", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "103", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "104", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "105", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "106", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "107", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "108", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "109", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "110", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "111", Position = "LX5", InstrumentName = "Alpha" });
            //strip1.Units.Add(new DimmerDistroUnit() { ChannelNumber = "112", Position = "LX5", InstrumentName = "Alpha" });

            //StripData strip2 = new StripData();
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "81", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "82", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "83", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "84", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "85", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "86", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "87", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "88", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "89", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "90", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "91", Position = "LX3", InstrumentName = "VL1k" });
            //strip2.Units.Add(new DimmerDistroUnit() { ChannelNumber = "92", Position = "LX3", InstrumentName = "VL1k" });

            //Globals.Strips.Add(strip1);
            //Globals.Strips.Add(strip2);

            Units.Add(new DimmerDistroUnit() { ChannelNumber = "101", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "102", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "103", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "104", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "105", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "106", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "107", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "108", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "109", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "110", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "111", Position = "LX5", InstrumentName = "Alpha" });
            Units.Add(new DimmerDistroUnit() { ChannelNumber = "112", Position = "LX5", InstrumentName = "Alpha" });
            #endregion

            // Global Event Subscriptions.
            Globals.Strips.CollectionChanged += Strips_CollectionChanged;
        }

        #region Fields
        private const string _NonEqualData = "***";
        #endregion

        #region CLR Properties - Binding Targets
        public ObservableCollection<StripData> Strips
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

        private StripData _SelectedStrip;

        public StripData SelectedStrip
        {
            get
            {
                return _SelectedStrip;
            }

            set
            {
                _SelectedStrip = value;
                PresentStripData(value);
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
        private void Strips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Strips));
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
            WriteDebugOutput("SelectedRows.Count = " + SelectedRows.Count);
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
        private void PresentStripData(StripData value)
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

            // Retreive and Load Template.

            // Notify Listeners.
            OnPropertyChanged(nameof(Units));
            OnPropertyChanged(nameof(SelectedCells));
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

        public void AddUnit()
        {
            Units.Add(new DimmerDistroUnit() { Position = "New Unit" });
        }

        public void RemoveUnit()
        {
            Units.Remove(Units.Last());
        }
        #endregion
    }
}
