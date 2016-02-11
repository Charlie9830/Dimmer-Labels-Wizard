using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelCell : ContentControl, INotifyPropertyChanged
    {
        #region Fields.
        // Constants
        protected const double UnitConversionRatio = 96d / 25.4d;

        protected SolidColorBrush _TextBrush;
        protected SolidColorBrush _Background;

        // Text Grid Rendering Elements.
        protected Grid _Grid = new Grid();

        private bool _InMouseSelectionEvent = false;
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
                new FrameworkPropertyMetadata(70d, options, null,
                new CoerceValueCallback(CoerceHeight));

            FrameworkPropertyMetadata widthPropertyMetadata =
                new FrameworkPropertyMetadata(70d, options, null,
                new CoerceValueCallback(CoerceWidth));

            HeightProperty.OverrideMetadata(typeof(LabelCell), heightPropertyMetadata);
            WidthProperty.OverrideMetadata(typeof(LabelCell), widthPropertyMetadata);
        }

        /// <summary>
        /// Constructs a LabelCell.
        /// </summary>
        public LabelCell()
        {
            _Rows = new ObservableCollection<CellRow>();
            _Rows.CollectionChanged += Rows_CollectionChanged;

            // Setup Grid
            _Grid.Background = Brushes.White;
            _Grid.HorizontalAlignment = HorizontalAlignment.Left;
            _Grid.VerticalAlignment = VerticalAlignment.Top;
            Content = _Grid;

            // Collection Type Dependency Properties.
            SetValue(SelectedRowsPropertyKey, new ObservableCollection<CellRow>());
            SetValue(ConsumedReferencesPropertyKey, new List<DimmerDistroUnit>());
        }
        #endregion

        #region CLR Properties
        public bool IsMerged
        {
            get
            {
                return ConsumedReferences.Count > 0;
            }
        }

        public int ConsumedReferencesCount
        {
            get
            {
                return ConsumedReferences.Count;
            }
        }

        public LabelField[] DisplayedDataFields
        {
            get
            {
                if (CellDataMode == CellDataMode.SingleField)
                {
                    // Single Field.
                    return new LabelField[] { SingleFieldDataField };
                }

                else
                {
                    // Multi Field.
                    List<LabelField> dataFields = new List<LabelField>();

                    foreach (var element in Rows)
                    {
                        if (dataFields.Contains(element.DataField) == false)
                        {
                            dataFields.Add(element.DataField);
                        }
                    }

                    return dataFields.ToArray();
                }
            }
        }

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

        private CellVerticalPosition _CellVerticalPosition;

        public CellVerticalPosition CellVerticalPosition
        {
            get { return _CellVerticalPosition; }
            set { _CellVerticalPosition = value; }
        }

        private int _HorizontalIndex;

        public int HorizontalIndex
        {
            get { return _HorizontalIndex; }
            set { _HorizontalIndex = value; }
        }

        private int _RowCount = 0;
        public int RowCount
        {
            get { return _RowCount; }
            set
            {
                if (_RowCount != value)
                {
                    int newCount = ValidateRowCount(value);
                    AdjustRowCollectionCount(_RowCount, newCount);
                    _RowCount = newCount;

                    ManageRowSplitters(this);
                }
            }
        }

        #endregion

        #region Dependency Properties
        public double BaseWidth
        {
            get { return (double)GetValue(BaseWidthProperty); }
            set { SetValue(BaseWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaseWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseWidthProperty =
            DependencyProperty.Register("BaseWidth", typeof(double), typeof(LabelCell),
                new FrameworkPropertyMetadata(70d, new PropertyChangedCallback(OnBaseWidthPropertyChanged),
                    new CoerceValueCallback(CoerceBaseWidth)));

        private static object CoerceBaseWidth(DependencyObject d, object value)
        {
            var baseWidth = (double)value;

            if (baseWidth < 0)
            {
                return 0d;
            }
            
            else
            {
                return baseWidth;
            }
               
        }

        private static void OnBaseWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var newValue = (double)e.NewValue;
            int widthMultiplier = instance.ConsumedReferencesCount + 1;

            instance.Width = newValue * widthMultiplier;
        }

        public DimmerDistroUnit DataReference
        {
            get { return (DimmerDistroUnit)GetValue(DataReferenceProperty); }
            set { SetValue(DataReferenceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataReference.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataReferenceProperty =
            DependencyProperty.Register("DataReference", typeof(DimmerDistroUnit),
                typeof(LabelCell), new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnDataReferencePropertyChanged)));

        private static void OnDataReferencePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var dataMode = instance.CellDataMode;
            var rows = instance.Rows;
            var newValue = e.NewValue as DimmerDistroUnit;
            var oldValue = e.OldValue as DimmerDistroUnit;

            if (newValue != null)
            {
                if (dataMode == CellDataMode.SingleField)
                {
                    instance.SingleFieldData = newValue.GetData(instance.SingleFieldDataField);
                }

                else
                {
                    foreach (var element in rows)
                    {
                        element.Data = newValue.GetData(element.DataField);
                    }
                }

                // Connect Event Handler to track Future Changes.
                newValue.PropertyChanged += instance.DataReference_PropertyChanged;
            }

            else
            {
                // DataReference has been set to null.
                if (dataMode == CellDataMode.SingleField)
                {
                    instance.SingleFieldData = "No Reference";
                }

                else
                {
                    foreach (var element in rows)
                    {
                        element.Data = "No Reference";
                    }
                }
            }

            if (oldValue != null)
            {
                // Disconnect outgoing Event.
                oldValue.PropertyChanged -= instance.DataReference_PropertyChanged;
            }
        }

        public List<DimmerDistroUnit> ConsumedReferences
        {
            get { return (List<DimmerDistroUnit>)GetValue(ConsumedReferencesProperty); }
            set { SetValue(ConsumedReferencesPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for ConsumedReferences.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey ConsumedReferencesPropertyKey =
            DependencyProperty.RegisterReadOnly("ConsumedReferences", typeof(List<DimmerDistroUnit>), typeof(LabelCell), new PropertyMetadata(null));

        public static readonly DependencyProperty ConsumedReferencesProperty = ConsumedReferencesPropertyKey.DependencyProperty;

        public List<CellRowTemplate> RowTemplates
        {
            get { return (List<CellRowTemplate>)GetValue(RowTemplatesProperty); }
            set { SetValue(RowTemplatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowTemplates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowTemplatesProperty =
            DependencyProperty.Register("RowTemplates", typeof(List<CellRowTemplate>),
                typeof(LabelCell), new FrameworkPropertyMetadata(new List<CellRowTemplate>(),
                    new PropertyChangedCallback(OnCellRowTemplatesPropertyChanged)));

        private static void OnCellRowTemplatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var templates = e.NewValue as List<CellRowTemplate>;
            ObservableCollection<CellRow> rows = instance.Rows;

            // Assign Row Templates.
            AssignMultiFieldRowTemplates(instance, templates, rows);
            
        }


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

            if (lineWeight < 0d)
            {
                return 0d;
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

            if (lineWeight < 0d)
            {
                return 0d;
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

            if (lineWeight < 0d)
            {
                return 0d;
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

            if (lineWeight < 0d)
            {
                return 0d;
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
            var instance = d as LabelCell;
            double lineWeight = (double)value;

            // Double Lineweight if Cell is Selected.
            if (lineWeight < 0d)
            {
                if (instance.IsSelected)
                {
                    return 2d;
                }

                else
                {
                    return 0d;
                }
            }

            else
            {
                if (instance.IsSelected)
                {
                    return lineWeight * 2;
                }

                else
                {
                    return lineWeight;
                }
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
            var instance = d as LabelCell;
            double lineWeight = (double)value;

            // Double Lineweight if Cell is Selected.
            if (lineWeight < 0d)
            {
                if (instance.IsSelected)
                {
                    return 2d;
                }

                else
                {
                    return 0d;
                }
            }

            else
            {
                if (instance.IsSelected)
                {
                    return lineWeight * 2;
                }

                else
                {
                    return lineWeight;
                }
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
            var instance = d as LabelCell;
            double lineWeight = (double)value;

            // Double Lineweight if Cell is Selected.
            if (lineWeight < 0d)
            {
                if (instance.IsSelected)
                {
                    return 2d;
                }

                else
                {
                    return 0d;
                }
            }

            else
            {
                if (instance.IsSelected)
                {
                    return lineWeight * 2;
                }

                else
                {
                    return lineWeight;
                }
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
            var instance = d as LabelCell;
            double lineWeight = (double)value;

            // Double Lineweight if Cell is Selected.
            if (lineWeight < 0d)
            {
                if (instance.IsSelected)
                {
                    return 2d;
                }

                else
                {
                    return 0d;
                }
            }

            else
            {
                if (instance.IsSelected)
                {
                    return lineWeight * 2;
                }

                else
                {
                    return lineWeight;
                }
            }
        }

        private static void OnActualBottomWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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
                    }
                    break;
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
                typeof(LabelCell), new FrameworkPropertyMetadata(CellDataMode.MultiField,
                    new PropertyChangedCallback(OnCellDataModePropertyChanged)));

        private static void OnCellDataModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var oldValue = (CellDataMode)e.OldValue;
            var newValue = (CellDataMode)e.NewValue;
            ObservableCollection<CellRow> rows = instance.Rows;

            if (newValue == CellDataMode.MultiField)
            {
                if (oldValue == CellDataMode.SingleField)
                {
                    // If coming from SingleField Mode. Reset RowHeightMode Value and
                    // Regenerate Cellrows from Templates.
                    instance.ClearValue(RowHeightModeProperty);

                    var templates = instance.RowTemplates;

                    AssignMultiFieldRowTemplates(instance, templates, rows);
                }

                foreach (var element in rows)
                {
                    instance.SetRowData(element, instance);
                }
            }

            if (newValue == CellDataMode.SingleField)
            {
                // SingleField mode Requires RowHeightMode to be set to Static.
                instance.RowHeightMode = CellRowHeightMode.Static;

                SetSingleFieldRows(instance, instance.SingleFieldData);
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
            return value;
        }


        public string SingleFieldData
        {
            get { return (string)GetValue(SingleFieldDataProperty); }
            set { SetValue(SingleFieldDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleFieldData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleFieldDataProperty =
            DependencyProperty.Register("SingleFieldData", typeof(string), typeof(LabelCell),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnSingleFieldDataPropertyChanged),
                    new CoerceValueCallback(CoerceSingleFieldData)));

        private static object CoerceSingleFieldData(DependencyObject d, object value)
        {
            var data = (string)value;

            // Replaces multiple occurances of Whitespace with single Space. Trims Whitespace off of ends.
            return Regex.Replace(data, @"\s+", " ").Trim();

        }

        private static void OnSingleFieldDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            string newData = (string)e.NewValue;

            SetSingleFieldRows(instance, newData);
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
            DimmerDistroUnit reference = instance.DataReference;

            if (reference != null)
            {
                instance.SingleFieldData = reference.GetData(newDataField);
            }

            else
            {
                instance.SingleFieldData = string.Empty;
            }
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
            double scaledFontSize;

            AssignSingleFieldDataLayouts(instance, out scaledFontSize);

            instance.SingleFieldActualFontSize = scaledFontSize;
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
            double scaledFontSize;

            AssignSingleFieldDataLayouts(instance, out scaledFontSize);

            instance.SingleFieldActualFontSize = scaledFontSize;
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

        public ObservableCollection<CellRow> SelectedRows
        {
            get { return (ObservableCollection<CellRow>)GetValue(SelectedRowsProperty); }
        }

        // Using a DependencyProperty as the backing store for SelectedRows.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey SelectedRowsPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedRows", typeof(ObservableCollection<CellRow>), typeof(LabelCell),
                new FrameworkPropertyMetadata(new ObservableCollection<CellRow>(),
                    new PropertyChangedCallback(OnSelectedRowsPropertyChanged)));

        public static readonly DependencyProperty SelectedRowsProperty = SelectedRowsPropertyKey.DependencyProperty;

        private static void OnSelectedRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                // Connect incoming Events.
                var collection = e.NewValue as ObservableCollection<CellRow>;
                collection.CollectionChanged += SelectedRows_CollectionChanged;
            }

            if (e.OldValue != null)
            {
                // Disconnect Outgoing events.
                var collection = e.OldValue as ObservableCollection<CellRow>;
                collection.CollectionChanged -= SelectedRows_CollectionChanged;

            }
        }



        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(LabelCell),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedPropertyChanged)));

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var newValue = (bool)e.NewValue;

            // If Cell has been Deselected, deselect child Rows.
            if (newValue == false)
            {
                while (instance.SelectedRows.Count > 0)
                {
                    instance.SelectedRows.RemoveAt(instance.SelectedRows.Count - 1);
                }
            }

            // Coerce Render State.
            instance.CoerceValue(ActualLeftWeightProperty);
            instance.CoerceValue(ActualTopWeightProperty);
            instance.CoerceValue(ActualRightWeightProperty);
            instance.CoerceValue(ActualBottomWeightProperty);

            // Notify Parent LabelStrip.
            OnPropertyChanged(instance, nameof(IsSelected));
        }



        public List<double> RowHeightProportions
        {
            get { return (List<double>)GetValue(RowHeightProportionsProperty); }
            set { SetValue(RowHeightProportionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeightProportions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProportionsProperty =
            DependencyProperty.Register("RowHeightProportions", typeof(List<double>), typeof(LabelCell),
                new FrameworkPropertyMetadata(null));



        public bool ShowRowSplitters
        {
            get { return (bool)GetValue(ShowRowSplittersProperty); }
            set { SetValue(ShowRowSplittersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowGridSplitters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowRowSplittersProperty =
            DependencyProperty.Register("ShowRowSplitters", typeof(bool), typeof(LabelCell),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnShowRowSplittersPropertyChanged)));

        private static void OnShowRowSplittersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            var dataReference = instance.DataReference;
            var rows = instance.Rows;

            // Manage number of Row Splitters.
            ManageRowSplitters(instance);

            if (dataReference != null)
            {
                throw new NotSupportedException("Switching RowSplitters on could write back junk data to Cells Data Reference. Ensure Data Reference is null before switching on RowSplitters");
            }
            
            foreach (var element in rows)
            {
                element.Data = element.Height.Value.ToString();
            }

        }

        private static void RemoveRowSplitters(LabelCell instance)
        {
            // Remove all GridSplitters from Grid.Children.
            foreach (var element in GetExistingSplitters(instance).ToList())
            {
                instance.Grid.Children.Remove(element);
            }
        }

        private static void ManageRowSplitters(LabelCell instance)
        {
            if (instance.ShowRowSplitters == false)
            {
                // Remove any existing Splitters then Bail.
                RemoveRowSplitters(instance);
                return;
            }

            int rowCount = instance.Rows.Count;
            int requiredSplitterCount = rowCount - 1;

            if (requiredSplitterCount <= 0)
            {
                // Zero or 1 row. Cannot apply Splitters. Remove any Existing then
                // Bail out.
                RemoveRowSplitters(instance);
                return;
            }

            List<GridSplitter> existingSplitters = GetExistingSplitters(instance).ToList();
            int existingSplitterCount = existingSplitters.Count;
            int difference = existingSplitterCount - requiredSplitterCount;

            // Adjust existing SplitterCount.
            if (difference > 0)
            {
                // Decrease existing Splitter Count.
                for (int count = 1; count <= difference; count++)
                {
                    instance.Grid.Children.Remove(existingSplitters.Last());
                    existingSplitters.Remove(existingSplitters.Last());
                }
            }

            if (difference < 0)
            {
                // Increase existing Splitter Count.
                difference = Math.Abs(difference);

                for (int count = 1; count <= difference; count++)
                {
                    var splitter = CreateRowSplitter();

                    instance.Grid.Children.Add(splitter);
                    existingSplitters.Add(splitter);
                }
            }


            for (int index = 0; index < requiredSplitterCount; index++)
            {
                // Set GridRow then add to Grid.
                Grid.SetRow(existingSplitters[index], index);

                if (instance.Grid.Children.Contains(existingSplitters[index]) == false)
                {
                    instance.Grid.Children.Add(existingSplitters[index]);
                }
            }

        }

        protected static GridSplitter CreateRowSplitter()
        {
            return new GridSplitter()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                ResizeDirection = GridResizeDirection.Rows,
                ResizeBehavior = GridResizeBehavior.CurrentAndNext,
                Style = (Style)Application.Current.FindResource("RowSplitterStyle")
            };
        }

        protected static IEnumerable<GridSplitter> GetExistingSplitters(LabelCell instance)
        {
            var gridSplitters = new List<GridSplitter>();

            // LINQ unavailable. Collect a List of references to GridSplitters in the Grid.
            foreach (var element in instance.Grid.Children)
            {
                if (element.GetType() == typeof(GridSplitter))
                {
                    gridSplitters.Add(element as GridSplitter);
                }
            }

            return gridSplitters;
        }

        #endregion

        #region Overrides
        protected override void OnRender(DrawingContext drawingContext)
        {
            // Declare Resources.
            var lineBrush = new SolidColorBrush(Colors.Black);

            // Pens.
            var leftPen = new Pen(lineBrush, ActualLeftWeight);
            var topPen = new Pen(lineBrush, ActualTopWeight);
            var rightPen = new Pen(lineBrush, ActualRightWeight);
            var bottomPen = new Pen(lineBrush, ActualBottomWeight);

            // Points.
            var leftA = new Point(ActualLeftWeight / 2, 0);
            var leftB = new Point(ActualLeftWeight / 2, Height);

            var topA = new Point(0, ActualTopWeight / 2);
            var topB = new Point(Width, ActualTopWeight / 2);

            var rightA = new Point(Width - (ActualRightWeight / 2), 0);
            var rightB = new Point(Width - (ActualRightWeight / 2), Height);

            var bottomA = new Point(0, Height - (ActualBottomWeight / 2));
            var bottomB = new Point(Width, Height - (ActualBottomWeight / 2));
            
            // Drawing 
            drawingContext.DrawLine(leftPen, leftA, leftB);
            drawingContext.DrawLine(topPen, topA, topB);
            drawingContext.DrawLine(rightPen, rightA, rightB);
            drawingContext.DrawLine(bottomPen, bottomA, bottomB);


            // Setup Grid.
            // Don't set Grid Width or Height too less than zero.
            double gridWidth = Width - (ActualLeftWeight) - (ActualRightWeight);
            _Grid.Width = (gridWidth < 0d) ? 0d : gridWidth;

            
            double gridHeight = Height - (ActualTopWeight) - (ActualBottomWeight);
            _Grid.Height = (gridHeight) < 0d ? 0d : gridHeight;

            _Grid.Margin = new Thickness(ActualLeftWeight, ActualTopWeight,
                ActualRightWeight, ActualBottomWeight);

        }
        #endregion

        #region Event Handlers

        protected void DataReference_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            LabelField dataField;

            // Map Property Name to matching LabelField Value.
            if (Enum.TryParse(propertyName, out dataField) == true)
            {
                // A matching LabelField (DataField) has been found. Collect new Data.
                string newData = DataReference.GetData(dataField);

                if (DisplayedDataFields.Contains(dataField))
                {
                    // Data is currently displayed and requires updating.
                    if (CellDataMode == CellDataMode.SingleField)
                    {
                        // Single Field.
                        SingleFieldData = newData;
                    }

                    else
                    {
                        // Multi Field, Collect all rows that display the intended DataField.
                        var targetRows = Rows.Where(item => item.DataField == dataField);

                        // Push Data.
                        foreach (var element in targetRows)
                        {
                            element.Data = newData;
                        }
                    }
                }
            }
        }

        private static void SelectedRows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<CellRow>;

            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var row = element as CellRow;
                    row.IsSelected = true;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var row = element as CellRow;
                    row.IsSelected = false;
                }
            }
        }

        private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<CellRow>;
            int newIndex = e.NewStartingIndex;

            // Keep Grid.RowDefinitions explicitly linked to Rows Collection.
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cellRow = element as CellRow;
                    Grid.RowDefinitions.Insert(e.NewStartingIndex, cellRow);
                    cellRow.PropertyChanged += CellRow_PropertyChanged;
                    cellRow.MouseDown += CellRow_MouseDown;
                    cellRow.MouseUp += CellRow_MouseUp;

                    // Assign template.
                    if (newIndex < RowTemplates.Count)
                    {
                        collection[newIndex].Style = RowTemplates[newIndex];
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cellRow = element as CellRow;
                    Grid.RowDefinitions.Remove(cellRow);
                    Grid.Children.Remove(cellRow.Border);
                    cellRow.PropertyChanged -= CellRow_PropertyChanged;
                    cellRow.MouseDown -= CellRow_MouseDown;
                    cellRow.MouseUp -= CellRow_MouseUp;
                }
            }


            if (e.OldItems == null && e.NewItems == null && collection.Count == 0)
            {
                // Collection has Been Cleared.
                Grid.RowDefinitions.Clear();
                Grid.Children.Clear();
            }

            // Push new Count back to RowCount.
            RowCount = collection.Count;

            // Update Row Indexes and Coerce RowHeights.
            int rowIndexCounter = 0;
            foreach (var element in collection)
            {
                element.Index = rowIndexCounter;
                rowIndexCounter++;
                element.CoerceValue(CellRow.RowHeightProperty);
            }

            if (ShowRowSplitters)
            {
                // Push collection state back to RowHeightProportions if Row Splitters are Visible.
                RowHeightProportions = (from row in collection
                                       select row.Height.Value).ToList();

                // Push Height data to Row Data.
                foreach (var element in collection)
                {
                    element.Data = element.Height.Value.ToString();
                }
            }
        }

        private void CellRow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cellRow = sender as CellRow;
            string propertyName = e.PropertyName;

            // IsSelected.
            if (propertyName == CellRow.IsSelectedProperty.Name)
            {
                if (_InMouseSelectionEvent == false)
                {
                    if (cellRow.IsSelected == true && SelectedRows.Contains(cellRow) == false)
                    {
                        SelectedRows.Add(cellRow);
                    }

                    else
                    {
                        SelectedRows.Remove(cellRow);
                    }
                }
            }

            // Height.
            if (propertyName == CellRow.HeightProperty.Name)
            {
                if (ShowRowSplitters && RowHeightProportions != null &&
                    RowHeightProportions.Count() > 0 && RowHeightMode == CellRowHeightMode.Manual)
                {
                    // Collect rowIndex and Current state of RowProportions Collection.
                    int rowIndex = cellRow.Index;
                    var currentRowProportions = RowHeightProportions.ToList();

                    // Modify RowProportions Collection.
                    currentRowProportions[rowIndex] = cellRow.Height.Value;

                    // Write back to Dependency Property.
                    RowHeightProportions = currentRowProportions;

                    // Write new Height value back to Row's Data.
                    cellRow.Data = cellRow.Height.Value.ToString();
                }
            }
        }

        #region Mouse Event Handlers
        private void CellRow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var row = sender as CellRow;

            _InMouseSelectionEvent = true;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Additive Selection.
                // Add Row to Selection.
                if (SelectedRows.Contains(row) == false)
                {
                    SelectedRows.Add(row);
                }
            }

            else
            {
                // Exclusive Selection.

                // Clear SelectedRows.
                while (SelectedRows.Count > 0)
                {
                    SelectedRows.RemoveAt(SelectedRows.Count - 1);
                }

                // Add new Selection.
                SelectedRows.Add(row);
            }

            _InMouseSelectionEvent = false;

        }

        private void CellRow_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _InMouseSelectionEvent = true;
            _InMouseSelectionEvent = false;
        }

        #endregion
        #endregion.


        #region Methods.
        private static void AssignMultiFieldRowTemplates(LabelCell instance, List<CellRowTemplate> newTemplates,
            ObservableCollection<CellRow> rows)
        {
            if (instance.CellDataMode == CellDataMode.SingleField)
            {
                // This method intereferes with SingleField Mode. Bail out.
                return;
            }

            // Set RowCount.
            instance.RowCount = newTemplates.Count;

            // Assign Templates to Rows.
            for (int index = 0; index < newTemplates.Count && index < rows.Count; index++)
            {
                rows[index].Style = newTemplates[index];
            }
        }

        /// <summary>
        /// Sets the Data Property of a single Child Row based of it's current assigned Data Field. Should only be
        /// used whilst Cell is in Multi Field Mode.
        /// </summary>
        /// <param name="cellRow"></param>
        /// <param name="cellParent"></param>

        protected int ValidateRowCount(int newValue)
        {
            if (newValue < 0)
            {
                return 0;
            }

            else
            {
                return newValue;
            }
        }

        protected void AdjustRowCollectionCount(int oldCount, int newCount)
        {
            int difference = newCount - oldCount; // Testing Replacement of oldCount with Rows.Count.
            int collectionCount = Rows.Count;

            if (newCount == collectionCount)
            {
                // Collection modification has been triggered elsewhere,
                // and has already taken place. No further action is 
                // required from RowCount.

                return;
            }

            if (difference < 0)
            {
                difference = Math.Abs(difference);

                // Decrease collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    Rows.RemoveAt(Rows.Count - 1);
                }
            }

            else if (difference > 0)
            {
                // Increase collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    Rows.Add(new CellRow(this));
                }
            }
        }

        protected void SetRowData(CellRow cellRow, LabelCell cellParent)
        {
            DimmerDistroUnit reference = cellParent.DataReference;
            CellDataMode cellDataMode = cellParent.CellDataMode;
            LabelField dataField = cellRow.DataField;

            if (reference != null)
            {
                if (cellDataMode == CellDataMode.MultiField)
                {
                    cellRow.Data = reference.GetData(cellRow.DataField);
                }
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

            if (data != string.Empty)
            {
                instance.RowCount = dataQty;
            }

            else
            {
                instance.RowCount = 0;
            }
        }

        /// <summary>
        /// When used in SingleFieldMode. Will Setup Rows correctly a propagate Data to each Row.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="data"></param>
        private static void SetSingleFieldRows(LabelCell instance, string data)
        {
            ObservableCollection<CellRow> rowCollection = instance.Rows;
            LabelField dataField = instance.SingleFieldDataField;

            if (instance.CellDataMode != CellDataMode.SingleField)
            {
                // Cell is not in SingleField Mode. Bail out.
                return;
            }

            // Set RowQty to Correct Value.
            CurateRowQty(instance, data);

            if (instance.RowCount > 0)
            {
                // Set Datafield and Data properties of Rows.
                foreach (var element in rowCollection)
                {
                    element.SetDataFieldCurrentValue(dataField);
                    element.Data = data;
                }

                double scaledFontSize;
                AssignSingleFieldDataLayouts(instance, out scaledFontSize);

                // Writeback Scaled font Size.
                instance.SingleFieldActualFontSize = scaledFontSize;
            }
        }

        private static void AssignSingleFieldDataLayouts(LabelCell instance, out double scaledFontSize)
        {
            char delimiter = ' ';
            string data = instance.SingleFieldData;
            Typeface font = instance.SingleFieldFont;
            double desiredFontSize = instance.SingleFieldDesiredFontSize;
            double actualFontSize = desiredFontSize;
            List<DataLayout> dataLayouts = new List<DataLayout>();
            ObservableCollection<CellRow> rowCollection = instance.Rows;
            int rowCount = rowCollection.Count;
            string[] dataElements = instance.SingleFieldData.Split(delimiter);
            int dataElementsCount = dataElements.Count();
            List<double> scaledFontSizes = new List<double>();
            ScaleDirection ignore;

            // Generate Data Layouts.
            int startIndex = 0;
            for (int index = 0; index < rowCount; index++)
            {
                if (index < dataElementsCount)
                {
                    // Populated Row.
                    dataLayouts.Add(new DataLayout(startIndex, dataElements[index].Length,
                        data, font, desiredFontSize));

                    // Advance the Starting Index.
                    startIndex += dataElements[index].Length + 1;
                }

                else
                {
                    // Blank Row.
                    dataLayouts.Add(new DataLayout(0, 0, data, font, desiredFontSize));
                }
            }

            // Scale Font Sizes.
            for (int index = 0; index < dataLayouts.Count && index < rowCount; index++)
            {
                string displayedData = dataLayouts[index].DisplayedText;
                double containerWidth = rowCollection[index].AvailableTextWidth;
                double containerHeight = rowCollection[index].AvailableTextHeight;

                scaledFontSizes.Add(ScaleDownFontSize(displayedData, font, desiredFontSize,
                    containerWidth, containerHeight, ScaleDirection.Both, out ignore));
            }

            if (scaledFontSizes.Count > 0)
            {
                var query = scaledFontSizes.OrderBy(item => item);
                actualFontSize = query.First();
            }

            // Assign DataLayouts to Rows.
            int listIndex = 0;
            foreach (var element in rowCollection)
            {
                element.DataLayout = dataLayouts[listIndex];
                element.ActualFontSize = actualFontSize;
                listIndex++;
            }

            // Set Out Parameter.
            scaledFontSize = actualFontSize;
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



    }
}
