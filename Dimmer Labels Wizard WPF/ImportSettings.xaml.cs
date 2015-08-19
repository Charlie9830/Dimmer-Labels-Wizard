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
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for ImportSettings.xaml
    /// </summary>
    public partial class ImportSettings : Window
    {
        public ImportSettings()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ImportSettingsViewModel;
            viewModel.UpdateModel(dimmerRangeSelectorHost,distroRangeSelectorHost);

            this.Close();
            ApplicationWindows.FileImportWindow = new FileImportDialog();
            ApplicationWindows.FileImportWindow.Show();
        }
    }
}
