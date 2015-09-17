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
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        public CellControl()
        {
            InitializeComponent();
        }

        #region ViewModel Interfacing Methods.
        public void ShowControl(List<FooterCellText> selectedFooterText , 
            List<HeaderCellWrapper> selectedHeaderText, Point mouseLocation )
        {
            var viewModel = DataContext as CellControlViewModel;

            viewModel.Reset();

            foreach (var element in selectedHeaderText)
            {
                foreach (var cell in element.Cells)
                {
                    viewModel.HeaderCells.Add(cell);
                }
            }

            foreach (var element in selectedFooterText)
            {
                viewModel.FooterCells.Add(element);
            }

            Margin = new Thickness(mouseLocation.X, mouseLocation.Y, 0, 0);
            Visibility = Visibility.Visible;
        }

        public void HideControl()
        {
            Visibility = Visibility.Hidden;
            var viewModel = DataContext as CellControlViewModel;
            viewModel.Reset();
        }
        #endregion

        void CellControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
