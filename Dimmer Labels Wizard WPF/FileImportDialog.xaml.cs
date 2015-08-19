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
    /// Interaction logic for FileImportDialog.xaml
    /// </summary>
    public partial class FileImportDialog : Window
    {
        public FileImportDialog()
        {
            InitializeComponent();
            Loaded += FileImportDialog_Loaded;
        }

        private void FileImportDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (FileImport.ImportFile() == true)
            {
                Hide();
                if (Globals.UnresolvableUnits.Count > 0)
                {
                    ApplicationWindows.UnResolveableDataWindow = new UnResolveableData();
                    ApplicationWindows.UnResolveableDataWindow.Show();
                }
                else
                {
                    ApplicationWindows.SanitizationWindow = new SanitizationDialog();
                    ApplicationWindows.SanitizationWindow.Show();
                }
            }
        }
    }
}
