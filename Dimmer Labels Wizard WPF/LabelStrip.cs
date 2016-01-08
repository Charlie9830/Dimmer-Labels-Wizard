using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Globalization;
using System.Printing;
using System.Diagnostics;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelStrip : Grid
    {
        #region Constructors
        static LabelStrip()
        {

        }

        /// <summary>
        /// Generates a blank LabelStrip.
        /// </summary>
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

            // Row Defintion. (Executed order affects Visual Order).
            RowDefinitions.Add(_UpperDeadSpaceGridRow);
            RowDefinitions.Add(_UpperContentGridRow);
            RowDefinitions.Add(_DividerGridRow);
            RowDefinitions.Add(_LowerContentGridRow);
            RowDefinitions.Add(_LowerDeadSpaceGridRow);

            // Column Defintion. (Executed order affects Visual Order).
            ColumnDefinitions.Add(_LeftDeadSpaceGridColumn);
            ColumnDefinitions.Add(_ContentGridColumn);
            ColumnDefinitions.Add(_RightDeadSpaceGridColumn);

            // Row Height Setting.
            _UpperDeadSpaceGridRow.Height = new GridLength(1d, GridUnitType.Star);
            _UpperContentGridRow.Height = new GridLength(70d, GridUnitType.Auto);
            _DividerGridRow.Height = new GridLength(_StripDividerDistance);
            _LowerContentGridRow.Height = new GridLength(70d, GridUnitType.Auto);
            _LowerDeadSpaceGridRow.Height = new GridLength(1d, GridUnitType.Star);

            // Column Width Setting.
            _LeftDeadSpaceGridColumn.Width = new GridLength(1d, GridUnitType.Star);
            _ContentGridColumn.Width = new GridLength(1d, GridUnitType.Auto);
            _RightDeadSpaceGridColumn.Width = new GridLength(1d, GridUnitType.Star);

            // Row Setting.
            SetRow(_UpperStackPanel, 1);
            SetRow(_LowerStackPanel, 3);

            // Column Setting.
            SetColumn(_UpperStackPanel, 1);
            SetColumn(_LowerStackPanel, 1);

            // Background. (Background must be set to a Brush in order to consume Mouse Events.)
            Background = Brushes.Transparent;

            // Events
            MouseDown += LabelStrip_MouseDown;
        }

        public LabelStrip(LabelStripStorage storage)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constants
        private static double unitConversionRatio = 96d / 25.4d;
        private static double _SingleLabelStripUpperHeightRatio = 0.25d;
        private static double _SingleLabelStripLowerHeightRatio = 0.75d;
        private static double _StripDividerDistance = 40d;
        #endregion

        #region Fields
        public RackType RackUnitType;
        public int RackNumber;

        // StackPanels.
        protected StackPanel _UpperStackPanel = new StackPanel();
        protected StackPanel _LowerStackPanel = new StackPanel();

        // Grid Row Definitions.
        protected RowDefinition _UpperDeadSpaceGridRow = new RowDefinition();
        protected RowDefinition _UpperContentGridRow = new RowDefinition();
        protected RowDefinition _DividerGridRow = new RowDefinition();
        protected RowDefinition _LowerContentGridRow = new RowDefinition();
        protected RowDefinition _LowerDeadSpaceGridRow = new RowDefinition();

        // Grid Column Defintions.
        protected ColumnDefinition _LeftDeadSpaceGridColumn = new ColumnDefinition();
        protected ColumnDefinition _ContentGridColumn = new ColumnDefinition();
        protected ColumnDefinition _RightDeadSpaceGridColumn = new ColumnDefinition();

        private bool _InMouseSelectionEvent = false;
        #endregion

        #region CLR Properties.
        #endregion

        #region Dependency Properties



        public IEnumerable<Merge> Mergers
        {
            get { return (IEnumerable<Merge>)GetValue(MergersProperty); }
            set { SetValue(MergersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mergers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MergersProperty =
            DependencyProperty.Register("Mergers", typeof(IEnumerable<Merge>), typeof(LabelStrip),
                new PropertyMetadata(null, new PropertyChangedCallback(OnMergersPropertyChanged)));

        private static void OnMergersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= instance.Mergers_CollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += instance.Mergers_CollectionChanged;

                // Handle Existing Elements
                var collection = e.NewValue as IEnumerable<Merge>;
                foreach (var element in collection)
                {
                    instance.Merge(element);
                }
            }
        }

        private void Mergers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var merge = element as Merge;
                    Merge(merge);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var merge = element as Merge;
                    DeMerge(merge);
                }
            }
        }

        public double StripWidth
        {
            get { return (double)GetValue(StripWidthProperty); }
            set { SetValue(StripWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StripWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StripWidthProperty =
            DependencyProperty.Register("StripWidth", typeof(double), typeof(LabelStrip),
                new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnStripWidthPropertyChanged),
                    new CoerceValueCallback(CoerceStripWidth)));

        private static object CoerceStripWidth(DependencyObject d, object value)
        {
            var width = (double)value;

            if (width < 1d)
            {
                return 1d;
            }
            else
            {
                return width;
            }
        }

        private static void OnStripWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var upperCells = instance.UpperCells;
            var upperCellCount = instance.UpperCellCount;
            var lowerCellCount = instance.LowerCellCount;
            var lowerCells = instance.LowerCells;
            var newValue = (double)e.NewValue;
            double newUpperWidth = newValue / upperCellCount;
            double newLowerWidth = newValue / lowerCellCount;

            // Push new Width to child Cells.
            foreach (var element in upperCells)
            {
                element.Width = newUpperWidth;
            }

            foreach (var element in lowerCells)
            {
                element.Width = newLowerWidth;
            }
        }

        public IEnumerable<DimmerDistroUnit> DataSource
        {
            get { return (IEnumerable<DimmerDistroUnit>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IEnumerable<DimmerDistroUnit>),
                typeof(LabelStrip), new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnDataSourcePropertyChanged)));

        private static void OnDataSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;

            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= instance.DataSource_CollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += instance.DataSource_CollectionChanged;

                // Handle already existing items.
                instance.RefreshCellDataSources(newCollection as IEnumerable<DimmerDistroUnit>);
            }
        }

        public List<LabelCellTemplate> UpperCellTemplates
        {
            get { return (List<LabelCellTemplate>)GetValue(UpperCellTemplatesProperty); }
            set { SetValue(UpperCellTemplatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpperCellsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpperCellTemplatesProperty =
            DependencyProperty.Register("UpperCellTemplates", typeof(List<LabelCellTemplate>),
                typeof(LabelStrip), new FrameworkPropertyMetadata(new List<LabelCellTemplate>(),
                    new PropertyChangedCallback(OnUpperCellTemplatesPropertyChanged)));

        private static void OnUpperCellTemplatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var templates = e.NewValue as IEnumerable<LabelCellTemplate>;
            var upperCells = instance.UpperCells;

            // Push changed style to Upper Cell Elements.
            for (int index = 0; index < templates.Count() && index < upperCells.Count; index++ )
            {
                upperCells[index].Style = templates.ElementAt(index);
            }
        }


        public List<LabelCellTemplate> LowerCellTemplates
        {
            get { return (List<LabelCellTemplate>)GetValue(LowerCellTemplatesProperty); }
            set { SetValue(LowerCellTemplatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerCellsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LowerCellTemplatesProperty =
            DependencyProperty.Register("LowerCellTemplates", typeof(List<LabelCellTemplate>), typeof(LabelStrip),
                new FrameworkPropertyMetadata(new List<LabelCellTemplate>(), new PropertyChangedCallback(OnLowerCellTemplatesPropertyChanged)));

        private static void OnLowerCellTemplatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var templates = e.NewValue as IEnumerable<LabelCellTemplate>;
            var lowerCells = instance.LowerCells;

            // Push changed style to Upper Cell Elements.
            for (int index = 0; index < templates.Count() && index < lowerCells.Count; index++ )
            {
                lowerCells[index].Style = templates.ElementAt(index);
            }
        }


        public double StripHeight
        {
            get { return (double)GetValue(StripHeightProperty); }
            set { SetValue(StripHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StripHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StripHeightProperty =
            DependencyProperty.Register("StripHeight", typeof(double), typeof(LabelStrip),
                new FrameworkPropertyMetadata(70d, new PropertyChangedCallback(OnStripHeightPropertyChanged),
                    new CoerceValueCallback(CoerceStripHeight)));

        private static object CoerceStripHeight(DependencyObject d, object value)
        {
            var height = (double)value;

            if (height < 1d)
            {
                return 1d;
            }
            else
            {
                return height;
            }
        }

        private static void OnStripHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;

            // Push new Height to Cell children.
            AdjustCellHeights(instance);

            instance.CoerceValue(HeightProperty);
        }

        public double LineWeight
        {
            get { return (double)GetValue(LineWeightProperty); }
            set { SetValue(LineWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineWeightProperty =
            DependencyProperty.Register("LineWeight", typeof(double),
                typeof(LabelStrip), new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnLineWeightPropertyChanged),
                    new CoerceValueCallback(CoerceLineWeight)));

        private static object CoerceLineWeight(DependencyObject d, object value)
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

        private static void OnLineWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            double lineWeight = (double)e.NewValue;
            CellCollection upperCells = instance.UpperCells;
            CellCollection lowerCells = instance.LowerCells;

            foreach (var element in upperCells)
            {
                element.LeftWeight = lineWeight;
                element.TopWeight = lineWeight;
                element.RightWeight = lineWeight;
                element.BottomWeight = lineWeight;
            }

            foreach (var element in lowerCells)
            {
                element.LeftWeight = lineWeight;
                element.TopWeight = lineWeight;
                element.RightWeight = lineWeight;
                element.BottomWeight = lineWeight;
            }
            
        }

        public int UpperCellCount
        {
            get { return (int)GetValue(UpperCellCountProperty); }
            set { SetValue(UpperCellCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpperCellCountProperty =
            DependencyProperty.Register("UpperCellCount", typeof(int), typeof(LabelStrip),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnUpperCellCountPropertyChanged),
                    new CoerceValueCallback(CoerceUpperCellCount)));

        private static object CoerceUpperCellCount(DependencyObject d, object value)
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

        private static void OnUpperCellCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            int newCount = (int)e.NewValue;
            int oldCount = (int)e.OldValue;
            int difference = newCount - oldCount;
            CellCollection upperCells = instance.UpperCells;
            int collectionCount = upperCells.Count;

            if (newCount == collectionCount)
            {
                // Collection modification has been triggered elsewhere,
                // and has already taken place. No further action is 
                // required from CellCount.

                return;
            }

            if (difference < 0)
            {
                difference = Math.Abs(difference);

                // Decrease collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    upperCells.RemoveAt(upperCells.Count - 1);
                }
            }

            else if (difference > 0)
            {
                // Increase collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    upperCells.Add(new LabelCell());
                }
            }
        }



        public int LowerCellCount
        {
            get { return (int)GetValue(LowerCellCountProperty); }
            set { SetValue(LowerCellCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerCellCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LowerCellCountProperty =
            DependencyProperty.Register("LowerCellCount", typeof(int), typeof(LabelStrip),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnLowerCellCountPropertyChanged),
                    new CoerceValueCallback(CoerceLowerCellCount)));

        private static object CoerceLowerCellCount(DependencyObject d, object value)
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

        private static void OnLowerCellCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            int newCount = (int)e.NewValue;
            int oldCount = (int)e.OldValue;
            int difference = newCount - oldCount;
            CellCollection lowerCells = instance.LowerCells;
            int collectionCount = lowerCells.Count;

            if (newCount == collectionCount)
            {
                // Collection modification has been triggered elsewhere,
                // and has already taken place. No further action is 
                // required from CellCount.

                return;
            }

            if (difference < 0)
            {
                difference = Math.Abs(difference);

                // Decrease collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    lowerCells.RemoveAt(lowerCells.Count - 1);
                }
            }

            else if (difference > 0)
            {
                // Increase collection Size.
                for (int count = 1; count <= difference; count++)
                {
                    lowerCells.Add(new LabelCell());
                }
            }
        }

      


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
            var value = (LabelStripMode)e.NewValue;

            // Push new Heights to Child Cells.
            AdjustCellHeights(instance);

            // Set state of Divider Row.
            if (value == LabelStripMode.Dual)
            {
                instance._DividerGridRow.Height = new GridLength(_StripDividerDistance);
            }

            else
            {
                instance._DividerGridRow.Height = new GridLength(0d);
            }

            // instance.CoerceValue(HeightProperty);
        }

       

        public IEnumerable<LabelCell> SelectedCells
        {
            get { return (IEnumerable<LabelCell>)GetValue(SelectedCellsProperty); }
            set { SetValue(SelectedCellsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCells.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCellsProperty =
            DependencyProperty.Register("SelectedCells", typeof(IEnumerable<LabelCell>), typeof(LabelStrip),
                new FrameworkPropertyMetadata(new List<LabelCell>(),
                    new PropertyChangedCallback(OnSelectedCellsPropertyChanged)));

        private static void OnSelectedCellsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;

            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= instance.SelectedCells_CollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += instance.SelectedCells_CollectionChanged;
            }
        }

        #endregion

        #region Override and Overriden Handlers
        
        #endregion

        #region Event Handlers
        private void DataSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as IEnumerable<DimmerDistroUnit>;

            // Refresh Cell Data Sources.
            RefreshCellDataSources(collection);
        }

        private static void UpperCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                        // Add to StackPanel
                        instance._UpperStackPanel.Children.Insert(e.NewStartingIndex, cell);

                        // Set Vertical Position Flag.
                        cell.CellVerticalPosition = CellVerticalPosition.Upper;

                        // Set Height.
                        if (instance.StripMode == LabelStripMode.Dual)
                        {
                            cell.Height = instance.StripHeight;
                        }

                        else
                        {
                            cell.Height = instance.StripHeight * _SingleLabelStripUpperHeightRatio;
                        }


                        // Set Template
                        if (collection.IndexOf(cell) < instance.LowerCellTemplates.Count)
                        {
                            cell.Style = instance.LowerCellTemplates[collection.IndexOf(cell)];
                        }

                        // Connect Event Handler.
                        cell.PropertyChanged += instance.UpperCell_PropertyChanged;
                        cell.MouseDown += instance.Cell_MouseDown;
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;

                    // Remove from StackPanel.
                    instance._UpperStackPanel.Children.Remove(cell);

                    // Disconnect Event Handler.
                    cell.PropertyChanged -= instance.UpperCell_PropertyChanged;
                    cell.MouseDown -= instance.Cell_MouseDown;
                }
            }

            // Set Cell Widths.
            foreach (var cell in collection)
            {
                cell.BaseWidth = instance.StripWidth / instance.DataSource.Count();
            }

            // Update Horizontal Position Indexes.
            for (int index = 0; index < collection.Count; index++)
            {
                collection[index].HorizontalIndex = index;
            }

            // Coercion.
            instance.CoerceValue(HeightProperty);

            // Push change to Cell Count Property.
            instance.UpperCellCount = collection.Count;
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
                        // Add to Stackpanel
                        instance._LowerStackPanel.Children.Insert(e.NewStartingIndex, cell);

                        // Set Vertical Position Flag.
                        cell.CellVerticalPosition = CellVerticalPosition.Lower;

                        // Set Height
                        if (instance.StripMode == LabelStripMode.Dual)
                        {
                            cell.Height = instance.StripHeight;
                        }

                        else
                        {
                            cell.Height = instance.StripHeight * _SingleLabelStripLowerHeightRatio;
                        }

                        // Set Template
                        if (collection.IndexOf(cell) < instance.LowerCellTemplates.Count)
                        {
                            cell.Style = instance.LowerCellTemplates[collection.IndexOf(cell)];
                        }
                        
                        // Connect event Handlers.
                        cell.PropertyChanged += instance.LowerCell_PropertyChanged;
                        cell.MouseDown += instance.Cell_MouseDown;
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;
                    instance._LowerStackPanel.Children.Remove(cell);

                    cell.PropertyChanged -= instance.LowerCell_PropertyChanged;
                    cell.MouseDown -= instance.Cell_MouseDown;
                }
            }

            // Set Cell Widths.
            foreach (var cell in collection)
            {
                cell.BaseWidth = instance.StripWidth / instance.DataSource.Count();
            }

            // Update Horizontal Position Indexes.
            for (int index = 0; index < collection.Count; index++)
            {
                collection[index].HorizontalIndex = index;
            }

            // Coercion.
            instance.CoerceValue(HeightProperty);

            // Push change to Cell Count Property.
            instance.LowerCellCount = collection.Count;
        }

        protected void UpperCell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = sender as LabelCell;

            // IsSelected
            if (e.PropertyName == nameof(cell.IsSelected))
            {
                if (_InMouseSelectionEvent == false)
                {
                    if (cell.IsSelected == true && SelectedCells.Contains(cell) == false)
                    {
                        ((IList<LabelCell>)SelectedCells).Add(cell);
                        cell.IsSelected = true;
                    }

                    else
                    {
                        ((IList<LabelCell>)SelectedCells).Remove(cell);
                        cell.IsSelected = false;
                    }
                }
            }

            // LeftWeight.
            if (e.PropertyName == nameof(cell.LeftWeight))
            {
                int cellIndex = UpperCells.IndexOf(cell);
                double desiredWeight = cell.LeftWeight;

                if (cellIndex == 0)
                {
                    // Lefthand Boundary Cell.
                    cell.ActualLeftWeight = desiredWeight;
                }

                else
                {
                    // Non boundary Cell.
                    cell.ActualLeftWeight = desiredWeight / 2;
                    UpperCells[cellIndex - 1].ActualRightWeight = desiredWeight / 2;
                }
            }

            // TopWeight
            if (e.PropertyName == nameof(cell.TopWeight))
            {
                cell.ActualTopWeight = cell.TopWeight;
            }

            // RightWeight
            if (e.PropertyName == nameof(cell.RightWeight))
            {
                int cellIndex = UpperCells.IndexOf(cell);
                double desiredWeight = cell.RightWeight;

                if (cellIndex == UpperCells.Count - 1)
                {
                    // Righthand Boundary cell.
                    cell.ActualRightWeight = desiredWeight;
                }

                else
                {
                    // Non Boundary cell.
                    cell.ActualRightWeight = desiredWeight / 2;
                    UpperCells[cellIndex + 1].ActualLeftWeight = desiredWeight / 2;
                }
            }

            // BottomWeight
            if (e.PropertyName == nameof(cell.BottomWeight))
            {
                cell.ActualBottomWeight = cell.BottomWeight;
            }
        }

        protected void LowerCell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void SelectedCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        #region Mouse Event Handlers.
        private void Cell_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cell = sender as LabelCell;

            _InMouseSelectionEvent = true;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                // Additive Selection.
                // Add Cell to Selection.
                if (SelectedCells.Contains(cell) == false)
                {
                    ((IList<LabelCell>)SelectedCells).Add(cell);
                    cell.IsSelected = true;
                }

                // Short out Event.
                e.Handled = true;
            }

            else
            {
                // Exclusive Selection.
                if (cell.IsSelected == true)
                {
                    // Exclusive Selection with actively selected Rows, Don't dump selected Rows.

                    // Clear SelectedRows EXCEPT the Cell that generated the Event.
                    for (int index = 0; index < SelectedCells.Count();)
                    {
                        if (SelectedCells.ElementAt(index) != cell)
                        {
                            SelectedCells.ElementAt(index).IsSelected = false;
                            ((IList<LabelCell>)SelectedCells).RemoveAt(index);
                        }

                        else
                        {
                            index++;
                        }
                    }
                }
                
                else
                {
                    // Selection has shifted to a new Cell. Okay it dump currently selected Cells AND rows.

                    // Clear SelectedRows.
                    while (SelectedCells.Count() > 0)
                    {
                        SelectedCells.ElementAt(SelectedCells.Count() - 1).IsSelected = false;
                        ((IList<LabelCell>)SelectedCells).RemoveAt(SelectedCells.Count() - 1);
                    }

                    // Add new Selection.
                    ((IList<LabelCell>)SelectedCells).Add(cell);
                    cell.IsSelected = true;
                }

                // Short out Event.
                e.Handled = true;
            }

            _InMouseSelectionEvent = false;
        }

        private void LabelStrip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // A Mouse down event has been triggered within the LabelStrip and has
            // not been Short Circuited by the Cell_MouseDown event Handler.
            if (SelectedCells.Count() > 0)
            {
                _InMouseSelectionEvent = true;

                // Clear Selections.
                while (SelectedCells.Count() > 0)
                {
                    SelectedCells.ElementAt(SelectedCells.Count() - 1).IsSelected = false;
                    ((IList<LabelCell>)SelectedCells).RemoveAt(SelectedCells.Count() - 1);
                }

                _InMouseSelectionEvent = false;
            }
        }
        #endregion

        #endregion

        #region Public Methods
        public void Merge(Merge mergeInstructions)
        {
            // Collect CellsList.
            ObservableCollection<LabelCell> cellCollection;

            if (mergeInstructions.VerticalPosition == CellVerticalPosition.Upper)
            {
                cellCollection = UpperCells;
                
            }
            else
            {
                cellCollection = LowerCells;
            }

            List<LabelCell> cellsList = cellCollection.ToList();

            LabelCell primaryCell = cellsList.Find(item => item.DataReference == mergeInstructions.PrimaryUnit);
            var consumedCells = cellsList.Where(item => mergeInstructions.ConsumedUnits.Contains(item.DataReference));

            // Expand primaryCell's Dimensions
            double newWidth = primaryCell.Width;
            foreach (var element in consumedCells)
            {
                newWidth += element.Width;
            }

            primaryCell.Width = newWidth;
            
            // Remove Consumed Cells via Removal of their Data Source Objects.
            foreach (var element in consumedCells)
            {
                cellCollection.Remove(element);
            }

            // Store consumed Data References.
            primaryCell.ConsumedReferences = new List<DimmerDistroUnit>(mergeInstructions.ConsumedUnits);
        }

        public void DeMerge(Merge mergeInstructions)
        {
            ObservableCollection<LabelCell> cellCollection;

            if (mergeInstructions.VerticalPosition == CellVerticalPosition.Upper)
            {
                cellCollection = UpperCells;

            }
            else
            {
                cellCollection = LowerCells;
            }

            List<LabelCell> cellsList = cellCollection.ToList();

            LabelCell primaryCell = cellsList.Find(item => item.DataReference == mergeInstructions.PrimaryUnit);
            int primaryCellIndex = cellsList.IndexOf(primaryCell);

            // Set Primary Cell back to it's Base Dimensions.
            primaryCell.Width = primaryCell.BaseWidth;

            // Generate new Cells.
            if (mergeInstructions.VerticalPosition == CellVerticalPosition.Upper)
            {
                UpperCellCount += mergeInstructions.ConsumedUnits.Count();
            }

            else
            {
                LowerCellCount += mergeInstructions.ConsumedUnits.Count();
            }

            // Clear Primary Cells ConsumedReferences List.
            primaryCell.ConsumedReferences.Clear();

            // Force Refresh of Cell Data References.
            RefreshCellDataSources(DataSource);

        }
        #endregion

        #region Private or Protected Methods
        /// <summary>
        /// Refreshes Cell Collections with new Data Source.
        /// </summary>
        /// <param name="newDataSource"></param>
        private void RefreshCellDataSources(IEnumerable<DimmerDistroUnit> newDataSource)
        {
            UpperCellCount = newDataSource.Count();
            LowerCellCount = newDataSource.Count();

            // Assign UpperCells DataReferences
            IEnumerator<LabelCell> upperCellsEnum = UpperCells.GetEnumerator();
            IEnumerator<DimmerDistroUnit> dataSourceEnum = newDataSource.GetEnumerator();

            // Iterate both UpperCells and DataSource.
            while (upperCellsEnum.MoveNext() && dataSourceEnum.MoveNext())
            {
                // Set Primary Data Reference.
                upperCellsEnum.Current.DataReference = dataSourceEnum.Current;

                if (upperCellsEnum.Current.IsMerged == true)
                {
                    // Cell is Merged, Set Consumed References.
                    int consumedReferencesCount = upperCellsEnum.Current.ConsumedReferences.Count;

                    for (int index = 0; index < consumedReferencesCount; index++)
                    {
                        if (dataSourceEnum.MoveNext())
                        {
                            upperCellsEnum.Current.ConsumedReferences[index] = dataSourceEnum.Current;
                        }
                    }
                }
            }

            // Reset
            dataSourceEnum.Reset();

            IEnumerator<LabelCell> lowerCellsEnum = LowerCells.GetEnumerator();

            // Iterate both LowerCells and DataSource.
            while (lowerCellsEnum.MoveNext() && dataSourceEnum.MoveNext())
            {
                // Set Primary Data Reference.
                lowerCellsEnum.Current.DataReference = dataSourceEnum.Current;

                if (lowerCellsEnum.Current.IsMerged == true)
                {
                    // Cell is Merged, Set Consumed References.
                    int consumedReferencesCount = lowerCellsEnum.Current.ConsumedReferences.Count;

                    for (int index = 0; index < consumedReferencesCount; index++)
                    {
                        if (dataSourceEnum.MoveNext())
                        {
                            upperCellsEnum.Current.ConsumedReferences[index] = dataSourceEnum.Current;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the Heights of Child Cells.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="newStripHeight"></param>
        /// <param name="upperCells"></param>
        /// <param name="lowerCells"></param>
        /// <param name="stripMode"></param>
        private static void AdjustCellHeights(LabelStrip instance)
        {
            CellCollection upperCells = instance.UpperCells;
            CellCollection lowerCells = instance.LowerCells;
            double newStripHeight = instance.StripHeight;
            LabelStripMode stripMode = instance.StripMode;

            foreach (var element in upperCells)
            {
                // Set Height.
                if (stripMode == LabelStripMode.Dual)
                {
                    element.Height = newStripHeight;
                }

                else
                {
                    element.Height = instance.StripHeight * _SingleLabelStripUpperHeightRatio;
                }
            }

            foreach (var element in lowerCells)
            {
                // Set Height.
                if (stripMode == LabelStripMode.Dual)
                {
                    element.Height = newStripHeight;
                }

                else
                {
                    element.Height = instance.StripHeight * _SingleLabelStripLowerHeightRatio;
                }
            }
        }

        private static bool CanCellsMerge(LabelCell cellA, LabelCell cellB)
        {
            // Primary clauses.
            if (cellA.CellDataMode != cellB.CellDataMode)
            {
                // Non matching Cell Data modes.
                return false;
            }

            if (cellA.RowCount == 0 && cellB.RowCount == 0)
            {
                // No Rows present, IE They are blank Cells.
                return false;
            }


            // Secondary clauses.
            if (cellA.CellDataMode == CellDataMode.SingleField)
            {
                // Single Field Mode.
                string aData = cellA.RowCount > 0 ? cellA.Rows.First().Data : string.Empty;
                string bData = cellB.RowCount > 0 ? cellB.Rows.First().Data : string.Empty;

                LabelField aDataField = cellA.RowCount > 0 ? cellA.Rows.First().DataField : LabelField.NoAssignment;
                LabelField bDataField = cellB.RowCount > 0 ? cellB.Rows.First().DataField : LabelField.NoAssignment;

                if (aData == bData && aDataField == bDataField)
                {
                    // Data and Data Fields Match.
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                // Mixed Field Mode.
                List<CellRow> aRows = cellA.Rows.ToList();
                List<CellRow> bRows = cellB.Rows.ToList();

                // Primary Clauses.
                if (aRows.Count != bRows.Count)
                {
                    return false;
                }

                if (aRows.Union(bRows, new CellRowDataValueComparer()).Count() == 0)
                {
                    // Union Operation return 0 count enumerable. aRows and bRows and Equal by Data Value
                    // and DataField Value.
                    return true;
                }

                else
                {
                    return false;
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
