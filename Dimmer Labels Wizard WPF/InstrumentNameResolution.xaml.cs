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

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for InstrumentNameResolution.xaml
    /// </summary>
    public partial class InstrumentNameResolution : UserControl
    {
        public InstrumentNameResolution()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InstrumentNameResolutionViewModel;
            viewModel.DataGridRefreshRequested += ViewModel_DataGridRefreshRequested;
        }

        // Provides a method of updating DataGrid Bindings without calling Property Changed Events.
        // The viewModel needs to be able to update Data Bindings from within a Property Changed Event Handler,
        // Raising a Property Changed Event from within it's Event Handler causes Stack Overflows.
        private void ViewModel_DataGridRefreshRequested(object sender, EventArgs e)
        {
            var source = dataGrid.ItemsSource;
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = source;
        }
    }
}
