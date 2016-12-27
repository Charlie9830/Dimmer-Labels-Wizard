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
using System.Data.Entity;
using Dimmer_Labels_Wizard_WPF.Repositories;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Drop Create Database.
            //Database.SetInitializer(new DropCreateDatabaseAlways<PrimaryDB>());

            // Migrate new Schema.
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PrimaryDB, Migrations.Configuration>());

            Properties.Settings.Default.LastUsedFilePath = string.Empty;

            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}
