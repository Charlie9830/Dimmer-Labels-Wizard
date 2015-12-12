using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Globalization;
using System.Printing;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelStrip : Grid
    {
        #region Constructors
        static LabelStrip()
        {
            // Metadata Options.
            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsArrange;
            options |= FrameworkPropertyMetadataOptions.AffectsMeasure;
            options |= FrameworkPropertyMetadataOptions.AffectsRender;
            options |= FrameworkPropertyMetadataOptions.AffectsParentArrange;
            options |= FrameworkPropertyMetadataOptions.AffectsParentMeasure;

            // Override Width Property.
            var widthOverride = new FrameworkPropertyMetadata(double.NaN, options,
                new PropertyChangedCallback(OnWidthPropertyChanged),
                    new CoerceValueCallback(CoerceWidthProperty));

            WidthProperty.OverrideMetadata(typeof(LabelStrip), widthOverride);

            // Override Height Property.
            var heightOverride = new FrameworkPropertyMetadata(double.NaN, options,
                new PropertyChangedCallback(OnHeightPropertyChanged),
                    new CoerceValueCallback(CoerceHeightProperty));

            HeightProperty.OverrideMetadata(typeof(LabelStrip), heightOverride);
        }

        public LabelStrip()
        {
            // Collection type Dependency properties.
            SetValue(UpperCellsPropertyKey, new CellCollection(this));
            SetValue(LowerCellsPropertyKey, new CellCollection(this)); 

            // Initialize
            _UpperStackPanel.Orientation = Orientation.Horizontal;
            _LowerStackPanel.Orientation = Orientation.Horizontal;
            Children.Add(_UpperStackPanel);
            Children.Add(_LowerStackPanel);

            RowDefinitions.Add(_UpperGridRow);
            RowDefinitions.Add(_LowerGridRow);

            SetRow(_UpperStackPanel, 0);
            SetRow(_LowerStackPanel, 1);
        }

        public LabelStrip(LabelStripStorage storage)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constants
        private static double unitConversionRatio = 96d / 25.4d;
        public static double LabelStripOffsetMultiplier = 1.5d; // Applied to Label Height to Offset Footer label From Header Label.
        #endregion

        #region Fields
        public RackType RackUnitType;
        public int RackNumber;

        protected StackPanel _UpperStackPanel = new StackPanel();
        protected StackPanel _LowerStackPanel = new StackPanel();

        protected RowDefinition _UpperGridRow = new RowDefinition();
        protected RowDefinition _LowerGridRow = new RowDefinition();
        #endregion

        #region CLR Properties

        #endregion

        #region Dependency Properties
        public CellCollection UpperCells
        {
            get { return (CellCollection)GetValue(UpperCellsProperty); }
        }

        // Using a DependencyProperty as the backing store for UpperCells.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey UpperCellsPropertyKey =
            DependencyProperty.RegisterReadOnly("UpperCells", typeof(CellCollection),
                typeof(LabelStrip), new FrameworkPropertyMetadata(new CellCollection(),
                    new PropertyChangedCallback(OnUpperCellsPropertyChanged)));

        public static readonly DependencyProperty UpperCellsProperty =
            UpperCellsPropertyKey.DependencyProperty;

        private static void OnUpperCellsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Disconnect Outgoing event.
            if (e.OldValue != null)
            {
                var collection = e.OldValue as CellCollection;
                collection.CollectionChanged -= UpperCells_CollectionChanged;
            }

            // Connect incoming event.
            if (e.NewValue != null)
            {
                var collection = e.NewValue as CellCollection;
                collection.CollectionChanged += UpperCells_CollectionChanged;
            }
        }

        public CellCollection LowerCells
        {
            get { return (CellCollection)GetValue(LowerCellsProperty); }
        }

        // Using a DependencyProperty as the backing store for LowerCells.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey LowerCellsPropertyKey =
            DependencyProperty.RegisterReadOnly("LowerCells", typeof(CellCollection),
                typeof(LabelStrip), new PropertyMetadata(new CellCollection(),
                    new PropertyChangedCallback(OnLowerCellsPropertyChanged)));

        public static readonly DependencyProperty LowerCellsProperty =
            LowerCellsPropertyKey.DependencyProperty;

        private static void OnLowerCellsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Disconnect Outgoing event.
            if (e.OldValue != null)
            {
                var collection = e.OldValue as CellCollection;
                collection.CollectionChanged -= LowerCells_CollectionChanged;
            }

            // Connect incoming event.
            if (e.NewValue != null)
            {
                var collection = e.NewValue as CellCollection;
                collection.CollectionChanged += LowerCells_CollectionChanged;
            }

        }

        public LabelStripMode StripMode
        {
            get { return (LabelStripMode)GetValue(StripModeProperty); }
            set { SetValue(StripModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StripMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StripModeProperty =
            DependencyProperty.Register("StripMode", typeof(LabelStripMode), typeof(LabelStrip),
                new FrameworkPropertyMetadata(LabelStripMode.Dual,
                    new PropertyChangedCallback(OnStripModePropertyChanged),
                    new CoerceValueCallback(CoerceStripMode)));

        private static object CoerceStripMode(DependencyObject d, object value)
        {
            return value;
        }

        private static void OnStripModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;

            instance.CoerceValue(HeightProperty);
        }



        #endregion

        #region Override and Overriden Handlers
        private static object CoerceWidthProperty(DependencyObject d, object value)
        {
            var instance = d as LabelStrip;
            double width = (double)value;
            IEnumerable<LabelCell> upperCells = instance.UpperCells;
            IEnumerable<LabelCell> lowerCells = instance.LowerCells;

            if (upperCells.Count() == 0 && lowerCells.Count() == 0)
            {
                return double.NaN;
            }

            // Calculate Total Width's.
            double upperTotalWidth = 0;
            double lowerTotalWidth = 0;

            foreach (var element in upperCells)
            {
                upperTotalWidth += element.Width;
            }

            foreach (var element in lowerCells)
            {
                lowerTotalWidth += element.Width;
            }

            // Return the larger of the two width's.
            return Math.Max(upperTotalWidth, lowerTotalWidth);
        }

        private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private static object CoerceHeightProperty(DependencyObject d, object value)
        {
            var instance = d as LabelStrip;
            double height = (double)value;
            LabelStripMode stripMode = instance.StripMode;
            double seperationDistance = 40d;
            List<LabelCell> upperCells = instance.UpperCells.ToList();
            List<LabelCell> lowerCells = instance.LowerCells.ToList();

            if (upperCells.Count == 0 && lowerCells.Count == 0)
            {
                return double.NaN;
            }

            // Calculate heights of tallest Upper and Lower Cells.
            double upperHeight = 0;
            double lowerHeight = 0;
            
            if (upperCells.Count != 0)
            {
               upperHeight = upperCells.OrderBy(item => item.Height).Last().Height;
            }
             
            if (lowerCells.Count != 0)
            {
                lowerHeight = lowerCells.OrderBy(item => item.Height).Last().Height;
            }

            double totalHeight = 0;
            switch (stripMode)
            {
                case LabelStripMode.Single:
                    totalHeight = lowerHeight + upperHeight;
                    break;
                case LabelStripMode.Dual:
                    totalHeight = lowerHeight + upperHeight + seperationDistance;
                    break;
                default:
                    totalHeight = lowerHeight + upperHeight + seperationDistance;
                    break;
            }

            return totalHeight;
            
        }

        private static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
        #endregion

        #region Event Handlers
        private static void UpperCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as CellCollection;
            LabelStrip instance = collection.Instance;
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cell = element as LabelCell;
                    if (instance._UpperStackPanel.Children.Contains(cell) == false)
                    {
                        instance._UpperStackPanel.Children.Add(cell);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;
                    instance._UpperStackPanel.Children.Remove(cell);
                }
            }
        }

        private static void LowerCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as CellCollection;
            LabelStrip instance = collection.Instance;
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cell = element as LabelCell;
                    if (instance._LowerStackPanel.Children.Contains(cell) == false)
                    {
                        instance._LowerStackPanel.Children.Add(cell);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;
                    instance._LowerStackPanel.Children.Remove(cell);
                }
            }
        }
        #endregion

        #region Print Helper Methods
        static Size GetMaxLabelStripDimensions()
        {
            double topHeight = Math.Max(UserParameters.DistroLabelHeightInMM * unitConversionRatio,
                UserParameters.DimmerLabelHeightInMM * unitConversionRatio);

            double topWidth = Math.Max(UserParameters.DistroLabelWidthInMM * unitConversionRatio,
                UserParameters.DimmerLabelWidthInMM * unitConversionRatio);

            Size returnSize = new Size(topWidth * 12, topHeight);

            return returnSize;
        }

        #endregion

        #region Serialization
        public LabelStripStorage GenerateStorage()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Interface Implementations
        public int CompareTo(LabelStrip other)
        {
            if (other.RackUnitType == RackUnitType)
            {
                return RackNumber - other.RackNumber;
            }

            return other.RackUnitType - RackUnitType;
        }
        #endregion
    }

    
    public class LabelStripStorage
    {
        // Needs Re Implementation.
    }
}
