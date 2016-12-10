﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Describes the Content and Layout of a Single Row within a Grid. Only provides abstract TextBlock
    /// rendering, requires a Parent LabelCell and accsess to its Grid in order to render correctly.
    /// </summary>
    public class CellRow : RowDefinition, INotifyPropertyChanged
    {
        #region Constructors.
        static CellRow()
        {
            // Override HeightProperty's Metadata.
            var heightMetadata = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnHeightPropertyChanged));
            HeightProperty.OverrideMetadata(typeof(CellRow), heightMetadata);
        }

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

            // Setup Border.
            Border.Background = DeSelectedBrush;

            // Setup TextBlock.
            TextBlock.VerticalAlignment = VerticalAlignment.Center;
            TextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock.Background = Brushes.Transparent;

            HeightMode = CellParent.RowHeightMode;

            // Assign TextBlock to Border then Border to Cellparent's Grid.
            Border.Child = TextBlock;
            CellParent.Grid.Children.Add(Border);

            // Events.
            Border.MouseEnter += Border_MouseEnter;
            Border.MouseDown += Border_MouseDown;
            Border.MouseUp += Border_MouseUp;
            Border.MouseLeave += Border_MouseLeave;

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
        protected TextBlock TextBlock = new TextBlock();

        /// <summary>
        /// Abstract Rendering Target. Do not write directly to this Field. Internal use only.
        /// </summary>
        public Border Border = new Border();

        /// <summary>
        /// Tracks whether a Data Layout Pass is in progress or not.
        /// </summary>
        public bool IsInDataLayoutPass = false;
        #endregion

        #region Protected Fields.
        protected static SolidColorBrush DeSelectedBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        protected static SolidColorBrush SelectedBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
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
                        // Clear Row Height Property so that it's value can fall back to Templated Value.
                        ClearValue(RowHeightProperty);
                        Height = new GridLength(RowHeight, GridUnitType.Star);
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

        #region Dependency Property Overrides.
        private static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;

            // Signal change of Height to Cell Parent.
            OnPropertyChanged(HeightProperty.Name, instance);
        }

        private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
 
        }

        #endregion

        #region Dependency Properties


        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Color), typeof(CellRow), new FrameworkPropertyMetadata(Colors.Black,
                new PropertyChangedCallback(OnTextColorPropertyChanged), new CoerceValueCallback(CoerceTextColorProperty)));

        private static object CoerceTextColorProperty(DependencyObject d, object baseValue)
        {
            var instance = d as CellRow;
            var backgroundBrush = instance.CellParent.Background as SolidColorBrush;
            Color backgroundColor = backgroundBrush.Color;

            return LabelCell.AdjustTextColorLuma(backgroundColor);
        }

        private static void OnTextColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var newValue = (Color)e.NewValue;

            // Pass value onto TextBlock.
            instance.TextBlock.Foreground = new SolidColorBrush(newValue);
        }

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
            var instance = d as CellRow;
            var dataLayout = (DataLayout)value;
            var data = instance.Data;

            if (dataLayout.FirstIndex + dataLayout.Length > data.Length)
            {
                return new DataLayout();
            }

            else
            {
                return dataLayout;
            }

        }

        private static void OnDataLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var layout = (DataLayout)e.NewValue;
            var data = instance.Data;

            // Update Textblock Text, but first, Remove any occurances of non ASCII Characters.
            string incomingText = instance.Data.Substring(layout.FirstIndex, layout.Length);
            instance.TextBlock.Text = Regex.Replace(incomingText, @"[^\u0000-\u007F]+", string.Empty);

            instance.CoerceValue(ActualFontSizeProperty);
            instance.CoerceValue(RowHeightProperty);
            instance.CoerceValue(TextColorProperty);
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
            var newValue = (string)e.NewValue;
            var dataField = instance.DataField;
            var dataReference = instance.CellParent.DataReference;
            var parentDataMode = instance.CellParent.CellDataMode;
            var font = instance.Font;
            var fontSize = instance.ActualFontSize;

            if (dataReference != null)
            {
                // Update Model.
                dataReference.SetData(newValue, dataField);
            }

            if (parentDataMode == CellDataMode.MultiField)
            {
                // DataLayout won't be set by ParentCell. Have to set it here instead.
                instance.DataLayout = new DataLayout(newValue, font, fontSize);
            }

            
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
                new FrameworkPropertyMetadata(LabelField.NoAssignment, new PropertyChangedCallback(OnDataFieldPropertyChanged)));

        private static void OnDataFieldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var newValue = (LabelField)e.NewValue;
            var dataReference = instance.CellParent.DataReference;

            if (dataReference != null)
            {
                instance.Data = dataReference.GetData(newValue);
            }
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
            DependencyProperty.Register("DesiredFontSize", typeof(double), typeof(CellRow), new FrameworkPropertyMetadata(12d, 
                new PropertyChangedCallback(OnDesiredFontSizePropertyChanged), new CoerceValueCallback(CoerceDesiredFontSize)));


        private static object CoerceDesiredFontSize(DependencyObject d, object baseValue)
        {
            double desiredFontSize = (double)baseValue;

            if (desiredFontSize <= 0d)
            {
                desiredFontSize = 1d;
            }

            return desiredFontSize;
        }

        // Property Changed Callback Handler.
        private static void OnDesiredFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var fontSize = (double)e.NewValue;

            instance.ActualFontSize = fontSize;         
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
                new FrameworkPropertyMetadata(12d, new PropertyChangedCallback(OnActualFontSizePropertyChanged),
                    new CoerceValueCallback(CoerceActualFontSize)));

        private static object CoerceActualFontSize(DependencyObject d, object value)
        {
            var instance = d as CellRow;
            double newValue = (double)value;
            var data = instance.DataLayout.DisplayedText;
            var font = instance.Font;
            var availableWidth = instance.AvailableTextWidth;
            var availableHeight = instance.AvailableTextHeight;
            ScaleDirection ignore;

            // Scale Font Size to Fit Row.
            newValue = LabelCell.ScaleDownFontSize(data, font, newValue, availableWidth, availableHeight,
                ScaleDirection.Both, out ignore);

            // Coerce Value into acceptable Boundries.
            if (newValue <= 0.0d)
            {
                newValue =  0.1d;
            }

            else if (newValue >= double.PositiveInfinity)
            {
                newValue = 300d;
            }


            return newValue;
        }

        private static void OnActualFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            double value = (double)e.NewValue;

            instance.TextBlock.FontSize = value;

            // Coerce RowHeight.
            instance.CoerceValue(RowHeightProperty);
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
                new FrameworkPropertyMetadata(new Typeface("Arial"),
                    new PropertyChangedCallback(OnFontPropertyChanged)));

        private static void OnFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var typeface = (Typeface)e.NewValue;

            // Set TextBlock State.
            instance.TextBlock.FontFamily = typeface.FontFamily;
            instance.TextBlock.FontStretch = typeface.Stretch;
            instance.TextBlock.FontStyle = typeface.Style;
            instance.TextBlock.FontWeight = typeface.Weight;

            instance.CoerceValue(ActualFontSizeProperty);
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

        private static object CoerceRowHeight(DependencyObject d, object value)
        {
            var instance = d as CellRow;

            // rowQty must atleast be 0. This method belongs to a Row Object therefore atleast one
            // must exist in the collection. It may just not have been added yet.
            int rowQty = instance.CellParent.RowCount == 0 ? 1 : instance.CellParent.RowCount;
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

        private static void OnRowHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            var value = (double)e.NewValue;
            var heightMode = instance.HeightMode;
            GridLength currentHeight = instance.Height;

            // Copy Values from currentHeight into newHeight and Assign to Height Property.
            GridLength newHeight = new GridLength(value, currentHeight.GridUnitType);
            instance.Height = newHeight;

            instance.CoerceValue(ActualFontSizeProperty);
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
            Grid.SetRow(instance.Border, (int)e.NewValue);
        }



        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(CellRow),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedPropertyChanged)));

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellRow;
            bool selected = (bool)e.NewValue;

            // Render Cell Selection State.
            if (selected)
            {
                instance.Border.Background = SelectedBrush;
            }

            else
            {
                // Deselected.
                instance.Border.Background = DeSelectedBrush;
            }

            // Notify Parent Cell.
            OnPropertyChanged(nameof(IsSelected), instance);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Internally calls SetCurrentValue() to set DataField to desired value without changing Value Source.
        /// </summary>
        /// <param name="dataField"></param>
        public void SetDataFieldCurrentValue(LabelField dataField)
        {
            SetCurrentValue(DataFieldProperty, dataField);
        }

        /// <summary>
        /// Internally calls SetCurrentValue() to set Font to desired Value without changing Value Source.
        /// </summary>
        /// <param name="font"></param>
        public void SetFontCurrentValue(Typeface font)
        {
            SetCurrentValue(FontProperty, font);
        }
        #endregion

        #region Overrides.
        public override string ToString()
        {
            return "Row " + (Index + 1) + ":   " + Data; 
        }
        #endregion

        #region Events
        // MouseEnter.
        public new event MouseEventHandler MouseEnter;

        protected new void OnMouseEnter(MouseEventArgs e)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, e);
            }
        }

        // MouseDown.
        public new event MouseButtonEventHandler MouseDown;

        protected new void OnMouseDown(MouseButtonEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        // MouseUp.
        public new event MouseButtonEventHandler MouseUp;

        protected new void OnMouseUp(MouseButtonEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
        }

        // MouseLeave.
        public new event MouseEventHandler MouseLeave;

        protected new void OnMouseLeave(MouseEventArgs e)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, e);
            }
        }
        #endregion

        #region Event Handlers
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            // Forward Event.
            OnMouseEnter(e);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Forward Event.
            OnMouseDown(e);
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Forward Event.
            OnMouseUp(e);
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            // Forward Event.
            OnMouseLeave(e);
            
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
}
