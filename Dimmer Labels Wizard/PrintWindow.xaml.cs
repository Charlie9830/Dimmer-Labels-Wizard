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
using System.Printing;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Dimmer_Labels_Wizard
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintWindowViewModel ViewModel = new PrintWindowViewModel();

        public PrintWindow()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            PrintDialog pDialog = new PrintDialog();
            if (pDialog.ShowDialog() == true)
            {
                Size pageSize = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);
                FixedDocument printDocument = new FixedDocument();
                printDocument.DocumentPaginator.PageSize = pageSize;

                List<Canvas> pageCanvases = LabelStrip.RenderToPrinter(Globals.LabelStrips, pDialog.PrintableAreaWidth,
                    pDialog.PrintableAreaHeight);

                foreach (var canvas in pageCanvases)
                {
                    FixedPage page = new FixedPage();
                    page.Width = pageSize.Width;
                    page.Height = pageSize.Height;

                    page.Children.Add(canvas);

                    PageContent pageContent = new PageContent();
                    ((IAddChild)pageContent).AddChild(page);

                    printDocument.Pages.Add(pageContent);
                }


                pDialog.PrintDocument(printDocument.DocumentPaginator, "Labels");
            }
        }
    }
}
