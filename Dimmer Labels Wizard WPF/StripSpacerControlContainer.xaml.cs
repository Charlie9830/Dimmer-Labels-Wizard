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
    /// Interaction logic for StripSpacerControlContainer.xaml
    /// </summary>
    public partial class StripSpacerControlContainer : UserControl, INotifyPropertyChanged
    {
        protected bool _UpdatingExternalStripSpacers = false;

        public StripSpacerControlContainer()
        {
            InitializeComponent();

            // Commands.
            _AddStripSpacerCommand = new RelayCommand(AddStripSpacerCommandExecute);
            _RemoveStripSpacerCommand = new RelayCommand(RemoveStripSpacerCommandExecute, RemoveStripSpacerCommandCanExecute);


            // DP Collections.
            SetValue(StripSpacersProperty, new List<StripSpacer>());
        }

        #region Internal Binding.

        protected ObservableCollection<StripSpacer> _InternalStripSpacers = new ObservableCollection<StripSpacer>();

        public ObservableCollection<StripSpacer> InternalStripSpacers
        {
            get { return _InternalStripSpacers; }
            set
            {
                if (_InternalStripSpacers != value)
                {
                    _InternalStripSpacers = value;

                    // Notify.
                    RaisePropertyChanged(nameof(InternalStripSpacers));
                }
            }
        }


        protected StripSpacer _SelectedStripSpacer = null;

        public StripSpacer SelectedStripSpacer
        {
            get { return _SelectedStripSpacer; }
            set
            {
                if (_SelectedStripSpacer != value)
                {
                    _SelectedStripSpacer = value;

                    // Notify.
                    RaisePropertyChanged(nameof(SelectedStripSpacer));

                    // Executes.
                    _RemoveStripSpacerCommand.CheckCanExecute();
                }
            }
        }
        #endregion

        #region Commands.
        protected RelayCommand _AddStripSpacerCommand;
        public ICommand AddStripSpacerCommand
        {
            get
            {
                return _AddStripSpacerCommand;
            }
        }

        protected void AddStripSpacerCommandExecute(object parameter)
        {
            var collection = StripSpacers as IList<StripSpacer>;

            collection.Add(new StripSpacer() { Index = 1, Width = 50 });
        }


        protected RelayCommand _RemoveStripSpacerCommand;
        public ICommand RemoveStripSpacerCommand
        {
            get
            {
                return _RemoveStripSpacerCommand;
            }
        }

        protected void RemoveStripSpacerCommandExecute(object parameter)
        {
            if (SelectedStripSpacer != null)
            {
                var collection = StripSpacers as IList<StripSpacer>;
                collection.Remove(SelectedStripSpacer);
            }
        }

        protected bool RemoveStripSpacerCommandCanExecute(object parameter)
        {
            return InternalStripSpacers.Count > 0 && SelectedStripSpacer != null;
        }
        #endregion

        #region External Binding.


        public IEnumerable<StripSpacer> StripSpacers
        {
            get { return (IEnumerable<StripSpacer>)GetValue(StripSpacersProperty); }
            set { SetValue(StripSpacersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StripSpacers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StripSpacersProperty =
            DependencyProperty.Register("StripSpacers", typeof(IEnumerable<StripSpacer>),
                typeof(StripSpacerControlContainer),
                new FrameworkPropertyMetadata(new List<StripSpacer>(),
                    new PropertyChangedCallback(StripSpacersPropertyChanged)));

        private static void StripSpacersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as StripSpacerControlContainer;
            var newValue = e.NewValue as INotifyCollectionChanged;
            var oldValue = e.OldValue as INotifyCollectionChanged;
            var newCollection = e.NewValue as Collection<StripSpacer>;
            var internalCollection = instance.InternalStripSpacers;

            if (oldValue != null)
            {
                oldValue.CollectionChanged -= instance.StripSpacers_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += instance.StripSpacers_CollectionChanged;

                // Handle Existing Elements.
                while (internalCollection.Count > 0)
                {
                    internalCollection.RemoveAt(internalCollection.Count - 1);
                }

                foreach (var element in newCollection)
                {
                    internalCollection.Add(element);
                }
            }
        }

        protected void StripSpacers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems;
            var oldItems = e.OldItems;

            if (oldItems != null)
            {
                foreach (var element in oldItems)
                {
                    var stripSpacer = element as StripSpacer;
                    InternalStripSpacers.Remove(stripSpacer);
                }
            }

            if (newItems != null)
            {
                foreach (var element in newItems)
                {
                    var stripSpacer = element as StripSpacer;
                    InternalStripSpacers.Add(stripSpacer);
                }
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
