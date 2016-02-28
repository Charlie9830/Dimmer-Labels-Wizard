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
using Microsoft.Win32;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new Editor();
            window.ShowDialog();

            Application.Current.Shutdown();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new LabelManager();

            window.Show();
        }
    }
}
