using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Dimmer_Labels_Wizard_WPF
{
    public class CellControlViewModel : ViewModelBase
    {
        protected ObservableCollection<HeaderCell> _HeaderCells = new ObservableCollection<HeaderCell>();
        protected ObservableCollection<FooterCellText> _FooterCells = new ObservableCollection<FooterCellText>();

        protected string _Data = string.Empty;
        protected Typeface _Typeface = new Typeface("Arial");
        protected string _FontSize = string.Empty;
        protected bool _isBold = false;
        protected bool _isItalics = false;
        protected bool _isUnderlined = false;

        protected FontFamily _FontFamily = new FontFamily();

        protected double[] _fontSizes = {1,2,3,4,5,6,8,9,10,12,14,16,18,20,22,
                                        24,26,28,30,32,34,36,48,60,72};

        protected FontFamily[] _SystemFonts = Fonts.SystemFontFamilies.ToArray();

        protected const string nonEqualData = "***";
        protected const string nonEqualFontSize = "";

        protected bool Resetting = false;

        public CellControlViewModel()
        {
            _HeaderCells.CollectionChanged += Cells_CollectionChanged;
            _FooterCells.CollectionChanged += Cells_CollectionChanged;
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

        public ObservableCollection<FooterCellText> FooterCells
        {
            get
            {
                return _FooterCells;
            }
            set
            {
                _FooterCells = value;
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
                _Data = value;
                OnPropertyChanged("Data");
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
                _Typeface = value;
                OnPropertyChanged("Typeface");
            }
        }

        public string FontSize
        {
            get
            {
                return _FontSize;
            }

            set
            {
                _FontSize = value;
                OnPropertyChanged("FontSize");
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
                _isBold = value;
                OnPropertyChanged("IsBold");
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
                _isItalics = value;
                OnPropertyChanged("IsItalics");
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
                _isUnderlined = value;
                OnPropertyChanged("IsUnderlined");
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
                _FontFamily = value;
                OnPropertyChanged("FontFamily");
                UpdateTypeface();
                OnRenderRequested();
            }
        }

        #endregion

        #region UpdateMethods.
        void UpdateData()
        {
            if (_Data != nonEqualData)
            {
                foreach (var element in _HeaderCells)
                {
                    element.Data = _Data;
                }

                foreach (var element in _FooterCells)
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

            foreach (var element in _FooterCells)
            {
                element.Font = newTypeface;
            }
        }

        void UpdateFontSize()
        {
            if (_FontSize != string.Empty)
            {
                double selectedFontSize = Convert.ToDouble(_FontSize);

                foreach (var element in _HeaderCells)
                {
                    element.FontSize = selectedFontSize;
                }

                foreach (var element in _FooterCells)
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
        void Cells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Resetting == false)
            {
                string outData;
                _Data = CheckDataEquality(out outData) ? outData : nonEqualData;

                double outFontSize;
                _FontSize = CheckFontSizeEquality(out outFontSize) ? outFontSize.ToString() : string.Empty;

                Typeface outTypeface;
                bool typefaceEquality = CheckTypefaceEquality(out outTypeface);

                _isBold = typefaceEquality == true ? outTypeface.Weight == FontWeights.Bold : false;

                _isItalics = typefaceEquality == true ? outTypeface.Style == FontStyles.Italic : false;
                _Typeface = typefaceEquality ? outTypeface : null;
                _FontFamily = typefaceEquality ? outTypeface.FontFamily : null;

                // Signal Listeners to Update.
                OnPropertyChanged("Data");
                OnPropertyChanged("Typeface");
                OnPropertyChanged("FontFamily");
                OnPropertyChanged("FontSize");
                OnPropertyChanged("IsBold");
                OnPropertyChanged("IsItalics");
            }
        }

        #endregion

        #region Equality Checking Methods.

        private bool CheckDataEquality(out string value)
        {
            string referenceData;
            if (_HeaderCells.Count == 0)
            {
                referenceData = _FooterCells.FirstOrDefault().Data;
            }

            else
            {
                referenceData = _HeaderCells.FirstOrDefault().Data;
            }

            if (_HeaderCells.All(item => item.Data == referenceData) == true &&
                _FooterCells.All(item => item.Data == referenceData) == true)
            {
                value = referenceData;
                return true;
            }

            else
            {
                value = nonEqualData;
                return false;
            }
        }

        private bool CheckTypefaceEquality(out Typeface value)
        {
            Typeface referenceTypeface;

            if (_HeaderCells.Count == 0)
            {
                referenceTypeface = _FooterCells.First().Font;
            }
            else
            {
                referenceTypeface = _HeaderCells.First().Font;
            }

            if (_HeaderCells.All(item => item.Font == referenceTypeface) == true &&
                _FooterCells.All(item => item.Font == referenceTypeface) == true)
            {
                value = referenceTypeface;
                return true;
            }

            else
            {
                value = null;
                return false;
            }
        }

        private bool CheckFontSizeEquality(out double value)
        {
            double referenceFontSize;

            if (_HeaderCells.Count == 0)
            {
                referenceFontSize = _FooterCells.First().FontSize;
            }
            else
            {
                referenceFontSize = _HeaderCells.First().FontSize;
            }

            if (_HeaderCells.All(item => item.FontSize == referenceFontSize) == true &&
                _FooterCells.All(item => item.FontSize == referenceFontSize) == true)
            {
                value = referenceFontSize;
                return true;
            }

            else
            {
                value = 0.0D;
                return false;
            }
        }
        #endregion

        #region Public Methods
        public void Reset()
        {
            Resetting = true;

            _HeaderCells.Clear();
            _FooterCells.Clear();

            Resetting = false;
        }
        #endregion
    }
}
