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
using System.Diagnostics;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for DatabaseManager.xaml
    /// </summary>
    public partial class DatabaseManager : Window
    {
        public DatabaseManager()
        {
            Stopwatch.Start();
            InitializeComponent();

            Loaded += DatabaseManager_Loaded;
        }

        private void DatabaseManager_Loaded(object sender, RoutedEventArgs e)
        {
            Stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine("==========");
            Console.WriteLine("Load Time: {0}", Stopwatch.ElapsedMilliseconds);
            Console.WriteLine("==========");
            Console.WriteLine();
        }

        protected Stopwatch Stopwatch = new Stopwatch();
    }
}
