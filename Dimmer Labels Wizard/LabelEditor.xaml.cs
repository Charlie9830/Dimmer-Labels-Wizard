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
    /// Interaction logic for LabelEditor.xaml
    /// </summary>
    public partial class LabelEditor : UserControl
    {
        // Label Selection
        public LabelStripSelection ActiveLabelStrip = new LabelStripSelection();

        // View Controls
        private ScaleTransform LabelCanvasScaleTransform = new ScaleTransform();

        public LabelEditor()
        {
            InitializeComponent();

            // View Setup
            CanvasBorder.ClipToBounds = true;
            LabelCanvasScaleTransform.ScaleX = 1;
            LabelCanvasScaleTransform.ScaleY = 1;
            LabelCanvas.RenderTransform = LabelCanvasScaleTransform;

            this.Loaded += LabelEditor_Loaded;
            RackSelector.SelectedItemChanged += RackSelector_SelectedItemChanged;

            LabelCanvas.MouseDown += LabelCanvas_MouseDown;

            HeaderCellControl.PropertyChanged += HeaderCellControl_PropertyChanged;
            FooterTopCellControl.PropertyChanged += FooterTopCellControl_PropertyChanged;
            FooterMiddleCellControl.PropertyChanged += FooterMiddleCellControl_PropertyChanged;
            FooterBottomCellControl.PropertyChanged += FooterBottomCellControl_PropertyChanged;

            ActiveLabelStrip.SelectedHeadersChanged += ActiveLabelStrip_SelectedHeadersChanged;
            ActiveLabelStrip.SelectedFootersChanged += ActiveLabelStrip_SelectedFootersChanged;
        }

        void ForceRender()
        {
            if (ActiveLabelStrip.LabelStrip != null)
            {
                LabelCanvas.Children.Clear();
                // Offset Point is given in WPF Pixels (Inches).
                ActiveLabelStrip.LabelStrip.RenderToDisplay(LabelCanvas, new Point(20, 20));
                CollectSelectionEvents();
                ActiveLabelStrip.RenderSelections(LabelCanvas);
            }
        }

        void PopulateHeaderCellControls()
        {
            if (ActiveLabelStrip.SelectedHeaders.Count > 0)
            {
                // Header Cell
                List<string> headerData = new List<string>();
                List<Typeface> headerTypefaces = new List<Typeface>();
                List<double> headerFontSizes = new List<double>();

                foreach (var element in ActiveLabelStrip.SelectedHeaders)
                {
                    headerData.Add(element.Data);
                    headerTypefaces.Add(element.Font);
                    headerFontSizes.Add(element.FontSize);
                }

                HeaderCellControl.Data = headerData.ToArray();
                HeaderCellControl.Typefaces = headerTypefaces.ToArray();
                HeaderCellControl.FontSizes = headerFontSizes.ToArray();
                HeaderCellControl.RenderControl();
            }

            else
            {
                HeaderCellControl.ResetControl();
            }
        }

        void PopulateFooterCellsControls()
        {
            if (ActiveLabelStrip.SelectedFooters.Count > 0)
            {
                // Top
                List<string> topData = new List<string>();
                List<Typeface> topTypefaces = new List<Typeface>();
                List<double> topFontSizes = new List<double>();

                // Middle
                List<string> middleData = new List<string>();
                List<Typeface> middleTypefaces = new List<Typeface>();
                List<double> middleFontSizes = new List<double>();

                // Bottom
                List<string> bottomData = new List<string>();
                List<Typeface> bottomTypefaces = new List<Typeface>();
                List<double> bottomFontSizes = new List<double>();

                foreach (var element in ActiveLabelStrip.SelectedFooters)
                {
                    topData.Add(element.TopData);
                    topTypefaces.Add(element.TopFont);
                    topFontSizes.Add(element.TopFontSize);

                    middleData.Add(element.MiddleData);
                    middleTypefaces.Add(element.MiddleFont);
                    middleFontSizes.Add(element.MiddleFontSize);

                    bottomData.Add(element.BottomData);
                    bottomTypefaces.Add(element.BottomFont);
                    bottomFontSizes.Add(element.BottomFontSize);
                }

                FooterTopCellControl.Data = topData.ToArray();
                FooterTopCellControl.Typefaces = topTypefaces.ToArray();
                FooterTopCellControl.FontSizes = topFontSizes.ToArray();
                FooterTopCellControl.RenderControl();

                FooterMiddleCellControl.Data = middleData.ToArray();
                FooterMiddleCellControl.Typefaces = middleTypefaces.ToArray();
                FooterMiddleCellControl.FontSizes = middleFontSizes.ToArray();
                FooterMiddleCellControl.RenderControl();

                FooterBottomCellControl.Data = bottomData.ToArray();
                FooterBottomCellControl.Typefaces = bottomTypefaces.ToArray();
                FooterBottomCellControl.FontSizes = bottomFontSizes.ToArray();
                FooterBottomCellControl.RenderControl();

            }

            else
            {
                FooterTopCellControl.ResetControl();
                FooterMiddleCellControl.ResetControl();
                FooterBottomCellControl.ResetControl();
            }
        }

        void CollectSelectionEvents()
        {
            foreach (var element in LabelCanvas.Children)
            {
                Border outline = (Border)element;

                outline.MouseDown += outline_MouseDown;
            }
        }

        private void PopulateRackLabelSelector()
        {
            TreeViewItem DistrosNode = new TreeViewItem();
            TreeViewItem DimmersNode = new TreeViewItem();

            DistrosNode.Header = "Distros";
            DimmersNode.Header = "Dimmers";

            List<LabelStrip> Distros = Globals.LabelStrips.FindAll(item => item.RackUnitType == RackType.Distro);
            List<LabelStrip> Dimmers = Globals.LabelStrips.FindAll(item => item.RackUnitType == RackType.Dimmer);

            foreach (var element in Distros)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Tag = element;
                treeViewItem.Header = "Rack: " + element.RackNumber;

                DistrosNode.Items.Add(treeViewItem);
                
            }

            foreach (var element in Dimmers)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Tag = element;
                treeViewItem.Header = "Rack: " + element.RackNumber;

                DimmersNode.Items.Add(treeViewItem);
            }

            RackSelector.Items.Add(DistrosNode);
            RackSelector.Items.Add(DimmersNode);
        }



        #region Event Handling
        void outline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ActiveLabelStrip.MakeSelection(sender,LabelCanvas);
        }

        void LabelCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled != true)
            {
                foreach (var element in LabelCanvas.Children)
                {
                    Border child = (Border)element;

                    child.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                ActiveLabelStrip.ClearSelections();
            }
        }

        void RackSelector_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = (TreeViewItem)e.NewValue;
            LabelStrip selectedLabelStrip = (LabelStrip)selectedItem.Tag;

            ActiveLabelStrip.ClearSelections();
            ActiveLabelStrip.LabelStrip = selectedLabelStrip;

            ForceRender();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in ActiveLabelStrip.SelectedHeaders)
            {
                Console.WriteLine(element.Data);
            }
        }

        void LabelEditor_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateRackLabelSelector();
            HeaderCellControl.SetTitle("Header Text");
            FooterTopCellControl.SetTitle("Footer Top Text");
            FooterMiddleCellControl.SetTitle("Footer Middle Text");
            FooterBottomCellControl.SetTitle("Footer Bottom Text");
        }

        private void MagnifyPlusButton_Click(object sender, RoutedEventArgs e)
        {
            LabelCanvasScaleTransform.ScaleX += 0.10;
            LabelCanvasScaleTransform.ScaleY += 0.10;
        }

        private void MagnifyMinusButton_Click(object sender, RoutedEventArgs e)
        {
            LabelCanvasScaleTransform.ScaleX -= 0.10;
            LabelCanvasScaleTransform.ScaleY -= 0.10;
        }

        void HeaderCellControl_PropertyChanged(object sender, EventArgs e)
        {
            int index = 0;

            // Update LabelStrip Values.
            foreach (var element in ActiveLabelStrip.SelectedHeaders)
            {
                ActiveLabelStrip.LabelStrip.UpdateHeaderData(HeaderCellControl.Data[index], element);
                element.Font = HeaderCellControl.Typefaces[index];
                element.FontSize = HeaderCellControl.FontSizes[index];

                index++;
            }

            ForceRender();
        }

        void FooterTopCellControl_PropertyChanged(object sender, EventArgs e)
        {
            int index = 0;

            // Update LabelStrip Values.
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.TopData = FooterTopCellControl.Data[index];
                element.TopFont = FooterTopCellControl.Typefaces[index];
                element.TopFontSize = FooterTopCellControl.FontSizes[index];

                index++;
            }

            ForceRender();
        }

        void FooterMiddleCellControl_PropertyChanged(object sender, EventArgs e)
        {
            int index = 0;

            // Update LabelStrip Values.
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.MiddleData = FooterMiddleCellControl.Data[index];
                element.MiddleFont = FooterMiddleCellControl.Typefaces[index];
                element.MiddleFontSize = FooterMiddleCellControl.FontSizes[index];

                index++;
            }

            ForceRender();
        }

        void FooterBottomCellControl_PropertyChanged(object sender, EventArgs e)
        {
            int index = 0;

            // Update LabelStrip Values.
            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                element.BottomData = FooterBottomCellControl.Data[index];
                element.BottomFont = FooterBottomCellControl.Typefaces[index];
                element.BottomFontSize = FooterBottomCellControl.FontSizes[index];

                index++;
            }

            ForceRender();
        }


        void ActiveLabelStrip_SelectedHeadersChanged(object sender, EventArgs e)
        {
            PopulateHeaderCellControls();
        }

        void ActiveLabelStrip_SelectedFootersChanged(object sender, EventArgs e)
        {
            PopulateFooterCellsControls();
        }

        #endregion
    }
}
