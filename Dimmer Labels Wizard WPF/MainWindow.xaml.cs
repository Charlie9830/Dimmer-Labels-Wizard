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
using Microsoft.Win32;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindows.ImportSettingsWindow = new ImportSettings();
            Hide();
            ApplicationWindows.ImportSettingsWindow.Show();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new Editor();
            window.ShowDialog();

            Application.Current.Shutdown();

            #region Hardcoded UserParameters
            UserParameters.CreateDimmerObjects = true;

            UserParameters.CreateDistroObjects = true;
            UserParameters.DistroImportFormat = ImportFormatting.Format1;
            UserParameters.DistroNumberPrefix = "N";
            UserParameters.DimmerImportFormat = ImportFormatting.Format3;

            UserParameters.DimmerRanges.Add(new DimmerRange(1, 1, 12));
            UserParameters.DistroRanges.Add(new DistroRange(1, 12));

            UserParameters.GenerateDistroRange();

            UserParameters.DMXAddressImportFormat = ImportFormatting.NoUniverseData;

            UserParameters.ChannelNumberColumnIndex = 1;
            UserParameters.DimmerNumberColumnIndex = 0;
            UserParameters.InstrumentTypeColumnIndex = 4;
            UserParameters.MulticoreNameColumnIndex = 2;
            UserParameters.PositionColumnIndex = 3;

            UserParameters.DimmerLabelWidthInMM = 16;
            UserParameters.DimmerLabelHeightInMM = 18;

            UserParameters.DistroLabelWidthInMM = 16;
            UserParameters.DistroLabelHeightInMM = 18;

            UserParameters.HeaderField = LabelField.MulticoreName;
            UserParameters.FooterMiddleField = LabelField.ChannelNumber;
            UserParameters.FooterBottomField = LabelField.InstrumentName;

            UserParameters.SingleLabel = true;
            #endregion
            FileImport.ImportFile();

            if (Globals.UnresolvableUnits.Count > 0)
            {
                ApplicationWindows.UnResolveableDataWindow = new UnResolveableData();
                Hide();
                ApplicationWindows.UnResolveableDataWindow.Show();
            }

            else
            {
                DataHandling.SanitizeDimDistroUnits();

                Hide();
                ApplicationWindows.LabelSetupWindow = new LabelSetup();
                ApplicationWindows.LabelSetupWindow.Show();
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Import File";
            fileDialog.Filter = "Comma Seperated Values File (*.csv) | *.csv";
            fileDialog.FilterIndex = 0;
            fileDialog.DefaultExt = ".csv";

            if (fileDialog.ShowDialog() == true)
            {
                string message = string.Empty;
                FileImport.ValidateFile(fileDialog.FileName, out message);

                MessageBox.Show(message);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.Show();
        }
    }
}
