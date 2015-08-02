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
        public HeaderCellControlViewModel HeaderViewModel
        {
            get
            {
                return _HeaderViewModel;
            }
            set
            {
                _HeaderViewModel = value;
            }
        }
        protected HeaderCellControlViewModel _HeaderViewModel = new HeaderCellControlViewModel();

        public FooterTopCellControlViewModel FooterTopViewModel
        {
            get
            {
                return _FooterTopViewModel;
            }
            set
            {
                _FooterTopViewModel = value;
            }
        }
        protected FooterTopCellControlViewModel _FooterTopViewModel = new FooterTopCellControlViewModel();

        public FooterMiddleCellControlViewModel FooterMiddleViewModel
        {
            get
            {
                return _FooterMiddleViewModel;
            }
            set
            {
                _FooterMiddleViewModel = value;
            }
        }
        protected FooterMiddleCellControlViewModel _FooterMiddleViewModel = new FooterMiddleCellControlViewModel();

        public FooterBottomCellControlViewModel FooterBottomViewModel
        {
            get
            {
                return _FooterBottomViewModel;
            }
            set
            {
                _FooterBottomViewModel = value;
            }
        }
        protected FooterBottomCellControlViewModel _FooterBottomViewModel = new FooterBottomCellControlViewModel();

        public CellControl()
        {
            InitializeComponent();
            DataTextBox.KeyUp += DataTextBox_KeyUp;
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
    }
}
