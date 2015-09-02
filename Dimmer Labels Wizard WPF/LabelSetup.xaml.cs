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
    /// Interaction logic for LabelSetup.xaml
    /// </summary>
    public partial class LabelSetup : Window
    {
        public LabelSetup()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = DataContext as LabelSetupViewModel;
            mainViewModel.UpdateModel();

            var backgroundColorTableViewModel = backgroundColorTable.DataContext as BackgroundColorTableViewModel;
            backgroundColorTableViewModel.UpdateModel();

            // Push Instrument Name Resolutions Back to Model.
            var instrumentNameViewModel = instrumentNameResolution.DataContext as InstrumentNameResolutionViewModel;
            instrumentNameViewModel.UpdateModel();

            Hide();
            ApplicationWindows.LabelGenerationWindow = new LabelGenerationDialog();
            ApplicationWindows.LabelGenerationWindow.Show();

        }
    }
}
