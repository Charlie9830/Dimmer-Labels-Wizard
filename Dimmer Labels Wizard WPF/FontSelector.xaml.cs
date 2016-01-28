using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for FontSelector.xaml
    /// </summary>
    public partial class FontSelector : UserControl, INotifyPropertyChanged
    {
        public FontSelector()
        {
            InitializeComponent();
        }

        #region Internal Binding
        private FontFamily _SelectedFontFamily;
        public FontFamily SelectedFontFamily
        {
            get
            {
                return _SelectedFontFamily;
            }
            set
            {
                if (_SelectedFontFamily != value)
                {
                    _SelectedFontFamily = value;

                    // Modify External Dependency Property Value.
                    if (value != SelectedFont.FontFamily)
                    {
                        SelectedFont = new Typeface(value, GetStyle(), GetWeight(), FontStretches.Normal);
                    }
                    
                    // Notify.
                    RaisePropertyChanged(nameof(SelectedFontFamily));
                }
            }
        }

        private bool _IsBold = false;

        public bool IsBold
        {
            get { return _IsBold; }
            set
            {
                if (_IsBold != value)
                {
                    _IsBold = value;

                    // Modifiy External Dependency Property Value.
                    SelectedFont = new Typeface(SelectedFontFamily, GetStyle(), GetWeight(), FontStretches.Normal);

                    // Notify
                    RaisePropertyChanged(nameof(IsBold));
                }
            }
        }

        private bool _IsItalics = false;

        public bool IsItalics
        {
            get { return _IsItalics; }
            set
            {
               if (_IsItalics != value)
                {
                    _IsItalics = value;

                    // Modifiy External Dependency Property Value.
                    SelectedFont = new Typeface(SelectedFontFamily, GetStyle(), GetWeight(), FontStretches.Normal);

                    // Notify.
                    RaisePropertyChanged(nameof(IsItalics));
                }
            }
        }

        private bool _IsUnderlined = false;

        public bool IsUnderlined
        {
            get { return _IsUnderlined; }
            set
            {
                if (_IsUnderlined != value)
                {
                    _IsUnderlined = value;

                    // Modifiy External Dependency Property Value.
                    SelectedFont = new Typeface(SelectedFontFamily, GetStyle(), GetWeight(), FontStretches.Normal);

                    // Notify.
                    RaisePropertyChanged(nameof(IsUnderlined));
                }
            }
        }
        #endregion

        #region Dependency Property External Binding
        public Typeface SelectedFont
        {
            get { return (Typeface)GetValue(SelectedFontProperty); }
            set { SetValue(SelectedFontProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFont.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFontProperty =
            DependencyProperty.Register("SelectedFont", typeof(Typeface), typeof(FontSelector),
                new FrameworkPropertyMetadata(new Typeface("Arial"), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnSelectedFontPropertyChanged)));

        private static void OnSelectedFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as FontSelector;
            var newFont = e.NewValue as Typeface;

            if (newFont != null)
            {
                instance.SelectedFontFamily = newFont.FontFamily;
                instance.IsBold = newFont.Weight == FontWeights.Bold ? true : false;
                instance.IsItalics = newFont.Style == FontStyles.Italic ? true : false;
                instance.IsUnderlined = false;
            }
        }

        #endregion

        #region Methods
        protected FontStyle GetStyle()
        {
            return IsItalics == true ? FontStyles.Italic : FontStyles.Normal;
        }

        protected FontWeight GetWeight()
        {
            return IsBold == true ? FontWeights.Bold : FontWeights.Regular;
        }
        #endregion

        #region Interface Implementations
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
