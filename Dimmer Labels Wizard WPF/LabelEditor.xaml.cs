using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        // Selection Mode
        private CellSelectionMode SelectionMode = CellSelectionMode.Cell;

        // Drag Selection
        private Point MouseDownLocation;
        private const double DragThreshold = 5;
        private bool isDragging = false;
        private List<Border> selectedOutlines = new List<Border>();
        private List<TextBlock> selectedTextBlocks = new List<TextBlock>();

        // Popup UI
        private CellControl CellControl = new CellControl();
        private bool IsCellControlVisible = false;
        private AdornerLayer MainGridAdornerLayer;
        private PopUpControlAdorner PopUpAdorner;

        // Dialog First Time Tracking
        private bool GlobalApplyWarningShowAgain = true;

        // Exit Message Handling.
        private bool BackButtonPressed = false;

        public LabelEditor()
        {
            InitializeComponent();

            // View Setup
            CanvasBorder.ClipToBounds = true;

            // Transformation Setup
            LabelCanvasScaleTransform.ScaleX = 1;
            LabelCanvasScaleTransform.ScaleY = 1;

            LabelCanvasTranslateTransform.X = 0;
            LabelCanvasTranslateTransform.Y = 0;

            LabelCanvasTransformGroup.Children.Add(LabelCanvasScaleTransform);
            LabelCanvasTransformGroup.Children.Add(LabelCanvasTranslateTransform);

            LabelCanvas.RenderTransform = LabelCanvasTransformGroup;

            // PopUp UI Setup.
            MainGridAdornerLayer = AdornerLayer.GetAdornerLayer(MainGrid);
            PopUpAdorner = new PopUpControlAdorner(MainGrid);
            PopUpAdorner.Content = CellControl;

            // Event Hookups.
            Loaded += LabelEditor_Loaded;
            Closing += LabelEditor_Closing;

            CellControl.RenderRequested += CellControl_RenderRequested;

            RackSelector.SelectedItemChanged += RackSelector_SelectedItemChanged;

            CanvasBorder.MouseDown += CanvasBorder_MouseDown;
            CanvasBorder.MouseMove += CanvasBorder_MouseMove;
            CanvasBorder.MouseUp += CanvasBorder_MouseUp;

            ActiveLabelStrip.SelectedHeadersChanged += ActiveLabelStrip_SelectedHeadersChanged;
            ActiveLabelStrip.SelectedFootersChanged += ActiveLabelStrip_SelectedFootersChanged;
            ActiveLabelStrip.SelectedFooterCellTextChanged += ActiveLabelStrip_SelectedFooterCellTextChanged;
            ActiveLabelStrip.SelectedHeaderCellTextChanged += ActiveLabelStrip_SelectedHeaderCellTextChanged;
        }

        void ForceRender()
        {
            if (ActiveLabelStrip.LabelStrip != null)
            {
                LabelCanvas.Children.Clear();
                // Offset Point is given in WPF Pixels (Inches).
                ActiveLabelStrip.LabelStrip.RenderToDisplay(LabelCanvas, new Point(20, 20),UserParameters.SingleLabel,
                    ActiveLabelStrip.SelectedHeaderCellText, ActiveLabelStrip.SelectedFooterCellText);
                ActiveLabelStrip.ReAttachAdorners(LabelCanvas, SelectionMode);
                CollectSelectionEvents(SelectionMode);
            }
        }

        void CollectSelectionEvents(CellSelectionMode selectionMode)
        {
            foreach (var element in LabelCanvas.Children)
            {
                Border outline = (Border)element;

                if (selectionMode == CellSelectionMode.Cell)
                {
                    // Connect incoming Events.
                    outline.MouseDown += outline_MouseDown;
                    outline.MouseUp += outline_MouseUp;

                    // Disconnect Outgoing Events.
                    var textCanvas = outline.Child as Canvas;
                    foreach (var child in textCanvas.Children)
                    {
                        if (child is TextBlock)
                        {
                            var textBlock = child as TextBlock;
                            textBlock.MouseDown -= TextBlock_MouseDown;
                            textBlock.MouseUp -= TextBlock_MouseUp;
                        }
                    }
                }

                if (selectionMode == CellSelectionMode.Text)
                {
                    // Connect incoming Events.
                    var textCanvas = outline.Child as Canvas;
                    foreach (var child in textCanvas.Children)
                    {
                        if (child is TextBlock)
                        {
                            var textBlock = child as TextBlock;
                            textBlock.MouseDown += TextBlock_MouseDown;
                            textBlock.MouseUp += TextBlock_MouseUp;
                        }
                    }

                    // Disconnect Outgoing Events.
                    outline.MouseDown -= outline_MouseDown;
                    outline.MouseUp -= outline_MouseUp;
                }
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

        private void ClearRackLabelSelector()
        {
            RackSelector.Items.Clear();
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

        private void ReloadLabelEditor()
        {
            ClearRackLabelSelector();
            PopulateRackLabelSelector();
            LabelCanvas.Children.Clear();
        }

        private void InitiateSave()
        {
            Persistance persitance = new Persistance();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".dlw";
            saveFileDialog.Filter = "Dimmer Labels Wizard File (*.dlw) | *.dlw";
            saveFileDialog.FilterIndex = 0;

            if (saveFileDialog.ShowDialog() == true)
            {
                persitance.SaveToFile(saveFileDialog.FileName);
            }
        }

        private void InitiateLoad()
        {
            string message = "Are you sure you want to Load? All unsaved changes will be lost.";
            string caption = "Load from File";

            if (MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Persistance persistance = new Persistance();

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".dlw";
                openFileDialog.Filter = "Dimmer Labels Wizard File (*.dlw) | *.dlw";
                openFileDialog.FilterIndex = 0;

                if (openFileDialog.ShowDialog() == true)
                {
                    persistance.LoadFromFile(openFileDialog.FileName);
                }

                ReloadLabelEditor();
            }
        }

        #region Drag Selection Handling
        private void InitiateDragSelection(Point pointA, Point pointB)
        {
            if (isDragging == false)
            {
                isDragging = true;

                if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
                {
                    ActiveLabelStrip.ClearSelections();
                }
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

            // Generate a Rect Struct to Add to create a RectangleGeometry Object.
            double x = Canvas.GetLeft(DragRectangle);
            double y = Canvas.GetTop(DragRectangle);
            double width = DragRectangle.Width;
            double height = DragRectangle.Height;
            Rect dragRect = new Rect(x, y, width, height);

            GeometryHitTestParameters geometryHitTest = new GeometryHitTestParameters(new RectangleGeometry(dragRect));

            VisualTreeHelper.HitTest(LabelAreaGrid, null, new HitTestResultCallback(ResultCallBack), geometryHitTest);

            if (SelectionMode == CellSelectionMode.Cell)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    // Additive Selection.
                    ActiveLabelStrip.MakeSelections(selectedOutlines);
                }
                else
                {
                    // Exclusive Selection.
                    ActiveLabelStrip.ClearSelections();
                    ActiveLabelStrip.MakeSelections(selectedOutlines);
                }
                
                selectedOutlines.Clear();
            }

            if (SelectionMode == CellSelectionMode.Text)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    // Additive Selection.
                    ActiveLabelStrip.MakeSelections(selectedTextBlocks);
                }
                else
                {
                    // Exclusive Selection.
                    ActiveLabelStrip.ClearSelections();
                    ActiveLabelStrip.MakeSelections(selectedTextBlocks);
                }

                selectedTextBlocks.Clear();
            }
            
            DragRectangle.Width = 0;
            DragRectangle.Height = 0;
        }

        private HitTestResultBehavior ResultCallBack(HitTestResult result)
        {
            if (SelectionMode == CellSelectionMode.Cell)
            {
                if (result.VisualHit.GetType() == typeof(Border))
                {
                    selectedOutlines.Add(result.VisualHit as Border);
                }
            }

            else
            {
                if (result.VisualHit is TextBlock)
                {
                    selectedTextBlocks.Add(result.VisualHit as TextBlock);
                }
            }

            return HitTestResultBehavior.Continue;
        }
        #endregion

        #region Pop Up UI Handling
        protected void TogglePopUpUI(Point mouseLocation)
        {
            if (IsCellControlVisible == false)
            {
                LoadCellControl();
                CellControl.Margin = new Thickness(mouseLocation.X, mouseLocation.Y, 0, 0);
                MainGridAdornerLayer.Add(PopUpAdorner);
                IsCellControlVisible = true;
            }

            else
            {
                MainGridAdornerLayer.Remove(PopUpAdorner);
                ClearCellControl();
                IsCellControlVisible = false;
            }
        } 

        protected void LoadCellControl()
        {
            CellControl.LoadControl(ActiveLabelStrip.SelectedFooterCellText,
                ActiveLabelStrip.SelectedHeaderCellText);
        }

        protected void ClearCellControl()
        {
            CellControl.ClearControl();
        }
        #endregion

        #region Event Handling

        private void LabelEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (BackButtonPressed != true)
            {
                string message = "Any unsaved changes will be lost. Are you sure you want to Exit?";
                string caption = "Exit";

                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MouseDownLocation = e.GetPosition(CanvasBorder);
            }
        }

        void outline_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.ChangedButton == MouseButton.Left)
            {
                if (isDragging == false)
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        // Additive Selection.
                        ActiveLabelStrip.MakeSelection(sender as Border);
                    }

                    else
                    {
                        // Exclusive Selection.
                        ActiveLabelStrip.ClearSelections();
                        ActiveLabelStrip.MakeSelection(sender as Border);
                    }
                    
                }

                if (isDragging == true)
                {
                    CompleteDragSelection();
                }
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SelectionMode == CellSelectionMode.Text)
            {
                e.Handled = true;
                if (isDragging == false)
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        // Additive Selection.
                        ActiveLabelStrip.MakeSelection(sender as TextBlock);
                    }

                    else
                    {
                        // Exclusive Selection.
                        ActiveLabelStrip.ClearSelections();
                        ActiveLabelStrip.MakeSelection(sender as TextBlock);
                    }
                    
                }

                if (isDragging == true)
                {
                    CompleteDragSelection();
                }
            }
        }

        void CanvasBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MouseDownLocation = e.GetPosition(CanvasBorder);
            }
            
        }

        private void CanvasBorder_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentLocation = e.GetPosition(CanvasBorder);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var dragDelta = currentLocation - MouseDownLocation;
                double dragDistance = Math.Abs(dragDelta.Length);

                if (dragDistance > DragThreshold)
                {
                    InitiateDragSelection(MouseDownLocation,currentLocation);
                }
            }

            if (e.LeftButton == MouseButtonState.Released && isDragging == true)
            {
                UpdateDragSelectionBorder(MouseDownLocation, currentLocation);
            }
        }

        private void CanvasBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (isDragging == true)
                {
                    CompleteDragSelection();
                }

                else
                {
                    ActiveLabelStrip.ClearSelections();
                }
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                TogglePopUpUI(e.GetPosition(MainGrid));
                
            }
        }

        void RackSelector_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                TreeViewItem selectedItem = (TreeViewItem)e.NewValue;
                LabelStrip selectedLabelStrip = (LabelStrip)selectedItem.Tag;

                ActiveLabelStrip.ClearSelections();
                ActiveLabelStrip.SetSelectedLabelStrip(selectedLabelStrip, LabelCanvas);

                ForceRender();
            }
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
        }

        void LabelEditor_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateRackLabelSelector();
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

        private void CellControl_RenderRequested(object sender, EventArgs e)
        {
            ForceRender();
        }

        void ActiveLabelStrip_SelectedHeadersChanged(object sender, EventArgs e)
        {
        }

        void ActiveLabelStrip_SelectedFootersChanged(object sender, EventArgs e)
        {
        }

        private void ActiveLabelStrip_SelectedFooterCellTextChanged(object sender, EventArgs e)
        {
            // This will change when new Popup UI is Implemented. Just needs to not Throw an exception at Event Declaration
            // right now.
        }

        private void ActiveLabelStrip_SelectedHeaderCellTextChanged(object sender, EventArgs e)
        {
            // This will change when new Popup UI is Implemented. Just needs to not Throw an exception at Event Declaration
            // right now.
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

            else
            {
                string message = "You must select automatically merged Header cells before you can split them.";
                MessageBox.Show(message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CellControl_GlobalApplySelected(object sender, EventArgs e)
        {
            if (GlobalApplyWarningShowAgain == true)
            {
                GlobalApplyWarning globalApplyDialog = new GlobalApplyWarning();

                if (globalApplyDialog.ShowDialog() == true)
                {
                    GlobalApplyWarningShowAgain = (bool)!globalApplyDialog.DontShowDialogAgain;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            InitiateSave();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            InitiateLoad();
        }

        private void SelectionModeToggle_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;

            if (toggle.IsChecked == true)
            {
                SelectionMode = CellSelectionMode.Text;
            }

            else
            {
                SelectionMode = CellSelectionMode.Cell;
            }

            ActiveLabelStrip.ClearSelections();
            CollectSelectionEvents(SelectionMode);
        }

        #endregion

    }
}
