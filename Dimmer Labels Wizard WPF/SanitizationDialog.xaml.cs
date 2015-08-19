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
    /// Interaction logic for SanitizationWindow.xaml
    /// </summary>
    public partial class SanitizationDialog : Window
    {
        public SanitizationDialog()
        {
            InitializeComponent();
            this.Loaded += SanitizationWindow_Loaded;
        }

        private void SanitizationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataHandling.SanitizeDimDistroUnits();

            PleaseWaitLabel.Content = "Complete";
        }
    }
}
