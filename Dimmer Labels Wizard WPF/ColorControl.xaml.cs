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
using Xceed.Wpf.Toolkit;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for ColorControl.xaml
    /// </summary>
    public partial class ColorControl : UserControl
    {
        public ColorControl()
        {
            InitializeComponent();
            var viewModel = DataContext as ColorControlViewModel;
            viewModel.RenderRequested += ViewModel_RenderRequested;
        }

        public void LoadControl(List<HeaderCell> headerCells, List<FooterCell> footerCells)
        {
            var viewModel = DataContext as ColorControlViewModel;

            foreach (var element in headerCells)
            {
                viewModel.SelectedHeaderCells.Add(element);
            }

            foreach (var element in footerCells)
            {
                viewModel.SelectedFooterCells.Add(element);
            }
        }

        public void ClearControl()
        {
            var viewModel = DataContext as ColorControlViewModel;

            viewModel.Clear();
        }

        #region Event Handlers
        private void ViewModel_RenderRequested(object sender, EventArgs e)
        {
            OnRenderRequested();
        }
        #endregion

        #region External Events
        public event EventHandler RenderRequested;

        protected void OnRenderRequested()
        {
            RenderRequested(this, new EventArgs());
        }
        #endregion
    }
}
