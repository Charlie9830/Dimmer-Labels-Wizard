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
                    // Mixed Field.
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

            // Assign Templates to Rows.
            for (int index = 0; index < templates.Count && index < rows.Count; index++)
            {
                rows[index].Style = templates[index];
            }
        }

        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", typeof(int), typeof(LabelCell),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnRowCountPropertyChanged),
                    new CoerceValueCallback(CoerceRowCount)));

        private static object CoerceRowCount(DependencyObject d, object value)
        {
            int count = (int)value;

            if (count < 0)
            {
                return 0;
            }

            else
            {
                return count;
            }
        }

        private static void OnRowCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelCell;
            int newCount = (int)e.NewValue;
            int oldCount = (int)e.OldValue;
            int difference = newCount - oldCount;
            ObservableCollection<CellRow> rows = instance.Rows;
            int collectionCount = rows.Count;

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
                    rows.RemoveAt(rows.Count - 1);
                }
            }

            else if (difference > 0)
            {
                // Increase collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    rows.Add(new CellRow(instance));
                }
            }
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
                        // Throw Exception. Need some way of Translating Users Selection of Row Percentages
                        // to the actual Rows Here. Perhaps a List of Values generated during setup Dialog.
                        throw new NotImplementedException("Implement this Feature Charlie!");
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

                instance.RowCount = 1;
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
            DimmerDistroUnit reference = instance.DataReference;

            if (reference != null)
            {
                instance.SingleFieldData = reference.GetData(newDataField);
            }

            else
            {
                instance.SingleFieldData = "No Reference";
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
                        // Mixed Field, Collect all rows that display the intended DataField.
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

        private void Rows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
                    Grid.Children.Remove(cellRow.TextBlock);
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
                if (DataReference != null)
                {
                    DataReference.SetData(cellRow.Data, cellRow.DataField);

                    if (IsMerged == true)
                    {
                        foreach (var element in ConsumedReferences)
                        {
                            element.SetData(cellRow.Data, cellRow.DataField);
                        }
                    }
                }

                // Assign Data Layout(s).
                AssignDataLayouts(cellRow);
            }

            // DataField.
            if (propertyName == CellRow.DataFieldProperty.Name)
            {
                // Collect new Data.
                if (DataReference != null)
                {
                    cellRow.Data = DataReference.GetData(cellRow.DataField);
                }

                // Update Cascading State.
                SetCascadingRows(Rows.ToList());
            }

            // Data Layout
            if (propertyName == CellRow.DataLayoutProperty.Name)
            {
                AssignDataLayouts(cellRow);
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
        /// <summary>
        /// Sets the Data Property of a single Child Row based of it's current assigned Data Field. Should only be
        /// used whilst Cell is in Mixed Field Mode.
        /// </summary>
        /// <param name="cellRow"></param>
        /// <param name="cellParent"></param>
        protected void SetRowData(CellRow cellRow, LabelCell cellParent)
        {
            DimmerDistroUnit reference = cellParent.DataReference;
            CellDataMode cellDataMode = cellParent.CellDataMode;
            LabelField dataField = cellRow.DataField;

            if (reference != null)
            {
                if (cellDataMode == CellDataMode.MixedField)
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



    }
}
