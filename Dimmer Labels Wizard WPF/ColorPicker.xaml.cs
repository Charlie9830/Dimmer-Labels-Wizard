using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl, INotifyPropertyChanged
    {
        public ColorPicker()
        {
            InitializeComponent();

            // Commands.
            _ShowAdvancedColorPickerCommand = new RelayCommand(ShowAdvancedColorPickerCommandExecute);

            // Populate Standard Colors.
            var standardColors = new List<Color>()
            {
                new Color() {A = 255, R = 0, G = 0, B = 0},
                new Color() {A = 255, R = 99, G = 56, B = 38},
                new Color() {A = 255, R = 79, G = 33, B = 112},
                new Color() {A = 255, R = 0, G = 158, B = 73},
                new Color() {A = 255, R = 206, G = 17, B = 38},
                new Color() {A = 255, R = 20, G = 20, B = 255},
                new Color() {A = 255, R = 232, G = 117, B = 17},
                new Color() {A = 255, R = 58, G = 117, B = 196},
                new Color() {A = 255, R = 130, G = 127, B = 119},
                new Color() {A = 255, R = 249, G = 224, B = 76},
                new Color() {A = 255, R = 221, G = 219, B = 209},
                new Color() {A = 255, R = 255, G = 255, B = 255},
            };

            StandardColors = standardColors as IEnumerable<Color>;
        }

        #region Fields.
        int RecentColorsMaxCount = 7;
        #endregion

        #region Internal Binding Sources.


        protected ObservableCollection<Color> _RecentColors = new ObservableCollection<Color>();

        public ObservableCollection<Color> RecentColors
        {
            get { return _RecentColors; }
            set
            {
                if (_RecentColors != value)
                {
                    _RecentColors = value;

                    // Notify.
                    RaisePropertyChanged(nameof(RecentColors));
                }
            }
        }


        protected Brush _InternalDisplayedBrush;

        public Brush InternalDisplayedBrush
        {
            get { return _InternalDisplayedBrush; }
            protected set
            {
                if (_InternalDisplayedBrush != value)
                {
                    _InternalDisplayedBrush = value;

                    // Notify.
                    RaisePropertyChanged(nameof(InternalDisplayedBrush));
                }
            }
        }

        protected Color? _InternalSelectedStandardColor = Colors.White;

        public Color? InternalSelectedStandardColor
        {
            get { return _InternalSelectedStandardColor; }
            set
            {
                if (_InternalSelectedStandardColor != value)
                {
                    _InternalSelectedStandardColor = value;

                    // Null other Selection.
                    InternalSelectedRecentColor = null;

                    // Update Dependency Property.
                    if (value != null)
                    {
                        SelectedColor = (Color)value;
                    }

                    // Notify.
                    RaisePropertyChanged(nameof(InternalSelectedStandardColor));
                }
            }
        }


        protected Color? _InternalSelectedRecentColor = Colors.White;

        public Color? InternalSelectedRecentColor
        {
            get { return _InternalSelectedRecentColor; }
            set
            {
                if (_InternalSelectedRecentColor != value)
                {
                    _InternalSelectedRecentColor = value;

                    // Null other Selection.
                    InternalSelectedStandardColor = null;

                    // Update Dependency Property.
                    if (value != null)
                    {
                        SelectedColor = (Color)value;
                    }

                    // Notify.
                    RaisePropertyChanged(nameof(InternalSelectedRecentColor));
                }
            }
        }

        protected IEnumerable<Color> _StandardColors;

        public IEnumerable<Color> StandardColors
        {
            get { return _StandardColors; }
            set
            {
                if (_StandardColors != value)
                {
                    _StandardColors = value;

                    // Notify.
                    RaisePropertyChanged(nameof(StandardColors));
                }
            }
        }


        protected Color _CanvasColor = Colors.White;

        public Color CanvasColor
        {
            get { return _CanvasColor; }
            set
            {
                if (_CanvasColor != value)
                {
                    _CanvasColor = value;

                    // Notify.
                    RaisePropertyChanged(nameof(CanvasColor));
                }
            }
        }

        #endregion

        #region Commands.

        protected RelayCommand _ShowAdvancedColorPickerCommand;
        public ICommand ShowAdvancedColorPickerCommand
        {
            get
            {
                return _ShowAdvancedColorPickerCommand;
            }
        }

        protected void ShowAdvancedColorPickerCommandExecute(object parameter)
        {
            // Initialize Dialog.
            var dialog = new AdvancedColorPicker();
            var viewModel = dialog.DataContext as AdvancedColorPickerViewModel;

            viewModel.SelectedColor = SelectedColor;

            // Execute Dialog.
            if (dialog.ShowDialog() == true)
            {
                PushToRecentColors(viewModel.SelectedColor);
            }

        }

        #endregion

        #region Dependency Properties.

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
                new FrameworkPropertyMetadata(Colors.White, new PropertyChangedCallback(OnSelectedColorPropertyChanged)));

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ColorPicker;
            var color = (Color)e.NewValue;
            var recentColors = instance.RecentColors;
            var standardColors = instance.StandardColors;

            // Set Displayed Brush.
            instance.InternalDisplayedBrush = new SolidColorBrush(color);

            if (standardColors.Contains(color))
            {
                // Select Color if existing in Collection.
                instance.InternalSelectedStandardColor = color;
            }
        }


        #endregion

        #region Methods.
        /// <summary>
        /// Provides stack like Push function for Recent Colors collection. 
        /// </summary>
        /// <param name="color"></param>
        protected void PushToRecentColors(Color color)
        {
            if (RecentColors.Contains(color))
            {
                // Color already exists in Collection. Select and Bail.
                InternalSelectedRecentColor = color;
                return;
            }

            if (RecentColors.Count == RecentColorsMaxCount)
            {
                // Remove Last element.
                RecentColors.RemoveAt(RecentColors.Count - 1);
            }

            // Insert. 
            RecentColors.Insert(0, color);

            // Select.
            InternalSelectedRecentColor = color;
        }

        #endregion.

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
