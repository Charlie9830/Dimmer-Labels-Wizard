using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Dimmer_Labels_Wizard
{
    public class ColorControlViewModel : ViewModelBase
    {
        protected ObservableCollection<HeaderCell> _SelectedHeaderCells = new ObservableCollection<HeaderCell>();
        protected ObservableCollection<FooterCell> _SelectedFooterCells = new ObservableCollection<FooterCell>();

        protected ObservableCollection<ColorItem> _StandardColorItems = new ObservableCollection<ColorItem>();
        protected Color _SelectedColor = new Color();

        public ColorControlViewModel()
        {
            PopulateStandardColorItems();
            _SelectedHeaderCells.CollectionChanged += SelectedCells_CollectionChanged;
            _SelectedFooterCells.CollectionChanged += SelectedCells_CollectionChanged;
        }

        #region Getters/Setters
        public Color SelectedColor
        {
            get
            {
                return _SelectedColor;
            }
            set
            {
                _SelectedColor = value;
                UpdateColor();
                OnPropertyChanged("SelectedColor");
            }
        }

        public ObservableCollection<HeaderCell> SelectedHeaderCells
        {
            get
            {
                return _SelectedHeaderCells;
            }
            set
            {
                _SelectedHeaderCells = value;
            }
        }

        public ObservableCollection<FooterCell> SelectedFooterCells
        {
            get
            {
                return _SelectedFooterCells;
            }
            set
            {
                _SelectedFooterCells = value;
            }
        }

        public ObservableCollection<ColorItem> StandardColorItems
        {
            get
            {
                return _StandardColorItems;
            }
        }

        #endregion

        #region Methods
        void UpdateColor()
        {
            bool updateOccured = false;

            foreach (var element in _SelectedHeaderCells)
            {
                element.BackgroundColor = new SolidColorBrush(_SelectedColor);
                updateOccured = true;
            }
            
            foreach (var element in _SelectedFooterCells)
            {
                element.BackgroundColor = new SolidColorBrush(_SelectedColor);
                updateOccured = true;
            }

            if (updateOccured == true)
            {
                OnRenderRequested();
            }
        }

        void PopulateStandardColorItems()
        {
            _StandardColorItems.Add(new ColorItem(Colors.Blue,"Blue"));
            _StandardColorItems.Add(new ColorItem(Colors.DarkBlue, "Dark Blue"));
            _StandardColorItems.Add(new ColorItem(Colors.Purple, "Purple"));
            _StandardColorItems.Add(new ColorItem(Colors.Yellow,"Yellow"));
            _StandardColorItems.Add(new ColorItem(Colors.Orange, "Orange"));
            _StandardColorItems.Add(new ColorItem(Colors.Red, "Red"));
            _StandardColorItems.Add(new ColorItem(Colors.Green,"Green"));
            _StandardColorItems.Add(new ColorItem(Colors.DarkGreen, "Dark Green"));
            _StandardColorItems.Add(new ColorItem(Colors.Brown,"Brown"));
            _StandardColorItems.Add(new ColorItem(Colors.Gray,"Gray"));
        }

        bool CheckColorEquality()
        {
            List<LabelCell> cells = new List<LabelCell>();

            foreach (var element in _SelectedHeaderCells)
            {
                cells.Add(element as LabelCell);
            }

            foreach (var element in _SelectedFooterCells)
            {
                cells.Add(element as LabelCell);
            }

            if (cells.Count > 1)
            {
                Color referenceColor = cells.First().BackgroundColor.Color;

                if (cells.All(item => item.BackgroundColor.Color == referenceColor))
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                return true;
            }
        }
        #endregion

        #region Event Handlers

        void SelectedCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CheckColorEquality() == true)
            {
                if (_SelectedHeaderCells.Count > 0)
                {
                    _SelectedColor = _SelectedHeaderCells.First().BackgroundColor.Color;
                }

                if (_SelectedFooterCells.Count > 0)
                {
                    _SelectedColor = _SelectedFooterCells.First().BackgroundColor.Color;
                }
            }

            else
            {
                _SelectedColor = Colors.Transparent;
            }

            OnPropertyChanged("SelectedColor");
        }

        #endregion
    }
}
