using System;
using System.Collections.Generic;
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
    /// Interaction logic for RowHeightEditor.xaml
    /// </summary>
    public partial class RowHeightEditor : UserControl, INotifyPropertyChanged
    {
        public RowHeightEditor()
        {
            InitializeComponent();

            // Command Bindings.
            _OkCommand = new RelayCommand(OkCommandExecute);
        }

        #region CLR Property Binding Sources

        protected LabelCellTemplate _InternalCellTemplate;

        public LabelCellTemplate InternalCellTemplate
        {
            get
            {
                return _InternalCellTemplate;
            }
            set
            {
                if (_InternalCellTemplate != value)
                {
                    _InternalCellTemplate = value;

                    // Notify.
                    RaisePropertyChanged(nameof(InternalCellTemplate));
                }
            }
        }


        protected List<double> _InternalRowProportions = new List<double>();

        public List<double> InternalRowProportions
        {
            get { return _InternalRowProportions; }
            set
            {
                if (_InternalRowProportions != value)
                {
                    _InternalRowProportions = value;

                    // Notify.
                    RaisePropertyChanged(nameof(InternalRowProportions));

                    // Write Back to Dependency Property.
                    RowProportions = value;
                }
            }
        }
        #endregion

        #region Dependency Properties


        public LabelCellTemplate CellTemplate
        {
            get { return (LabelCellTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellTemplateProperty =
            DependencyProperty.Register("CellTemplate", typeof(LabelCellTemplate), typeof(RowHeightEditor),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCellTemplatePropertyChanged)));

        private static void OnCellTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as RowHeightEditor;
            var newValue = e.NewValue as LabelCellTemplate;

            // Push Value to Internal Binding Source.
            instance.InternalCellTemplate = newValue;
        }




        public IEnumerable<double> RowProportions
        {
            get { return (IEnumerable<double>)GetValue(RowProportionsProperty); }
            set { SetValue(RowProportionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowProportions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowProportionsProperty =
            DependencyProperty.Register("RowProportions", typeof(IEnumerable<double>),
                typeof(RowHeightEditor), new FrameworkPropertyMetadata(null));


        #endregion

        #region Commands
        protected ICommand _OkCommand;
        public ICommand OkCommand
        {
            get
            {
                return _OkCommand;
            }
        }

        protected void OkCommandExecute(object parameter)
        {
            var bindingExpression = GetBindingExpression(RowProportionsProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }
        #endregion

        #region Interfaces
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
