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
            var viewModel = DataContext as CellControlViewModel;
            viewModel.RenderRequested += ViewModel_RenderRequested;
        }

        #region ViewModel Interfacing Methods.
        public void LoadControl(List<FooterCellText> selectedFooterText , 
            List<HeaderCellWrapper> selectedHeaderText)
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

        }

        public void ClearControl()
        {
            var viewModel = DataContext as CellControlViewModel;
            viewModel.Reset();
        }
        #endregion

        #region Event Handling
        private void ViewModel_RenderRequested(object sender, EventArgs e)
        {
            OnRenderRequested();
        }

        void CellControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DataTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BindingExpression bindingExpression = DataTextBox.GetBindingExpression(TextBox.TextProperty);
                bindingExpression.UpdateSource();
            }
        }
        #endregion

        #region Event Declarations
        public event EventHandler RenderRequested;

        protected void OnRenderRequested()
        {
            RenderRequested(this, new EventArgs());
        }
        #endregion
    }
}
