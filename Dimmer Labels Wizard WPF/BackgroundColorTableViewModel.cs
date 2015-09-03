using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class BackgroundColorTableViewModel : ViewModelBase
    {
        // Data Bound Fields.
        protected ObservableCollection<ColorGridRowViewModel> _Items = new ObservableCollection<ColorGridRowViewModel>();
        protected string _Debug = "Debug Messages";
        protected LabelField _SelectedLabelField = LabelField.Position;

        protected Color _SelectedColor = Colors.Transparent;

        // Non Data Bound Fields
        protected bool ModelUpdateRequried = false;

        // Not directly Bound to. Selected items are pushed to this list via Property Changed events from ColorGridRowViewModels.
        protected ObservableCollection<ColorGridRowViewModel> _SelectedItems = new ObservableCollection<ColorGridRowViewModel>();

        public BackgroundColorTableViewModel()
        {
            _Items.CollectionChanged += _Items_CollectionChanged;
            _SelectedItems.CollectionChanged += _SelectedItems_CollectionChanged;
            PopulateItems(_SelectedLabelField);
        }

        private void _SelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        #region Getters/Setters
        public ObservableCollection<ColorGridRowViewModel> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                OnPropertyChanged("Items");
            }
        }

        public string Debug
        {
            get
            {
                return _Debug;
            }
        }

        public LabelField SelectedLabelField
        {
            get
            {
                return _SelectedLabelField;
            }
            set
            {
                _SelectedLabelField = value;
                _SelectedItems.Clear();
                PopulateItems(value);
                OnPropertyChanged("SelectedLabelField");
            }
        }

        public Color SelectedColor
        {
            get
            {
                return GetItemColor();
            }
            set
            {
                _SelectedColor = value;
                PushColorToSelectedItems();
                OnPropertyChanged("SelectedColor");
            }
        }
        #endregion

        #region General Methods
        // Searches through SelectedItems and Returns Color if all items match, Transparent if Otherwise.
        protected Color GetItemColor()
        {
            if (_SelectedItems.Count == 0)
            {
                return Colors.Transparent;
            }

            if (_SelectedItems.First().BackgroundBrush == null)
            {
                return Colors.Transparent;
            }

            Color referenceColor = _SelectedItems.First().BackgroundBrush.Color;
            if (_SelectedItems.All(item => item.BackgroundBrush.Color == referenceColor))
            {
                return referenceColor;
            }

            else
            {
                return Colors.Transparent;
            }
        }
        #endregion

        #region Populate Methods
        void PopulateItems(LabelField showField)
        {
            // UpdateModel() Method will determine itself if an Update is actually required.
            UpdateModel();
            
            _Items.Clear();
            ModelUpdateRequried = false;

            switch (showField)
            {
                case LabelField.ChannelNumber:
                    CollectChannels();
                    break;
                case LabelField.InstrumentName:
                    CollectInstrumentNames();
                    break;
                case LabelField.MulticoreName:
                    CollectMulticoreNames();
                    break;
                case LabelField.Position:
                    CollectPositions();
                    break;
                case LabelField.UserField1:
                    CollectUserField1();
                    break;
                case LabelField.UserField2:
                    CollectUserField2();
                    break;
                case LabelField.UserField3:
                    CollectUserField3();
                    break;
                case LabelField.UserField4:
                    CollectUserField4();
                    break;
                default:
                    break;
            }
        }

        #region PopulateItems() Helper Methods.
        protected void CollectChannels()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().ChannelNumber == element.ChannelNumber) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits = 
                        Globals.DimmerDistroUnits.Where(item => item.ChannelNumber == element.ChannelNumber).ToList();
                    _Items.Last().ItemName = element.ChannelNumber;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectInstrumentNames()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().InstrumentName == element.InstrumentName) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.InstrumentName == element.InstrumentName).ToList();
                    _Items.Last().ItemName = element.InstrumentName;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectMulticoreNames()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().MulticoreName == element.MulticoreName) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.MulticoreName == element.MulticoreName).ToList();
                    _Items.Last().ItemName = element.MulticoreName;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectPositions()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().Position == element.Position) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.Position == element.Position).ToList();
                    _Items.Last().ItemName = element.Position;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectUserField1()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().UserField1 == element.UserField1) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.UserField1 == element.UserField1).ToList();
                    _Items.Last().ItemName = element.UserField1;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectUserField2()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().UserField2 == element.UserField2) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.UserField2 == element.UserField2).ToList();
                    _Items.Last().ItemName = element.UserField2;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                         new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectUserField3()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().UserField3 == element.UserField3) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.UserField3 == element.UserField3).ToList();
                    _Items.Last().ItemName = element.UserField3;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }

        protected void CollectUserField4()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().UserField4 == element.UserField4) == false)
                {
                    _Items.Add(new ColorGridRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.UserField4 == element.UserField4).ToList();
                    _Items.Last().ItemName = element.UserField4;
                    _Items.Last().SneakBackgroundBrush = Globals.GetLabelColor(element) == null ?
                        new SolidColorBrush(Colors.Transparent) : Globals.GetLabelColor(element);
                }
            }
        }
        #endregion
        #endregion

        #region Update Methods
        // Pushes Selected Colour to all Selected items.
        public void PushColorToSelectedItems()
        {
            foreach (var element in _SelectedItems)
            {
                element.BackgroundBrush = new SolidColorBrush(_SelectedColor);
            }

            OnPropertyChanged("SelectedItems");
        }

        public void UpdateModel()
        {
            if (ModelUpdateRequried == true)
            {
                // Globals.LabelColors.Clear();
                foreach (var rowViewModel in _Items)
                {
                    // Only Push update to Model if User has Actually Modified Color Data.
                    if (rowViewModel.UserChanged == true)
                    {
                        foreach (var element in rowViewModel.DimmerDistroUnits)
                        {
                            if (Globals.LabelColors.ContainsKey(element))
                            {
                                // Edit Value if already existing in Dictionary.
                                Globals.LabelColors[element] = rowViewModel.BackgroundBrush;
                            }

                            else
                            {
                                // Add if not already Existing.
                                Globals.LabelColors.Add(element, rowViewModel.BackgroundBrush);
                            }
                            
                        }
                    }
                }
            }

            ModelUpdateRequried = false;
        }

        #endregion

        #region Internal Event Handling
        private void _Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Hookup Property Changed Events.
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    ColorGridRowViewModel viewModel = element as ColorGridRowViewModel;
                    viewModel.PropertyChanged += ColorGridRowViewModel_PropertyChanged;
                }
            }

            // Disconnect Outgoing Property Changed Events.
            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    ColorGridRowViewModel viewModel = element as ColorGridRowViewModel;
                    viewModel.PropertyChanged -= ColorGridRowViewModel_PropertyChanged;
                }
            }
        }

        private void ColorGridRowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Update _SelectedItems.
            if (e.PropertyName == "IsSelected")
            {
                ColorGridRowViewModel viewModel = sender as ColorGridRowViewModel;

                // Add if selected and not already existing.
                if (viewModel.IsSelected == true && _SelectedItems.Contains(viewModel) == false)
                {
                    _SelectedItems.Add(viewModel);
                }

                // Remove if Deselected and already existing.
                if (viewModel.IsSelected == false && _SelectedItems.Contains(viewModel) == true)
                {
                    _SelectedItems.Remove(viewModel);
                }

                OnPropertyChanged("SelectedColor");
            }

            if (e.PropertyName == "BackgroundBrush")
            {
                ModelUpdateRequried = true;
            }
        }
        #endregion
    }

    public class ColorGridRowViewModel : ViewModelBase
    {
        protected List<DimmerDistroUnit> _DimmerDistroUnits = new List<DimmerDistroUnit>();
        protected string _ItemName = string.Empty;
        protected bool _IsSelected = false;
        protected SolidColorBrush _BackgroundBrush = new SolidColorBrush(Colors.Transparent);
        protected Brush _DisplayBrush = new SolidColorBrush(Colors.Purple);

        public bool UserChanged = false;

        #region Getters/Setters
        public List<DimmerDistroUnit> DimmerDistroUnits
        {
            get
            {
                return _DimmerDistroUnits;
            }
            set
            {
                _DimmerDistroUnits = value;
                OnPropertyChanged("DimmerDistroUnits");
            }
        }

        public string ItemName
        {
            get
            {
                return _ItemName;
            }

            set
            {
                _ItemName = value;
                OnPropertyChanged("ItemName");
            }
        }

        public SolidColorBrush BackgroundBrush
        {
            get
            {
                return _BackgroundBrush;
            }
            set
            {
                if (_BackgroundBrush != value)
                {
                    _BackgroundBrush = value;
                    // Property name string is Directly Referenced in BackgroundColorTableViewModel.
                    OnPropertyChanged("BackgroundBrush");
                    OnPropertyChanged("DisplayBrush");
                    UserChanged = true;
                }
            }
        }

        // Sets the Value of BackgroundBrush without triggering the "UserChanged" boolean.
        public SolidColorBrush SneakBackgroundBrush
        {
            set
            {
                _BackgroundBrush = value;
                // Property name string is Directly Referenced in BackgroundColorTableViewModel.
                OnPropertyChanged("DisplayBrush");
            }
        }

        public Brush DisplayBrush
        {
            get
            {
                bool uniformity;
                List<SolidColorBrush> brushes = CollectBrushes(out uniformity);

                if (uniformity == true)
                {
                    return _BackgroundBrush;
                }

                else
                {
                    if (UserChanged == false)
                    {
                        // Generate Gradient.
                        return new LinearGradientBrush(GenerateGradientStops(brushes), 0d);
                    }
                    else
                    {
                        // User has chosen to Overide non Uniform colors. Display Solid Color
                        // instead of a Gradient.
                        return _BackgroundBrush;
                    }
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                // Property name string is directly referenced in BackgroundColorTableViewModel.
                OnPropertyChanged("IsSelected");
            }
        }
        #endregion

        #region General Methods
        protected List<SolidColorBrush> CollectBrushes(out bool uniformityResult)
        {
            List<SolidColorBrush> brushes = new List<SolidColorBrush>();

            foreach (var element in _DimmerDistroUnits)
            {
                brushes.Add(Globals.GetLabelColor(element));
            }

            uniformityResult = brushes.All(item => item.Color == brushes.First().Color);

            return brushes;
        }

        protected GradientStopCollection GenerateGradientStops(List<SolidColorBrush> brushes)
        {
            List<SolidColorBrush> refinedBrushes = new List<SolidColorBrush>();
            refinedBrushes.AddRange(brushes.Where(item => 
            refinedBrushes.Contains(item, new SolidColorBrushComparer()) == false));

            
            int brushQTY = refinedBrushes.Count;
            double offset = 1d / brushQTY;
            GradientStopCollection stops = new GradientStopCollection();

            for (int index = 0; index < brushQTY; index++)
            {
                stops.Add(new GradientStop());
                stops.Last().Color = refinedBrushes[index].Color;
                stops.Last().Offset = offset * index;

                stops.Add(new GradientStop());
                stops.Last().Color = refinedBrushes[index].Color;
                stops.Last().Offset = (offset * index) + offset;
            }

            return stops;
        }
        #endregion

        
    }

    public class SolidColorBrushComparer : IEqualityComparer<SolidColorBrush>
    {
        #region Interface Implementations
        public bool Equals(SolidColorBrush x, SolidColorBrush y)
        {
            if (x.Color == y.Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(SolidColorBrush obj)
        {
            return obj.GetHashCode();
        }
        #endregion
    }
}
