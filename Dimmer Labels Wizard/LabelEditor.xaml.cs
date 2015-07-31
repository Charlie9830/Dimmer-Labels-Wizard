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
using System.Windows.Forms.Integration;

namespace Dimmer_Labels_Wizard
{
    /// <summary>
    /// Interaction logic for LabelEditor.xaml
    /// </summary>
    public partial class LabelEditor : Window
    {
        // Label Selection
        public LabelStripSelection ActiveLabelStrip = new LabelStripSelection();

        // View Controls
        private TransformGroup LabelCanvasTransformGroup = new TransformGroup();
        private ScaleTransform LabelCanvasScaleTransform = new ScaleTransform();
        private TranslateTransform LabelCanvasTranslateTransform = new TranslateTransform();

        // Drag Selection
        private Point MouseDownLocation;
        private const double DragThreshold = 10;
        private bool isDragging = false;
        private List<Border> selectedOutlines = new List<Border>();

        public LabelEditor()
        {
            InitializeComponent();

            // View Setup
            CanvasBorder.ClipToBounds = true;

            LabelCanvasScaleTransform.ScaleX = 1;
            LabelCanvasScaleTransform.ScaleY = 1;

            LabelCanvasTranslateTransform.X = 0;
            LabelCanvasTranslateTransform.Y = 0;

            LabelCanvasTransformGroup.Children.Add(LabelCanvasScaleTransform);
            LabelCanvasTransformGroup.Children.Add(LabelCanvasTranslateTransform);

            LabelCanvas.RenderTransform = LabelCanvasTransformGroup;

            this.Loaded += LabelEditor_Loaded;
            RackSelector.SelectedItemChanged += RackSelector_SelectedItemChanged;

            CanvasBorder.MouseDown += CanvasBorder_MouseDown;
            CanvasBorder.MouseMove += CanvasBorder_MouseMove;
            CanvasBorder.MouseUp += CanvasBorder_MouseUp;

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
                ActiveLabelStrip.LabelStrip.RenderToDisplay(LabelCanvas, new Point(20, 20),UserParameters.SingleLabel);
                CollectSelectionEvents();

            }
        }

        void PopulateHeaderCellControls()
        {
            HeaderCellControl.HeaderViewModel.HeaderCells.Clear();

            foreach (var element in ActiveLabelStrip.SelectedHeaders)
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

            ColorControl.ViewModel.ActiveLabelStrip = ActiveLabelStrip.LabelStrip;
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

        protected Border GetSplitCellOutline(HeaderCell headerCell)
        {
            foreach (var element in LabelCanvas.Children)
            {
                Border outline = (Border)element;
                if (outline.Tag.GetType() == typeof(HeaderCellWrapper))
                {
                    HeaderCellWrapper wrapper = (HeaderCellWrapper)outline.Tag;

                    foreach (var cell in wrapper.Cells)
                    {
                        if (headerCell == cell)
                        {
                            return outline;
                        }
                    }
                }
            }

            return null;
        }

        private void InitiateDragSelection(Point pointA, Point pointB)
        {
            if (isDragging == false)
            {
                isDragging = true;
                DragCanvas.Visibility = Visibility.Visible;
            }
            
            else
            {
                UpdateDragSelectionBorder(pointA, pointB);
            }
            
        }

        private void UpdateDragSelectionBorder(Point pointA, Point pointB)
        {
            double x;
            double y;
            double width;
            double height;

            if (pointB.X < pointA.X)
            {
                x = pointB.X;
                width = pointA.X - pointB.X;
            }
            else
            {
                x = pointA.X;
                width = pointB.X - pointA.X;
            }

            if (pointB.Y < pointA.Y)
            {
                y = pointB.Y;
                height = pointA.Y - pointB.Y;
            }
            else
            {
                y = pointA.Y;
                height = pointB.Y - pointA.Y;
            }

            Canvas.SetLeft(DragRectangle, x);
            Canvas.SetTop(DragRectangle, y);

            DragRectangle.Width = width;
            DragRectangle.Height = height;

        }

        private void CompleteDragSelection()
        {
            DragCanvas.Visibility = Visibility.Collapsed;
            isDragging = false;

            GeometryHitTestParameters geometryHitTest = new GeometryHitTestParameters(DragRectangle.RenderedGeometry);
            Console.WriteLine("Hit Test Starting");
            VisualTreeHelper.HitTest(LabelAreaGrid, null, new HitTestResultCallback(ResultCallBack), geometryHitTest);
            Console.WriteLine("Hit Test Complete");
            foreach (var element in selectedOutlines)
            {
                ActiveLabelStrip.MakeSelection(element, LabelCanvas);
            }
            Console.WriteLine("Selection Complete");

            selectedOutlines.Clear();
        }

        private HitTestResultBehavior ResultCallBack(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(Border))
            {
                selectedOutlines.Add(result.VisualHit as Border);
            }

            return HitTestResultBehavior.Continue;
        }

