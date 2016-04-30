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
    /// Interaction logic for CellTemplateControl.xaml
    /// </summary>
    public partial class CellTemplateControl : UserControl, INotifyPropertyChanged
    {
        public CellTemplateControl()
        {
            InitializeComponent();

            // Commands
            _MoveRowTemplateUpCommand = new RelayCommand(MoveRowTemplateUpCommandExecute,
                MoveRowTemplateUpCommandCanExecute);

            _MoveRowTemplateDownCommand = new RelayCommand(MoveRowTemplateDownCommandExecute,
                MoveRowTemplateDownCommandCanExecute);

            _AddRowTemplateCommand = new RelayCommand(AddRowTemplateCommandExecute);

            _RemoveRowTemplateCommand = new RelayCommand(RemoveRowTemplateCommandExecute,
                RemoveRowTemplateCommandCanExecute)
                ;
            _ShowCellManualRowDialogCommand = new RelayCommand(ShowCellManualRowDialogCommandExecute,
                ShowCellManualRowDialogComandCanExecute);

            // Events.
            _RowTemplates.CollectionChanged += RowTemplates_CollectionChanged;
        }

        #region Fields.
        protected bool IsInRowCollectionChangedEvent = false;
        protected bool IncomingTemplate = false;
        #endregion

        #region Binding Source Properties.

        protected string _Header = string.Empty;

        public string Header
        {
            get { return _Header; }
            set
            {
                if (_Header != value)
                {
                    _Header = value;

                    // Notify.
                    RaisePropertyChanged(nameof(Header));
                }
            }
        }

        public IEnumerable<CellDataMode> CellDataModes
        {
            get
            {
                return new CellDataMode[] { CellDataMode.MultiField, CellDataMode.SingleField };

            }
        }

        public CellDataMode SelectedCellDataMode
        {
            get
            {
                return CellTemplate.CellDataMode;
            }
            set
            {
                if (CellTemplate.CellDataMode != value)
                {
                    CellTemplate.CellDataMode = value;

                    // Notify.
                    RaisePropertyChanged(nameof(MultiFieldModeEnable));
                    RaisePropertyChanged(nameof(SingleFieldModeEnable));
                    ForceCellTemplateUpdate();
                }
            }
        }

        private ObservableCollection<CellRowTemplate> _RowTemplates = new ObservableCollection<CellRowTemplate>();

        public ObservableCollection<CellRowTemplate> RowTemplates
        {
            get { return _RowTemplates; }
            set { _RowTemplates = value; }
        }

        private CellRowTemplate _SelectedRowTemplate;

        public CellRowTemplate SelectedRowTemplate
        {
            get { return _SelectedRowTemplate; }
            set
            {
                if (_SelectedRowTemplate != value)
                {
                    _SelectedRowTemplate = value;

                    _SelectedRowFont = value == null ? null : value.Font;
                    _SelectedRowFontSize = value == null ? 0 : value.DesiredFontSize;
                    _SelectedRowDataField = value == null ? LabelField.NoAssignment : value.DataField;

                    // Notify.
                    RaisePropertyChanged(nameof(SelectedRowTemplate));
                    RaisePropertyChanged(nameof(SelectedRowFont));
                    RaisePropertyChanged(nameof(SelectedRowFontSize));
                    RaisePropertyChanged(nameof(SelectedRowDataField));

                    // Check Executes.
                    _MoveRowTemplateUpCommand.CheckCanExecute();
                    _MoveRowTemplateDownCommand.CheckCanExecute();
                    _RemoveRowTemplateCommand.CheckCanExecute();
                }
            }
        }

        public IEnumerable<CellRowHeightMode> RowHeightModes
        {
            get
            {
                return new CellRowHeightMode[]
                { CellRowHeightMode.Automatic, CellRowHeightMode.Manual, CellRowHeightMode.Static };
            }
        }

        public CellRowHeightMode SelectedRowHeightMode
        {
            get
            {
                return CellTemplate.RowHeightMode;
            }
            set
            {
                if (CellTemplate.RowHeightMode != value)
                {
                    CellTemplate.RowHeightMode = value;

                    // Notify.
                    ForceCellTemplateUpdate();
                }
            }
        }

        private Typeface _SelectedRowFont = null;

        public Typeface SelectedRowFont
        {
            get { return _SelectedRowFont; }
            set
            {
                if (_SelectedRowFont != value)
                {
                    _SelectedRowFont = value;

                    if (SelectedRowTemplate != null)
                    {
                        SelectedRowTemplate.Font = value;
                    }

                    // Notify.
                    RaisePropertyChanged(nameof(SelectedRowFont));
                    ForceCellTemplateUpdate();
                }
            }
        }

        private double _SelectedRowFontSize;

        public double SelectedRowFontSize
        {
            get { return _SelectedRowFontSize; }
            set
            {
                if (_SelectedRowFontSize != value)
                {
                    _SelectedRowFontSize = value;

                    if (SelectedRowTemplate != null)
                    {
                        SelectedRowTemplate.DesiredFontSize = value;
                    }

                    // Notify.
                    RaisePropertyChanged(nameof(SelectedRowFontSize));
                    ForceCellTemplateUpdate();
                }
            }
        }

        public LabelField[] LabelFields
        {
            get
            {
                return new LabelField[] {LabelField.ChannelNumber, LabelField.InstrumentName, LabelField.MulticoreName,
                    LabelField.Position, LabelField.UserField1, LabelField.UserField2, LabelField.UserField3,
                    LabelField.UserField4 };
            }
        }

        private LabelField _SelectedRowDataField;

        public LabelField SelectedRowDataField
        {
            get { return _SelectedRowDataField; }
            set
            {
                if (_SelectedRowDataField != value)
                {
                    _SelectedRowDataField = value;

                    if (SelectedRowTemplate != null)
                    {
                        SelectedRowTemplate.DataField = value;
                    }

                    // Notify.
                    RaisePropertyChanged(nameof(SelectedRowDataField));
                    ForceCellTemplateUpdate();
                }
            }
        }

        public Typeface SingleFieldFont
        {
            get
            {
                return CellTemplate.SingleFieldFont;
            }
            set
            {
                if (CellTemplate.SingleFieldFont != value)
                {
                    CellTemplate.SingleFieldFont = value;

                    // Notify.
                    ForceCellTemplateUpdate();
                }
            }
        }

        public double SingleFieldFontSize
        {
            get
            {
                return CellTemplate.SingleFieldDesiredFontSize;
            }
            set
            {
                if (CellTemplate.SingleFieldDesiredFontSize != value)
                {
                    CellTemplate.SingleFieldDesiredFontSize = value;

                    // Notify.
                    RaisePropertyChanged(nameof(SingleFieldFontSize));
                    ForceCellTemplateUpdate();
                }
            }
        }

       
        public LabelField SingleFieldDataField
        {
            get
            {
                return CellTemplate.SingleFieldDataField;
            }
            set
            {
                if (CellTemplate.SingleFieldDataField != value)
                {
                    CellTemplate.SingleFieldDataField = value;

                    // Notify.
                    ForceCellTemplateUpdate();
                }
            }
        }

        protected bool _RowHeightEditorOpen = false;

        public bool RowHeightEditorOpen
        {
            get { return _RowHeightEditorOpen; }
            set
            {
                if (_RowHeightEditorOpen != value)
                {
                    _RowHeightEditorOpen = value;

                    // Notify.
                    RaisePropertyChanged(nameof(RowHeightEditorOpen));
                }
            }
        }

        public bool MultiFieldModeEnable
        {
            get
            {
                return SelectedCellDataMode == CellDataMode.MultiField;
            }
        }

        public bool SingleFieldModeEnable
        {
            get
            {
                return SelectedCellDataMode == CellDataMode.SingleField;
            }
        }

        protected List<double> _RowHeightProportions = new List<double>();

        public List<double> RowHeightProportions
        {
            get { return _RowHeightProportions; }
            set
            {
                if (value != null && _RowHeightProportions.SequenceEqual(value) == false)
                {
                    _RowHeightProportions = value;

                    var newCellTemplates = new List<CellRowTemplate>();

                    // Iterate through both the Proportions Collection and the existing CellRowTemplates collection.
                    var rowTemplateEnumerator = CellTemplate.CellRowTemplates.GetEnumerator();
                    var proportionsEnumerator = value.GetEnumerator();

                    while (rowTemplateEnumerator.MoveNext() && proportionsEnumerator.MoveNext())
                    {
                        rowTemplateEnumerator.Current.ManualRowHeight = proportionsEnumerator.Current;
                    }

                    // Notify.
                    ForceCellTemplateUpdate();
                }
            }
        }

        #endregion

        #region Commands.
        private RelayCommand _AddRowTemplateCommand;
        public ICommand AddRowTemplateCommand
        {
            get
            {
                return _AddRowTemplateCommand;
            }
        }

        protected void AddRowTemplateCommandExecute(object parameter)
        {
            // Create a new Template, Add it to the collection and Select it.
            CellRowTemplate rowTemplate = new CellRowTemplate();
            RowTemplates.Add(rowTemplate);
            SelectedRowTemplate = rowTemplate;
        }

        private RelayCommand _RemoveRowTemplateCommand;
        public ICommand RemoveRowTemplateCommand
        {
            get
            {
                return _RemoveRowTemplateCommand;
            }
        }

        protected void RemoveRowTemplateCommandExecute(object parameter)
        {
            var collection = RowTemplates;
            var selectedItem = SelectedRowTemplate;
            int selectedItemIndex = collection.IndexOf(selectedItem);
            int collectionCount = collection.Count;
            int originalCollectionCount = collection.Count;

            if (collectionCount != 0)
            {
                // Remove Item.
                collection.Remove(selectedItem);

                // Update Collection Count.
                collectionCount = collection.Count;

                if (collectionCount > 0)
                {
                    // Set new Row Template Selection.
                    if (selectedItemIndex == 0)
                    {
                        // Top most Item was Removed.
                        SelectedRowTemplate = collection[0];
                    }

                    else if (selectedItemIndex == originalCollectionCount - 1)
                    {
                        // Bottom most Item was Removed.
                        SelectedRowTemplate = collection[collectionCount - 1];
                    }

                    else
                    {
                        // Middle Item was removed.
                        if (selectedItemIndex < collectionCount)
                        {
                            SelectedRowTemplate = collection[selectedItemIndex];
                        }

                        else
                        {
                            SelectedRowTemplate = collection[selectedItemIndex - 1];
                        }
                    }
                }
            }
        }

        protected bool RemoveRowTemplateCommandCanExecute(object parameter)
        {
            var collection = RowTemplates;
            var selectedItem = SelectedRowTemplate;

            if (selectedItem == null)
            {
                return false;
            }

            if (collection.Count == 0)
            {
                return false;
            }

            return true;
        }

        private RelayCommand _MoveRowTemplateUpCommand;
        public ICommand MoveRowTemplateUpCommand
        {
            get
            {
                return _MoveRowTemplateUpCommand;
            }
        }

        protected void MoveRowTemplateUpCommandExecute(object parameter)
        {
            var rowTemplate = SelectedRowTemplate;
            var collection = RowTemplates;

            if (rowTemplate == null)
            {
                // Nothing Selected.
                return;
            }

            // Execute Move.
            int currentIndex = collection.IndexOf(rowTemplate);
            if (currentIndex != 0)
            {
                collection.Move(currentIndex, currentIndex - 1);
            }
        }

        protected bool MoveRowTemplateUpCommandCanExecute(object parameter)
        {
            var rowTemplate = SelectedRowTemplate;
            var collection = RowTemplates;

            if (rowTemplate == null)
            {
                return false;
            }

            if (collection.IndexOf(rowTemplate) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private RelayCommand _MoveRowTemplateDownCommand;
        public ICommand MoveRowTemplateDownCommand
        {
            get
            {
                return _MoveRowTemplateDownCommand;
            }
        }

        protected void MoveRowTemplateDownCommandExecute(object parameter)
        {
            var rowTemplate = SelectedRowTemplate;
            var collection = RowTemplates;

            if (rowTemplate == null)
            {
                // Nothing Selected.
                return;
            }

            // Execute Move.
            int currentIndex = collection.IndexOf(rowTemplate);
            if (currentIndex != collection.Count - 1)
            {
                collection.Move(currentIndex, currentIndex + 1);
            }
        }

        protected bool MoveRowTemplateDownCommandCanExecute(object parameter)
        {
            var rowTemplate = SelectedRowTemplate;
            var collection = RowTemplates;

            if (rowTemplate == null)
            {
                return false;
            }

            if (collection.IndexOf(rowTemplate) == collection.Count - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private RelayCommand _ShowCellManualRowDialogCommand;
        public ICommand ShowCellManualRowDialogCommand
        {
            get
            {
                return _ShowCellManualRowDialogCommand;
            }
        }

        protected void ShowCellManualRowDialogCommandExecute(object parameter)
        {
            RowHeightEditorOpen = !RowHeightEditorOpen;
        }

        protected bool ShowCellManualRowDialogComandCanExecute(object parameter)
        {
            return SelectedRowHeightMode == CellRowHeightMode.Manual;
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
            DependencyProperty.Register("CellTemplate", typeof(LabelCellTemplate), typeof(CellTemplateControl),
                new FrameworkPropertyMetadata(new LabelCellTemplate(),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnCellTemplatePropertyChanged)));

        private static void OnCellTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CellTemplateControl;
            var newValue = e.NewValue as LabelCellTemplate;

            // Notify.
            instance.RaisePropertyChanged(nameof(SelectedCellDataMode));
            instance.RaisePropertyChanged(nameof(SelectedRowHeightMode));
            instance.RaisePropertyChanged(nameof(SelectedRowFont));
            instance.RaisePropertyChanged(nameof(SelectedRowFontSize));
            instance.RaisePropertyChanged(nameof(SelectedRowDataField));
            instance.RaisePropertyChanged(nameof(SingleFieldFont));
            instance.RaisePropertyChanged(nameof(SingleFieldFontSize));
            instance.RaisePropertyChanged(nameof(SingleFieldDataField));

            // Enables
            instance.RaisePropertyChanged(nameof(MultiFieldModeEnable));
            instance.RaisePropertyChanged(nameof(SingleFieldModeEnable));

            // Executes.
            instance._ShowCellManualRowDialogCommand.CheckCanExecute();

            // RowTemplates Collection.
            if (instance.IsInRowCollectionChangedEvent == false)
            {
                instance.IncomingTemplate = true;

                instance.RowTemplates.Clear();
                foreach (var element in newValue.CellRowTemplates)
                {
                    instance.RowTemplates.Add(element);
                }

                instance.IncomingTemplate = false;
            }
        }

        #endregion

        #region Methods
        protected void ForceCellTemplateUpdate()
        {
            BindingOperations.GetBindingExpressionBase(this, CellTemplateProperty).UpdateSource();
        }

        protected void BeginRowTemplateCollectionChanged()
        {
            IsInRowCollectionChangedEvent = true;
        }

        protected void EndRowTemplateCollectionChanged()
        {
            IsInRowCollectionChangedEvent = false;
        }
        #endregion

        #region Event Handlers
        private void RowTemplates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Flag Collection event has begun.
            BeginRowTemplateCollectionChanged();

            var collection = sender as ObservableCollection<CellRowTemplate>;

            // Push state of collection to CellTemplate.
            if (CellTemplate.CellRowTemplates.SequenceEqual(collection) == false &&
                IncomingTemplate == false)
            {
                CellTemplate.CellRowTemplates = collection.ToList();
            }

            // Notify.
            _RemoveRowTemplateCommand.CheckCanExecute();
            _MoveRowTemplateUpCommand.CheckCanExecute();
            _MoveRowTemplateDownCommand.CheckCanExecute();
            ForceCellTemplateUpdate();

            // Reset collection Event Flags.
            EndRowTemplateCollectionChanged();
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
