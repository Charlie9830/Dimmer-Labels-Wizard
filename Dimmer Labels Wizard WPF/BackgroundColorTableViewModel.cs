using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace Dimmer_Labels_Wizard_WPF
{
    public class BackgroundColorTableViewModel : ViewModelBase
    {
        protected ObservableCollection<ColorGridRowViewModel> _Items = new ObservableCollection<ColorGridRowViewModel>();
        protected string _Debug = "Debug Messages";
        protected LabelField _SelectedLabelField;

        // Not directly Bound to. Selected items are pushed to this list via Property Changed events from ColorGridRowViewModels.
        protected ObservableCollection<ColorGridRowViewModel> _SelectedItems = new ObservableCollection<ColorGridRowViewModel>();


        public BackgroundColorTableViewModel()
        {
            _Items.CollectionChanged += _Items_CollectionChanged;

            FooterCell test1 = new FooterCell();
            test1.BackgroundBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            test1.TopData = "test1";
            FooterCell test2 = new FooterCell();
            test2.BackgroundBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            test2.TopData = "test2";
            FooterCell test3 = new FooterCell();
            test3.BackgroundBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            test3.TopData = "test3";
            FooterCell test4 = new FooterCell();
            test4.BackgroundBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
            test4.TopData = "test4";

            _Items.Add(new ColorGridRowViewModel());
            _Items.Last().FooterCell = test1;
            _Items.Add(new ColorGridRowViewModel());
            _Items.Last().FooterCell = test2;
            _Items.Add(new ColorGridRowViewModel());
            _Items.Last().FooterCell = test3;
            _Items.Add(new ColorGridRowViewModel());
            _Items.Last().FooterCell = test4;
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
                OnPropertyChanged("SelectedLabelField");

                _Debug = value.ToString() + " : " + value.GetType().ToString();
                OnPropertyChanged("Debug");
            }
        }
        #endregion

        #region General Methods
        void PopulateItems(LabelField showField)
        {
            switch (showField)
            {
                case LabelField.ChannelNumber:
                    break;
                case LabelField.InstrumentName:
                    break;
                case LabelField.MulticoreName:
                    break;
                case LabelField.Position:
                    break;
                case LabelField.UserField1:
                    break;
                case LabelField.UserField2:
                    break;
                case LabelField.UserField3:
                    break;
                case LabelField.UserField4:
                    break;
                default:
                    break;
            }
        }

        protected void CollectChannels()
        {
            foreach (var element in
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

                _Debug = _SelectedItems.Count.ToString();
                OnPropertyChanged("Debug");
            }
        }
        #endregion
    }

    public class ColorGridRowViewModel : ViewModelBase
    {
        protected FooterCell _FooterCell;
        protected HeaderCell _HeaderCell;
        protected string _ItemName;
        protected bool _IsSelected;

        #region Getters/Setters
        public FooterCell FooterCell
        {
            get
            {
                return _FooterCell;
            }
            set
            {
                _FooterCell = value;
                OnPropertyChanged("FooterCell");
            }
        }

        public HeaderCell HeaderCell
        {
            get
            {
                return _HeaderCell;
            }
            set
            {
                _HeaderCell = value;
                OnPropertyChanged("HeaderCell");
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
