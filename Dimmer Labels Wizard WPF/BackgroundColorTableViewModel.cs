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

        protected Color _SelectedColor = Colors.White;

        // Non Data Bound Fields
        protected bool ModelUpdateRequried = false;

        // Not directly Bound to. Selected items are pushed to this list via Property Changed events from ColorGridRowViewModels.
        protected ObservableCollection<ColorGridRowViewModel> _SelectedItems = new ObservableCollection<ColorGridRowViewModel>();

        public BackgroundColorTableViewModel()
        {
            _Items.CollectionChanged += _Items_CollectionChanged;
            PopulateItems(_SelectedLabelField);
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
                UpdateColorToSelectedItems();
                OnPropertyChanged("SelectedColor");
            }
        }
        #endregion

        #region General Methods
        protected Color GetItemColor()
        {
            if (_SelectedItems.Count == 0)
            {
                return Colors.White;
            }

            Color referenceColor = _SelectedItems.First().BackgroundBrush.Color;
            if (_SelectedItems.All(item => item.BackgroundBrush.Color == referenceColor))
            {
                return referenceColor;
            }

            else
            {
                return Colors.White;
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
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
                    _Items.Last().BackgroundBrush = Globals.GetLabelColor(element);
                }
            }
        }

        #endregion

        #endregion

        #region Update Methods
        public void UpdateColorToSelectedItems()
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
                Globals.LabelColors.Clear();
                foreach (var rowViewModel in _Items)
                {
                    // Only Push update to Model if User has Actually Modified Color Data.
                    if (rowViewModel.UserChanged == true)
                    {
                        foreach (var element in rowViewModel.DimmerDistroUnits)
                        {
                            Globals.LabelColors.Add(element, rowViewModel.BackgroundBrush);
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
        protected SolidColorBrush _BackgroundBrush = new SolidColorBrush(Colors.White);

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
                    UserChanged = true;
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
    }
}
