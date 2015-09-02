using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelSetupViewModel : ViewModelBase
    {
        // Data Bound Fields.
        protected bool _SingleLabelStripMode = false;
        protected bool _HeaderBackgroundColorOnly;

        protected string[] _DimmerPresets = { "Custom", "Jands HPC", "Jands HP" };  // String Literals are Directly Referenced elsewhere.
        protected string[] _DistroPresets = { "Custom" ,"Jands HPC", "Jands PDS12" }; // String Literals are Directly Referenced Elsewhere.
        protected string _SelectedDimmerPreset = "Custom";
        protected string _SelectedDistroPreset = "Custom";
        protected float _DimmerCellWidth = 16;
        protected float _DimmerCellHeight = 18;
        protected float _DistroCellWidth = 16;
        protected float _DistroCellHeight = 18;

        protected LabelField _HeaderField = LabelField.MulticoreName;
        protected LabelField _FooterTopField = LabelField.NoAssignment;
        protected LabelField _FooterMiddleField = LabelField.ChannelNumber;
        protected LabelField _FooterBottomField = LabelField.InstrumentName;

        #region Getters/Setters
        public bool SingleLabelStripMode
        {
            get
            {
                return _SingleLabelStripMode;
            }
            set
            {
                _SingleLabelStripMode = value;
                OnPropertyChanged("SingleLabelStripMode");
                OnPropertyChanged("FooterTopEnable");
            }
        }

        public bool HeaderBackgroundColorOnly
        {
            get
            {
                return _HeaderBackgroundColorOnly;
            }
            set
            {
                _HeaderBackgroundColorOnly = value;
                OnPropertyChanged("HeaderBackgroundColorOnly");
            }
        }

        public bool FooterTopEnable
        {
            get
            {
                return !_SingleLabelStripMode;
            }
        }

        public string[] DimmerPresets
        {
            get
            {
                return _DimmerPresets;
            }
        }

        public string[] DistroPresets
        {
            get
            {
                return _DistroPresets;
            }
        }

        public string SelectedDimmerPreset
        {
            get
            {
                return _SelectedDimmerPreset;
            }
            set
            {
                _SelectedDimmerPreset = value;
                OnPropertyChanged("SelectedDimmerPreset");

                SetDimmerCellSizes(value);
            }
        }

        public string SelectedDistroPreset
        {
            get
            {
                return _SelectedDistroPreset;
            }
            set
            {
                _SelectedDistroPreset = value;
                OnPropertyChanged("SelectedDistroPreset");

                SetDistroCellSizes(value);
            }
        }

        public float DimmerCellWidth
        {
            get
            {
                return _DimmerCellWidth;
            }
            set
            {
                _DimmerCellWidth = value;
                OnPropertyChanged("DimmerCellWidth");
            }
        }

        public float DimmerCellHeight
        {
            get
            {
                return _DimmerCellHeight;
            }
            set
            {
                _DimmerCellHeight = value;
                OnPropertyChanged("DimmerCellWidth");
            }
        }

        public float DistroCellWidth
        {
            get
            {
                return _DistroCellWidth;
            }
            set
            {
                _DistroCellWidth = value;
                OnPropertyChanged("DistroCellWidth");
            }
        }

        public float DistroCellHeight
        {
            get
            {
                return _DistroCellHeight;
            }
            set
            {
                _DistroCellHeight = value;
                OnPropertyChanged("DistroCellHeight");
            }
        }

        public LabelField HeaderField
        {
            get
            {
                return _HeaderField;
            }
            set
            {
                _HeaderField = value;
                OnPropertyChanged("HeaderField");
                OnPropertyChanged("InstrumentNameResolutionEnable");
            }
        }

        public LabelField FooterTopField
        {
            get
            {
                return _FooterTopField;
            }
            set
            {
                _FooterTopField = value;
                OnPropertyChanged("FooterTopField");
                OnPropertyChanged("InstrumentNameResolutionEnable");
            }
        }

        public LabelField FooterMiddleField
        {
            get
            {
                return _FooterMiddleField;
            }
            set
            {
                _FooterMiddleField = value;
                OnPropertyChanged("FooterMiddleField");
                OnPropertyChanged("InstrumentNameResolutionEnable");
            }
        }

        public LabelField FooterBottomField
        {
            get
            {
                return _FooterBottomField;
            }
            set
            {
                _FooterBottomField = value;
                OnPropertyChanged("FooterBottomField");
                OnPropertyChanged("InstrumentNameResolutionEnable");
            }
        }

        public bool InstrumentNameResolutionEnable
        {
            get
            {
                return InstrumentNameResolutionCanEnable();
            }
        }
        #endregion

        #region Setter Methods
        protected void SetDimmerCellSizes(string selectedValue)
        {
            switch (selectedValue)
            {
                case "Custom":
                    // Don't Change a thing.
                    return;
                case "Jands HPC":
                    _DimmerCellWidth = 21.3f;
                    _DimmerCellHeight = 16f;
                    break;
                case "Jands HP":
                    _DimmerCellWidth = 0;
                    _DimmerCellHeight = 0;
                    break;
                default:
                    break;
            }

            OnPropertyChanged("DimmerCellWidth");
            OnPropertyChanged("DimmerCellHeight");
        }

        protected void SetDistroCellSizes(string selectedValue)
        {
            switch (selectedValue)
            {
                case "Custom":
                    // Don't change a thing.
                    return;
                case "Jands HPC":
                    _DistroCellWidth = 21.3f;
                    _DistroCellHeight = 16f;
                    break;
                case "Jands PDS12":
                    _DistroCellWidth = 18.1f;
                    _DistroCellHeight = 16f;
                    break;
            }

            OnPropertyChanged("DistroCellWidth");
            OnPropertyChanged("DistroCellHeight");
        }
        #endregion

        #region General Methods
        protected bool InstrumentNameResolutionCanEnable()
        {
            if (_HeaderField == LabelField.InstrumentName ||
                _FooterTopField == LabelField.InstrumentName ||
                _FooterMiddleField == LabelField.InstrumentName ||
                _FooterBottomField == LabelField.InstrumentName)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        #endregion

        #region Update Methods
        public void UpdateModel()
        {
            UserParameters.SingleLabel = _SingleLabelStripMode;
            UserParameters.HeaderBackGroundColourOnly = _HeaderBackgroundColorOnly;

            UserParameters.DimmerLabelWidthInMM = _DimmerCellWidth;
            UserParameters.DimmerLabelHeightInMM = _DimmerCellHeight;
            UserParameters.DistroLabelWidthInMM = _DistroCellWidth;
            UserParameters.DistroLabelHeightInMM = _DistroCellHeight;

            UserParameters.HeaderField = _HeaderField;
            UserParameters.FooterTopField = _FooterTopField;
            UserParameters.FooterMiddleField = _FooterMiddleField;
            UserParameters.FooterBottomField = _FooterBottomField;
            
        }
        #endregion
    }

}
