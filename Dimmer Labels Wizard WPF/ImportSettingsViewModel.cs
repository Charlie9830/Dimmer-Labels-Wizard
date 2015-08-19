using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class ImportSettingsViewModel : ViewModelBase
    {
        protected const string _None = "None";

        protected string[] _CSVColumnHeaders;

        // Available Data Formats
        protected string[] _DimmerDataFormats = {"#/### (eg: Universe Number / Dimmer Number)", "### (eg: Dimmer Number)",
        "A### (eg: Universe Letter  Dimmer Number)", "A/### (eg: Universe Letter / Dimmer Number)"};
        protected string[] _DistroDataFormats = { "A### or AA### (eg: N160 or ND160)", "###",
            "#/### (eg: Rack Number / Non Dim Number)", "A/### (eg: Rack Letter / Non Dim Number)"};

        // Data Format Selections.
        protected string _DimmerDataFormatSelectedItem = string.Empty;
        protected string _UniverseDataFormatSelectedItem = string.Empty;

        protected int _DimmerDataFormatSelectedIndex = 0;
        protected int _UniverseDataFormatSelectedIndex = 0;

        protected string _DistroDataFormatSelectedItem = string.Empty;
        protected string _DistroNumberPrefix = string.Empty;

        protected int _DistroDataFormatSelectedIndex = 0;

        // Dimmer and Distro Ranges
        protected List<DimmerRange> _DimmerRanges = new List<DimmerRange>();
        protected List<DistroRange> _DistroRanges = new List<DistroRange>();
        
        // isEnabled Fields.
        protected bool _GenerateDimmerLabels = false;
        protected bool _GenerateDistroLabels = false;
        protected bool _DimmerLabelRangeEnable = false;
        protected bool _DistroLabelRangeEnable = false;
        protected bool _DimmerUniverseDataEnable = false;

        // CSV Column Mapping Selected Items Fields.
        protected string _ChannelNumberSelectedItem = string.Empty;
        protected string _DimmerNumberSelectedItem = string.Empty;
        protected string _InstrumentNameSelectedItem = string.Empty;
        protected string _MulticoreNameSelectedItem = string.Empty;
        protected string _PositionSelectedItem = string.Empty;
        protected string _UserField1SelectedItem = string.Empty;
        protected string _UserField2SelectedItem = string.Empty;
        protected string _UserField3SelectedItem = string.Empty;
        protected string _UserField4SelectedItem = string.Empty;

        protected string _UniverseDataColumnSelectedItem = string.Empty;


        public ImportSettingsViewModel()
        {
            InitializeColumnMapping();
        }

        #region Getters/Setters
        public string[] CSVColumnHeaders
        {
            get
            {
                return _CSVColumnHeaders;
            }
        }

        public string[] DimmerDataFormats
        {
            get
            {
                return _DimmerDataFormats;
            }
        }

        public string[] DistroDataFormats
        {
            get
            {
                return _DistroDataFormats;
            }
        }

        public bool GenerateDimmerLabels
        {
            get
            {
                return _GenerateDimmerLabels;
            }
            set
            {
                _GenerateDimmerLabels = value;
                _DimmerLabelRangeEnable = value;
                _DimmerUniverseDataEnable = value;
                OnPropertyChanged("GenerateDimmerLabels");
                OnPropertyChanged("DimmerLabelRangeEnable");
                OnPropertyChanged("DimmerUniverseDataEnable");
            }
        }

        public bool GenerateDistroLabels
        {
            get
            {
                return _GenerateDistroLabels;
            }
            set
            {
                _GenerateDistroLabels = value;
                _DistroLabelRangeEnable = value;
                OnPropertyChanged("GenerateDistroLabels");
                OnPropertyChanged("DistroLabelRangeEnable");
            }
        }

        public bool DimmerLabelRangeEnable
        {
            get
            {
                return _DimmerLabelRangeEnable;
            }
        }

        public bool DistroLabelRangeEnable
        {
            get
            {
                return _DistroLabelRangeEnable;
            }
        }

        public bool DimmerUniverseDataEnable
        {
            get
            {
                return _DimmerUniverseDataEnable;
            }
            set
            {
                _DimmerUniverseDataEnable = value;
                OnPropertyChanged("DimmerUniverseDataEnable");
            }
        }

        public string ChannelNumberSelectedItem
        {
            get
            {
                return _ChannelNumberSelectedItem;
            }
            set
            {
                _ChannelNumberSelectedItem = value;
                OnPropertyChanged("ChannelNumberSelectedItem");
            }
        }

        public string DimmerNumberSelectedItem
        {
            get
            {
                return _DimmerNumberSelectedItem;
            }
            set
            {
                _DimmerNumberSelectedItem = value;
                OnPropertyChanged("DimmerNumberSelectedItem");
            }
        }

        public string InstrumentNameSelectedItem
        {
            get
            {
                return _InstrumentNameSelectedItem;
            }
            set
            {
                _InstrumentNameSelectedItem = value;
                OnPropertyChanged("InstrumentNameSelectedItem");
            }
        }

        public string MulticoreNameSelectedItem
        {
            get
            {
                return _MulticoreNameSelectedItem;
            }
            set
            {
                _MulticoreNameSelectedItem = value;
                OnPropertyChanged("MulticoreNameSelectedItem");
            }
        }

        public string PositionSelectedItem
        {
            get
            {
                return _PositionSelectedItem;
            }
            set
            {
                _PositionSelectedItem = value;
                OnPropertyChanged("PositionSelectedItem");
            }
        }

        public string UserField1SelectedItem
        {
            get
            {
                return _UserField1SelectedItem;
            }
            set
            {
                _UserField1SelectedItem = value;
                OnPropertyChanged("UserField1SelectedItem");
            }
        }

        public string UserField2SelectedItem
        {
            get
            {
                return _UserField2SelectedItem;
            }
            set
            {
                _UserField2SelectedItem = value;
                OnPropertyChanged("UserField2SelectedItem");
            }
        }

        public string UserField3SelectedItem
        {
            get
            {
                return _UserField3SelectedItem;
            }
            set
            {
                _UserField3SelectedItem = value;
                OnPropertyChanged("UserField3SelectedItem");
            }
        }

        public string UserField4SelectedItem
        {
            get
            {
                return _UserField4SelectedItem;
            }
            set
            {
                _UserField4SelectedItem = value;
                OnPropertyChanged("UserField4SelectedItem");
            }
        }

        public string DimmerDataFormatSelectedItem
        {
            get
            {
                return _DimmerDataFormatSelectedItem;
            }
            set
            {
                _DimmerDataFormatSelectedItem = value;
                OnPropertyChanged("DimmerDataFormatSelectedItem");
            }
        }

        public string UniverseDataFormatSelectedItem
        {
            get
            {
                return _UniverseDataFormatSelectedItem;
            }
            set
            {
                _UniverseDataFormatSelectedItem = value;
                OnPropertyChanged("UniverseDataFormatSelectedItem");
            }
        }

        public string DistroDataFormatSelectedItem
        {
            get
            {
                return _DistroDataFormatSelectedItem;
            }
            set
            {
                _DistroDataFormatSelectedItem = value;
                OnPropertyChanged("DistroDataFormatSelectedItem");
            }
        }

        public string DistroNumberPrefix
        {
            get
            {
                return _DistroNumberPrefix;
            }
            set
            {
                _DistroNumberPrefix = value;
                OnPropertyChanged("DistroNumberPrefix");
            }
        }

        public string UniverseDataColumnSelectedItem
        {
            get
            {
                return _UniverseDataColumnSelectedItem;
            }
            set
            {
                _UniverseDataColumnSelectedItem = value;
                OnPropertyChanged("UniverseDataColumnSelectedItem");
            }
        }

        public List<DimmerRange> DimmerRanges
        {
            get
            {
                return _DimmerRanges;
            }
            set
            {
                _DimmerRanges = value;
                OnPropertyChanged("DimmerRanges");
            }
        }

        public List<DistroRange> DistroRanges
        {
            get
            {
                return _DistroRanges;
            }
            set
            {
                _DistroRanges = value;
                OnPropertyChanged("DistroRanges");
            }
        }

        public int DimmerDataFormatSelectedIndex
        {
            get
            {
                return _DimmerDataFormatSelectedIndex;
            }
            set
            {
                _DimmerDataFormatSelectedIndex = value;
                OnPropertyChanged("DimmerDataFormatSelectedIndex");
            }
        }

        public int UniverseDataFormatSelectedIndex
        {
            get
            {
                return _UniverseDataFormatSelectedIndex;
            }
            set
            {
                _UniverseDataFormatSelectedIndex = value;
                OnPropertyChanged("UniverseDataFormatSelectedIndex");
            }
        }

        public int DistroDataFormatSelectedIndex
        {
            get
            {
                return _DistroDataFormatSelectedIndex;
            }
            set
            {
                _DistroDataFormatSelectedIndex = value;
                OnPropertyChanged("DistroDataFormatSelectedIndex");   
            }
        }
        #endregion

        #region General Methods
        /// <summary>
        /// Collects Header Strings from CSV File. Pre Selects matching Headers to LabelFields.
        /// </summary>
        protected void InitializeColumnMapping()
        {
            List<string> columnHeadersBuffer = new List<string>();
            columnHeadersBuffer.Add(_None);
            columnHeadersBuffer.AddRange(FileImport.CollectHeaders());
            _CSVColumnHeaders = columnHeadersBuffer.ToArray();

            // Channel
            foreach (var element in _CSVColumnHeaders)
            {
                if (element.Contains("Channel"))
                {
                    ChannelNumberSelectedItem = element;
                    break;
                }
            }

            // Dimmer Number
            foreach (var element in _CSVColumnHeaders)
            {
                if (element.Contains("Dimmer"))
                {
                    DimmerNumberSelectedItem = element;
                    break;
                }
            }

            // Instrument Name
            foreach (var element in _CSVColumnHeaders)
            {
                if (element.Contains("Instrument"))
                {
                    InstrumentNameSelectedItem = element;
                    break;
                }
            }

            // Multicore Name
            foreach (var element in _CSVColumnHeaders)
            {
                if (element.Contains("Multicore") || element.Contains("Circuit"))
                {
                    MulticoreNameSelectedItem = element;
                    break;
                }
            }

            // Position
            foreach (var element in _CSVColumnHeaders)
            {
                if (element.Contains("Position"))
                {
                    PositionSelectedItem = element;
                    break;
                }
            }

            // User Fields
            _UserField1SelectedItem = _None;
            _UserField2SelectedItem = _None;
            _UserField3SelectedItem = _None;
            _UserField4SelectedItem = _None;


        }

        #endregion

        #region Update Model Methods
        public void UpdateModel(DimmerRangeSelectorHost dimmerSelectorHost,
            DistroRangeSelectorHost distroSelectorHost)
        {
            // Collect Dimmer Ranges.
            dimmerSelectorHost.UpdateDimmerRanges();
            UserParameters.DimmerRanges = _DimmerRanges;
            UserParameters.DimmerRanges.Sort();

            distroSelectorHost.UpdateDistroRanges();
            UserParameters.DistroRanges = _DistroRanges;
            UserParameters.DistroRanges.Sort();

            // Convert _CSVColumnHeaders to List<> to gain acsess to IndexOf() Method.
            List<string> headers = _CSVColumnHeaders.ToList();

            // CSV Column Mapping
            UserParameters.ChannelNumberColumnIndex = headers.IndexOf(_ChannelNumberSelectedItem) - 1;
            UserParameters.DimmerNumberColumnIndex = headers.IndexOf(_DimmerNumberSelectedItem) - 1;
            UserParameters.InstrumentTypeColumnIndex = headers.IndexOf(_InstrumentNameSelectedItem) - 1;
            UserParameters.MulticoreNameColumnIndex = headers.IndexOf(_MulticoreNameSelectedItem) - 1;
            UserParameters.PositionColumnIndex = headers.IndexOf(_PositionSelectedItem) - 1;
            UserParameters.UniverseDataColumnIndex = headers.IndexOf(_UniverseDataColumnSelectedItem);
            UserParameters.UserField1ColumnIndex = headers.IndexOf(_UserField1SelectedItem) - 1;
            UserParameters.UserField2ColumnIndex = headers.IndexOf(_UserField2SelectedItem) - 1;
            UserParameters.UserField3ColumnIndex = headers.IndexOf(_UserField3SelectedItem) - 1;
            UserParameters.UserField4ColumnIndex = headers.IndexOf(_UserField4SelectedItem) - 1;

            // ImportFormats.
            UserParameters.DimmerImportFormat = CollectDimmerFormatValue();
            if (_DimmerUniverseDataEnable == true)
            {
                UserParameters.UniverseDataColumnIndex = _UniverseDataFormatSelectedIndex - 1;
                UserParameters.DMXAddressImportFormat = CollectDMXAddressFormatValue();
            }

            UserParameters.DistroImportFormat = CollectDistroFormatValue();
            if (UserParameters.DistroImportFormat == ImportFormatting.Format1)
            {
                UserParameters.DistroNumberPrefix = _DistroNumberPrefix;
            }
        }

        private ImportFormatting CollectDimmerFormatValue()
        {
            if (_DimmerLabelRangeEnable == false)
            {
                return ImportFormatting.NoAssignment;
            }

            int valueIndex = _DimmerDataFormatSelectedIndex;

            switch (valueIndex)
            {
                case 0:
                    return ImportFormatting.Format1;
                case 1:
                    return ImportFormatting.Format2;
                case 2:
                    return ImportFormatting.Format3;
                case 3:
                    return ImportFormatting.Format4;
                default:
                    Console.WriteLine("CollectDimmerFormatValue Hit the default case. Reverting to Format1");
                    return ImportFormatting.Format1;
            }
        }

        private ImportFormatting CollectDistroFormatValue()
        {
            if (_DistroLabelRangeEnable == false)
            {
                return ImportFormatting.NoAssignment;
            }

            int valueIndex = _DistroDataFormatSelectedIndex;

            switch (valueIndex)
            {
                case 0:
                    return ImportFormatting.Format1;
                case 1:
                    return ImportFormatting.Format2;
                case 2:
                    return ImportFormatting.Format3;
                case 3:
                    return ImportFormatting.Format4;
                default:
                    Console.WriteLine("CollectDistroFormatValue Hit the default case. Reverting to Format1");
                    return ImportFormatting.Format1;
            }
        }

        private ImportFormatting CollectDMXAddressFormatValue()
        {
            int valueIndex = _UniverseDataFormatSelectedIndex;

            switch (valueIndex)
            {
                case 0:
                    return ImportFormatting.Format1;
                case 1:
                    return ImportFormatting.Format2;
                case 2:
                    return ImportFormatting.Format3;
                case 3:
                    return ImportFormatting.Format4;
                default:
                    Console.WriteLine("CollectDMXAddressFormatValue has Hit the default case. Reverting to Format1");
                    return ImportFormatting.Format1;
            }
        }
        #endregion
    }
}
