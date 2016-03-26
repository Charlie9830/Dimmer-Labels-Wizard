using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class ImportUnitsViewModel : ViewModelBase
    {

        #region Constructor.
        public ImportUnitsViewModel()
        {
            // Commands.
            _BrowseCommand = new RelayCommand(BrowseCommandExecute);
        }
        #endregion

        #region Public Non Bound Properties.

        protected ImportConfiguration _ImportConfiguration = new ImportConfiguration();

        public ImportConfiguration ImportConfiguration
        {
            get { return _ImportConfiguration; }
        }
        #endregion

        #region Binding Source Properties.


        protected IEnumerable<FriendlyImportFormat> _DimmerImportFormats = new List<FriendlyImportFormat>()
        {
            new FriendlyImportFormat(ImportFormat.Format1, "Universe / Address (1/123)"),
            new FriendlyImportFormat(ImportFormat.Format2, "Address (123)"),
            new FriendlyImportFormat(ImportFormat.Format3, "Universe Letter Address (A123)"),
            new FriendlyImportFormat(ImportFormat.Format4, "Universe Letter / Address (A/123)")
        };

        public IEnumerable<FriendlyImportFormat> DimmerImportFormats
        {
            get
            {
                return _DimmerImportFormats;
            }
        }

        protected IEnumerable<FriendlyImportFormat> _DistroImportFormats = new List<FriendlyImportFormat>()
        {
            new FriendlyImportFormat(ImportFormat.Format1, "Distro Prefix Dimmer Number (ND123)"),
            new FriendlyImportFormat(ImportFormat.Format2, "Dimmer Number (123)"),
            new FriendlyImportFormat(ImportFormat.Format3, "Distro Prefix / Dimmer Number (ND/123)"),
            new FriendlyImportFormat(ImportFormat.Format4, "ID Letter / Dimmer Number (A/123)")
        };

        public IEnumerable<FriendlyImportFormat> DistroImportFormats
        {
            get
            {
                return _DistroImportFormats;
            }
        }

        protected string _FilePath = string.Empty;

        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                if (_FilePath != value)
                {
                    _FilePath = value;

                    // Notify.
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }


        protected object _FileName = string.Empty;

        public object FileName
        {
            get { return _FileName; }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;

                    // Notify.
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }


        protected string _FileErrorMessage = string.Empty;

        public string FileErrorMessage
        {
            get { return _FileErrorMessage; }
            set
            {
                if (_FileErrorMessage != value)
                {
                    _FileErrorMessage = value;

                    // Notify.
                    OnPropertyChanged(nameof(FileErrorMessage));
                }
            }
        }


        protected bool _ImportSettingsEnabled = false;

        public bool ImportSettingsEnabled
        {
            get { return _ImportSettingsEnabled; }
            set
            {
                if (_ImportSettingsEnabled != value)
                {
                    _ImportSettingsEnabled = value;

                    // Notify.
                    OnPropertyChanged(nameof(ImportSettingsEnabled));
                }
            }
        }


        protected IEnumerable<ColumnHeader> _ColumnHeaders;

        public IEnumerable<ColumnHeader> ColumnHeaders
        {
            get { return _ColumnHeaders; }
            set
            {
                if (_ColumnHeaders != value)
                {
                    _ColumnHeaders = value;

                    // Notify.
                    OnPropertyChanged(nameof(ColumnHeaders));
                }
            }
        }


        protected ColumnHeader _SelectedDimmerNumberHeader;

        public ColumnHeader SelectedDimmerNumberHeader
        {
            get { return _SelectedDimmerNumberHeader; }
            set
            {
                if (_SelectedDimmerNumberHeader != value)
                {
                    _SelectedDimmerNumberHeader = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedDimmerNumberHeader));
                }
            }
        }


        protected ColumnHeader _SelectedChannelNumberHeader;

        public ColumnHeader SelectedChannelNumberHeader
        {
            get { return _SelectedChannelNumberHeader; }
            set
            {
                if (_SelectedChannelNumberHeader != value)
                {
                    _SelectedChannelNumberHeader = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedChannelNumberHeader));
                }
            }
        }


        protected ColumnHeader _SelectedPositionHeader;

        public ColumnHeader SelectedPostionHeader
        {
            get { return _SelectedPositionHeader; }
            set
            {
                if (_SelectedPositionHeader != value)
                {
                    _SelectedPositionHeader = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedPostionHeader));
                }
            }
        }


        protected ColumnHeader _SelectedMulticoreNameHeader;

        public ColumnHeader SelectedMulticoreNameHeader
        {
            get { return _SelectedMulticoreNameHeader; }
            set
            {
                if (_SelectedMulticoreNameHeader != value)
                {
                    _SelectedMulticoreNameHeader = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedMulticoreNameHeader));
                }
            }
        }


        protected ColumnHeader _SelectedInstrumentNameHeader;

        public ColumnHeader SelectedInstrumentNameHeader
        {
            get { return _SelectedInstrumentNameHeader; }
            set
            {
                if (_SelectedInstrumentNameHeader != value)
                {
                    _SelectedInstrumentNameHeader = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedInstrumentNameHeader));
                }
            }
        }


        protected ColumnHeader _SelectedUserField1Header;

        public ColumnHeader SelectedUserField1Header
        {
            get { return _SelectedUserField1Header; }
            set
            {
                if (_SelectedUserField1Header != value)
                {
                    _SelectedUserField1Header = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUserField1Header));
                }
            }
        }

        protected ColumnHeader _SelectedUserField2Header;

        public ColumnHeader SelectedUserField2Header
        {
            get { return _SelectedUserField2Header; }
            set
            {
                if (_SelectedUserField2Header != value)
                {
                    _SelectedUserField2Header = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUserField2Header));
                }
            }
        }

        protected ColumnHeader _SelectedUserField3Header;

        public ColumnHeader SelectedUserField3Header
        {
            get { return _SelectedUserField3Header; }
            set
            {
                if (_SelectedUserField3Header != value)
                {
                    _SelectedUserField3Header = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUserField3Header));
                }
            }
        }

        protected ColumnHeader _SelectedUserField4Header;

        public ColumnHeader SelectedUserField4Header
        {
            get { return _SelectedUserField4Header; }
            set
            {
                if (_SelectedUserField4Header != value)
                {
                    _SelectedUserField4Header = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUserField4Header));
                }
            }
        }


        protected List<DimmerRange> _DimmerRanges = new List<DimmerRange>();

        public List<DimmerRange> DimmerRanges
        {
            get { return _DimmerRanges; }
            set
            {
                if (_DimmerRanges != value)
                {
                    _DimmerRanges = value;

                    // Notify.
                    OnPropertyChanged(nameof(DimmerRanges));
                }
            }
        }


        protected List<DistroRange> _DistroRanges = new List<DistroRange>();

        public List<DistroRange> DistroRanges
        {
            get { return _DistroRanges; }
            set
            {
                if (_DistroRanges != value)
                {
                    _DistroRanges = value;

                    // Notify.
                    OnPropertyChanged(nameof(DistroRanges));
                }
            }
        }

        protected FriendlyImportFormat _SelectedDimmerImportFormat;

        public FriendlyImportFormat SelectedDimmerImportFormat
        {
            get { return _SelectedDimmerImportFormat; }
            set
            {
                if (_SelectedDimmerImportFormat != value)
                {
                    _SelectedDimmerImportFormat = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedDimmerImportFormat));
                }
            }
        }

        protected bool _NoUniverseDataProvided = false;

        public bool NoUniverseDataProvided
        {
            get { return _NoUniverseDataProvided; }
            set
            {
                if (_NoUniverseDataProvided != value)
                {
                    _NoUniverseDataProvided = value;

                    // Notify.
                    OnPropertyChanged(nameof(NoUniverseDataProvided));
                }
            }
        }


        protected bool _UniverseDataInSeperateColumn = true;

        public bool UniverseDataInSeperateColumn
        {
            get { return _UniverseDataInSeperateColumn; }
            set
            {
                if (_UniverseDataInSeperateColumn != value)
                {
                    _UniverseDataInSeperateColumn = value;

                    // Notify.
                    OnPropertyChanged(nameof(UniverseDataInSeperateColumn));
                }
            }
        }



        protected ColumnHeader _SelectedUniverseHeader;

        public ColumnHeader SelectedUniverseHeader
        {
            get { return _SelectedUniverseHeader; }
            set
            {
                if (_SelectedUniverseHeader != value)
                {
                    _SelectedUniverseHeader = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUniverseHeader));
                }
            }
        }


        protected FriendlyImportFormat _SelectedUniverseImportFormat;

        public FriendlyImportFormat SelectedUniverseImportFormat
        {
            get { return _SelectedUniverseImportFormat; }
            set
            {
                if (_SelectedUniverseImportFormat != value)
                {
                    _SelectedUniverseImportFormat = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUniverseImportFormat));
                }
            }
        }


        protected FriendlyImportFormat _SelectedDistroImportFormat;

        public FriendlyImportFormat SelectedDistroImportFormat
        {
            get { return _SelectedDistroImportFormat; }
            set
            {
                if (_SelectedDistroImportFormat != value)
                {
                    _SelectedDistroImportFormat = value;

                    // Set Enables.
                    SetDistroEnables(value.ImportFormat);

                    // Notify.
                    OnPropertyChanged(nameof(SelectedDistroImportFormat));
                }
            }
        }


        protected string _DistroPrefix;

        public string DistroPrefix
        {
            get { return _DistroPrefix; }
            set
            {
                if (_DistroPrefix != value)
                {
                    _DistroPrefix = value;

                    // Notify.
                    OnPropertyChanged(nameof(DistroPrefix));
                }
            }
        }


        protected bool _DistroPrefixEnable;

        public bool DistroPrefixEnable
        {
            get { return _DistroPrefixEnable; }
            set
            {
                if (_DistroPrefixEnable != value)
                {
                    _DistroPrefixEnable = value;

                    // Notify.
                    OnPropertyChanged(nameof(DistroPrefixEnable));
                }
            }
        }
        


        #endregion

        #region Commands.

        protected RelayCommand _BrowseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                return _BrowseCommand;
            }
        }

        protected void BrowseCommandExecute(object parameter)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
                FilePath = openDialog.FileName;
                FileName = openDialog.SafeFileName;

                // Validate File.
                string errorMessage = string.Empty;
                if (FileImport.ValidateFile(FilePath, out errorMessage) == false)
                {
                    // File is Invalid.
                    FileErrorMessage = errorMessage;
                    ImportSettingsEnabled = false;
                }

                else
                {
                    // File is Valid.
                    FileErrorMessage = string.Empty;
                    ImportSettingsEnabled = true;

                    // Populate Controls.
                    PopulateColumnMappingControl();
                }
            }
        }

        #endregion

        #region Methods.
        protected void SetDistroEnables(ImportFormat distroImportFormat)
        {
            switch (distroImportFormat)
            {
                case ImportFormat.Format1:
                    DistroPrefixEnable = true;
                    break;
                case ImportFormat.Format2:
                    DistroPrefixEnable = false;
                    break;
                case ImportFormat.Format3:
                    DistroPrefixEnable = true;
                    break;
                case ImportFormat.Format4:
                    DistroPrefixEnable = true;
                    break;
                default:
                    break;
            }
        }

        protected void PopulateColumnMappingControl()
        {
            ColumnHeaders = FileImport.CollectHeaders(FilePath);
            PredictColumnSelections();
        }

        protected void PredictColumnSelections()
        {
            var headers = ColumnHeaders.ToList();

            // Dimmer.
            SelectedDimmerNumberHeader = headers.Find(item => item.HeaderLowerCase.Contains("dimmer"));

            // Position.
            SelectedPostionHeader = headers.Find(item => item.HeaderLowerCase.Contains("position"));

            // Multicore Name.
            SelectedMulticoreNameHeader = headers.Find(item => item.HeaderLowerCase.Contains("multicore name") ||
            item.HeaderLowerCase.Contains("weiland") ||
            item.HeaderLowerCase.Contains("multi"));

            // Instrument Name.
            SelectedInstrumentNameHeader = headers.Find(item => item.HeaderLowerCase.Contains("instrument") ||
            item.HeaderLowerCase.Contains("unit"));

            // UserField1.
            SelectedUserField1Header = headers.Find(item => item.HeaderLowerCase.Contains("user field 1"));

            // UserField2.
            SelectedUserField2Header = headers.Find(item => item.HeaderLowerCase.Contains("user field 2"));

            // UserField3.
            SelectedUserField3Header = headers.Find(item => item.HeaderLowerCase.Contains("user field 3"));

            // UserField4.
            SelectedUserField4Header = headers.Find(item => item.HeaderLowerCase.Contains("user field 4"));
        }
        #endregion
    }
}
