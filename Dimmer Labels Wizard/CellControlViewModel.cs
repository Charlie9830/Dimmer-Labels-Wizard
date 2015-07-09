using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace Dimmer_Labels_Wizard
{
    public class CellControlViewModel : INotifyPropertyChanged
    {
        protected ObservableCollection<HeaderCell> _HeaderCells = new ObservableCollection<HeaderCell>();

        protected string _ControlTitle = string.Empty;
        protected string _Data = string.Empty;
        protected Typeface _Typeface = new Typeface("Arial");
        protected double _FontSize = 0;
        protected bool _isBold = false;
        protected bool _isItalics = false;
        protected bool _isUnderlined = false;

        protected FontFamily _FontFamily = new FontFamily();

        protected double[] _fontSizes = {1,2,3,4,5,6,8,9,10,12,14,16,18,20,22,
                                        24,26,28,30,32,34,36,48,60,72};

        protected FontFamily[] _SystemFonts = Fonts.SystemFontFamilies.ToArray();

        protected string _DifferingEntries = "*";
        protected string _NullFontSize = string.Empty;

        public CellControlViewModel()
        {
            _HeaderCells.CollectionChanged += _headerCells_CollectionChanged;
        }

        #region Property Getters/Setters
        public ObservableCollection<HeaderCell> HeaderCells
        {
            get
            {
                return _HeaderCells;
            }

            set
            {
                _HeaderCells = value;
            }
        }

        public string ControlTitle
        {
            get
            {

                return _ControlTitle;
            }

            set
            {
                OnPropertyChanged("ControlTitle");
                _ControlTitle = value;
            }
        }

        public string Data
        {
            get
            {
                return _Data;
            }

            set
            {
                OnPropertyChanged("Data");
                _Data = value;
                UpdateData();
                OnRenderRequested();
            }
        }

        public Typeface Typeface
        {
            get
            {
                return _Typeface;
            }

            set
            {
                OnPropertyChanged("Typeface");
                _Typeface = value;
            }
        }

        public double FontSize
        {
            get
            {
                return _FontSize;
            }

            set
            {
                OnPropertyChanged("FontSize");
                _FontSize = value;
                UpdateFontSize();
                OnRenderRequested();
            }
        }

        public bool IsBold
        {
            get
            {
                return _isBold;
            }

            set
            {
                OnPropertyChanged("IsBold");
                _isBold = value;
                UpdateTypeface();
                OnRenderRequested();
            }
        }

        public bool IsItalics
        {
            get
            {
                return _isItalics;
            }

            set
            {
                OnPropertyChanged("IsItalics");
                _isItalics = value;
                UpdateTypeface();
                OnRenderRequested();
            }
        }

        public bool IsUnderlined
        {
            get
            {
                return _isUnderlined;
            }

            set
            {
                OnPropertyChanged("IsUnderlined");
                _isUnderlined = value;
            }
        }

        public double[] FontSizes
        {
            get
            {
                return _fontSizes;
            }
        }

        public FontFamily[] SystemFonts
        {
            get
            {
                return _SystemFonts;
            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return _FontFamily;
            }

            set
            {
                OnPropertyChanged("FontFamily");
                _FontFamily = value;
                UpdateTypeface();
                OnRenderRequested();

            }
        }

        #endregion

        #region UpdateMethods.
        void UpdateData()
        {
            if (_Data != _DifferingEntries)
            {
                foreach (var element in _HeaderCells)
                {
                    element.Data = _Data;
                }
            }
        }

        void UpdateTypeface()
        {
            Typeface newTypeface = new Typeface(_FontFamily,GetFontStyle(),GetFontWeight(),new FontStretch());

            foreach (var element in _HeaderCells)
            {
                element.Font = newTypeface;
            }
        }

        void UpdateFontSize()
        {
            if (_FontSize != 0)
            {
                double selectedFontSize = Convert.ToDouble(_FontSize);

                foreach (var element in _HeaderCells)
                {
                    element.FontSize = selectedFontSize;
                }
            }
        }

        FontStyle GetFontStyle()
        {
            if (_isItalics == true)
            {
                return FontStyles.Italic;
            }

            else
            {
                return FontStyles.Normal;
            }
        }

        FontWeight GetFontWeight()
        {
            if (_isBold == true)
            {
                return FontWeights.Bold;
            }

            else
            {
                return FontWeights.Normal;
            }
        }

        #endregion

        #region Event Handlers
        void _headerCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_HeaderCells.Count > 0)
            {
                _Data = CheckDataEquality() ? _HeaderCells.First().Data : _DifferingEntries;
                _FontSize = CheckFontSizeEquality() ? _HeaderCells.First().FontSize : 0;
                _isBold = CheckTypefaceEquality() ? _HeaderCells.First().Font.Weight == FontWeights.Bold : false;
                _isItalics = CheckTypefaceEquality() ? _HeaderCells.First().Font.Style == FontStyles.Italic : false;
                _Typeface = CheckTypefaceEquality() ? _HeaderCells.First().Font : null;

                if (CheckTypefaceEquality() == true)
                {
                    _Typeface = HeaderCells.First().Font;
                    _FontFamily = _Typeface.FontFamily;
                }

                else
                {
                    _Typeface = null;
                }
            }

            else
            {
                _Data = string.Empty;
            }

            // Signal Listeners to Update.
            OnPropertyChanged("Data");
            OnPropertyChanged("Typeface");
            OnPropertyChanged("FontFamily");
            OnPropertyChanged("FontSize");
            OnPropertyChanged("IsBold");
            OnPropertyChanged("IsItalics");
        }
        #endregion

        #region Data Equality Check Methods.

        private bool CheckDataEquality()
        {
            string referenceData = _HeaderCells.First().Data;

            if (_HeaderCells.All(item => item.Data == referenceData) == true)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        private bool CheckTypefaceEquality()
        {
            Typeface referenceTypeface = _HeaderCells.First().Font;

            if (_HeaderCells.All(item => item.Font.FontFamily.Source == referenceTypeface.FontFamily.Source) == true)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private bool CheckFontSizeEquality()
        {
            double referenceFontSize = _HeaderCells.First().FontSize;

            if (_HeaderCells.All(item => item.FontSize == referenceFontSize) == true)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        #endregion

        #region External Events
        public event PropertyChangedEventHandler PropertyChanged;
 
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event EventHandler RenderRequested;

        private void OnRenderRequested()
        {
            if (RenderRequested != null)
            {
                RenderRequested(this, new EventArgs());
            }
        }
        #endregion
    }
}
