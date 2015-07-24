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

namespace Dimmer_Labels_Wizard
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
        protected double _ItemsControlWidth = 0;
        protected double _ItemsControlHeight = 0;

        #region InfoText
        protected string _InfoText = "Header Cells are automatically merged when their Text matches the text of it's " +
        "neighbouring cells. To split merged cells, Select the cells that you wish to change the text of above then enter " +
        "the new Text in the Textbox.";
        #endregion

        // No Public Access.
        protected Point _RenderOffset = new Point(20, 20);
        protected List<Border> _SelectionOutlines = new List<Border>();
        protected ObservableCollection<Border> _SelectedOutlines = new ObservableCollection<Border>();

        public SplitCellViewModel()
        {
            _SelectedOutlines.CollectionChanged += _SelectedOutlines_CollectionChanged;

            // Transformations.
            _TransformGroup.Children.Add(_ScaleTransform);
            _TransformGroup.Children.Add(_TranslateTransform);
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

        public double ItemsControlWidth
        {
            get
            {
                return _ItemsControlWidth;
            }
            set
            {
                _ItemsControlWidth = value;
                OnPropertyChanged("ItemsControlWidth");
            }
        }

        public double ItemsControlHeight
        {
            get
            {
                return _ItemsControlHeight;
            }
            set
            {
                _ItemsControlHeight = value;
                OnPropertyChanged("ItemsControlHeight");
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
            _RenderCanvas.First().Children.Add(_Outline);
            Canvas.SetTop(_Outline, _RenderOffset.Y);
            Canvas.SetLeft(_Outline, _RenderOffset.X);

            DrawCellOutlines(_RenderCanvas.First());

            FitToCanvas();

            OnPropertyChanged("RenderCanvas");
        }

        public void DrawCellOutlines(Canvas canvas)
        {
            HeaderCellWrapper wrapper = Outline.Tag as HeaderCellWrapper;

            int cellQTY = wrapper.Cells.Count;

            double width = _LabelStrip.LabelWidthInMM * (96d / 25.4d);
            double height = Outline.Height; //_LabelStrip.LabelHeightInMM * (96d / 25.4d);

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
                    Canvas.SetTop(outline, _RenderOffset.Y);
                    Canvas.SetLeft(outline, _RenderOffset.X);
                }

                // Subsequent Iterations.
                else
                {
                    Canvas.SetTop(outline, _RenderOffset.Y);
                    Canvas.SetLeft(outline, (width * count) + _RenderOffset.X);
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
