using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard_WPF
{
    /// <summary>
    /// Interaction logic for RangeControlContainer.xaml
    /// </summary>
    public partial class RangeControlContainer : UserControl, INotifyPropertyChanged
    {
        public RangeControlContainer()
        {
            InitializeComponent();

            // Commands.
            _AddRangeCommand = new RelayCommand(AddRangeCommandExecute);
            _RemoveRangeCommand = new RelayCommand(RemoveRangeCommandExecute, RemoveRangeCommandCanExecute);
        }

        #region Interal Binding Sources.

        protected ObservableCollection<UserControl> _InternalRangeControl = new ObservableCollection<UserControl>();

        public ObservableCollection<UserControl> InternalRangeControls
        {
            get { return _InternalRangeControl; }
            set
            {
                if (_InternalRangeControl != value)
                {
                    _InternalRangeControl = value;

                    // Notify.
                    RaisePropertyChanged(nameof(InternalRangeControls));
                }
            }
        }


        protected RackType _ControlType = RackType.Dimmer;

        public RackType ControlType
        {
            get { return _ControlType; }
            set
            {
                if (_ControlType != value)
                {
                    _ControlType = value;

                    // Notify.
                    RaisePropertyChanged(nameof(ControlType));
                }
            }
        }


        public bool ShowStartMessage
        {
            get { return !(Ranges.Count() > 0); }
        }

        #endregion

        #region Commands

        protected RelayCommand _AddRangeCommand;
        public ICommand AddRangeCommand
        {
            get
            {
                return _AddRangeCommand;
            }
        }

        protected void AddRangeCommandExecute(object parameter)
        {
            if (ControlType == RackType.Dimmer)
            {
                // Dimmer.
                ((IList<UnitRangeBase>)Ranges).Add(
                    new DimmerRange() { Universe = 1, FirstDimmerNumber = 1, LastDimmerNumber = 12 });
            }

            else
            {
                // Distro.
                ((IList<UnitRangeBase>)Ranges).Add(
                    new DistroRange() { FirstDimmerNumber = 1, LastDimmerNumber = 12 });
            }
        }


        protected RelayCommand _RemoveRangeCommand;
        public ICommand RemoveRangeCommand
        {
            get
            {
                return _RemoveRangeCommand;
            }
        }

        protected void RemoveRangeCommandExecute(object parameter)
        {
            if (Ranges.Count() > 0)
            {
                ((IList<UnitRangeBase>)Ranges).RemoveAt(Ranges.Count() - 1);
            }
        }

        protected bool RemoveRangeCommandCanExecute(object parameter)
        {
            return Ranges.Count() > 0;
        }
        #endregion

        #region DependencyProperties.


        public IEnumerable<UnitRangeBase> Ranges
        {
            get { return (IEnumerable<UnitRangeBase>)GetValue(RangesProperty); }
            set { SetValue(RangesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeControls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangesProperty =
            DependencyProperty.Register("Ranges", typeof(IEnumerable<UnitRangeBase>),
                typeof(RangeControlContainer), new FrameworkPropertyMetadata(new List<UnitRangeBase>(),
                    new PropertyChangedCallback(OnRangesPropertyChanged)));

        private static void OnRangesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as RangeControlContainer;

            INotifyCollectionChanged newCollection = e.NewValue as INotifyCollectionChanged;
            INotifyCollectionChanged oldCollection = e.OldValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= instance.RangeControls_CollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += instance.RangeControls_CollectionChanged;

                // Handle Existing Elements.
                var collection = e.NewValue as IEnumerable<UnitRangeBase>;

                // Clear Internal Collection.
                instance.InternalRangeControls.Clear();

                foreach (var element in collection)
                {
                    instance.AddRangeControl(element);       
                }
            }

            // Executes.
            instance._RemoveRangeCommand.CheckCanExecute();
        }

        private void RangeControls_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var element in e.NewItems)
                {
                    var range = element as UnitRangeBase;
                    AddRangeControl(range);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var element in e.OldItems)
                {
                    var range = element as UnitRangeBase;
                    RemoveRangeControl(range);
                }
            }

            // Notify.
            RaisePropertyChanged(nameof(ShowStartMessage));

            // Executes.
            _RemoveRangeCommand.CheckCanExecute();
        }


        #endregion

        #region Methods.
        protected void AddRangeControl(UnitRangeBase rangeBase)
        {
            if (rangeBase.GetType() == typeof(DimmerRange))
            {
                // Dimmer.
                var range = rangeBase as DimmerRange;
                var rangeControl = new DimmerRangeControl();
                var viewModel = rangeControl.DataContext as RangeControlViewModel;

                viewModel.Range = range;

                InternalRangeControls.Add(rangeControl);

            }

            else
            {
                // Distro.
                var range = rangeBase as DistroRange;
                var rangeControl = new DistroRangeControl();
                var viewModel = rangeControl.DataContext as RangeControlViewModel;

                viewModel.Range = range;

                InternalRangeControls.Add(rangeControl);
            }
        }

        protected void RemoveRangeControl(UnitRangeBase range)
        {
            var viewModelPairings = from control in InternalRangeControls
                                    select new {
                                        Control = control,
                                        ViewModel = control.DataContext as RangeControlViewModel
                                    };

            var removalQuery = (from pairing in viewModelPairings
                               where pairing.ViewModel.Range == range
                               select pairing.Control).ToList();

            foreach (var element in removalQuery)
            {
                InternalRangeControls.Remove(element);
            }
        }



        #endregion

        #region Interfaces.
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
