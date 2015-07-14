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
using Xceed.Wpf.Toolkit;

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

            ActiveLabelStrip.SelectedHeadersChanged += ActiveLabelStrip_SelectedHeadersChanged;
            ActiveLabelStrip.SelectedFootersChanged += ActiveLabelStrip_SelectedFootersChanged;

            HeaderCellControl.HeaderViewModel.RenderRequested += Control_RenderRequested;
            FooterTopCellControl.FooterTopViewModel.RenderRequested += Control_RenderRequested;
            FooterMiddleCellControl.FooterMiddleViewModel.RenderRequested += Control_RenderRequested;
            FooterBottomCellControl.FooterBottomViewModel.RenderRequested += Control_RenderRequested;

            ColorControl.ViewModel.RenderRequested += Control_RenderRequested;

            // Set Data Contexts for Cell Control Binding.
            HeaderCellControl.DataContext = HeaderCellControl.HeaderViewModel;
            FooterTopCellControl.DataContext = FooterTopCellControl.FooterTopViewModel;
            FooterMiddleCellControl.DataContext = FooterMiddleCellControl.FooterMiddleViewModel;
            FooterBottomCellControl.DataContext = FooterBottomCellControl.FooterBottomViewModel;
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
            HeaderCellControl.HeaderViewModel.HeaderCells.Clear();

            foreach(var element in ActiveLabelStrip.SelectedHeaders)
            {
                HeaderCellControl.HeaderViewModel.HeaderCells.Add(element);
            }
        }

        void PopulateFooterCellsControls()
        {
            FooterTopCellControl.FooterTopViewModel.FooterCells.Clear();
            FooterMiddleCellControl.FooterMiddleViewModel.FooterCells.Clear();
            FooterBottomCellControl.FooterBottomViewModel.FooterCells.Clear();

            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                FooterTopCellControl.FooterTopViewModel.FooterCells.Add(element);
                FooterMiddleCellControl.FooterMiddleViewModel.FooterCells.Add(element);
                FooterBottomCellControl.FooterBottomViewModel.FooterCells.Add(element);
            }
        }

        void PopulateColorControl()
        {
            ColorControl.ViewModel.SelectedHeaderCells.Clear();
            ColorControl.ViewModel.SelectedFooterCells.Clear();

            foreach (var element in ActiveLabelStrip.SelectedHeaders)
            {
                ColorControl.ViewModel.SelectedHeaderCells.Add(element);
            }

            foreach (var element in ActiveLabelStrip.SelectedFooters)
            {
                ColorControl.ViewModel.SelectedFooterCells.Add(element);
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

        void InvokePrint(object sender, RoutedEventArgs e)
        {
            PrintWindow printWindow = new PrintWindow();
            printWindow.Show();
        }

        void Control_RenderRequested(object sender, EventArgs e)
        {
            ForceRender();
        }


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
        }

        void LabelEditor_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateRackLabelSelector();
            HeaderCellControl.HeaderViewModel.SetTitle("Header Text");

            FooterTopCellControl.FooterTopViewModel.SetTitle("Footer Top Text");
            FooterMiddleCellControl.FooterMiddleViewModel.SetTitle("Footer Middle Text");
            FooterBottomCellControl.FooterBottomViewModel.SetTitle("Footer Bottom Text");

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

        void ActiveLabelStrip_SelectedHeadersChanged(object sender, EventArgs e)
        {
            PopulateHeaderCellControls();
            PopulateColorControl();
        }

        void ActiveLabelStrip_SelectedFootersChanged(object sender, EventArgs e)
        {
            PopulateFooterCellsControls();
            PopulateColorControl();
        }

        #endregion
    }
}
