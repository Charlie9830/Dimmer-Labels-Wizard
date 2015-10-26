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
        public CellRow(LabelCell parentLabelCell, LabelField dataField)
        {
            if (parentLabelCell == null)
            {
                throw new NullReferenceException("Parameter 'parentLabelCell' cannot be Null");
            }

            CellParent = parentLabelCell;
            DataField = dataField;

            // Setup TextBlock.
            TextBlock.VerticalAlignment = VerticalAlignment.Center;
            TextBlock.HorizontalAlignment = HorizontalAlignment.Center;

            // Initial Values.
            Data = CellParent.PreviousReference.GetData(DataField);
            FontSize = 12;
            Font = new Typeface("Arial");
            HeightMode = CellParent.RowHeightMode;

            // Assign TextBlock to CellParent's Grid.
            CellParent.Grid.Children.Add(TextBlock);           
            
            // Unit Testing Code.
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
        #endregion

        #region CLR Properties and Fields.
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
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnDataPropertyChanged)));

        // Property Changed Callback handler.
        private static void OnDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var data = e.NewValue as string;

            if (instance.TextBlock.Text != data)
            {
                // Set TextBlock Text, Coerce FontSize to ReMeasure
                // and if Parent LabelCell is not currently Setting this Rows Data. Signal property Change.
                instance.TextBlock.Text = data;
                instance.CoerceValue(FontSizeProperty);

                if (instance.CellParent.IsSettingData == false)
                {
                    OnPropertyChanged(DataProperty.Name, instance);
                }
            }
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
            var oldValue = (LabelField)e.OldValue;
            var newValue = (LabelField)e.NewValue;

            if (newValue != oldValue)
            {
                // Externally signal Parent Cell to Update Data.
                OnPropertyChanged(e.Property.Name, d as CellRow);
            }
        }

        /// <summary>
        /// Sets the Font Size for Text Rendering. This is a Dependency Property
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Font Size Backing Field.
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(CellRow), new FrameworkPropertyMetadata(1d, 
                new PropertyChangedCallback(OnFontSizePropertyChanged), new CoerceValueCallback(CoerceFontSize)));

        // Property Changed Callback Handler.
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var fontSize = (double)e.NewValue;

            if (instance.TextBlock.FontSize != fontSize)
            {
                instance.TextBlock.FontSize = fontSize;

                // Coerce RowHeight.
                instance.CoerceValue(RowHeightProperty);
            }
        }

        private static object CoerceFontSize(DependencyObject d, object baseValue)
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

            // Downsize FontSize if required.
            var ignore = new ScaleDirection();
            double fontSize = HelperScaleDownFont(data, font, desiredFontSize,
                width, height, ScaleDirection.Both, out ignore);

            return fontSize;
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

            instance.CoerceValue(FontSizeProperty);
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

            instance.CoerceValue(FontSizeProperty);
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
            double fontSize = instance.FontSize;

            CellRowHeightMode heightMode = instance.HeightMode;
            if (heightMode == CellRowHeightMode.Static)
            {
                return cellHeight / rowQty;
            }

            // If Heightmode is set to Automatic Adjust Row Heights.
            if (heightMode == CellRowHeightMode.Automatic)
            {
                double textHeight = HelperMeasureText(data, font, fontSize).Height;
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

        #region Helper Methods
        /// <summary>
        /// Measures the size of provided text. Returns Size 0 if a null or Empty string was provided.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static Size HelperMeasureText(string data, Typeface font, double fontSize)
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
        public static double HelperScaleDownFont(string data, Typeface font, double fontSize,
            double containerWidth, double containerHeight, ScaleDirection scaleDirection,
            out ScaleDirection fontScaledDirection)
        {
            Size textSize = HelperMeasureText(data, font, fontSize);

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
    }
}
