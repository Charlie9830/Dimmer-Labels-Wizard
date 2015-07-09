using System;
using System.Collections.Generic;
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

namespace Dimmer_Labels_Wizard
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        public string[] Data { get; set; }
        public Typeface[] Typefaces { get; set; }
        public double[] FontSizes { get; set; }

        // When UpdatingUI is True, External Events are Surpressed.
        public bool UpdatingUI = false;

        private int[] fontSizes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,12,14,16,18,20,22,24,26,28,30,32,
                                  34,36,48,60,72};


        private List<Typeface> fontFamilyTypefaces = new List<Typeface>();

        private string differingEntries = "*";

        public CellControl()
        {
            InitializeComponent();
            this.Loaded += CellControl_Loaded;

            FontComboBox.SelectionChanged += FontComboBox_SelectionChanged;
            BoldToggleButton.Click += BoldToggleButton_Click;
            ItalicsToggleButton.Click += ItalicsToggleButton_Click;
            // SizeComboBox.TextChanged event Initialied in XAML.

            DataTextBox.TextChanged += DataTextBox_TextChanged;
        }

        void CellControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBoxes();
        }

        public void RenderControl()
        {
            UpdatingUI = true;
            #region Sanity Checks
            if (Data.Length != Typefaces.Length && Typefaces.Length != FontSizes.Length)
            {
                MessageBox.Show("Incomplete Data. Data, Typeface and Fontsize Array Lengths are not the same.");
            }

            if (Data.Length == 0 || Typefaces.Length == 0 || FontSizes.Length == 0)
            {
                MessageBox.Show("One or More Data,TypeFace,FontSize arrays has a length of 0.");
            }
            #endregion

            DataTextBox.Text = CheckDataEquality() ? Data.First() : differingEntries;
            FontComboBox.SelectedItem = CheckTypefaceEquality() ? Typefaces.First().FontFamily : null;
            BoldToggleButton.IsChecked = CheckTypefaceEquality() ? Typefaces.First().Weight == FontWeights.Bold : false;
            ItalicsToggleButton.IsChecked = CheckTypefaceEquality() ? Typefaces.First().Style == FontStyles.Italic : false;

            // Urnary Expression throws Compile time error. Cannot Differentiate null from Double.
            if (CheckFontSizeEquality() == true)
            {
                SizeComboBox.Text = Convert.ToString(FontSizes.First());
            }

            else
            {
                SizeComboBox.SelectedIndex = -1;
            }

            UpdatingUI = false;
        }

        public void ResetControl()
        {
            UpdatingUI = true;

            FontComboBox.SelectedIndex = -1;
            SizeComboBox.SelectedIndex = -1;
            BoldToggleButton.IsChecked = false;
            ItalicsToggleButton.IsChecked = false;
            DataTextBox.Text = null;

            UpdatingUI = false;
        }

        public void SetTitle(string title)
        {
            TitleLabel.Content = title;
        }

        private bool CheckDataEquality()
        {
            string referenceData = Data.First();

            if (Data.All(item => item == referenceData) == true)
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
            Typeface referenceTypeface = Typefaces.First();

            if (Typefaces.All(item => item.FontFamily.Source == referenceTypeface.FontFamily.Source) == true)
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
            double referenceFontSize = FontSizes.First();

            if (FontSizes.All(item => item == referenceFontSize) == true)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private void PopulateComboBoxes()
        {
            FontComboBox.ItemsSource = Fonts.SystemFontFamilies;
            SizeComboBox.ItemsSource = fontSizes;
            //TypefaceComboBox.ItemsSource = fontFamilyTypefaces;
        }

        private void PopulateFontFamilyTypefacesComboBox()
        {
            fontFamilyTypefaces.Clear();
            
            FontFamily selectedFontFamily = (FontFamily)FontComboBox.SelectedItem;

            foreach (var element in selectedFontFamily.FamilyTypefaces)
            {
                fontFamilyTypefaces.Add(new Typeface(element.DeviceFontName));
            }
        }

        #region Update Methods
        private void UpdateData()
        {
            if (DataTextBox.Text != differingEntries)
            {
                List<string> outgoingData = new List<string>();
                int entryQTY = Data.Length;

                for (int count = 1; count <= entryQTY; count++)
                {
                    outgoingData.Add(DataTextBox.Text);
                }

                Data = outgoingData.ToArray();
            }
        }

        private void UpdateTypeface()
        {
            if (FontComboBox.SelectedItem != null)
            {
                List<Typeface> outgoingTypeface = new List<Typeface>();
                int entryQTY = Typefaces.Length;

                for (int count = 1; count <= entryQTY; count++)
                {
                    outgoingTypeface.Add(GetTypeface());
                }

                Typefaces = outgoingTypeface.ToArray();
            }
        }

        private void UpdateFontSize()
        {
            // Cannot Differentiate between double and null. Use SelectedIndex instead.
            if (SizeComboBox.SelectedIndex != -1 && SizeComboBox.Text != "")
            {
                List<double> outgoingFontSize = new List<double>();
                int entryQTY = FontSizes.Length;

                double selectedValue = Convert.ToDouble(SizeComboBox.Text.ToString());
                Console.WriteLine(selectedValue);
                for (int count = 1; count <= entryQTY; count++)
                {
                    outgoingFontSize.Add(selectedValue);
                }

                FontSizes = outgoingFontSize.ToArray();
            }
        }
        #endregion

        private Typeface GetTypeface()
        {
            FontFamily selectedFontFamily = (FontFamily)FontComboBox.SelectedItem;

            Typeface returnFace = new Typeface(selectedFontFamily, GetFontStyle(), GetFontWeight(), new FontStretch());

            return returnFace;
        }

        private FontStyle GetFontStyle()
        {
            FontStyle returnStyle = new FontStyle();
            returnStyle = FontStyles.Normal;

            if (ItalicsToggleButton.IsChecked == true)
            {
                returnStyle = FontStyles.Italic;
            }

            return returnStyle;
        }

        private FontWeight GetFontWeight()
        {
            FontWeight returnWeight = new FontWeight();
            returnWeight = FontWeights.Regular;

            if (BoldToggleButton.IsChecked == true)
            {
                returnWeight = FontWeights.Bold;
            }

            return returnWeight;
        }
        
        #region Internal Event Handling

        void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTypeface();
            OnPropertyChanged(new EventArgs());
        }

        void ItalicsToggleButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTypeface();
            OnPropertyChanged(new EventArgs());
        }

        void BoldToggleButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTypeface();
            OnPropertyChanged(new EventArgs());
        }

        private void SizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFontSize();
            OnPropertyChanged(new EventArgs());
        }

        void DataTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UpdatingUI == false)
            {
                UpdateData();
                OnPropertyChanged(new EventArgs());
            }
        }
        #endregion

        #region External Events

        public event EventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(EventArgs e)
        {
            if (UpdatingUI == false)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
