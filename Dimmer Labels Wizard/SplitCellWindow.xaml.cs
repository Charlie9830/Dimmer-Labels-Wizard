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
    /// Interaction logic for SplitCellWindow.xaml
    /// </summary>
    public partial class SplitCellWindow : Window
    {
        public SplitCellViewModel ViewModel = new SplitCellViewModel();

        public SplitCellWindow()
        {
            InitializeComponent();
            this.Loaded += SplitCellWindow_Loaded;

            this.DataContext = ViewModel;
        }

        void SplitCellWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.DrawToCanvas();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Update();
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
