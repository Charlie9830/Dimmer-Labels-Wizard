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

        /// <summary>
        /// Generates a blank LabelStrip.
        /// </summary>
        public LabelStrip()
        {
            // Collection type Dependency properties.
            SetValue(UpperCellsPropertyKey, new CellCollection(this));
            SetValue(LowerCellsPropertyKey, new CellCollection(this));
            SetValue(MergedCellReferencesPropertyKey, new Dictionary<LabelCell, List<DimmerDistroUnit>>());

            // Initialize
            _UpperStackPanel.Orientation = Orientation.Horizontal;
            _LowerStackPanel.Orientation = Orientation.Horizontal;
            Children.Add(_UpperStackPanel);
            Children.Add(_LowerStackPanel);

            RowDefinitions.Add(_UpperGridRow);
            RowDefinitions.Add(_DividerGridRow);
            RowDefinitions.Add(_LowerGridRow);

            _UpperGridRow.Height = new GridLength(70);
            _DividerGridRow.Height = new GridLength(_StripDividerDistance);
            _LowerGridRow.Height = new GridLength(70);

            SetRow(_UpperStackPanel, 0);
            SetRow(_LowerStackPanel, 2);
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

        protected StackPanel _UpperStackPanel = new StackPanel();
        protected StackPanel _LowerStackPanel = new StackPanel();

        protected RowDefinition _UpperGridRow = new RowDefinition();
        protected RowDefinition _DividerGridRow = new RowDefinition();
        protected RowDefinition _LowerGridRow = new RowDefinition();

        private bool _InMouseSelectionEvent = false;
        #endregion

        #region CLR Properties.
        #endregion

        #region Dependency Properties


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

      

        public LabelCellTemplate UpperCellsTemplate
        {
            get { return (LabelCellTemplate)GetValue(UpperCellsTemplateProperty); }
            set { SetValue(UpperCellsTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpperCellsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpperCellsTemplateProperty =
            DependencyProperty.Register("UpperCellsTemplate", typeof(LabelCellTemplate),
                typeof(LabelStrip), new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnUpperCellsTemplatePropertyChanged)));

        private static void OnUpperCellsTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var template = e.NewValue as LabelCellTemplate;

            // Push changed style to Upper Cell Elements.
            foreach (var element in instance.UpperCells)
            {
                element.Style = template;
            }
        }



        public LabelCellTemplate LowerCellsTemplate
        {
            get { return (LabelCellTemplate)GetValue(LowerCellsTemplateProperty); }
            set { SetValue(LowerCellsTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerCellsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LowerCellsTemplateProperty =
            DependencyProperty.Register("LowerCellsTemplate", typeof(LabelCellTemplate), typeof(LabelStrip),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnLowerCellsTemplatePropertyChanged)));

        private static void OnLowerCellsTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            var template = e.NewValue as LabelCellTemplate;

            // Push changed style to Lower Cell Elements.
            foreach (var element in instance.LowerCells)
            {
                element.Style = template;
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

        public Dictionary<LabelCell, List<DimmerDistroUnit>> MergedCellReferences
        {
            get { return (Dictionary<LabelCell, List<DimmerDistroUnit>>)GetValue(MergedCellReferencesProperty); }
        }

        // Using a DependencyProperty as the backing store for MergedCellReferences.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey MergedCellReferencesPropertyKey =
            DependencyProperty.RegisterReadOnly("MergedCellReferences", typeof(Dictionary<LabelCell, List<DimmerDistroUnit>>),
                typeof(LabelStrip), new FrameworkPropertyMetadata(new Dictionary<LabelCell, List<DimmerDistroUnit>>()));

        public static readonly DependencyProperty MergedCellReferencesProperty = MergedCellReferencesPropertyKey.DependencyProperty;



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
       
            // Push new Heights to Child Cells.
            AdjustCellHeights(instance);

            instance.CoerceValue(HeightProperty);
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

            Console.WriteLine("OnSelectedCellsPropertyChanged");

            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= SelectedCells_CollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += SelectedCells_CollectionChanged;
            }
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
            double seperationDistance = _StripDividerDistance;
            List<LabelCell> upperCells = instance.UpperCells.ToList();
            List<LabelCell> lowerCells = instance.LowerCells.ToList();

            if (height < 0)
            {
                return 0d;
            }

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
            }

            return totalHeight;
            
        }

        private static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as LabelStrip;
            CellCollection upperCells = instance.UpperCells;
            CellCollection lowerCells = instance.LowerCells;
            LabelStripMode stripMode = instance.StripMode;

            // Calculate tallest upper and lower Cells.
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

            // Update GridRow Heights.
            instance._UpperGridRow.Height = new GridLength(upperHeight);
            instance._DividerGridRow.Height = stripMode == LabelStripMode.Dual ? new GridLength(_StripDividerDistance) :
                new GridLength(0d);
            instance._LowerGridRow.Height = new GridLength(lowerHeight);
            
        }
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

                        // Set Height.
                        if (instance.StripMode == LabelStripMode.Dual)
                        {
                            cell.Height = instance.StripHeight;
                        }

                        else
                        {
                            cell.Height = instance.StripHeight * _SingleLabelStripUpperHeightRatio;
                        }

                        // Set Template.
                        cell.Style = instance.UpperCellsTemplate;

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
                        cell.Style = instance.LowerCellsTemplate;

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

        private static void SelectedCells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("SelectedCells_CollectionChanged");
            var collection = sender as ObservableCollection<LabelCell>;
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
            }

            else
            {
                // Exclusive Selection.

                // Clear SelectedRows.
                while (SelectedCells.Count() > 0)
                {
                    ((IList<LabelCell>)SelectedCells).ElementAt(SelectedCells.Count() - 1).IsSelected = false;
                    ((IList<LabelCell>)SelectedCells).RemoveAt(SelectedCells.Count() - 1);
                }

                // Add new Selection.
                ((IList<LabelCell>)SelectedCells).Add(cell);
                cell.IsSelected = true;
            }

            _InMouseSelectionEvent = false;
        }

        #endregion

        #endregion

        #region Public Methods
        public void Merge(LabelCell target, List<LabelCell> mergingCells)
        {
            // Parameter Checking
            List<LabelCell> cells = new List<LabelCell>();
            cells.AddRange(mergingCells);
            cells.Add(target);

            bool isAllUpperCells = cells.Where(item => UpperCells.Contains(item)).Count() == cells.Count;
            bool isAllLowerCells = cells.Where(item => LowerCells.Contains(item)).Count() == cells.Count;

            if (isAllUpperCells ^ isAllLowerCells == false)
            {
                throw new FormatException("mergingCells and target do not belong to the same Cells Collection.");
            }

            // Function

            // Generate and Curate sourceCells.
            List<LabelCell> sourceCells = new List<LabelCell>(mergingCells);

            if (sourceCells.Contains(target))
            {
                sourceCells.Remove(target);
            }

            // Collect References.
            if (MergedCellReferences.ContainsKey(target))
            {
                // Dictionary entry already exists. Append to Dictionary Value List<> instead.
                List<DimmerDistroUnit> references = MergedCellReferences[target];

                foreach (var element in mergingCells)
                {
                    if (references.Contains(element.DataReference) == false)
                    {
                        references.Add(element.DataReference);
                    }
                }
            }

            else
            {
                MergedCellReferences.Add(target, mergingCells.Select(item => item.DataReference).ToList());
            }
            
            // Merge Cells
            foreach (var element in sourceCells)
            {
                LabelStrip.MergeCells(target, element);
            }

            // Remove sourceCells from Collections.
            if (isAllUpperCells == true)
            {
                foreach (var element in sourceCells)
                {
                    UpperCells.Remove(element);
                }
            }

            if (isAllLowerCells == true)
            {
                foreach (var element in sourceCells)
                {
                    LowerCells.Remove(element);
                }
            }

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

            // Assign UpperCell Data References.
            for (int index = 0; index < UpperCells.Count && index < newDataSource.Count(); index++)
            {
                UpperCells[index].DataReference = newDataSource.ElementAt(index);
            }

            // Assign LowerCell Data References.
            for (int index = 0; index < LowerCells.Count && index < newDataSource.Count(); index++)
            {
                LowerCells[index].DataReference = DataSource.ElementAt(index);
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

        /// <summary>
        /// Merges the Secondary cell into the Primary Cell.
        /// </summary>
        /// <param name="leftCell"></param>
        /// <param name="rightCell"></param>
        /// <returns></returns>
        private static void MergeCells(LabelCell primaryCell, LabelCell secondaryCell)
        {
            primaryCell.Width += secondaryCell.Width;
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
