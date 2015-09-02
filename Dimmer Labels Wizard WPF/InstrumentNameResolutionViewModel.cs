using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class InstrumentNameResolutionViewModel : ViewModelBase
    {
        // Data Bound Fields
        protected ObservableCollection<InstrumentRowViewModel> _Items = 
            new ObservableCollection<InstrumentRowViewModel>();

        // Indirectly Bound Fields
        protected ObservableCollection<InstrumentRowViewModel> _SelectedItems =
            new ObservableCollection<InstrumentRowViewModel>();

        public InstrumentNameResolutionViewModel()
        {
            _Items.CollectionChanged += _Items_CollectionChanged;

            PopulateItems();
        }

        #region Getters/Setters
        public ObservableCollection<InstrumentRowViewModel> Items
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
        #endregion

        #region Populate Methods
        protected void PopulateItems()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (_Items.Any(item => item.DimmerDistroUnits.First().InstrumentName == element.InstrumentName) == false)
                {
                    _Items.Add(new InstrumentRowViewModel());
                    _Items.Last().DimmerDistroUnits =
                        Globals.DimmerDistroUnits.Where(item => item.InstrumentName == element.InstrumentName).ToList();
                    _Items.Last().OriginalItemName = element.InstrumentName;
                }
            }
        }
        #endregion

        #region Internal Event Handling
        private void _Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Hookup Incoming Property Changed Events.
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    InstrumentRowViewModel viewModel = element as InstrumentRowViewModel;
                    viewModel.PropertyChanged += RowViewModel_PropertyChanged;
                }
            }

            // Disconnect outgoing Property Changed Events.
            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    InstrumentRowViewModel viewModel = element as InstrumentRowViewModel;
                    viewModel.PropertyChanged -= RowViewModel_PropertyChanged;
                }
            }
        }

        #region Update Methods
        public void UpdateModel()
        {
            foreach (var element in _Items)
            {
                if (element.ShortenedItemName != string.Empty)
                {
                    // Instruct Row View Models to Push changes back to DimmerDistroUnits.
                    element.UpdateModel();
                }
            }
        }
        #endregion

        private void RowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InstrumentRowViewModel viewModel = sender as InstrumentRowViewModel;

            // Update _SelectedItems.
            if (e.PropertyName == "IsSelected")
            {
                // Add if selected and not already in Collection.
                if (viewModel.IsSelected == true && _SelectedItems.Contains(viewModel) == false)
                {
                    _SelectedItems.Add(viewModel);
                }

                // Remove if Deselected and existing in Collection.
                if (viewModel.IsSelected == false && _SelectedItems.Contains(viewModel) == true)
                {
                    _SelectedItems.Remove(viewModel);
                }
            }

            if (e.PropertyName == "ShortenedItemName")
            {
                if (_SelectedItems.Count > 1)
                {
                    foreach (var element in _SelectedItems)
                    {
                        element.PreviewShortenedItemName = viewModel.ShortenedItemName;
                    }

                    // Request the Datagrid to Refresh it's Items Source.
                    OnDataGridRefreshRequested();
                }
            }
        }
        #endregion

        #region External Events
        public event EventHandler DataGridRefreshRequested;

        public void OnDataGridRefreshRequested()
        {
            if (DataGridRefreshRequested != null)
            {
                DataGridRefreshRequested(this, new EventArgs());
            }
        }

        #endregion
    }

    public class InstrumentRowViewModel : ViewModelBase
    {
        protected List<DimmerDistroUnit> _DimmerDistroUnits = new List<DimmerDistroUnit>();
        protected string _OriginalItemName = string.Empty;
        protected string _ShortenedItemName = string.Empty;
        protected bool _IsSelected = false;

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

        public string OriginalItemName
        {
            get
            {
                return _OriginalItemName;
            }
            set
            {
                _OriginalItemName = value;
                OnPropertyChanged("OriginalItemName");
            }
        }

        public string ShortenedItemName
        {
            get
            {
                return _ShortenedItemName;
            }
            set
            {
                _ShortenedItemName = value;
                OnPropertyChanged("ShortenedItemName");
            }
        }

        // Sets value of _ShortenedItemName without raising a PropertyChanged Event.
        public string PreviewShortenedItemName
        {
            set
            {
                _ShortenedItemName = value;
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
                OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Update Methods
        public void UpdateModel()
        {
            foreach (var element in DimmerDistroUnits)
            {
                element.InstrumentName = _ShortenedItemName;
            }
        }
        #endregion
    }
}
