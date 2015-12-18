using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            RowDefinitions.Add(_LowerGridRow);

            SetRow(_UpperStackPanel, 0);
            SetRow(_LowerStackPanel, 1);
        }

        public LabelStrip(List<LabelCell> upperCells, List<LabelCell> lowerCells, bool autoMergeCells)
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
            RowDefinitions.Add(_LowerGridRow);

            SetRow(_UpperStackPanel, 0);
            SetRow(_LowerStackPanel, 1);

            // Populate UpperCells Collection.
            foreach (var element in upperCells)
            {
                UpperCells.Add(element);
            }

            // Populate LowerCells Collection.
            foreach (var element in lowerCells)
            {
                LowerCells.Add(element);
            }

            if (autoMergeCells == true)
            {
                // Merge Cells.
                throw new NotImplementedException();
            }
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

                        // Connect Event Handler.
                        cell.PropertyChanged += instance.UpperCell_PropertyChanged;
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
                }
            }

            //// Set LineWeights
            //foreach (var element in collection)
            //{
            //    instance.SetLineWeight(element);
            //}
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
                        instance._LowerStackPanel.Children.Insert(e.NewStartingIndex, cell);

                        cell.PropertyChanged += instance.LowerCell_PropertyChanged;
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
                }
            }
        }

        protected void UpperCell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = sender as LabelCell;

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
        }

        protected void LowerCell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

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
                    if (references.Contains(element.PreviousReference) == false)
                    {
                        references.Add(element.PreviousReference);
                    }
                }
            }

            else
            {
                MergedCellReferences.Add(target, mergingCells.Select(item => item.PreviousReference).ToList());
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
        private static bool CanCellsMerge(LabelCell cellA, LabelCell cellB)
        {
            // Primary clauses.
            if (cellA.CellDataMode != cellB.CellDataMode)
            {
                // Non matching Cell Data modes.
                return false;
            }

            if (cellA.Rows.Count == 0 && cellB.Rows.Count == 0)
            {
                // No Rows present, IE They are blank Cells.
                return false;
            }


            // Secondary clauses.
            if (cellA.CellDataMode == CellDataMode.SingleField)
            {
                // Single Field Mode.
                string aData = cellA.Rows.Count > 0 ? cellA.Rows.First().Data : string.Empty;
                string bData = cellB.Rows.Count > 0 ? cellB.Rows.First().Data : string.Empty;

                LabelField aDataField = cellA.Rows.Count > 0 ? cellA.Rows.First().DataField : LabelField.NoAssignment;
                LabelField bDataField = cellB.Rows.Count > 0 ? cellB.Rows.First().DataField : LabelField.NoAssignment;

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
