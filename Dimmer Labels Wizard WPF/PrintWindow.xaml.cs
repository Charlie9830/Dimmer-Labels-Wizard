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
using System.Windows.Markup;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<LabelStrip> printStrips = new List<LabelStrip>();
            bool distroValidationResult;
            bool dimmerValidationResult;
            
            printStrips.AddRange(DistroPrintRangeControl.ViewModel.GetPrintRange(out distroValidationResult));
            printStrips.AddRange(DimmerPrintRangeControl.ViewModel.GetPrintRange(out dimmerValidationResult));

            if (distroValidationResult == false)
            {
                MessageBox.Show("Distro Print Range could not be verified.");
            }

            if (dimmerValidationResult == false)
            {
                MessageBox.Show("Dimmer Print Range could not be verified.");
            }

            if (distroValidationResult && dimmerValidationResult)
            {

                PrintDialog pDialog = new PrintDialog();
                if (pDialog.ShowDialog() == true)
                {
                    pDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                    double halfInch = 48;
                    Thickness safePrintingMargin = new Thickness(halfInch);
                    Size printableArea = new Size(pDialog.PrintableAreaWidth,
                        pDialog.PrintableAreaHeight);
                    PageMediaSize totalPageSize = pDialog.PrintTicket.PageMediaSize;
                    
                    FixedDocument printDocument = new FixedDocument();
                    printDocument.DocumentPaginator.PageSize = printableArea;

                    //List<Canvas> pageCanvases = LabelStrip.RenderToPrinter(printStrips, pDialog.PrintableAreaWidth,
                    //    pDialog.PrintableAreaHeight,UserParameters.SingleLabel);


                    //foreach (var canvas in pageCanvases)
                    //{
                    //    FixedPage page = new FixedPage();
                    //    page.Width = printableArea.Width;
                    //    page.Height = printableArea.Height;
                    //    page.Margin = safePrintingMargin;

                    //    page.Children.Add(canvas);

                    //    PageContent pageContent = new PageContent();
                    //    ((IAddChild)pageContent).AddChild(page);

                    //    printDocument.Pages.Add(pageContent);
                    //}
                    throw new NotImplementedException();

                    //pDialog.PrintDocument(printDocument.DocumentPaginator, "Labels");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
