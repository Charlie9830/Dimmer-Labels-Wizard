﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelCell : ContentControl, INotifyPropertyChanged
    {
        #region Fields.
        // Constants
        protected const double HeaderSingleLabelHeightDownFactor = 0.25d;
        protected const double FooterSingleLabelHeightDownFactor = 0.75d;
        protected const double UnitConversionRatio = 96d / 25.4d;

        // Properties.
        public DimmerDistroUnit PreviousReference { get; set; }

        protected SolidColorBrush _TextBrush;
        protected SolidColorBrush _Background;

        // Text Grid Rendering Elements.
        protected Grid _Grid = new Grid();
        #endregion

        #region Constructors
        /// <summary>
        /// Static Constructor. Property Metadata Overrides.
        /// </summary>
        static LabelCell()
        {
            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsArrange;
            options |= FrameworkPropertyMetadataOptions.AffectsMeasure;
            options |= FrameworkPropertyMetadataOptions.AffectsRender;
            options |= FrameworkPropertyMetadataOptions.AffectsParentArrange;
            options |= FrameworkPropertyMetadataOptions.AffectsParentMeasure;

            FrameworkPropertyMetadata heightPropertyMetadata =
                new FrameworkPropertyMetadata(69d, options, null, new CoerceValueCallback(CoerceHeight));

            FrameworkPropertyMetadata widthPropertyMetadata =
                new FrameworkPropertyMetadata(69d, options, null, new CoerceValueCallback(CoerceWidth));

            HeightProperty.OverrideMetadata(typeof(LabelCell), heightPropertyMetadata);
            WidthProperty.OverrideMetadata(typeof(LabelCell), widthPropertyMetadata);
        }

        /// <summary>
        /// Will Generate a LabelCell with a blank DimmerDistroUnit Data Model. Use for Examples and Testing.
        /// </summary>
        public LabelCell()
        {
            _Rows = new ObservableCollection<CellRow>();
            _Rows.CollectionChanged += Rows_CollectionChanged;

            // Assign Grid to Content Property.
            Content = _Grid;

            RealWidth = 16;
            RealHeight = 18;

            PreviousReference = new DimmerDistroUnit();

            // Unit Testing
            Grid.ShowGridLines = true;

            SnapsToDevicePixels = true;
        }

        /// <summary>
        /// Constructs a LabelCell with data Bound to dataReference parameter.
        /// </summary>
        /// <param name="dataReference"></param>
        public LabelCell(DimmerDistroUnit dataReference)
        {
            _Rows = new ObservableCollection<CellRow>();
            _Rows.CollectionChanged += Rows_CollectionChanged;

            // Assign Grid to Content Property.
            Content = _Grid;

            PreviousReference = dataReference;

            SnapsToDevicePixels = true;
        }
        #endregion

        #region CLR Properties
        /// <summary>
        /// The Grid in which CellRow's and their Child elements are Displayed.
        /// </summary>
        public Grid Grid
        {
            get { return _Grid; }
            set { _Grid = value; }
        }

        private ObservableCollection<CellRow> _Rows;

        /// <summary>
        /// Gets the a collection of CellRows currently inside the Cell.
        /// </summary>
        public ObservableCollection<CellRow> Rows
        {
            get { return _Rows; }
        }

        /// <summary>
        /// Gets a value indictating if Cascading Rows have been detected. 
        /// </summary>
        protected bool HasCascadingRows
        {
            get
            {
                return CellHasCascadingRows(Rows.ToList());
            }
        }

        #endregion

        #region Dependency Properties
        public bool IsMerged
        {
            get { return (bool)GetValue(IsMergedProperty); }
            set { SetValue(IsMergedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMerged.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMergedProperty =
            DependencyProperty.Register("IsMerged", typeof(bool), typeof(LabelCell), new FrameworkPropertyMetadata(false));


        public double LeftWeight
        {
            get { return (double)GetValue(LeftWeightProperty); }
            set { SetValue(LeftWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftWeightProperty =
            DependencyProperty.Register("LeftWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnLeftWeightPropertyChanged),
                    new CoerceValueCallback(CoerceLeftWeight)));

        private static object CoerceLeftWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnLeftWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;

            // Raise event to owner LabelStrip.
            OnPropertyChanged(instance, nameof(LeftWeight));
        }


        public double TopWeight
        {
            get { return (double)GetValue(TopWeightProperty); }
            set { SetValue(TopWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopWeightProperty =
            DependencyProperty.Register("TopWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d,new PropertyChangedCallback(OnTopWeightPropertyChanged),
                    new CoerceValueCallback(CoerceTopWeight)));

        private static object CoerceTopWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnTopWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;

            // Raise event to owner LabelStrip.
            OnPropertyChanged(instance, nameof(TopWeight));
        }



        public double RightWeight
        {
            get { return (double)GetValue(RightWeightProperty); }
            set { SetValue(RightWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightWeightProperty =
            DependencyProperty.Register("RightWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnRightWeightPropertyChanged),
                    new CoerceValueCallback(CoerceRightWeight)));

        private static object CoerceRightWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnRightWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;

            // Raise event to owner LabelStrip.
            OnPropertyChanged(instance, nameof(RightWeight));
        }


        public double BottomWeight
        {
            get { return (double)GetValue(BottomWeightProperty); }
            set { SetValue(BottomWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomWeightProperty =
            DependencyProperty.Register("BottomWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnBottomWeightPropertyChanged),
                    new CoerceValueCallback(CoerceBottomWeight)));

        private static object CoerceBottomWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnBottomWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;

            // Raise event to owner LabelStrip.
            OnPropertyChanged(instance, nameof(BottomWeight));
        }



        public double ActualLeftWeight
        {
            get { return (double)GetValue(ActualLeftWeightProperty); }
            set { SetValue(ActualLeftWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualLeftWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualLeftWeightProperty =
            DependencyProperty.Register("ActualLeftWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, GetLineWeightMetadataOptions(), new PropertyChangedCallback(OnActualLeftWeightPropertyChanged),
                    new CoerceValueCallback(CoerceActualLeftWeight)));

        private static object CoerceActualLeftWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnActualLeftWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }



        public double ActualTopWeight
        {
            get { return (double)GetValue(ActualTopWeightProperty); }
            set { SetValue(ActualTopWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualTopWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualTopWeightProperty =
            DependencyProperty.Register("ActualTopWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, GetLineWeightMetadataOptions(), new PropertyChangedCallback(OnActualTopWeightPropertyChanged),
                    new CoerceValueCallback(CoerceActualTopWeight)));

        private static object CoerceActualTopWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnActualTopWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }



        public double ActualRightWeight
        {
            get { return (double)GetValue(ActualRightWeightProperty); }
            set { SetValue(ActualRightWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualRightWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualRightWeightProperty =
            DependencyProperty.Register("ActualRightWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, GetLineWeightMetadataOptions(), new PropertyChangedCallback(OnActualRightWeightPropertyChanged),
                    new CoerceValueCallback(CoerceActualRightWeight)));

        private static object CoerceActualRightWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnActualRightWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }



        public double ActualBottomWeight
        {
            get { return (double)GetValue(ActualBottomWeightProperty); }
            set { SetValue(ActualBottomWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualBottomWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualBottomWeightProperty =
            DependencyProperty.Register("ActualBottomWeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(1d, GetLineWeightMetadataOptions(), new PropertyChangedCallback(OnActualBottomWeightPropertyChanged),
                    new CoerceValueCallback(CoerceActualBottomWeight)));

        private static object CoerceActualBottomWeight(DependencyObject d, object value)
        {
            double lineWeight = (double)value;

            if (lineWeight < 0)
            {
                return 0;
            }

            else
            {
                return lineWeight;
            }
        }

        private static void OnActualBottomWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Gets or Sets the Width of the Cell in Millimetres. This is a Dependency Property.
        /// </summary>
        public double RealWidth
        {
            get { return (double)GetValue(RealWidthProperty); }
            set { SetValue(RealWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealWidthProperty =
            DependencyProperty.Register("RealWidth", typeof(double),
                typeof(LabelCell), new FrameworkPropertyMetadata(0d, 
                    new PropertyChangedCallback(OnRealWidthPropertyChanged)));

        private static void OnRealWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;

            if (newValue != oldValue)
            {
                instance.Width = newValue * UnitConversionRatio;
            }
        }

        /// <summary>
        /// Gets or Sets the Height of the Cell in Millimetres. This is a Dependency Property.
        /// </summary>
        public double RealHeight
        {
            get { return (double)GetValue(RealHeightProperty); }
            set { SetValue(RealHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealHeightProperty =
            DependencyProperty.Register("RealHeight", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnRealHeightPropertyChanged)));

        private static void OnRealHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;

            if (newValue != oldValue)
            {
                instance.Height = newValue * UnitConversionRatio;
            }
        }

        public CellRowHeightMode RowHeightMode
        {
            get { return (CellRowHeightMode)GetValue(RowHeightModeProperty); }
            set { SetValue(RowHeightModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeightMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightModeProperty =
            DependencyProperty.Register("RowHeightMode", typeof(CellRowHeightMode), typeof(LabelCell),
                new FrameworkPropertyMetadata(CellRowHeightMode.Static,
                    new PropertyChangedCallback(OnRowHeightModePropertyChanged)));

        private static void OnRowHeightModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var newValue = (CellRowHeightMode)e.NewValue;
            var oldValue = (CellRowHeightMode)e.OldValue;
            ObservableCollection<CellRow> rows = instance.Rows;
            int rowQty = rows.Count;
            double cellHeight = instance.Height;
            double staticHeight = cellHeight / rowQty;

            if (newValue != oldValue)
            {
                switch (newValue)
                {
                    case CellRowHeightMode.Static:
                        foreach (var element in rows)
                        {
                            element.HeightMode = newValue;
                            element.RowHeight = staticHeight;
                        }
                        break;
                    case CellRowHeightMode.Automatic:
                        foreach (var element in rows)
                        {
                            element.HeightMode = newValue;
                        }
                        break;
                    case CellRowHeightMode.Manual:
                        foreach (var element in rows)
                        {
                            element.HeightMode = newValue;
                            // Throw Exception. Need some way of Translating Users Selection of Row Percentages
                            // to the actual Rows Here. Perhaps a List of Values generated during setup Dialog.
                            throw new NotImplementedException("Feature not fully Implemented Yet.");
                        }
                        break;
                }
            }
        }

        public CellDataMode CellDataMode
        {
            get { return (CellDataMode)GetValue(CellDataModeProperty); }
            set { SetValue(CellDataModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellDataMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellDataModeProperty =
            DependencyProperty.Register("CellDataMode", typeof(CellDataMode),
                typeof(LabelCell), new FrameworkPropertyMetadata(CellDataMode.MixedField,
                    new PropertyChangedCallback(OnCellDataModePropertyChanged)));

        private static void OnCellDataModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var oldValue = (CellDataMode)e.OldValue;
            var newValue = (CellDataMode)e.NewValue;
            ObservableCollection<CellRow> rows = instance.Rows;

            if (newValue == CellDataMode.MixedField)
            {
                foreach (var element in rows)
                {
                    instance.SetRowData(element, instance);
                }
            }

            if (newValue == CellDataMode.SingleField)
            {
                // Cell Data Mode has been Toggled to Single Field Mode. Re Initialize Cell in Single
                // Field Mode.
                rows.Clear();

                rows.Add(new CellRow(instance));
                rows.Last().DataField = instance.SingleFieldDataField;
                rows.Last().Data = instance.SingleFieldData;

                // Initialize to SingleField Mode.
                instance.RowHeightMode = CellRowHeightMode.Static;
            }
           
        }
        #endregion

        #region PropertyMetadata Overides.
        private static object CoerceWidth(DependencyObject d, object value)
        {
            var instance = d as LabelCell;
            var width = (double)value;

            return width;
        }

        /// <summary>
        /// Coerces derived Height Property based on value of LabelMode and LabelStripVerticalPosition.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object CoerceHeight(DependencyObject d, object value)
        {
            var instance = d as LabelCell;
            var height = (double)value;

            return height;
        }



        public string SingleFieldData
        {
            get { return (string)GetValue(SingleFieldDataProperty); }
            set { SetValue(SingleFieldDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleFieldData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleFieldDataProperty =
            DependencyProperty.Register("SingleFieldData", typeof(string), typeof(LabelCell),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnSingleFieldDataPropertyChanged)));

        private static void OnSingleFieldDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            string newData = (string)e.NewValue;
            ObservableCollection<CellRow> rowCollection = instance.Rows;
            LabelField dataField = instance.SingleFieldDataField;

            CurateRowQty(instance, newData);
            rowCollection.First().DataField = dataField;
            rowCollection.First().Data = newData;
        }

        public LabelField SingleFieldDataField
        {
            get { return (LabelField)GetValue(SingleFieldDataFieldProperty); }
            set { SetValue(SingleFieldDataFieldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleFieldDataFieldProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleFieldDataFieldProperty =
            DependencyProperty.Register("SingleFieldDataField", typeof(LabelField), typeof(LabelCell),
                new FrameworkPropertyMetadata(LabelField.NoAssignment, new PropertyChangedCallback(OnSingleFieldDataFieldPropertyChanged)));

        private static void OnSingleFieldDataFieldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            LabelField newDataField = (LabelField)e.NewValue;
            DimmerDistroUnit reference = instance.PreviousReference;

            instance.SingleFieldData = reference.GetData(newDataField);
        }


        public Typeface SingleFieldFont
        {
            get { return (Typeface)GetValue(SingleFieldFontProperty); }
            set { SetValue(SingleFieldFontProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleFieldFont.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleFieldFontProperty =
            DependencyProperty.Register("SingleFieldFont", typeof(Typeface), typeof(LabelCell),
                new FrameworkPropertyMetadata(new Typeface("Arial"), new PropertyChangedCallback(OnSingleFieldFontPropertyChanged)));

        private static void OnSingleFieldFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            ObservableCollection<CellRow> rowCollection = instance.Rows;
            Typeface newFont = e.NewValue as Typeface;

            foreach (var element in rowCollection)
            {
                element.Font = newFont;
            }
        }


        public double SingleFieldDesiredFontSize
        {
            get { return (double)GetValue(SingleFieldDesiredFontSizeProperty); }
            set { SetValue(SingleFieldDesiredFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleFieldDesiredFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleFieldDesiredFontSizeProperty =
            DependencyProperty.Register("SingleFieldDesiredFontSize", typeof(double),
                typeof(LabelCell), new FrameworkPropertyMetadata(12d, new PropertyChangedCallback(OnSingleFieldDesiredFontSizePropertyChanged)));

        private static void OnSingleFieldDesiredFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            double newDesiredFontSize = (double)e.NewValue;
            ObservableCollection<CellRow> rowCollection = instance.Rows;

            rowCollection.First().DesiredFontSize = newDesiredFontSize;
            instance.SingleFieldActualFontSize = rowCollection.First().ActualFontSize;
        }

        public double SingleFieldActualFontSize
        {
            get { return (double)GetValue(SingleFieldActualFontSizeProperty); }
            set { SetValue(SingleFieldActualFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleFieldActualFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleFieldActualFontSizeProperty =
            DependencyProperty.Register("SingleFieldActualFontSize", typeof(double), typeof(LabelCell), 
                new FrameworkPropertyMetadata(12d, new PropertyChangedCallback(OnSingleFieldActualFontSizePropertyChanged)));

        private static void OnSingleFieldActualFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region Overrides
        protected override void OnRender(DrawingContext drawingContext)
        {
            #region TextGrid Rendering.

            // Setup Grid.
            _Grid.Width = Width;
            _Grid.Height = Height;

            #endregion

            #region Outline Rendering
            // Declare Resources.
            var lineBrush = new SolidColorBrush(Colors.Black);

            // Pens.
            var leftPen = new Pen(lineBrush, ActualLeftWeight);
            var topPen = new Pen(lineBrush, ActualTopWeight);
            var rightPen = new Pen(lineBrush, ActualRightWeight);
            var bottomPen = new Pen(lineBrush, ActualBottomWeight);

            // Debugging.
            //FormattedText left = new FormattedText(ActualLeftWeight.ToString(), CultureInfo.CurrentCulture,
            //    FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Red);

            //FormattedText top = new FormattedText(ActualTopWeight.ToString(), CultureInfo.CurrentCulture,
            //    FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Red);

            //FormattedText right = new FormattedText(ActualRightWeight.ToString(), CultureInfo.CurrentCulture,
            //    FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Red);

            //FormattedText bottom = new FormattedText(ActualBottomWeight.ToString(), CultureInfo.CurrentCulture,
            //    FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Red);

            // Corner Points.
            var topLeft = new Point(0 + (LeftWeight / 4), 0 + (TopWeight / 4));
            var topRight = new Point(Width - (RightWeight / 4), 0 + (TopWeight / 4));
            var bottomRight = new Point(Width - (RightWeight / 4), Height - (BottomWeight / 4));
            var bottomLeft = new Point(0 + (LeftWeight / 4), Height - (BottomWeight / 4));

            // Drawing 
            drawingContext.DrawLine(leftPen, bottomLeft, topLeft);
            drawingContext.DrawLine(topPen, topLeft, topRight);
            drawingContext.DrawLine(rightPen, topRight, bottomRight);
            drawingContext.DrawLine(bottomPen, bottomRight, bottomLeft);

            // Debugging
            //drawingContext.DrawText(left, new Point(topLeft.X, topLeft.Y + (Height / 2)));
            //drawingContext.DrawText(top, new Point(topLeft.X + (Width / 2), topLeft.Y));
            //drawingContext.DrawText(right, new Point(topRight.X, topRight.Y + (Height / 2)));
            //drawingContext.DrawText(bottom, new Point(bottomLeft.X + (Width / 2), bottomLeft.Y));

            #endregion

        }
        #endregion

        #region Event Handlers
        private void Rows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<CellRow>;
            // Keep Grid.RowDefinitions explicitly linked to Rows Collection.
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cellRow = element as CellRow;
                    Grid.RowDefinitions.Insert(e.NewStartingIndex, cellRow);
                    cellRow.PropertyChanged += CellRow_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cellRow = element as CellRow;
                    Grid.RowDefinitions.Remove(cellRow);
                    Grid.Children.Remove(cellRow.TextBlock);
                    cellRow.PropertyChanged -= CellRow_PropertyChanged;
                }
            }


            if (e.OldItems == null && e.NewItems == null && collection.Count == 0)
            {
                // Collection has Been Cleared.
                Grid.RowDefinitions.Clear();
                Grid.Children.Clear();
            }

            // Update Row Indexes and Coerce RowHeights.
            int rowIndexCounter = 0;
            foreach (var element in collection)
            {
                element.Index = rowIndexCounter;
                rowIndexCounter++;
                element.CoerceValue(CellRow.RowHeightProperty);
            }

            // Update Cascading State.
            SetCascadingRows(collection.ToList());

            // Push Cascaded Data to new Rows.
            if (collection.Any(item => item.IsCascading == true))
            {
                if (e.NewItems != null)
                {
                    foreach (var element in e.NewItems)
                    {
                        var cellRow = element as CellRow;

                        if (cellRow.IsCascading == true)
                        {
                            cellRow.Data = cellRow.CascadingRows.First().Data;
                        }
                    }
                }
            }

            // Refresh Data Layouts.
            foreach (var element in collection)
            {
                AssignDataLayouts(element);
            }
        }

        private void CellRow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var cellRow = sender as CellRow;
            string propertyName = e.PropertyName;

            // Data
            if (propertyName == CellRow.DataProperty.Name)
            {
                if (cellRow.IsCascading == true)
                {
                    // Push Data Change to other CascadingRows in group.
                    foreach (var element in cellRow.CascadingRows)
                    {
                        if (element != cellRow)
                        {
                            element.Data = cellRow.Data;
                        }
                    }
                }

                // Update Model.
                PreviousReference.SetData(cellRow.Data, cellRow.DataField);

                // Assign Data Layout(s).
                AssignDataLayouts(cellRow);
            }

            // DataField.
            if (propertyName == CellRow.DataFieldProperty.Name)
            {
                // Collect new Data.
                cellRow.Data = PreviousReference.GetData(cellRow.DataField);

                // Update Cascading State.
                SetCascadingRows(Rows.ToList());
            }

            // Data Layout
            if (propertyName == CellRow.DataLayoutProperty.Name)
            {
                AssignDataLayouts(cellRow);
            }
        }
        #endregion.

        #region Methods.
        /// <summary>
        /// Sets the Data Property of a single Child Row based of it's current assigned Data Field. Should only be
        /// used whilst Cell is in Mixed Field Mode.
        /// </summary>
        /// <param name="cellRow"></param>
        /// <param name="cellParent"></param>
        protected void SetRowData(CellRow cellRow, LabelCell cellParent)
        {
            DimmerDistroUnit reference = cellParent.PreviousReference;
            CellDataMode cellDataMode = cellParent.CellDataMode;
            LabelField dataField = cellRow.DataField;

            if (cellDataMode == CellDataMode.MixedField)
            {
                cellRow.Data = reference.GetData(cellRow.DataField);
            }
        }

        /// <summary>
        /// Edits the length of the RowCollection of the given LabelCell Instance. Use only when in SingleField Mode.
        /// </summary>
        /// <param name="currentRow"></param>
        private static void CurateRowQty(LabelCell instance, string data)
        {
            if (instance.CellDataMode != CellDataMode.SingleField)
            {
                throw new FormatException("Given Instance of Labelcell is not in Single Field Mode");
            }

            ObservableCollection<CellRow> rows = instance.Rows;
            LabelCell cellParent = instance;
            LabelField dataField = instance.SingleFieldDataField;
            char delimiter = ' ';
            int rowQty = rows.Count;
            int dataQty = data.Split(delimiter).Length;
            int qtyDifference = rowQty - dataQty;

            if (qtyDifference < 0)
            {
                // Row Deficit. Create new rows.
                int count = Math.Abs(qtyDifference);

                while (count > 0)
                {
                    rows.Add(new CellRow(cellParent));
                    count--;
                }
            }

            if (qtyDifference > 0)
            {
                // Data Deficit. Remove excess Rows.
                int count = Math.Abs(qtyDifference);

                while (count > 0)
                {
                    rows.Remove(rows.Last());
                    count--;
                }
            }
        }

        /// <summary>
        /// Sets and Unsets Cascading row flags and collections on objects residing in Rows collection.
        /// </summary>
        private void SetCascadingRows(List<CellRow> rowCollection)
        {
            // Assign CellRow.IsCascading flags and CascadingRows Collections.
            List<List<CellRow>> cascadingRows = GetCascadedRows(rowCollection);

            if (cascadingRows.Count > 0)
            {
                // Assign.
                foreach (var list in cascadingRows)
                {
                    foreach (var element in list)
                    {
                        element.IsCascading = true;
                        element.CascadingRows = list;
                    }
                }

                // Flatten cascadingRows.
                IEnumerable<CellRow> flattenedRows = cascadingRows.SelectMany(x => x);
                IEnumerable<CellRow> unAssignRows = rowCollection.Where(x => flattenedRows.Contains(x) == false);

                if (unAssignRows != null)
                {
                    foreach (var element in unAssignRows)
                    {
                        element.IsCascading = false;
                        element.CascadingRows.Clear();
                    }
                }
            }

            else
            {
                // No Cascading rows found. UnAssign any remaining Cascading Flagged rows.
                foreach (var element in rowCollection)
                {
                    if (element.IsCascading == true)
                    {
                        element.IsCascading = false;
                        element.CascadingRows.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Assigns Data Layout(s) and acompany properties to the provided object.
        /// </summary>
        /// <param name="targetRow"></param>
        private void AssignDataLayouts(CellRow targetRow)
        {
            string data = targetRow.Data;
            double desiredFontSize = targetRow.DesiredFontSize;
            double actualFontSize = desiredFontSize;
            Typeface font = targetRow.Font;
            double availableTextWidth = targetRow.AvailableTextWidth;
            double availableTextHeight = targetRow.AvailableTextHeight;

            if (targetRow.IsCascading == false)
            {
                // Standard Row.
                // Update DataLayout.
                ScaleDirection ignore;
                actualFontSize = ScaleDownFontSize(data, font, desiredFontSize,
                    availableTextWidth, availableTextHeight, ScaleDirection.Both, out ignore);

                DataLayout dataLayout = new DataLayout(0, data.Length, data, font, actualFontSize);

                targetRow.DataLayout = dataLayout;
                targetRow.ActualFontSize = actualFontSize;
            }

            else
            {
                char delimiter = ' ';
                List<string> dataElements = data.Split(delimiter).ToList();
                List<CellRow> cascadingRows = targetRow.CascadingRows;
                int cascadedRowCount = cascadingRows.Count;
                List<DataLayout> dataLayouts = new List<DataLayout>();
                ScaleDirection ignore;
                List<double> scaledFontSizes = new List<double>();

                if (dataElements.Count > cascadedRowCount)
                {
                    // More Data Elements then Cascading Rows. Concatentate Data.
                    while(dataElements.Count > cascadedRowCount)
                    {
                        if (dataElements.Count > 1)
                        {
                            dataElements[dataElements.Count - 2] += delimiter + dataElements.Last();
                            dataElements.RemoveAt(dataElements.IndexOf(dataElements.Last()));
                        }

                        else
                        {
                            break;
                        }
                    }

                }

                // Generate Data Layouts.
                int startIndex = 0;
                for (int rowIndex = 0; rowIndex < cascadedRowCount; rowIndex++)
                {
                    if (rowIndex < dataElements.Count)
                    {
                        // Populated Row.
                        dataLayouts.Add(new DataLayout(startIndex, dataElements[rowIndex].Length,
                            data, font, desiredFontSize));

                        startIndex += dataElements[rowIndex].Length + 1;
                    }

                    else
                    {
                        // Blank Row.
                        dataLayouts.Add(new DataLayout(0, 0, data, font, desiredFontSize));
                    }
                }

                // Scale Font Sizes.
                for (int index = 0; index < dataLayouts.Count && index < cascadedRowCount; index++)
                {
                    string displayedData = dataLayouts[index].DisplayedText;
                    double containerWidth = cascadingRows[index].AvailableTextWidth;
                    double containerHeight = cascadingRows[index].AvailableTextHeight;

                    scaledFontSizes.Add(ScaleDownFontSize(displayedData, font, desiredFontSize,
                        containerWidth, containerHeight, ScaleDirection.Both, out ignore));
                }

                // Select smallest Font.
                scaledFontSizes.Sort();
                actualFontSize = scaledFontSizes.First();

                // Assign Data Layouts to Cells.
                int listIndex = 0;
                foreach (var element in cascadingRows)
                {
                    element.DataLayout = dataLayouts[listIndex];
                    element.ActualFontSize = actualFontSize;
                    listIndex++;
                }
                
            }
        }

        /// <summary>
        /// Detects whether any rows provided in the parameter are setup as Cascading rows.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        protected bool CellHasCascadingRows(IList<CellRow> rows)
        {
            // Determine if any Consecutive rows have matching DataFields.
            var groupedElements = rows.GroupBy(item => item.DataField);

            foreach (var group in groupedElements)
            {
                if (group.Count() > 1)
                {
                    CellRow reference = group.First();

                    foreach (var item in group)
                    {
                        if (item != reference && rows.IndexOf(item) - 1 == rows.IndexOf(reference))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;    
        }

        /// <summary>
        /// Returns a List of Lists of CellRow's that are setup as Cascading Rows.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<List<CellRow>> GetCascadedRows(List<CellRow> rows)
        {
            List<List<CellRow>> returnList = new List<List<CellRow>>();
            
            // Group by DataField.
            var groupedElements = rows.GroupBy(item => item.DataField);

            foreach (var group in groupedElements)
            {
                if (group.Count() > 1)
                {
                    returnList.Add(new List<CellRow>());

                    // Order by Index Number.
                    group.OrderBy(item => item.Index);

                    int referenceIndex = group.First().Index;
                    int counter = 0;

                    // Add Elements if they have consecutive Index Numbers.
                    foreach (var item in group)
                    {
                        if (item.Index - counter == referenceIndex)
                        {
                            returnList.Last().Add(item);
                            counter++;
                        }
                    }
                }
            }
            return returnList;
        }

        /// <summary>
        /// Measures the size of provided text. Returns Size 0 if a null or Empty string was provided.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static Size MeasureText(string data, Typeface font, double fontSize)
        {
            if (data == null || data == string.Empty)
            {
                return new Size(0, 0);
            }

            FormattedText formatter = new FormattedText(data, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                font, fontSize, Brushes.Black);

            return new Size(formatter.Width, formatter.Height);
        }

        /// <summary>
        /// Scales Font Size Down if Required, and rounds to nearest
        /// quarter. If not Required, will return Font Size untouched.
        /// Out parameter fontScaledDirection informs caller in which Axis font scaling was required
        /// the most, Ignore out parameter if scaleDirection was not set to Both.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="scaleDirection"></param>
        /// <returns></returns>
        public static double ScaleDownFontSize(string data, Typeface font, double fontSize,
            double containerWidth, double containerHeight, ScaleDirection scaleDirection,
            out ScaleDirection fontScaledDirection)
        {
            Size textSize = MeasureText(data, font, fontSize);

            switch (scaleDirection)
            {
                // Scale Down by Horizontal Axis only.
                case ScaleDirection.Horizontal:
                    fontScaledDirection = ScaleDirection.Horizontal;

                    if (textSize.Width > containerWidth)
                    {
                        double ratio = containerWidth / textSize.Width;

                        // Round and Return.
                        return Math.Round((fontSize * ratio) * 4, MidpointRounding.AwayFromZero) / 4;
                    }

                    else
                    {
                        // Round and Return.
                        return Math.Round(fontSize * 4, MidpointRounding.AwayFromZero) / 4;
                    }


                // Scale Down by Vertical axis only.
                case ScaleDirection.Vertical:
                    fontScaledDirection = ScaleDirection.Vertical;
                    if (textSize.Height > containerHeight)
                    {
                        double ratio = containerHeight / textSize.Height;

                        // Round and Return.
                        return Math.Round((fontSize * ratio) * 4, MidpointRounding.AwayFromZero) / 4;
                    }
                    else
                    {
                        // Round and Return.
                        return Math.Round(fontSize * 4, MidpointRounding.AwayFromZero) / 4;
                    }


                // Scale down by both Horizontal and Vertical axis.
                case ScaleDirection.Both:
                    double widthRatio = 1;
                    double heightRatio = 1;

                    // Calculate Ratios.
                    if (textSize.Width > containerWidth)
                    {
                        widthRatio = containerWidth / textSize.Width;
                    }

                    if (textSize.Height > containerHeight)
                    {
                        heightRatio = containerHeight / textSize.Height;
                    }

                    // Scale FontSize by largest Ratio. (Furthest from 1).
                    if (widthRatio < heightRatio)
                    {
                        fontScaledDirection = ScaleDirection.Horizontal;

                        // Round and Return.
                        return Math.Round((fontSize * widthRatio) * 4, MidpointRounding.AwayFromZero) / 4;
                    }

                    if (heightRatio > widthRatio)
                    {
                        fontScaledDirection = ScaleDirection.Vertical;

                        // Round and Return.
                        return Math.Round((fontSize * heightRatio) * 4, MidpointRounding.AwayFromZero) / 4;
                    }

                    // Ratio's are either Identical or both have been left at 1.
                    else
                    {
                        fontScaledDirection = ScaleDirection.Both;

                        // Round and Return.
                        return Math.Round((fontSize * widthRatio) * 4, MidpointRounding.AwayFromZero) / 4;
                    }

                default:
                    fontScaledDirection = ScaleDirection.Both;

                    // Round and Return.
                    return Math.Round(fontSize * 4, MidpointRounding.AwayFromZero) / 4;
            }

        }
        #endregion

        #region Static Methods
        protected static FrameworkPropertyMetadataOptions GetLineWeightMetadataOptions()
        {
            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            options |= FrameworkPropertyMetadataOptions.AffectsMeasure;
            options |= FrameworkPropertyMetadataOptions.AffectsArrange;

            return options;
        }
        #endregion

        #region Interface Implementations
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, eventArgs);
            }
        }

        private static void OnPropertyChanged(LabelCell instance, string propertyName)
        {
            if (instance.PropertyChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                instance.PropertyChanged(instance, eventArgs);
            }
        }
        #endregion

        #region Serialization Methods
        public void RebuildLabelCell(LabelCellStorage storage)
        {
            ByteColor textColor = storage.TextColor;
            ByteColor backgroundColor = storage.BackgroundColor;

            _TextBrush = new SolidColorBrush(storage.TextColor.ToColor());
            _Background = new SolidColorBrush(storage.BackgroundColor.ToColor());
        }

        public LabelCellStorage GenerateLabelCellStorage()
        {
            LabelCellStorage storage = new LabelCellStorage();

            storage.TextColor = new ByteColor(_TextBrush.Color);
            storage.BackgroundColor = new ByteColor(_Background.Color);

            storage.PreviousReference = PreviousReference;

            return storage;
        }
        #endregion

        #region Methods for Inhertitated Classes
        public static Typeface RebuildFont(string fontFamilyName, int openTypeFontWeight, string fontStyle)
        {
            FontFamily fontFamily = new FontFamily(fontFamilyName);
            FontWeight fontWeight = FontWeight.FromOpenTypeWeight(openTypeFontWeight);

            FontStyle style = new FontStyle();
            switch (fontStyle)
            {
                case "Normal":
                    style = FontStyles.Normal;
                    break;
                case "Italic":
                    style = FontStyles.Italic;
                    break;
                case "Oblique":
                    style = FontStyles.Oblique;
                    break;
                default:
                    style = FontStyles.Normal;
                    break;
            }

            return new Typeface(fontFamily, style, fontWeight, new FontStretch());
        }

        #endregion
    }
   
    public class LabelCellStorage
    {
        public DimmerDistroUnit PreviousReference;

        public ByteColor TextColor;
        public ByteColor BackgroundColor;
    }

    
    public struct ByteColor
    {
        public ByteColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public ByteColor(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;      
        }

        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}
