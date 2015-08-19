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
    /// Interaction logic for UnResolveableData.xaml
    /// </summary>
    public partial class UnResolveableData : Window
    {
        public UnResolveableData()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as UnresolveableDataViewModel;

            if (viewModel.UpdateModel() == true)
            {
                Hide();
                ApplicationWindows.SanitizationWindow = new SanitizationDialog();
                ApplicationWindows.SanitizationWindow.Show();
            }

            else
            {
                MessageBox.Show("Not all of the listed Units have been Resolved or Omitted.");
            }
        }
    }
}
