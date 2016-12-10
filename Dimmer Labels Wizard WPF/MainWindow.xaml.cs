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
            InitializeComponent();
            
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            //Database Viewer.
            //var window = new DatabaseViewer();
            //window.Show();

            //using (var context = new PrimaryDB())
            //{
            //    context.Database.ExecuteSqlCommand("DELETE from Strips");
            //    context.Database.ExecuteSqlCommand("DELETE from LabelStripTemplates");
            //    context.Database.ExecuteSqlCommand("DELETE from CellRowTemplates");
            //    context.Database.ExecuteSqlCommand("DELETE from LabelCellTemplates");
            //    context.Database.ExecuteSqlCommand("DELETE from SerializableFonts");
            //}
            
            //using (var context = new PrimaryDB())
            //{
            //    var cellRowTemplate0 = new CellRowTemplate() { DataField = LabelField.Position };
            //    var cellRowTemplate1 = new CellRowTemplate() { DataField = LabelField.ChannelNumber };

            //    var labelCellTemplate = new LabelCellTemplate()
            //    {
            //        EFTest = "CellRow Testing: LabelCellTemplate",
            //        CellRowTemplates = new List<CellRowTemplate>() { cellRowTemplate0, cellRowTemplate1 }
            //    };

            //    var labelStripTemplate = new LabelStripTemplate()
            //    {
            //        Name = "CellRow Testing: LabelStripTemplate",
            //        UpperCellTemplate = labelCellTemplate
            //    };

            //    context.Templates.Add(labelStripTemplate);

            //    context.SaveChanges();
            //}

            //TemplateRepository repo = new TemplateRepository(new PrimaryDB());

            //var templates = repo.GetTemplates();

            //var query = (from item in templates
            //            where item.Name == "CellRow Testing: LabelStripTemplate"
            //            select item).First();

            //Console.WriteLine(query.Name);
        }
    }
}
