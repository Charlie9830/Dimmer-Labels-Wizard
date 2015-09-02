using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public class SplitCellViewModel : ViewModelBase
    {
        // Assigned Getters/Setters
        protected Border _Outline = new Border();
        protected LabelStrip _LabelStrip = new LabelStrip();
        protected ObservableCollection<Canvas> _RenderCanvas = new ObservableCollection<Canvas>();
        protected string _Text = string.Empty;

        protected ScaleTransform _ScaleTransform = new ScaleTransform();
        protected TranslateTransform _TranslateTransform = new TranslateTransform();
        protected TransformGroup _TransformGroup = new TransformGroup();

        #region InfoText
        protected string _InfoText = "Header Cells are automatically merged when their Text matches the text of " +
        "neighbouring cells. To split currently merged cells, Select the Subcells that you wish to change the text of above then enter " +
        "the new differing Text in the Textbox.";
        #endregion

        // No Public Access.
        protected Point _RenderOffset = new Point(5,5);
        protected List<Border> _SelectionOutlines = new List<Border>();
        protected ObservableCollection<Border> _SelectedOutlines = new ObservableCollection<Border>();

        private const double ItemsControlHeight = 150;
        private const double ItemsControlWidth = 880;

        public SplitCellViewModel()
        {
            _SelectedOutlines.CollectionChanged += _SelectedOutlines_CollectionChanged;

            // Transformations.
            _TransformGroup.Children.Add(_ScaleTransform);
            _TransformGroup.Children.Add(_TranslateTransform);

            _ScaleTransform.ScaleX = 1;
            _ScaleTransform.ScaleY = 1;
        }



        #region Getters/Setters
        public Border Outline
        {
            get
            {
                return _Outline;
            }
            set
            {
                _Outline = value;
                OnPropertyChanged("Outline");
            }
        }

        public LabelStrip LabelStrip
        {
            get
            {
                return _LabelStrip;
            }
            set
            {
                _LabelStrip = value;
            }
        }

        public ObservableCollection<Canvas> RenderCanvas
        {
            get
            {
                return _RenderCanvas;
            }
            set
            {
                _RenderCanvas = value;
                OnPropertyChanged("RenderCanvas");
            }
        }

        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                OnPropertyChanged("Text");
            }
        }

        public string InfoText
        {
            get
            {
                return _InfoText;
            }
        }

        public TransformGroup TransformGroup
        {
            get
            {
                return _TransformGroup;
            }

            set
            {
                _TransformGroup = value;
                OnPropertyChanged("TransformGroup");
            }
        }

        #endregion

        #region Update Methods
        public void Update()
        {
            foreach (var element in _SelectedOutlines)
            {
                HeaderCell headerCell = element.Tag as HeaderCell;
                headerCell.Data = _Text;
            }
        }

        #endregion

        #region Canvas Rendering Methods
        public void DrawToCanvas()
        {
            _RenderCanvas.Clear();
            _RenderCanvas.Add(new Canvas());
            _RenderCanvas.First().Width = _Outline.Width + 10;
            _RenderCanvas.First().Height = _Outline.Height + 10;
            _RenderCanvas.First().Margin = new Thickness(0, (150 / 2) - (_Outline.Height / 2), 0, 0);
            
            _RenderCanvas.First().Children.Add(_Outline);
            Canvas.SetTop(_Outline,5);
            Canvas.SetLeft(_Outline,5);

            DrawCellOutlines(_RenderCanvas.First());

            FitToCanvas();
            _RenderCanvas.First().RenderTransformOrigin = new Point(0.5,0.5);

            double scaleRatio = (ItemsControlWidth - 20) / _Outline.Width;

            _ScaleTransform.ScaleX *= scaleRatio;
            _ScaleTransform.ScaleY *= scaleRatio;

            double verticalScaleRatio = (ItemsControlHeight - 10) / (_Outline.Height * _ScaleTransform.ScaleX);

            if (verticalScaleRatio < 1)
            {
                _ScaleTransform.ScaleX *= verticalScaleRatio;
                _ScaleTransform.ScaleY *= verticalScaleRatio;
            }

            _RenderCanvas.First().RenderTransform = _TransformGroup;
            
            OnPropertyChanged("RenderCanvas");
        }

        public void DrawCellOutlines(Canvas canvas)
        {
            HeaderCellWrapper wrapper = Outline.Tag as HeaderCellWrapper;

            int cellQTY = wrapper.Cells.Count;

            double width = _LabelStrip.LabelWidthInMM * (96d / 25.4d);
            double height = Outline.Height;

            for (int count = 0; count < cellQTY; count++)
            {
                Border outline = new Border();
                outline.Width = width;
                outline.Height = height;
                outline.Background = new SolidColorBrush(Colors.Transparent);

                outline.BorderBrush = new SolidColorBrush(Colors.Black);
                outline.BorderThickness = new Thickness(1);

                // First Iteration.
                if (count == 0)
                {
                    Canvas.SetTop(outline,5);
                    Canvas.SetLeft(outline,5);
                }

                // Subsequent Iterations.
                else
                {
                    Canvas.SetTop(outline,5);
                    Canvas.SetLeft(outline, (width * count) + 5);
                }

                // Tag the Outline.
                outline.Tag = wrapper.Cells[count];

                _SelectionOutlines.Add(outline);
                canvas.Children.Add(outline);
            }

            HookupSelectionOutlineEvents();
        }

        void FitToCanvas()
        {
            // Center Objects.
            _TranslateTransform.X = 0;
            _TranslateTransform.Y = 0;

        }

        void HookupSelectionOutlineEvents()
        {
            foreach (var element in _SelectionOutlines)
            {
                element.MouseDown += SelectionOutline_MouseDown;
            }
        }


        #endregion

        #region Internal Event Handling
        void _SelectedOutlines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // If First time list has been Changed since Creation or Reset.
            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    // Deselect
                    Border outline = element as Border;
                    outline.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
            foreach (var element in _SelectedOutlines)
            {
                Border outline = element as Border;
                outline.BorderBrush = SystemColors.HighlightBrush;
            }
        }

        void SelectionOutline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_SelectedOutlines.Contains(sender as Border))
            {
                _SelectedOutlines.Remove(sender as Border);
            }

            else
            {
                _SelectedOutlines.Add(sender as Border);
            }

            e.Handled = true;
        }

        #endregion

    }
}
