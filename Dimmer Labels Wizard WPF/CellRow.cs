using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Describes the Content and Layout of a Single Row within a Grid. Only provides abstract TextBlock
    /// rendering, requires a Parent LabelCell and accsess to its Grid in order to render correctly.
    /// </summary>
    public class CellRow : RowDefinition, INotifyPropertyChanged
    {
        #region Constructors.
        /// <summary>
        /// Available Text Width/Height Parameters must be supplied in WPF Units.
        /// </summary>
        /// <param name="availableTextWidth"></param>
        /// <param name="availableTextHeight"></param>
        public CellRow(LabelCell parentLabelCell)
        {
            if (parentLabelCell == null)
            {
                throw new NullReferenceException("Parameter 'parentLabelCell' cannot be Null");
            }

            CellParent = parentLabelCell;
            DataField = LabelField.NoAssignment;

            // Setup TextBlock.
            TextBlock.VerticalAlignment = VerticalAlignment.Center;
            TextBlock.HorizontalAlignment = HorizontalAlignment.Center;

            // Initial Values.
            Data = CellParent.PreviousReference.GetData(DataField);
            DesiredFontSize = 12;
            Font = new Typeface("Arial");
            HeightMode = CellParent.RowHeightMode;

            // Assign TextBlock to CellParent's Grid.
            CellParent.Grid.Children.Add(TextBlock);
        }

        #endregion

        #region Constants
        public const double RowHeightDefaultValue = 0d;
        public const CellRowHeightMode CellRowHeightModeDefaultValue = CellRowHeightMode.Static;
        #endregion

        #region Public Fields.
        /// <summary>
        /// Abstract rendering Target. Do not write directly to this Field. Internal use only.
        /// </summary>
        public TextBlock TextBlock = new TextBlock();

        /// <summary>
        /// Tracks whether a Data Layout Pass is in progress or not.
        /// </summary>
        public bool IsInDataLayoutPass = false;
        #endregion

        #region CLR Properties and Fields.

        /// <summary>
        /// Gets or Sets a collection of the Cells cascading alongside this Cell, also contains
        /// this cell.
        /// </summary>
        private List<CellRow> _CascadingRows = new List<CellRow>();

        public List<CellRow> CascadingRows
        {
            get { return _CascadingRows; }
            set { _CascadingRows = value; }
        }


        /// <summary>
        /// Unexpected results may occur when setting this property directly. To set this property, use
        /// the Parent Label Cell Row Height Mode property.
        /// </summary>
        public CellRowHeightMode HeightMode
        {
            get
            {
                return CellParent.RowHeightMode;
            }
            set
            {
                GridLength currentHeight = Height;
                switch (value)
                {
                    case CellRowHeightMode.Static:
                        Height = new GridLength(currentHeight.Value, GridUnitType.Pixel);
                        CoerceValue(RowHeightProperty);
                        break;
                    case CellRowHeightMode.Automatic:
                        Height = new GridLength(currentHeight.Value, GridUnitType.Star);
                        CoerceValue(RowHeightProperty);
                        break;
                    case CellRowHeightMode.Manual:
                        Height = new GridLength(currentHeight.Value, GridUnitType.Star);
                        break;
                }

                CoerceValue(RowHeightProperty);
            }
        }

        private bool _IsCascading;
        /// <summary>
        /// Gets or Sets a value indicating if this Row is Cascading Data.
        /// </summary>
        public bool IsCascading
        {
            get { return _IsCascading; }
            set { _IsCascading = value; }
        }


        /// <summary>
        /// The Width available for Text Placement in WPF Units.
        /// </summary>
        public double AvailableTextWidth
        {
            get
            {
                return CellParent.Width - CellParent.LeftWeight - CellParent.RightWeight;
            }
        }

        /// <summary>
        /// The Height available for Text Placement in WPF Units.
        /// </summary>
        public double AvailableTextHeight
        {
            get
            {
                return (CellParent.Height - CellParent.TopWeight - CellParent.BottomWeight) / CellParent.Rows.Count;
            }
        }
        #endregion

        #region Dependency Properties


        public DataLayout DataLayout
        {
            get { return (DataLayout)GetValue(DataLayoutProperty); }
            set { SetValue(DataLayoutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataLayout.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataLayoutProperty =
            DependencyProperty.Register("DataLayout", typeof(DataLayout), typeof(CellRow), new FrameworkPropertyMetadata(new DataLayout(), 
                new PropertyChangedCallback(OnDataLayoutPropertyChanged), new CoerceValueCallback(CoerceDataLayout)));

        private static object CoerceDataLayout(DependencyObject d, object value)
        {
            return value;
        }

        private static void OnDataLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var layout = (DataLayout)e.NewValue;

            instance.TextBlock.Text = instance.Data.Substring(layout.FirstIndex, layout.Length);
        }

        /// <summary>
        /// Gets or Sets the data currently displayed in the TextBlock. Changes to this value will be pushed to Data Model.
        ///  This is a Dependency Property.
        /// </summary>
        public string Data
        {
            get { return (string)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // DataProperty
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(string), typeof(CellRow),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnDataPropertyChanged),
                    new CoerceValueCallback(CoerceData)));

        // Property Changed Callback handler.
        private static void OnDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;

            // Signal Parent Cell to Update properties.
            OnPropertyChanged(e.Property.Name, instance);
        }

        private static object CoerceData(DependencyObject d, object value)
        {
            string data = (string)value;
            // Trim Leading and trailing Whitespace.
            return data.Trim();
        }

        /// <summary>
        /// Dictates which Label Field is used as Text Rendering Target. Also provides specific targeting
        /// for Updates pushed back to Previous Reference.
        /// </summary>
        public LabelField DataField
        {
            get { return (LabelField)GetValue(DataFieldProperty); }
            set { SetValue(DataFieldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataField.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataFieldProperty =
            DependencyProperty.Register("DataField", typeof(LabelField), typeof(CellRow),
                new FrameworkPropertyMetadata(LabelField.Custom, new PropertyChangedCallback(OnDataFieldPropertyChanged)));

        private static void OnDataFieldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;

            // Signal change to Parent Cell.
            OnPropertyChanged(DataFieldProperty.Name, instance);
        }

        /// <summary>
        /// Sets the Font Size for Text Rendering. This is a Dependency Property
        /// </summary>
        public double DesiredFontSize
        {
            get { return (double)GetValue(DesiredFontSizeProperty); }
            set { SetValue(DesiredFontSizeProperty, value); }
        }

        // Font Size Backing Field.
        public static readonly DependencyProperty DesiredFontSizeProperty =
            DependencyProperty.Register("DesiredFontSize", typeof(double), typeof(CellRow), new FrameworkPropertyMetadata(1d, 
                new PropertyChangedCallback(OnDesiredFontSizePropertyChanged), new CoerceValueCallback(CoerceDesiredFontSize)));

        // Property Changed Callback Handler.
        private static void OnDesiredFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var fontSize = (double)e.NewValue;
            DataLayout layout = instance.DataLayout;

            if (fontSize != layout.FontSize)
            {
                // Signal to Parent that Data Layout needs to be refreshed.
                OnPropertyChanged(DataLayoutProperty.Name, instance);
            }

            // Coerce RowHeight.
            instance.CoerceValue(RowHeightProperty);
            
        }

        private static object CoerceDesiredFontSize(DependencyObject d, object baseValue)
        {
            var instance = d as CellRow;
            string data = instance.Data;
            Typeface font = instance.Font;
            double desiredFontSize = (double)baseValue;
            double width = instance.AvailableTextWidth;
            double height = instance.AvailableTextHeight;

            if (desiredFontSize <= 0d)
            {
                desiredFontSize = 1d;
            }

            double fontSize = desiredFontSize;

            return fontSize;
        }

        /// <summary>
        /// Gets the Actual FontSize used after Scaling passes.
        /// </summary>
        public double ActualFontSize
        {
            get { return (double)GetValue(ActualFontSizeProperty); }
            set { SetValue(ActualFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualFontSizeProperty =
            DependencyProperty.Register("ActualFontSize", typeof(double), typeof(CellRow),
                new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnActualFontSizePropertyChanged),
                    new CoerceValueCallback(CoerceActualFontSize)));

        private static object CoerceActualFontSize(DependencyObject d, object value)
        {
            double fontSize = (double)value;
            if (fontSize <= 0.0d)
            {
                return 0.1d;
            }

            else if (fontSize >= double.PositiveInfinity)
            {
                return 300d;
            }

            else
            {
                return value;
            }

        }

        private static void OnActualFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            double value = (double)e.NewValue;

            instance.TextBlock.FontSize = value;
        }

        /// <summary>
        /// Gets or Sets the Typeface used to Render Row Text.
        /// </summary>
        public Typeface Font
        {
            get { return (Typeface)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Font.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontProperty =
            DependencyProperty.Register("Font", typeof(Typeface), typeof(CellRow), 
                new FrameworkPropertyMetadata(new Typeface("Wingdings"),
                    new PropertyChangedCallback(OnFontPropertyChanged)));

        private static void OnFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var typeface = (Typeface)e.NewValue;

            instance.TextBlock.FontFamily = typeface.FontFamily;
            instance.TextBlock.FontStretch = typeface.Stretch;
            instance.TextBlock.FontStyle = typeface.Style;
            instance.TextBlock.FontWeight = typeface.Weight;

            instance.CoerceValue(DesiredFontSizeProperty);
        }

        /// <summary>
        /// Gets or Sets the Height Value of the Row. The exact affect of this value is dicated by the value
        /// of the Height Mode Property. This is a Dependency Property.
        /// </summary>
        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(double), typeof(CellRow),
                new FrameworkPropertyMetadata(RowHeightDefaultValue,
                    new PropertyChangedCallback(OnRowHeightPropertyChanged),
                    new CoerceValueCallback(CoerceRowHeight)));

        private static void OnRowHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var value = (double)e.NewValue;
            var heightMode = instance.HeightMode;
            GridLength currentHeight = instance.Height;

            // Copy Values from currentHeight into newHeight and Assign to Height Property.
            GridLength newHeight = new GridLength(value, currentHeight.GridUnitType);
            instance.Height = newHeight;

            instance.CoerceValue(DesiredFontSizeProperty);
        }

        private static object CoerceRowHeight(DependencyObject d, object value)
        {
            var instance = d as CellRow;
            // rowQty must atleast be 0. This method belongs to a Row Object, therefore, abstractly, atleast one
            // must exist in the collection. It may just not have been added yet.
            int rowQty = instance.CellParent.Rows.Count == 0 ? 1 : instance.CellParent.Rows.Count;
            double cellHeight = instance.CellParent.Height;
            string data = instance.Data;
            Typeface font = instance.Font;
            double fontSize = instance.DesiredFontSize;

            CellRowHeightMode heightMode = instance.HeightMode;
            if (heightMode == CellRowHeightMode.Static)
            {
                return cellHeight / rowQty;
            }

            // If Heightmode is set to Automatic Adjust Row Heights.
            if (heightMode == CellRowHeightMode.Automatic)
            {
                double textHeight = LabelCell.MeasureText(data, font, fontSize).Height;
                return textHeight / cellHeight;
            }

            else
            {
                return value;
            }
        }

        /// <summary>
        /// Gets or Sets the LabelCell Parent to this Row. This is a Dependency Property.
        /// </summary>
        public LabelCell CellParent
        {
            get { return (LabelCell)GetValue(CellParentProperty); }
            set { SetValue(CellParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellParentProperty =
            DependencyProperty.Register("CellParent", typeof(LabelCell), typeof(CellRow),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or Sets the Index number of this Row.
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(CellRow),
                new PropertyMetadata(0,new PropertyChangedCallback(OnIndexPropertyChanged)));

        private static void OnIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;

            if (e.OldValue != e.NewValue)
            {
                Grid.SetRow(instance.TextBlock, (int)e.NewValue);
            }
        }

        #endregion

        #region Overrides.
        public override string ToString()
        {
            return "Row " + (Index + 1) + ":   " + Data; 
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

        protected static void OnPropertyChanged(string propertyName, CellRow instance)
        {
            if (instance.PropertyChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                instance.PropertyChanged(instance, eventArgs);
            }
        }
        #endregion
    }

    /// <summary>
    /// Equality Comparer for CellRows. Performs a Value Comparison of CellRow.Data AND CellRow.DataField.
    /// </summary>
    public class CellRowDataValueComparer : IEqualityComparer<CellRow>
    {
        public bool Equals(CellRow x, CellRow y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (x.Data == y.Data && x.DataField == y.DataField)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public int GetHashCode(CellRow obj)
        {
            int hCode = obj.Data.GetHashCode() ^ (int)obj.DataField;
            return hCode.GetHashCode();
        }
    }
}
