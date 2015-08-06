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

        protected bool _BackgroundColorGlobalApply = false;
        protected bool _LineWeightGlobalApply = false;

        protected double[] _StandardLineWeights = { 0.25, 0.50, 0.75, 1.00, 1.25, 1.50, 1.75, 2.00, 2.25 };
        protected double _LineWeight = 1.25d;

        protected LabelStrip _ActiveLabelStrip;

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

        public bool BackgroundColorGlobalApply
        {
            get
            {
                return _BackgroundColorGlobalApply;
            }
            set
            {
                _BackgroundColorGlobalApply = value;
                OnPropertyChanged("BackgroundColorGlobalApply");
                OnGlobalApplySelected();
            }
        }

        public bool LineWeightGlobalApply
        {
            get
            {
                return _LineWeightGlobalApply;
            }
            set
            {
                _LineWeightGlobalApply = value;
                OnPropertyChanged("LineWeightGlobalApply");
                OnGlobalApplySelected();
            }
        }

        public double[] StandardLineWeights
        {
            get
            {
                return _StandardLineWeights;
            }
        }

        public double LineWeight
        {
            get
            {
                return _LineWeight;
            }
            set
            {
                _LineWeight = value;
                UpdateLineWeight();
                OnPropertyChanged("LineWeight");
            }
        }

        public LabelStrip ActiveLabelStrip
        {
            get
            {
                return _ActiveLabelStrip;
            }
            set
            {
                if (value != null)
                {
                    _ActiveLabelStrip = value;
                    SetLineWeightSelection();
                }
            }
        }

        #endregion

        #region Methods
        void UpdateColor()
        {
            bool updateOccured = false;

            if (_BackgroundColorGlobalApply == false)
            {
                foreach (var element in _SelectedHeaderCells)
                {
                    element.BackgroundBrush = new SolidColorBrush(_SelectedColor);
                    updateOccured = true;
                }

                foreach (var element in _SelectedFooterCells)
                {
                    element.BackgroundBrush = new SolidColorBrush(_SelectedColor);
                    updateOccured = true;
                }
            }

            else
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    foreach (var header in labelStrip.Headers)
                    {
                        header.BackgroundBrush = new SolidColorBrush(_SelectedColor);
                    }

                    foreach (var footer in labelStrip.Footers)
                    {
                        footer.BackgroundBrush = new SolidColorBrush(_SelectedColor);
                    }
                }
                updateOccured = true;
            }

            if (updateOccured == true)
            {
                OnRenderRequested();
            }
        }

        void UpdateLineWeight()
        {
            if (_LineWeightGlobalApply == false)
            {
                ActiveLabelStrip.LineWeight = _LineWeight;
            }

            else
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    labelStrip.LineWeight = _LineWeight;
                }
            }

            OnRenderRequested();
        }

        void SetLineWeightSelection()
        {
            _LineWeight = ActiveLabelStrip.LineWeight;
            OnPropertyChanged("LineWeight");
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
                Color referenceColor = cells.First().BackgroundBrush.Color;

                if (cells.All(item => item.BackgroundBrush.Color == referenceColor))
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
                    _SelectedColor = _SelectedHeaderCells.First().BackgroundBrush.Color;
                }

                if (_SelectedFooterCells.Count > 0)
                {
                    _SelectedColor = _SelectedFooterCells.First().BackgroundBrush.Color;
                }
            }

            else
            {
                _SelectedColor = Colors.Transparent;
            }

            OnPropertyChanged("SelectedColor");
        }

        #endregion

        #region External Events
        public event EventHandler GlobalApplySelected;

        protected virtual void OnGlobalApplySelected()
        {
            GlobalApplySelected(this, new EventArgs());
            
        }
        #endregion
    }
}