        #region Event Handling

        void InvokePrint(object sender, RoutedEventArgs e)
        {
            PrintWindow printWindow = new PrintWindow();
            ElementHost.EnableModelessKeyboardInterop(printWindow);
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

        void CanvasBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled != true)
            {
                MouseDownLocation = e.GetPosition(CanvasBorder);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    ActiveLabelStrip.ClearSelections();
                }
            }
        }

        private void CanvasBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentLocation = e.GetPosition(CanvasBorder);

                var dragDelta = currentLocation - MouseDownLocation;
                double dragDistance = Math.Abs(dragDelta.Length);

                if (dragDistance > DragThreshold)
                {
                    InitiateDragSelection(MouseDownLocation,currentLocation);
                }
            }
        }

        private void CanvasBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (isDragging == true)
                {
                    CompleteDragSelection();
                }
            }
        }


        void RackSelector_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = (TreeViewItem)e.NewValue;
            LabelStrip selectedLabelStrip = (LabelStrip)selectedItem.Tag;

            ActiveLabelStrip.ClearSelections();
            ActiveLabelStrip.SetSelectedLabelStrip(selectedLabelStrip, LabelCanvas);

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

            ActiveLabelStrip.RefreshAdorners();
        }

        private void MagnifyMinusButton_Click(object sender, RoutedEventArgs e)
        {
            LabelCanvasScaleTransform.ScaleX -= 0.10;
            LabelCanvasScaleTransform.ScaleY -= 0.10;

            ActiveLabelStrip.RefreshAdorners();
        }

        private void CenterViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveLabelStrip.LabelStrip != null)
            {
                LabelCanvasTranslateTransform.X = 0;
                LabelCanvasTranslateTransform.Y = 0;

                ActiveLabelStrip.RefreshAdorners();
            }
        }

        private void MagnifyToFitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveLabelStrip.LabelStrip != null)
            {
                double xRatio = CanvasBorder.Width / (LabelCanvas.Width * LabelCanvasScaleTransform.ScaleX);
                LabelCanvasScaleTransform.ScaleX *= xRatio;
                LabelCanvasScaleTransform.ScaleY *= xRatio;

                ActiveLabelStrip.RefreshAdorners();
            }
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

        private void SplitCellsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveLabelStrip.LabelStrip != null &&
                ActiveLabelStrip.SelectedHeaders.Count > 0)
            {
                SplitCellWindow SplitCellDialog = new SplitCellWindow();
                SplitCellDialog.ViewModel.LabelStrip = ActiveLabelStrip.LabelStrip;
                Border outline = GetSplitCellOutline(ActiveLabelStrip.SelectedHeaders.First());

                if (outline != null)
                {
                    LabelCanvas.Children.Remove(outline);
                    outline.MouseDown -= outline_MouseDown;
                    SplitCellDialog.ViewModel.Outline = outline;
                    SplitCellDialog.ShowDialog();

                    ForceRender();

                }
            }
        }
        #endregion


    }
}
