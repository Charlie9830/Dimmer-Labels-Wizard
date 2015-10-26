using System;
using System.Collections.Generic;
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

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelCell : ContentControl
    {
        #region Fields.
        // Constants
        protected const double HeaderSingleLabelHeightDownFactor = 0.25d;
        protected const double FooterSingleLabelHeightDownFactor = 0.75d;
        protected const double UnitConversionRatio = 96d / 25.4d;

        // Properties.
        public DimmerDistroUnit PreviousReference { get; set; }

        protected NeighbourCells _Neighbours;

        protected SolidColorBrush _TextBrush;
        protected SolidColorBrush _Background;
        protected double _LeftWeight = 2;
        protected double _TopWeight = 2;
        protected double _RightWeight = 2;
        protected double _BottomWeight = 2;

        // Text Grid Rendering Elements.
        protected Grid _Grid = new Grid();
        #endregion

        #region Constructors
        /// <summary>
        /// Static Constructor. Property Metadata Overrides.
        /// </summary>
        static LabelCell()
        {
            FrameworkPropertyMetadataOptions heightOptions = FrameworkPropertyMetadataOptions.AffectsArrange;
            heightOptions |= FrameworkPropertyMetadataOptions.AffectsMeasure;
            heightOptions |= FrameworkPropertyMetadataOptions.AffectsRender;
            heightOptions |= FrameworkPropertyMetadataOptions.AffectsParentArrange;
            heightOptions |= FrameworkPropertyMetadataOptions.AffectsParentMeasure;

            FrameworkPropertyMetadata heightPropertyMetadata =
                new FrameworkPropertyMetadata(69d, heightOptions, null, new CoerceValueCallback(CoerceHeight));

            HeightProperty.OverrideMetadata(typeof(LabelCell), heightPropertyMetadata);
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
        }
        #endregion

        #region CLR Properties - General and Mixed Mode Field Properties.
        /// <summary>
        /// The Grid in which CellRow's and their Child elements are Displayed.
        /// </summary>
        public Grid Grid
        {
            get { return _Grid; }
            set { _Grid = value; }
        }

        public SolidColorBrush TextBrush
        {
            get
            {
                return _TextBrush;
            }

            protected set     // Set By BackgroundColor Setter
            {
                _TextBrush = value;
            }
        }

        public new SolidColorBrush Background
        {
            get
            {
                return _Background;
            }

            set
            {
                if (_Background == null || _Background.Color != value.Color)
                {
                    // Calculate Luminance of Color and set _textColor to White or Black based on this luminance result.
                    if ((0.299 * value.Color.R) + (0.587 * value.Color.G) + (0.114 * value.Color.B) > 128)
                    {
                        _TextBrush = new SolidColorBrush(Colors.Black);
                    }

                    else
                    {
                        _TextBrush = new SolidColorBrush(Colors.White);
                    }

                    _Background = value;
                    InvalidateVisual();
                }
            }
        }

        public NeighbourCells Neighbours
        {
            get
            {
                if (_Neighbours == null)
                {
                    throw new NullReferenceException("LabelCell.Neighbours is null");
                }

                else
                {
                    return _Neighbours;
                }
            }
            set
            {
                _Neighbours = value;
            }
        }

        public double LeftWeight
        {
            get
            {
                return _LeftWeight;
            }
            set
            {   if (_LeftWeight != value)
                {
                    _LeftWeight = value;
                    InvalidateVisual();
                }
                
            }
        }

        public double TopWeight
        {
            get
            {
                return _TopWeight;
            }
            set
            {
                if (_TopWeight != value)
                {
                    _TopWeight = value;
                    InvalidateVisual();
                }
            }
        }

        public double RightWeight
        {
            get
            {
                return _RightWeight;
            }
            set
            {
                if (_RightWeight != value)
                {
                    _RightWeight = value;
                    InvalidateVisual();
                }
            }
        }

        public double BottomWeight
        {
            get
            {
                return _BottomWeight;
            }
            set
            {
                if (_BottomWeight != value)
                {
                    _BottomWeight = value;
                    InvalidateVisual();
                }
            }
        }

        private ObservableCollection<CellRow> _Rows;

        /// <summary>
        /// Gets the a collection of CellRows currently inside the Cell.
        /// </summary>
        public ObservableCollection<CellRow> Rows
        {
            get { return _Rows; }
        }

        private bool _IsSettingData;

        /// <summary>
        /// True if this Cell is currently Assigning Data to Child Row Elements.
        /// </summary>
        public bool IsSettingData
        {
            get { return _IsSettingData; }
            protected set { _IsSettingData = value; }
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

        #region CLR Properties - Single Field Mode Properties.
        protected LabelField _SFcellDataField = LabelField.NoAssignment;
        public LabelField SFcellDataField
        {
            get
            {
                return _SFcellDataField;
            }
            set
            {
                if (value != _SFcellDataField)
                {
                    SFcellData = PreviousReference.GetData(value);
                    SFcellDataField = value;
                }
            } 
        }

        protected string _SFcellData = string.Empty;
        public string SFcellData
        {
            get
            {
                return _SFcellData;
            }
            set
            {
                if (value != SFcellData)
                {
                    // Send Change to Rows.
                    double availableTextWidth = Width - LeftWeight - RightWeight;
                    double availableTextHeight = Height - BottomWeight - TopWeight;

                    // Assign to Rows, collect and Update adjusted Font Size.
                    Rows.Clear();
                    List<CellRow> newRows = AssignToChildren(value, SFfont, SFfontSize, null,
                        availableTextWidth, availableTextHeight, SFcellDataField, out _SFfontSize);

                    foreach (var element in newRows)
                    {
                        Rows.Add(element);
                    }

                    // Assign value to Data Model.
                    PreviousReference.SetData(value, SFcellDataField);

                    // Store Value
                    _SFcellData = value;
                }
            }
        }

        protected Typeface _SFfont = new Typeface("Arial");
        public Typeface SFfont
        {
            get
            {
                return _SFfont;
            }
            set
            {
                if (value != _SFfont)
                {
                    // Send Change to Rows.
                    double availableTextWidth = Width - LeftWeight - RightWeight;
                    double availableTextHeight = Height - BottomWeight - TopWeight;

                    // Assign to Rows, collect and Update adjusted Font Size.
                    Rows.Clear();
                    List<CellRow> newRows = AssignToChildren(SFcellData, value, SFfontSize, null,
                        availableTextWidth, availableTextHeight, SFcellDataField, out _SFfontSize);

                    foreach (var element in newRows)
                    {
                        Rows.Add(element);
                    }

                    // Store Value
                    _SFfont = value;
                }
            }
        }

        protected double _SFfontSize = 12d;
        public double SFfontSize
        {
            get
            {
                return _SFfontSize;
            }

            set
            {
                // Assign to Rows, collect and Update adjusted Font Size.
                double availableTextWidth = Width - LeftWeight - RightWeight;
                double availableTextHeight = Height - BottomWeight - TopWeight;
                double adjustedFontSize;

                Rows.Clear();
                List<CellRow> newRows = AssignToChildren(SFcellData, SFfont, value, null,
                    availableTextWidth, availableTextHeight, SFcellDataField, out adjustedFontSize);

                foreach (var element in newRows)
                {
                    Rows.Add(element);
                }

                // Store Values.
                _SFfontSize = adjustedFontSize;
                
            }
        }

        #endregion

        #region Dependency Properties
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

        public LabelStripMode LabelStripMode
        {
            get { return (LabelStripMode)GetValue(LabelStripModeProperty); }
            set { SetValue(LabelStripModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelStripMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelStripModeProperty =
            DependencyProperty.Register("LabelStripMode", typeof(LabelStripMode), typeof(LabelCell),
                new FrameworkPropertyMetadata(LabelStripMode.Double,
                    new PropertyChangedCallback(OnLabelStripModePropertyChanged)));

        private static void OnLabelStripModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            double realHeight = instance.RealHeight;

            // Push Change to Height Property.
            CoerceHeight(d, realHeight);
        }

        public LabelStripVerticalPosition VerticalPosition
        {
            get { return (LabelStripVerticalPosition)GetValue(VerticalPositionProperty); }
            set { SetValue(VerticalPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerticalPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalPositionProperty =
            DependencyProperty.Register("VerticalPosition", typeof(LabelStripVerticalPosition),
                typeof(LabelCell), new FrameworkPropertyMetadata(LabelStripVerticalPosition.Header,
                    new PropertyChangedCallback(OnVerticalPositionPropertyChanged)));

        private static void OnVerticalPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            double realHeight = instance.RealHeight;

            // Push Change to Height Property.
            CoerceHeight(d, realHeight);
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
                // Cell Data Mode has been Toggled to Single Field Mode. Delete existing Rows
                // in preperation for new Rows being Generated.
                rows.Clear();

                // Initialize to SingleField Mode.
                instance.RowHeightMode = CellRowHeightMode.Static;
            }
           
        }

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

        #endregion

        #region PropertyMetadata Overides.
        /// <summary>
        /// Coerces derived Height Property based on value of LabelMode and LabelStripVerticalPosition.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object CoerceHeight(DependencyObject d, object value)
        {
            var instance = d as LabelCell;
            LabelStripMode mode = instance.LabelStripMode;
            LabelStripVerticalPosition verticalPosition = instance.VerticalPosition;

            // If LabelCell is in Single Label Strip Mode. Downsize Height Accordingly.
            if (mode == LabelStripMode.Single)
            {
                if (verticalPosition == LabelStripVerticalPosition.Header)
                {
                    return (double)value * HeaderSingleLabelHeightDownFactor;
                }

                if (verticalPosition == LabelStripVerticalPosition.Footer)
                {
                    return (double)value * FooterSingleLabelHeightDownFactor;
                }

                else
                {
                    return (double)value;
                }
            }

            if (mode == LabelStripMode.Double)
            {
                return (double)value;
            }

            return (double)value;
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
            var leftPen = new Pen(lineBrush, _LeftWeight);
            var topPen = new Pen(lineBrush, _TopWeight);
            var rightPen = new Pen(lineBrush, _RightWeight);
            var bottomPen = new Pen(lineBrush, _BottomWeight);

            // Corner Points.
            var topLeft = new Point(0, 0);
            var topRight = new Point(Width, 0);
            var bottomRight = new Point(Width, Height);
            var bottomLeft = new Point(0, Height);

            // Drawing 
            drawingContext.DrawLine(leftPen, bottomLeft, topLeft);
            drawingContext.DrawLine(topPen, topLeft, topRight);
            drawingContext.DrawLine(rightPen, topRight, bottomRight);
            drawingContext.DrawLine(bottomPen, bottomRight, bottomLeft);

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
                    Grid.RowDefinitions.Add(cellRow);
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
        }

        private void CellRow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var cellRow = sender as CellRow;
            string propertyName = e.PropertyName;

            // DataField.
            if (propertyName == CellRow.DataFieldProperty.Name)
            {
                // Set Cell Data.
                SetRowData(cellRow, this);
            }

            // Data.
            if (propertyName == CellRow.DataProperty.Name)
            {
                Console.WriteLine("Data is Being Set");
                // Signal that this Cell is currently setting Data.
                IsSettingData = true;

                if (HasCascadingRows == true)
                {
                    List<List<CellRow>> cascadedRows = GetCascadedRows(Rows.ToList());
                    
                    foreach (var list in cascadedRows)
                    {
                        if (list.Contains(cellRow))
                        {
                            string data = string.Empty;
                            Typeface font = list.First().Font;
                            double fontSize = list.First().FontSize;
                            LabelField dataField = list.First().DataField;
                            double availableTextWidth = list.First().AvailableTextWidth;
                            double availableTextHeight = 0d;
                            double ignoreFontSize;
                            
                            foreach (var element in list)
                            {
                                // Concatenate Data.
                                data += element.Data + " ";

                                // Sum available Text Height.
                                availableTextHeight += element.AvailableTextHeight;
                            }

                            Console.WriteLine("Data = " + data);
                            // Assign data to Children.
                            AssignToChildren(data, font, fontSize, list, availableTextWidth, availableTextHeight,
                                dataField, out ignoreFontSize);
                        }
                    }
                }

                else
                {
                    // No Cascading Rows.
                    PreviousReference.SetData(cellRow.Data, cellRow.DataField);
                }

                IsSettingData = false;
            }
        }
        #endregion.

        #region Methods.
        /// <summary>
        /// Populates and intializes new Rows or modifies Existing Rows with data provided.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <param name="existingRows"></param>
        /// <param name="availableTextWidth"></param>
        /// <param name="availableTextHeight"></param>
        /// <param name="dataField"></param>
        /// <param name="newFonSize"></param>
        /// <returns></returns>
        protected List<CellRow> AssignToChildren(string data, Typeface font, double fontSize, List<CellRow> existingRows,
            double availableTextWidth, double availableTextHeight, LabelField dataField, out double adjustedFontSize)
        {
            char stringDelimiter = ' ';
            List<CellRow> returnList = new List<CellRow>();
            double oldFontSize = fontSize;
            ScaleDirection ignore;

            // Validate Parameters.
            if (existingRows != null)
            {
                if (existingRows.Count == 0)
                {
                    throw new FormatException("existingRows.Count cannot be 0");
                }
                CellRow referenceRow = existingRows.First();
                foreach (var element in existingRows)
                {
                    if (element.DataField != referenceRow.DataField)
                    {
                        throw new FormatException("Not all DataField's of existingRows Match");
                    }
                }
            }

            // Determine scaled font size (if any).
            ScaleDirection fontScaledDirection;
            double newFontSize = CellRow.HelperScaleDownFont(data, font, fontSize,
                availableTextWidth, availableTextHeight, ScaleDirection.Both, out fontScaledDirection);

            if (oldFontSize == newFontSize)
            {
                // No Font Size Scaling Required.
                if (existingRows == null)
                {
                    // Generate new Row.
                    string[] dataArray = { data };

                    adjustedFontSize = oldFontSize;
                    return HelperPopulateRows(this, null, dataArray, font, oldFontSize, dataField);
                }

                else
                {
                    // Use Existing Rows.
                    double scaledFontSize = CellRow.HelperScaleDownFont(data, font, fontSize,
                        availableTextWidth, HelperGetShortestRow(existingRows).Height.Value,
                        ScaleDirection.Vertical, out ignore);

                    string[] dataArray = { data };

                    adjustedFontSize = scaledFontSize;
                    return HelperPopulateRows(this, existingRows, dataArray, font, scaledFontSize, dataField);
                }
            }

            else
            {
                // Font is too large.
                string[] splitStrings = data.Split(stringDelimiter);

                if (splitStrings.Length > 1)
                {
                    // String can be Split.
                    int longestStringIndex = HelperGetLongestStringIndex(splitStrings, font, fontSize);

                    // Scale Font Size Further if needed.
                    double scaledFontSize = CellRow.HelperScaleDownFont(splitStrings[longestStringIndex], font, fontSize,
                        availableTextWidth, availableTextHeight / splitStrings.Length, ScaleDirection.Both, out ignore);

                    if (existingRows == null)
                    {
                        // Generate new Rows.
                        adjustedFontSize = scaledFontSize;
                        return HelperPopulateRows(this, null, splitStrings, font, scaledFontSize, dataField);
                    }

                    else
                    {
                        // Can existing rows be used?
                        double heightScaledFontSize = CellRow.HelperScaleDownFont(data, font, fontSize,
                        availableTextWidth, HelperGetShortestRow(existingRows).Height.Value,
                        ScaleDirection.Vertical, out ignore);

                        if (splitStrings.Length <= existingRows.Count)
                        {
                            // Use all existing Rows or Use available rows and assign Blank data to remaining Rows.
                            adjustedFontSize = heightScaledFontSize;
                            return HelperPopulateRows(this, existingRows, splitStrings, font, heightScaledFontSize, dataField);
                        }

                        else
                        {
                            // Reprocess String to Fit into Avaiable Rows. 
                            double textLayoutFontSizeReturn;
                            string[] scaledStrings = HelperScaleTextLayout(splitStrings, existingRows, font,
                                heightScaledFontSize, out textLayoutFontSizeReturn);

                            adjustedFontSize = textLayoutFontSizeReturn;
                            return HelperPopulateRows(this, existingRows, scaledStrings, font, textLayoutFontSizeReturn, dataField);
                        }
                    }
                }

                else
                {
                    // String cannot be split.
                    double scaledFontSize = CellRow.HelperScaleDownFont(data, font, fontSize,
                        availableTextWidth, availableTextHeight, ScaleDirection.Both, out ignore);

                    if (existingRows == null)
                    {
                        string[] dataArray = { data };

                        adjustedFontSize = scaledFontSize;
                        return HelperPopulateRows(this, null, dataArray, font, scaledFontSize, dataField);
                    }

                    else
                    {
                        // Use Existing rows.
                        string[] dataArray = { data };

                        adjustedFontSize = scaledFontSize;
                        return HelperPopulateRows(this, existingRows, dataArray, font, scaledFontSize, dataField);
                    }
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

        #endregion

        #region Helper Methods.
        public string[] HelperScaleTextLayout(string[] text, List<CellRow>existingRows, Typeface font,
            double fontSize, out double scaledFontSize)
        {
            int lastStringsIndex = text.Length - 1;
            int textQty = text.Length;
            List<string> textBuffer = new List<string>(text);
            double currentFontSize = fontSize;
            string delimiter = " ";
            int rowCount = existingRows.Count;
            int currentTextCount = text.Length;

            // Change Me when Restarting a Concatenation Pass.
            double delimiterWidth = CellRow.HelperMeasureText(delimiter, font, currentFontSize).Width;

            // Concatenation Pass.
            for (int rowIndex = 0; rowIndex < existingRows.Count; rowIndex++)
            {
                if (rowIndex - 1 != lastStringsIndex)
                {
                    int primaryIndex = rowIndex;
                    int secondaryIndex = rowIndex + 1;

                    string primaryText = text[primaryIndex];
                    string secondaryText = text[secondaryIndex];

                    bool canConcatenate = HelperCanTextConcatenate(primaryText, secondaryText, font,
                        currentFontSize, existingRows[rowIndex].AvailableTextWidth, delimiter);

                    if (canConcatenate == true)
                    {
                        textBuffer.Insert(rowIndex, primaryText + delimiter + secondaryIndex);

                        currentTextCount -= 1;
                        if (currentTextCount <= rowCount)
                        {
                            // Text Layout has been Completed.
                            scaledFontSize = currentFontSize;
                            return textBuffer.ToArray();
                        }
                    }

                    else
                    {
                        rowCount++;
                    }
                }
            }

            // Concatenation Pass was unable to scale down Layout enough.
            // Scale Font down and recursively call method again.
            ScaleDirection ignore;
            int longestStringIndex = HelperGetLongestStringIndex(textBuffer.ToArray(), font, currentFontSize);

            currentFontSize = CellRow.HelperScaleDownFont(textBuffer[longestStringIndex], font, currentFontSize,
                existingRows[longestStringIndex].AvailableTextWidth, existingRows[longestStringIndex].AvailableTextHeight,
                ScaleDirection.Both, out ignore);

            scaledFontSize = currentFontSize;

            return HelperScaleTextLayout(textBuffer.ToArray(), existingRows, font, currentFontSize, out scaledFontSize);
           
        }

        protected bool HelperCanTextConcatenate(string primaryText, string secondaryText,
            Typeface font, double fontSize, double availableWidth, string delimiter)
        {
            double primaryWidth = CellRow.HelperMeasureText(primaryText, font, fontSize).Width;
            double secondaryWidth = CellRow.HelperMeasureText(secondaryText, font, fontSize).Width;
            double delimiterWidth = CellRow.HelperMeasureText(delimiter, font, fontSize).Width;

            if (primaryWidth + delimiterWidth + secondaryWidth <= availableWidth)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Measures the Width of each of the strings in the strings Array Parameter. Returns the Index of the longest
        /// string or equally longest string.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        protected int HelperGetLongestStringIndex(string[] strings, Typeface font, double fontSize)
        {
            if (strings.Length == 0)
            {
                throw new FormatException("Parameter strings has no Elements.");
            }

            List<double> lengths = new List<double>();

            foreach (var element in strings)
            {
                lengths.Add(CellRow.HelperMeasureText(element, font, fontSize).Width);
            }

            lengths.OrderBy(item => item);

            return lengths.IndexOf(lengths.Last());
            
        }

        /// <summary>
        /// Adds Data to and initializes/updates collection of Rows provided. If provided row Collection is null,
        /// new rows will be Generated and returned.
        /// </summary>
        /// <param name="labelCell"></param>
        /// <param name="existingRows"></param>
        /// <param name="splitStrings"></param>
        /// <param name="font"></param>
        /// <param name="heightScaledFontSize"></param>
        /// <param name="dataField"></param>
        /// <returns></returns>
        private List<CellRow> HelperPopulateRows(LabelCell labelCell, List<CellRow> existingRows, string[] data, 
            Typeface font, double fontSize, LabelField dataField)
        {
            var returnList = new List<CellRow>();

            if (existingRows == null)
            {
                // Generate new Rows.
                foreach (var element in data)
                {
                    returnList.Add(new CellRow(labelCell, dataField));
                    returnList.Last().Data = element;
                    returnList.Last().Font = font;
                    returnList.Last().FontSize = fontSize;
                }

                return returnList;
            }
            
            else
            {
                // Use Existing Rows as much as Possible.
                int lastExistingRowIndex = existingRows.Count - 1;

                if (data.Length >= existingRows.Count)
                {
                    // Equal ammount of strings and Rows.
                    for (int index = 0; index < data.Length; index++)
                    {
                        if (index <= lastExistingRowIndex)
                        {
                            // Populate existing Rows.
                            existingRows[index].Data = data[index];
                            existingRows[index].DataField = dataField;
                            existingRows[index].Font = font;
                            existingRows[index].FontSize = fontSize;

                            returnList.Add(existingRows[index]);
                        }
                    }
                }

                if (data.Length < existingRows.Count)
                {
                    // Less strings then already Existing Rows.
                    int lastDataIndex = data.Length - 1;

                    for (int index = 0; index < existingRows.Count; index++)
                    {
                        if (index <= lastDataIndex)
                        {
                            // Populate existing rows with existing Data.
                            existingRows[index].Data = data[index];
                            existingRows[index].DataField = dataField;
                            existingRows[index].Font = font;
                            existingRows[index].FontSize = fontSize;

                            returnList.Add(existingRows[index]);
                        }

                        else
                        {
                            // Not enough Data, Populate existing rows with blank Data.
                            existingRows[index].Data = string.Empty;
                            existingRows[index].DataField = dataField;
                            existingRows[index].Font = font;
                            existingRows[index].FontSize = fontSize;

                            returnList.Add(existingRows[index]);
                        }
                    }
                }

                if (data.Length > existingRows.Count)
                {
                    // Too many Strings.
                    throw new FormatException("HelperPopulateRows(), To many strings, not enough Rows! " +
                        "Use HelperScaleTextLayout() before calling this Method");
                }
                return returnList;
            }
        }

        /// <summary>
        /// Returns the row with the smallest Height Value from the provided List.
        /// </summary>
        /// <param name="existingRows"></param>
        /// <returns></returns>
        protected CellRow HelperGetShortestRow(List<CellRow> existingRows)
        {
            var sortList = new List<CellRow>(existingRows);
            sortList.OrderBy(item => item.Height.Value);

            return sortList.Last();
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

    public class NeighbourCells
    {
        public LabelCell Left { get; set; }
        public LabelCell Top { get; set; }
        public LabelCell Right { get; set; }
        public LabelCell Bottom { get; set; }

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
