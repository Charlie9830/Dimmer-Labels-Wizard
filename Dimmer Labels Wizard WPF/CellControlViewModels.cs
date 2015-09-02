using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public class HeaderCellControlViewModel : ViewModelBase
    {
        protected ObservableCollection<HeaderCell> _HeaderCells = new ObservableCollection<HeaderCell>();
        
        protected string _ControlTitle = string.Empty;
        protected string _Data = string.Empty;
        protected Typeface _Typeface = new Typeface("Arial");
        protected string _FontSize = string.Empty;
        protected bool _isBold = false;
        protected bool _isItalics = false;
        protected bool _isUnderlined = false;
        protected bool _GlobalApply = false;

        protected FontFamily _FontFamily = new FontFamily();

        protected double[] _fontSizes = {1,2,3,4,5,6,8,9,10,12,14,16,18,20,22,
                                        24,26,28,30,32,34,36,48,60,72};

        protected FontFamily[] _SystemFonts = Fonts.SystemFontFamilies.ToArray();

        protected string _DifferingEntries = "*";
        protected string _NullFontSize = string.Empty;

        public HeaderCellControlViewModel()
        {
            _HeaderCells.CollectionChanged += _headerCells_CollectionChanged;
        }

        #region Setup Methods
        public void SetTitle(string title)
        {
            _ControlTitle = title;
            OnPropertyChanged("ControlTitle");
        }
        #endregion

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
                _ControlTitle = value;
                OnPropertyChanged("ControlTitle");
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

        public bool GlobalApply
        {
            get
            {
                return _GlobalApply;
            }
            set
            {
                _GlobalApply = value;
                OnPropertyChanged("GlobalApply");
                OnGlobalApplySelected();
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

            if (_GlobalApply == true)
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    foreach (var cell in labelStrip.Headers)
                    {
                        cell.Font = newTypeface;
                    }
                }
            }

            else
            {
                foreach (var element in _HeaderCells)
                {
                    element.Font = newTypeface;
                }
            }
        }

        void UpdateFontSize()
        {
            if (_FontSize != string.Empty)
            {
                double selectedFontSize = Convert.ToDouble(_FontSize);

                if (_GlobalApply == true)
                {
                    foreach (var labelStrip in Globals.LabelStrips)
                    {
                        foreach (var cell in labelStrip.Headers)
                        {
                            cell.FontSize = selectedFontSize;
                        }
                    }
                }

                else
                {
                    foreach (var element in _HeaderCells)
                    {
                        element.FontSize = selectedFontSize;
                    }
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
                _FontSize = CheckFontSizeEquality() ? _HeaderCells.First().FontSize.ToString() : string.Empty;
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
                _FontFamily = null;
                _FontSize = string.Empty;
                _isBold = false;
                _isItalics = false;
                _isUnderlined = false;
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
        public event EventHandler GlobalApplySelected;

        protected virtual void OnGlobalApplySelected()
        {
            if (GlobalApply == true)
            {
                GlobalApplySelected(this, new EventArgs());
            }
        }
        #endregion

    }

    public class FooterTopCellControlViewModel : ViewModelBase
    {
        protected ObservableCollection<FooterCell> _FooterCells = new ObservableCollection<FooterCell>();

        protected string _ControlTitle = string.Empty;
        protected string _Data = string.Empty;
        protected Typeface _Typeface = new Typeface("Arial");
        protected string _FontSize = string.Empty;
        protected bool _isBold = false;
        protected bool _isItalics = false;
        protected bool _isUnderlined = false;
        protected bool _GlobalApply = false;

        protected FontFamily _FontFamily = new FontFamily();

        protected double[] _fontSizes = {1,2,3,4,5,6,8,9,10,12,14,16,18,20,22,
                                        24,26,28,30,32,34,36,48,60,72};

        protected FontFamily[] _SystemFonts = Fonts.SystemFontFamilies.ToArray();

        protected string _DifferingEntries = "*";
        protected string _NullFontSize = string.Empty;

        public FooterTopCellControlViewModel()
        {
            _FooterCells.CollectionChanged += _FooterCells_CollectionChanged;
        }

        #region Setup Methods
        public void SetTitle(string title)
        {
            _ControlTitle = title;
            OnPropertyChanged("ControlTitle");
        }
        #endregion

        #region Property Getters/Setters
        public ObservableCollection<FooterCell> FooterCells
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

        public string ControlTitle
        {
            get
            {

                return _ControlTitle;
            }

            set
            {
                _ControlTitle = value;
                OnPropertyChanged("ControlTitle");
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

        public bool GlobalApply
        {
            get
            {
                return _GlobalApply;
            }
            set
            {
                _GlobalApply = value;
                OnPropertyChanged("GlobalApply");
                OnGlobalApplySelected();
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
            if (_Data != _DifferingEntries)
            {
                foreach (var element in _FooterCells)
                {
                    element.TopData = _Data;
                }
            }
        }

        void UpdateTypeface()
        {
            Typeface newTypeface = new Typeface(_FontFamily, GetFontStyle(), GetFontWeight(), new FontStretch());

            if (_GlobalApply == true)
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    foreach (var cell in labelStrip.Footers)
                    {
                        cell.TopFont = newTypeface;
                    }
                }
            }

            else
            {
                foreach (var element in _FooterCells)
                {
                    element.TopFont = newTypeface;
                }
            }
        }

        void UpdateFontSize()
        {
            if (_FontSize != string.Empty)
            {
                double selectedFontSize = Convert.ToDouble(_FontSize);

                if (_GlobalApply == true)
                {
                    foreach (var labelStrip in Globals.LabelStrips)
                    {
                        foreach (var cell in labelStrip.Footers)
                        {
                            cell.TopFontSize = selectedFontSize;
                        }
                    }
                }

                else
                {
                    foreach (var element in _FooterCells)
                    {
                        element.TopFontSize = selectedFontSize;
                    }
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
        void _FooterCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_FooterCells.Count > 0)
            {
                _Data = CheckDataEquality() ? _FooterCells.First().TopData : _DifferingEntries;
                _FontSize = CheckFontSizeEquality() ? _FooterCells.First().TopFontSize.ToString() : string.Empty;
                _isBold = CheckTypefaceEquality() ? _FooterCells.First().TopFont.Weight == FontWeights.Bold : false;
                _isItalics = CheckTypefaceEquality() ? _FooterCells.First().TopFont.Style == FontStyles.Italic : false;
                _Typeface = CheckTypefaceEquality() ? _FooterCells.First().TopFont : null;

                if (CheckTypefaceEquality() == true)
                {
                    _Typeface = FooterCells.First().TopFont;
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
                _FontFamily = null;
                _FontSize = string.Empty;
                _isBold = false;
                _isItalics = false;
                _isUnderlined = false;
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
            string referenceData = _FooterCells.First().TopData;

            if (_FooterCells.All(item => item.TopData == referenceData) == true)
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
            Typeface referenceTypeface = _FooterCells.First().TopFont;

            if (_FooterCells.All(item => item.TopFont.FontFamily.Source == referenceTypeface.FontFamily.Source) == true)
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
            double referenceFontSize = _FooterCells.First().TopFontSize;

            if (_FooterCells.All(item => item.TopFontSize == referenceFontSize) == true)
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
        public event EventHandler GlobalApplySelected;

        protected virtual void OnGlobalApplySelected()
        {
            if (GlobalApply == true)
            {
                GlobalApplySelected(this, new EventArgs());
            }
        }
        #endregion
    }

    public class FooterMiddleCellControlViewModel : ViewModelBase
    {
        protected ObservableCollection<FooterCell> _FooterCells = new ObservableCollection<FooterCell>();

        protected string _ControlTitle = string.Empty;
        protected string _Data = string.Empty;
        protected Typeface _Typeface = new Typeface("Arial");
        protected string _FontSize = string.Empty;
        protected bool _isBold = false;
        protected bool _isItalics = false;
        protected bool _isUnderlined = false;
        protected bool _GlobalApply = false;

        protected FontFamily _FontFamily = new FontFamily();

        protected double[] _fontSizes = {1,2,3,4,5,6,8,9,10,12,14,16,18,20,22,
                                        24,26,28,30,32,34,36,48,60,72};

        protected FontFamily[] _SystemFonts = Fonts.SystemFontFamilies.ToArray();

        protected string _DifferingEntries = "*";
        protected string _NullFontSize = string.Empty;

        public FooterMiddleCellControlViewModel()
        {
            _FooterCells.CollectionChanged += _FooterCells_CollectionChanged;
        }

        #region Setup Methods
        public void SetTitle(string title)
        {
            _ControlTitle = title;
            OnPropertyChanged("ControlTitle");
        }
        #endregion

        #region Property Getters/Setters
        public ObservableCollection<FooterCell> FooterCells
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

        public string ControlTitle
        {
            get
            {

                return _ControlTitle;
            }

            set
            {
                _ControlTitle = value;
                OnPropertyChanged("ControlTitle");
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
                UpdateData();
                OnPropertyChanged("Data");
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

        public bool GlobalApply
        {
            get
            {
                return _GlobalApply;
            }
            set
            {
                _GlobalApply = value;
                OnPropertyChanged("GlobalApply");
                OnGlobalApplySelected();
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
            if (_Data != _DifferingEntries)
            {
                foreach (var element in _FooterCells)
                {
                    element.MiddleData = _Data;
                }
            }
        }

        void UpdateTypeface()
        {
            Typeface newTypeface = new Typeface(_FontFamily, GetFontStyle(), GetFontWeight(), new FontStretch());

            if (_GlobalApply == true)
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    foreach (var cell in labelStrip.Footers)
                    {
                        cell.MiddleFont = newTypeface;
                    }
                }
            }

            else
            {
                foreach (var element in _FooterCells)
                {
                    element.MiddleFont = newTypeface;
                }
            }
        }

        void UpdateFontSize()
        {
            if (_FontSize != string.Empty)
            {
                double selectedFontSize = Convert.ToDouble(_FontSize);

                if (_GlobalApply == true)
                {
                    foreach (var labelStrip in Globals.LabelStrips)
                    {
                        foreach (var cell in labelStrip.Footers)
                        {
                            cell.MiddleFontSize = selectedFontSize;
                        }
                    }
                }

                else
                {
                    foreach (var element in _FooterCells)
                    {
                        element.MiddleFontSize = selectedFontSize;
                    }
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
        void _FooterCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_FooterCells.Count > 0)
            {
                _Data = CheckDataEquality() ? _FooterCells.First().MiddleData : _DifferingEntries;
                _FontSize = CheckFontSizeEquality() ? _FooterCells.First().MiddleFontSize.ToString() : string.Empty;
                _isBold = CheckTypefaceEquality() ? _FooterCells.First().MiddleFont.Weight == FontWeights.Bold : false;
                _isItalics = CheckTypefaceEquality() ? _FooterCells.First().MiddleFont.Style == FontStyles.Italic : false;
                _Typeface = CheckTypefaceEquality() ? _FooterCells.First().MiddleFont : null;

                if (CheckTypefaceEquality() == true)
                {
                    _Typeface = FooterCells.First().MiddleFont;
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
                _FontFamily = null;
                _FontSize = string.Empty;
                _isBold = false;
                _isItalics = false;
                _isUnderlined = false;
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
            string referenceData = _FooterCells.First().MiddleData;

            if (_FooterCells.All(item => item.MiddleData == referenceData) == true)
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
            Typeface referenceTypeface = _FooterCells.First().MiddleFont;

            if (_FooterCells.All(item => item.MiddleFont.FontFamily.Source == referenceTypeface.FontFamily.Source) == true)
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
            double referenceFontSize = _FooterCells.First().MiddleFontSize;

            if (_FooterCells.All(item => item.MiddleFontSize == referenceFontSize) == true)
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
        public event EventHandler GlobalApplySelected;

        protected virtual void OnGlobalApplySelected()
        {
            if (GlobalApply == true)
            {
                GlobalApplySelected(this, new EventArgs());
            }
        }
        #endregion
    }

    public class FooterBottomCellControlViewModel : ViewModelBase
    {
        protected ObservableCollection<FooterCell> _FooterCells = new ObservableCollection<FooterCell>();

        protected string _ControlTitle = string.Empty;
        protected string _Data = string.Empty;
        protected Typeface _Typeface = new Typeface("Arial");
        protected string _FontSize = string.Empty;
        protected bool _isBold = false;
        protected bool _isItalics = false;
        protected bool _isUnderlined = false;
        protected bool _GlobalApply = false;

        protected FontFamily _FontFamily = new FontFamily();

        protected double[] _fontSizes = {1,2,3,4,5,6,8,9,10,12,14,16,18,20,22,
                                        24,26,28,30,32,34,36,48,60,72};

        protected FontFamily[] _SystemFonts = Fonts.SystemFontFamilies.ToArray();

        protected string _DifferingEntries = "*";
        protected string _NullFontSize = string.Empty;

        public FooterBottomCellControlViewModel()
        {
            _FooterCells.CollectionChanged += _FooterCells_CollectionChanged;
        }

        #region Setup Methods
        public void SetTitle(string title)
        {
            _ControlTitle = title;
            OnPropertyChanged("ControlTitle");
        }
        #endregion

        #region Property Getters/Setters
        public ObservableCollection<FooterCell> FooterCells
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

        public string ControlTitle
        {
            get
            {

                return _ControlTitle;
            }

            set
            {
                _ControlTitle = value;
                OnPropertyChanged("ControlTitle");
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
                UpdateData();
                OnPropertyChanged("Data");
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

        public bool GlobalApply
        {
            get
            {
                return _GlobalApply;
            }
            set
            {
                _GlobalApply = value;
                OnPropertyChanged("GlobalApply");
                OnGlobalApplySelected();
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
            if (_Data != _DifferingEntries)
            {
                foreach (var element in _FooterCells)
                {
                    element.BottomData = _Data;
                }
            }
        }

        void UpdateTypeface()
        {
            Typeface newTypeface = new Typeface(_FontFamily, GetFontStyle(), GetFontWeight(), new FontStretch());

            if (_GlobalApply == true)
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    foreach (var cell in labelStrip.Footers)
                    {
                        cell.BottomFont = newTypeface;
                    }
                }
            }

            else
            {
                foreach (var element in _FooterCells)
                {
                    element.BottomFont = newTypeface;
                }
            }
        }

        void UpdateFontSize()
        {
            if (_FontSize != string.Empty)
            {
                double selectedFontSize = Convert.ToDouble(_FontSize);

                if (_GlobalApply == true)
                {
                    foreach (var labelStrip in Globals.LabelStrips)
                    {
                        foreach (var cell in labelStrip.Footers)
                        {
                            cell.BottomFontSize = selectedFontSize;
                        }
                    }
                }

                else
                {
                    foreach (var element in _FooterCells)
                    {
                        element.BottomFontSize = selectedFontSize;
                    }
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
        void _FooterCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_FooterCells.Count > 0)
            {
                _Data = CheckDataEquality() ? _FooterCells.First().BottomData : _DifferingEntries;
                _FontSize = CheckFontSizeEquality() ? _FooterCells.First().BottomFontSize.ToString() : string.Empty;
                _isBold = CheckTypefaceEquality() ? _FooterCells.First().BottomFont.Weight == FontWeights.Bold : false;
                _isItalics = CheckTypefaceEquality() ? _FooterCells.First().BottomFont.Style == FontStyles.Italic : false;
                _Typeface = CheckTypefaceEquality() ? _FooterCells.First().BottomFont : null;

                if (CheckTypefaceEquality() == true)
                {
                    _Typeface = FooterCells.First().BottomFont;
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
                _FontFamily = null;
                _FontSize = string.Empty;
                _isBold = false;
                _isItalics = false;
                _isUnderlined = false;
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
            string referenceData = _FooterCells.First().BottomData;

            if (_FooterCells.All(item => item.BottomData == referenceData) == true)
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
            Typeface referenceTypeface = _FooterCells.First().BottomFont;

            if (_FooterCells.All(item => item.BottomFont.FontFamily.Source == referenceTypeface.FontFamily.Source) == true)
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
            double referenceFontSize = _FooterCells.First().BottomFontSize;

            if (_FooterCells.All(item => item.BottomFontSize == referenceFontSize) == true)
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
        public event EventHandler GlobalApplySelected;

        protected virtual void OnGlobalApplySelected()
        {
            if (GlobalApply == true)
            {
                GlobalApplySelected(this, new EventArgs());
            }
        }
        #endregion
    }
}
