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

        #region Internal Binding Sources.

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
            get { return !(RangesCount > 0); }
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
                ((IList<DimmerRange>)Ranges).Add(
                    new DimmerRange() { Universe = 0, FirstDimmerNumber = 0, LastDimmerNumber = 0 });
            }

            else
            {
                // Distro.
                ((IList<DistroRange>)Ranges).Add(
                    new DistroRange() { FirstDimmerNumber = 0, LastDimmerNumber = 0 });
            }

            // Add Range Control and Notify.
            AddRangeControl(Ranges.Last());
            RaisePropertyChanged(nameof(ShowStartMessage));
            _RemoveRangeCommand.CheckCanExecute();
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
            if (RangesCount > 0)
            {
                UnitRangeBase rangeBuffer;

                if (ControlType == RackType.Dimmer)
                {
                    // Dimmer.
                    rangeBuffer = Ranges.Last();
                    ((IList<DimmerRange>)Ranges).RemoveAt(RangesCount - 1);
                }

                else
                {
                    // Distro.
                    rangeBuffer = Ranges.Last();
                    ((IList<DistroRange>)Ranges).RemoveAt(RangesCount - 1);
                }

                // Remove Range Control and Notify.
                RemoveRangeControl(rangeBuffer);
                RaisePropertyChanged(nameof(ShowStartMessage));
                _RemoveRangeCommand.CheckCanExecute();
            }
        }

        protected bool RemoveRangeCommandCanExecute(object parameter)
        {
            return RangesCount > 0;     
        }
        #endregion

        #region Internal Properties.
        protected int RangesCount
        {
            get
            {
                if (Ranges == null)
                {
                    return 0;
                }

                else
                {
                    return Ranges.Count();
                }
            }
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

            if (e.NewValue != null)
            {
                // Handle Existing Elements.
                var collection = e.NewValue as IEnumerable<UnitRangeBase>;

                // Clear Internal Collection.
                instance.InternalRangeControls.Clear();

                foreach (var element in collection)
                {
                    instance.AddRangeControl(element);       
                }
            }

            // Notify.
            instance.RaisePropertyChanged(nameof(ShowStartMessage));

            // Executes.
            instance._RemoveRangeCommand.CheckCanExecute();
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
