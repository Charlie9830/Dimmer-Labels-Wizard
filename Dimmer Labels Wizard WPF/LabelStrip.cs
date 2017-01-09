﻿using System;
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
            // DP Collections.
            SetValue(StripSpacersProperty, new List<StripSpacer>());
            ClearValue(StripSpacersProperty); // Clear the Value so that it may be modified by a Style Setter.
            // These two lines of code may be acheiving nothing as they are probably just undoing themselves.

            // Event Handlers.
            UpperCells.CollectionChanged += UpperCells_CollectionChanged;
            LowerCells.CollectionChanged += LowerCells_CollectionChanged;

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

        #endregion

        #region Constants
        private static double unitConversionRatio = 96d / 25.4d;
        private static double _SingleLabelStripUpperHeightRatio = 0.35d;
        private static double _SingleLabelStripLowerHeightRatio = 0.65d;
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
        protected ObservableCollection<LabelCell> _UpperCells = new ObservableCollection<LabelCell>();

        public ObservableCollection<LabelCell> UpperCells
        {
            get { return _UpperCells; }
        }

        protected ObservableCollection<LabelCell> _LowerCells = new ObservableCollection<LabelCell>();

        public ObservableCollection<LabelCell> LowerCells
        {
            get { return _LowerCells; }
        }

        #endregion

        #region Dependency Properties


        public LabelCell MouseOverCell
        {
            get { return (LabelCell)GetValue(MouseOverCellProperty); }
            set { SetValue(MouseOverCellProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverCell.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverCellProperty =
            DependencyProperty.Register("MouseOverCell", typeof(LabelCell), typeof(LabelStrip), new FrameworkPropertyMetadata(null));



        public IList<StripSpacer> StripSpacers
        {
            get { return (IList<StripSpacer>)GetValue(StripSpacersProperty); }
            set { SetValue(StripSpacersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StripSpacers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StripSpacersProperty =
            DependencyProperty.Register("StripSpacers", typeof(IList<StripSpacer>), typeof(LabelStrip),
                new FrameworkPropertyMetadata(new List<StripSpacer>(),
                    new PropertyChangedCallback(OnStripSpacersPropertyChanged),
                    new CoerceValueCallback(CoerceStripSpacers))); 

        private static object CoerceStripSpacers(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static void OnStripSpacersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;

            // Curate and Re-Assert Strip Spacers.
            instance.CurateStripSpacers(CellVerticalPosition.Upper);
            instance.CurateStripSpacers(CellVerticalPosition.Lower);        
        }


        private void CurateStripSpacers(CellVerticalPosition collectionVerticalPosition)
        {
            ObservableCollection<LabelCell> cellCollection;
            double spacerHeight = 0;

            // Prepare collections.
            if (collectionVerticalPosition == CellVerticalPosition.Upper)
            {
                cellCollection = UpperCells;
                spacerHeight = StripMode == LabelStripMode.Dual ?
                    StripHeight : StripHeight / _SingleLabelStripUpperHeightRatio;
            }

            else
            {
                cellCollection = LowerCells;
                spacerHeight = StripMode == LabelStripMode.Dual ?
                    StripHeight : StripHeight / _SingleLabelStripLowerHeightRatio;
            }

            // Purge Collection of Old Spacers.
            var spacerIndexes = from spacer in StripSpacers
                        select spacer.Index;

            var oldSpacerCells = (from cell in cellCollection
                                  where cell.IsSpacerCell == true &&
                                  !spacerIndexes.Contains(cellCollection.IndexOf(cell) + 1)
                                  select cell).ToList();

            foreach (var element in oldSpacerCells)
            {
                cellCollection.Remove(element);
            }

            // Add or Update Spacers in Cell Collection.
            foreach (var spacer in StripSpacers)
            {
                if (spacer.Index - 1 < cellCollection.Count)
                {
                    if (cellCollection[spacer.Index - 1].IsSpacerCell == true)
                    {
                        // Update Existing Spacer Cell.
                        var spacerCell = cellCollection[spacer.Index - 1];
                        spacerCell.Width = spacer.Width;
                        spacerCell.Height = spacerHeight;
                    }

                    else
                    {
                        // Insert New Cell.
                        cellCollection.Insert(spacer.Index, CreateNewSpacerCell(spacer.Width, spacerHeight));
                    }
                }
            }
        }
      
        public IList<LabelCell> Cells
        {
            get { return (IList<LabelCell>)GetValue(CellsProperty); }
            set { SetValue(CellsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cells.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellsProperty =
            DependencyProperty.Register("Cells", typeof(IList<LabelCell>), typeof(LabelStrip),
                new PropertyMetadata(new List<LabelCell>(), new PropertyChangedCallback(OnCellsPropertyChanged)));

        private static void OnCellsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            IList<LabelCell> newCollection = e.NewValue as IList<LabelCell>;

            if (newCollection != null)
            {
                // Push Current items to New Collection.
                foreach (var element in instance.UpperCells)
                {
                    newCollection.Add(element);
                }

                foreach (var element in instance.LowerCells)
                {
                    newCollection.Add(element);
                }
            }
        }

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

            if (oldCollection != null && newCollection != null)
            {
                // Toggling Mergers Collection causes issues with Mergers "Crossing Over" to other
                // Strips. 
                throw new NotSupportedException("Toggling of Mergers Collection is not allowed.");
            }

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= instance.Mergers_CollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += instance.Mergers_CollectionChanged;
            }

            // Handle Existing Elements
            var collection = e.NewValue as IEnumerable<Merge>;

            foreach (var element in collection)
            {
                instance.Merge(element);
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
            double newCellBaseWidth = newValue / instance.DataSource.Count();

            // Push new Width to child Cells.
            foreach (var element in upperCells)
            {
                element.BaseWidth = newCellBaseWidth;
            }

            foreach (var element in lowerCells)
            {
                element.BaseWidth = newCellBaseWidth;
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


            if (newCollection == null && oldCollection == null)
            {
                // Check if a Non Observable Collection has been Bound.
                IEnumerable<DimmerDistroUnit> newStandardCollection = e.NewValue as IEnumerable<DimmerDistroUnit>;

                if (newStandardCollection != null)
                {
                    // Handle already existing Items.
                    instance.RefreshCellDataSources(newStandardCollection);
                }
            }
        }

        public LabelCellTemplate UpperCellTemplate
        {
            get { return (LabelCellTemplate)GetValue(UpperCellTemplateProperty); }
            set { SetValue(UpperCellTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpperCellsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpperCellTemplateProperty =
            DependencyProperty.Register("UpperCellTemplate", typeof(LabelCellTemplate),
                typeof(LabelStrip), new FrameworkPropertyMetadata(new LabelCellTemplate(),
                    new PropertyChangedCallback(OnUpperCellTemplatePropertyChanged)));

        private static void OnUpperCellTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var newTemplate = e.NewValue as LabelCellTemplate;
            var uniqueTemplateWrappers = instance.UniqueCellTemplateWrappers;
            var upperCells = instance.UpperCells;

            RefreshCellTemplates(newTemplate, uniqueTemplateWrappers, upperCells, CellVerticalPosition.Upper);
        }


        public IEnumerable<LabelCellTemplateWrapper> UniqueCellTemplateWrappers
        {
            get { return (IEnumerable<LabelCellTemplateWrapper>)GetValue(UniqueCellTemplateWrappersProperty); }
            set { SetValue(UniqueCellTemplateWrappersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UniqueCellTemplates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UniqueCellTemplateWrappersProperty =
            DependencyProperty.Register("UniqueCellTemplateWrappers", typeof(IEnumerable<LabelCellTemplateWrapper>),
                typeof(LabelStrip), new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnUniqueCellTemplateWrappersPropertyChanged)));

        private static void OnUniqueCellTemplateWrappersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var newTemplateWrappers = e.NewValue as IEnumerable<LabelCellTemplateWrapper>;
            var upperStandardTemplate = instance.UpperCellTemplate;
            var lowerStandardTemplate = instance.LowerCellTemplate;
            var upperCellCollection = instance.UpperCells;
            var lowerCellCollection = instance.LowerCells;

            // Handle Existing Elements.
            // Refresh Upper and Lower Templates.
            RefreshCellTemplates(upperStandardTemplate, newTemplateWrappers, upperCellCollection, CellVerticalPosition.Upper);
            RefreshCellTemplates(lowerStandardTemplate, newTemplateWrappers, lowerCellCollection, CellVerticalPosition.Lower);
        }

        public LabelCellTemplate LowerCellTemplate
        {
            get { return (LabelCellTemplate)GetValue(LowerCellTemplateProperty); }
            set { SetValue(LowerCellTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerCellsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LowerCellTemplateProperty =
            DependencyProperty.Register("LowerCellTemplate", typeof(LabelCellTemplate), typeof(LabelStrip),
                new FrameworkPropertyMetadata(new LabelCellTemplate(), new PropertyChangedCallback(OnLowerCellTemplatePropertyChanged)));

        private static void OnLowerCellTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var newTemplate = e.NewValue as LabelCellTemplate;
            var uniqueTemplateWrappers = instance.UniqueCellTemplateWrappers;
            var lowerCells = instance.LowerCells;

            RefreshCellTemplates(newTemplate, uniqueTemplateWrappers, lowerCells, CellVerticalPosition.Lower);
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
            var upperCells = instance.UpperCells;
            int collectionCount = upperCells.Count;
            int difference = newCount - oldCount;
            var stripSpacers = instance.StripSpacers;

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

                // Re-Asssert Strip Spacers.
                instance.CurateStripSpacers(CellVerticalPosition.Upper);
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
            var lowerCells = instance.LowerCells;
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

                // Re-Asssert Strip Spacers.
                instance.CurateStripSpacers(CellVerticalPosition.Lower);
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

        private void UpperCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<LabelCell>;

            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cell = element as LabelCell;
                    if (_UpperStackPanel.Children.Contains(cell) == false)
                    {
                        // Add to StackPanel
                        _UpperStackPanel.Children.Insert(e.NewStartingIndex, cell);

                        // Add to Cells Collection.
                        if (cell.IsSpacerCell == false)
                        {
                            Cells.Add(cell);
                        }

                        // Set Vertical Position Flag.
                        cell.CellVerticalPosition = CellVerticalPosition.Upper;

                        // Set Height.
                        if (StripMode == LabelStripMode.Dual)
                        {
                            cell.Height = StripHeight;
                        }

                        else
                        {
                            cell.Height = StripHeight * _SingleLabelStripUpperHeightRatio;
                        }


                        // Set Template
                        if (cell.IsSpacerCell == false)
                        {
                            cell.Style = UpperCellTemplate.Style;
                        }

                        // Refresh LineWeights.
                        RefreshLineweights(cell);

                        // Connect Event Handler.
                        if (cell.IsSpacerCell == false)
                        {
                            cell.PropertyChanged += UpperCell_PropertyChanged;
                            cell.MouseDown += Cell_MouseDown;
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;

                    // Remove from StackPanel.
                    _UpperStackPanel.Children.Remove(cell);

                    // Remove from Cells Collection.
                    if (cell.IsSpacerCell == false)
                    {
                        Cells.Remove(cell);
                    }

                    // Deselect if Selected.
                    if (cell.IsSelected == true)
                    {
                        cell.IsSelected = false;
                        ((IList<LabelCell>)SelectedCells).Remove(cell);
                    }

                    // Reset Merge Flags if Merged.
                    if (cell.IsMerged == true)
                    {
                        cell.ConsumedReferences.Clear();
                    }

                    // Refresh remaining LineWeights.
                    RefreshLineweights();

                    // Disconnect Event Handler.
                    if (cell.IsSpacerCell == false)
                    {
                        cell.PropertyChanged -= UpperCell_PropertyChanged;
                        cell.MouseDown -= Cell_MouseDown;
                    }
                }
            }

            // Set Cell Widths.
            foreach (var cell in collection)
            {
                if (cell.IsSpacerCell == false)
                {
                    int dataSourceCount = DataSource.Count();

                    if (dataSourceCount == 0)
                    {
                        cell.BaseWidth = 0;
                    }

                    else
                    {
                        cell.BaseWidth = StripWidth / dataSourceCount;
                    }
                }
            }

            // Update Horizontal Position Indexes.
            for (int index = 0; index < collection.Count; index++)
            {
                var cell = collection[index];
                cell.HorizontalIndex = index;
            }

            // Coercion.
            CoerceValue(HeightProperty);

            // Push change to Cell Count Property.
            UpperCellCount = collection.Count;
        }

        private void LowerCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<LabelCell>;

            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var cell = element as LabelCell;
                    if (_LowerStackPanel.Children.Contains(cell) == false)
                    {
                        // Add to Stackpanel
                        _LowerStackPanel.Children.Insert(e.NewStartingIndex, cell);

                        // Add to Cells Collection.
                        if (cell.IsSpacerCell == false)
                        {
                            Cells.Add(cell);
                        }

                        // Set Vertical Position Flag.
                        cell.CellVerticalPosition = CellVerticalPosition.Lower;

                        // Set Height
                        if (StripMode == LabelStripMode.Dual)
                        {
                            cell.Height = StripHeight;
                        }

                        else
                        {
                            cell.Height = StripHeight * _SingleLabelStripLowerHeightRatio;
                        }

                        // Set Template
                        if (cell.IsSpacerCell == false)
                        {
                            cell.Style = LowerCellTemplate.Style;
                        }

                        // Refresh new LineWeights.
                        RefreshLineweights(cell);

                        // Connect event Handlers.
                        if (cell.IsSpacerCell == false)
                        {
                            cell.PropertyChanged += LowerCell_PropertyChanged;
                            cell.MouseDown += Cell_MouseDown;
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var cell = element as LabelCell;

                    // Remove from StackPanel.
                    _LowerStackPanel.Children.Remove(cell);

                    // Remove from Cells Collection.
                    if (cell.IsSpacerCell == false)
                    {
                        Cells.Remove(cell);
                    }

                    // Deselect if Selected.
                    if (cell.IsSelected == true)
                    {
                        cell.IsSelected = false;
                        ((IList<LabelCell>)SelectedCells).Remove(cell);
                    }

                    // Reset Merge Flags if Merged.
                    if (cell.IsMerged == true)
                    {
                        cell.ConsumedReferences.Clear();
                    }

                    // Refresh All Remaining Line Weights.
                    RefreshLineweights();

                    // Disconnect Event Handlers.
                    if (cell.IsSpacerCell == false)
                    {
                        cell.PropertyChanged -= LowerCell_PropertyChanged;
                        cell.MouseDown -= Cell_MouseDown;
                    }
                }
            }

            // Set Cell Widths.
            foreach (var cell in collection)
            {
                if (cell.IsSpacerCell == false)
                {
                    int dataSourceCount = DataSource.Count();

                    if (dataSourceCount == 0)
                    {
                        cell.BaseWidth = 0;
                    }

                    else
                    {
                        cell.BaseWidth = StripWidth / dataSourceCount;
                    }
                }
            }

            // Update Horizontal Position Indexes.
            for (int index = 0; index < collection.Count; index++)
            {
                var cell = collection[index];
                cell.HorizontalIndex = index;
            }

            // Coercion.
            CoerceValue(HeightProperty);

            // Push change to Cell Count Property.
            LowerCellCount = collection.Count;
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
        }

        protected void LowerCell_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                    // Selection has shifted to a new Cell. Dump currently selected Cells AND Rows.

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
        protected void RefreshLineweights()
        {
            // UpperCells
            foreach (var element in UpperCells)
            {
                RefreshLineweights(element);
            }

            // LowerCells
            foreach (var element in LowerCells)
            {
                RefreshLineweights(element);
            }
        }

        protected void RefreshLineweights(LabelCell cell)
        {
            var cellVerticalPosition = UpperCells.IndexOf(cell) == -1 ?
                CellVerticalPosition.Lower : CellVerticalPosition.Upper;

            var collection = cellVerticalPosition == CellVerticalPosition.Upper ?
                UpperCells : LowerCells;

            // LeftWeight.
            int leftCellIndex = collection.IndexOf(cell);
            double leftDesiredWeight = cell.LeftWeight;

            if (leftCellIndex == 0)
            {
                // Lefthand Boundary Cell.
                cell.ActualLeftWeight = leftDesiredWeight;
            }

            else
            {
                // Non boundary Cell.
                cell.ActualLeftWeight = leftDesiredWeight / 2;
                collection[leftCellIndex - 1].ActualRightWeight = leftDesiredWeight / 2;
            }


            // TopWeight
            cell.ActualTopWeight = cell.TopWeight;
            

            // RightWeight
            int rightCellIndex = collection.IndexOf(cell);
            double rightDesiredWeight = cell.RightWeight;

            if (rightCellIndex == collection.Count - 1)
            {
                // Righthand Boundary cell.
                cell.ActualRightWeight = rightDesiredWeight;
            }

            else
            {
                // Non Boundary cell.
                cell.ActualRightWeight = rightDesiredWeight / 2;
                collection[rightCellIndex + 1].ActualLeftWeight = rightDesiredWeight / 2;
            }
    
            // BottomWeight
            cell.ActualBottomWeight = cell.BottomWeight;
        }

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

            if (primaryCell == null)
            {
                // This LabelStrip does not hold a Cell matching that Data Reference currently. Operation can not
                // continue.
                return;
            }

            // Expand primaryCell's Dimensions
            double newWidth = primaryCell.Width;
            foreach (var element in consumedCells)
            {
                newWidth += element.Width;
            }

            primaryCell.Width = newWidth;

            // Store consumed Data References.
            primaryCell.ConsumedReferences = new List<DimmerDistroUnit>(mergeInstructions.ConsumedUnits);

            // Push Primary Cell Data to consumed Units.
            if (primaryCell.CellDataMode == CellDataMode.SingleField)
            {
                foreach (var element in primaryCell.ConsumedReferences)
                {
                    element.SetData(primaryCell.SingleFieldData, primaryCell.SingleFieldDataField);
                }
            }

            else
            {
                foreach (var row in primaryCell.Rows)
                {
                    foreach (var unit in primaryCell.ConsumedReferences)
                    {
                        unit.SetData(row.Data, row.DataField);
                    }
                }
            }

            // Remove Consumed Cells from Collection.
            foreach (var element in consumedCells)
            {
                cellCollection.Remove(element);
            }
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

            if (primaryCell == null)
            {
                // This LabelStrip does not hold a Cell matching that Data Reference currently. Operation can not
                // continue.
                return;
            }

            int primaryCellIndex = cellsList.IndexOf(primaryCell);

            // Set Primary Cell back to it's Base Dimensions.
            primaryCell.Width = primaryCell.BaseWidth;

            // Re Insert Consumed Cells into Collection.
            int consumedCellsCount = primaryCell.ConsumedReferencesCount;
            for (int index = primaryCellIndex + 1, count = 1; count <= consumedCellsCount; index++, count++)
            {
                // Cell initilization will be finshed by CollectionChangedEvent. Data Reference will be assigned by
                // RefreshCellDataSources().
                cellCollection.Insert(index, new LabelCell());
            }

            // Clear Primary Cells ConsumedReferences List.
            primaryCell.ConsumedReferences.Clear();

            // Force Refresh of Cell Data References.
            RefreshCellDataSources(DataSource);
        }
        #endregion

        #region Private or Protected Methods

        private static LabelCell CreateNewSpacerCell(double width, double height)
        {
            return new LabelCell() { IsSpacerCell = true, Width = width, Height = height };
        }


        private static void RefreshCellTemplates(LabelCellTemplate newTemplate, IEnumerable<LabelCellTemplateWrapper> uniqueTemplateWrappers,
            ObservableCollection<LabelCell> cellCollection, CellVerticalPosition cellVerticalPosition)
        {
            int collectionCount = cellCollection.Count;

            // Push change to Cells.
            if (uniqueTemplateWrappers == null || uniqueTemplateWrappers.Count() == 0)
            {
                // No Unique Templates. Push Standard template.
                for (int index = 0; index < collectionCount; index++)
                {
                    cellCollection[index].Style = newTemplate.Style;
                }
            }

            else
            {
                // Unique Templates.
                foreach (var templateWrapper in uniqueTemplateWrappers)
                {
                    // Query for a List of the Horizontal indexes filtering for Cell Vertical Postion.
                    var uniqueTemplateIndexes = (from address in templateWrapper.FilteredStripAddresses
                                                 where address.VerticalPosition == cellVerticalPosition
                                                 select address.HorizontalIndex).ToList();

                    for (int index = 0; index < collectionCount; index++)
                    {
                        var cell = cellCollection[index];

                        // Look for a matching Cell Index.
                        int uniqueCellIndex = uniqueTemplateIndexes.FindIndex(item => item == index);

                        if (uniqueCellIndex != -1)
                        {
                            // Assign Unique Template.
                            cell.Style = templateWrapper.CellTemplate.Style;
                        }

                        else
                        {
                            // Assign Standard Template.
                            cell.Style = newTemplate.Style;
                        }
                    }
                }
                
            }
        }
        /// <summary>
        /// Refreshes Cell Collections with new Data Source. Will adjust Collection sizes.
        /// </summary>
        /// <param name="newDataSource"></param>
        private void RefreshCellDataSources(IEnumerable<DimmerDistroUnit> newDataSource)
        {
            int upperTotalConsumedUnits = 0;
            int lowerTotalConsumedUnits = 0;

            if (Mergers != null)
            {
                foreach (var element in Mergers)
                {
                    if (element.VerticalPosition == CellVerticalPosition.Upper)
                    {
                        upperTotalConsumedUnits += element.ConsumedUnits.Count;
                    }

                    else if (element.VerticalPosition == CellVerticalPosition.Lower)
                    {
                        lowerTotalConsumedUnits += element.ConsumedUnits.Count;
                    }
                }
            }

            // Update Cell Counts, but ensure that StripSpacers cannot exist by themselves.
            int upperCellCountBuffer = newDataSource.Count() - upperTotalConsumedUnits;
            int lowerCellCountBuffer = newDataSource.Count() - lowerTotalConsumedUnits;

            UpperCellCount = upperCellCountBuffer != 0 ? upperCellCountBuffer + StripSpacers.Count : 0;
            LowerCellCount = lowerCellCountBuffer != 0 ? lowerCellCountBuffer + StripSpacers.Count : 0;

            // Assign UpperCells DataReferences
            IEnumerator<LabelCell> upperCellsEnum = UpperCells.GetEnumerator();
            IEnumerator<DimmerDistroUnit> dataSourceEnum = newDataSource.GetEnumerator();

            // Iterate both UpperCells and DataSource.
            while (upperCellsEnum.MoveNext() && dataSourceEnum.MoveNext())
            {
                // You can Skip a Spacer Cell safely as long as it's Not the last Cell in the Collection.
                // if it is the last in the collection, you need to convert it to a Standard Cell.
                if (upperCellsEnum.Current.IsSpacerCell)
                {
                    if (upperCellsEnum.Current != UpperCells.Last())
                    {
                        upperCellsEnum.MoveNext();
                    }

                    else
                    {
                        upperCellsEnum.Current.IsSpacerCell = false;
                    }
                }

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
            dataSourceEnum = newDataSource.GetEnumerator();

            IEnumerator<LabelCell> lowerCellsEnum = LowerCells.GetEnumerator();

            // Iterate both LowerCells and DataSource.
            while (lowerCellsEnum.MoveNext() && dataSourceEnum.MoveNext())
            {
                // You can Skip a Spacer Cell safely as long as it's Not the last Cell in the Collection.
                // if it is the last in the collection, you need to convert it to a Standard Cell.
                if (lowerCellsEnum.Current.IsSpacerCell)
                {
                    if (lowerCellsEnum.Current != LowerCells.Last())
                    {
                        lowerCellsEnum.MoveNext();
                    }

                    else
                    {
                        lowerCellsEnum.Current.IsSpacerCell = false;
                    }
                }

                if (lowerCellsEnum.Current.IsSpacerCell)
                {
                    lowerCellsEnum.MoveNext();
                }

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
                            lowerCellsEnum.Current.ConsumedReferences[index] = dataSourceEnum.Current;
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
            var upperCells = instance.UpperCells;
            var lowerCells = instance.LowerCells;
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
                // Multi Field Mode.
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

        private class UniqueLabelCellWrapperCollections
        {
            public List<LabelCellTemplateWrapper> UniqueUpperCellWrappers { get; set; } = new List<LabelCellTemplateWrapper>();
            public List<LabelCellTemplateWrapper> UniqueLowerCellWrappers { get; set; } = new List<LabelCellTemplateWrapper>();
        }
    }
}
