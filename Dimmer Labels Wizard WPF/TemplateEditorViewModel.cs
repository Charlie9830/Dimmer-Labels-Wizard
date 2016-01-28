using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class TemplateEditorViewModel : ViewModelBase
    {
        #region Constructor
        public TemplateEditorViewModel()
        {
            BasedOnTemplateSelection = Globals.Templates[1];

            // Command Binding
            _MoveUpperRowTemplateUpCommand = new RelayCommand(MoveUpperRowTemplateUpCommandExecute,
                MoveUpperRowTemplateUpCommandCanExecute);

            // Event Subscriptions
            UpperRowTemplates.CollectionChanged += UpperRowTemplates_CollectionChanged;
        }
        #endregion

        #region Fields.
        // Constants
        protected const double UnitConversionRatio = 96d / 25.4d;
        #endregion

        #region Binding Properties.

        public List<DimmerDistroUnit> ExampleUnits
        {
            get { return GenerateExampleUnits(); }
        }
        
        protected LabelStripTemplate _DisplayedTemplate;

        public LabelStripTemplate DisplayedTemplate
        {
            get
            {
                return _DisplayedTemplate;
            }
            set
            {
                if (_DisplayedTemplate != value)
                {
                    _DisplayedTemplate = value;
                    DisplayedUpperCellTemplate = value.UpperCellTemplate;

                    // Notify.
                    OnPropertyChanged(nameof(DisplayedTemplate));
                }
            }
        }

        private LabelCellTemplate _DisplayedUpperCellTemplate;
        public LabelCellTemplate DisplayedUpperCellTemplate
        {
            get
            {
                return _DisplayedUpperCellTemplate;
            }
            set
            {
                if (_DisplayedUpperCellTemplate != value)
                {
                    _DisplayedUpperCellTemplate = value;

                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        UpperCellTemplate = value
                    };
                }
            }
        }

        public string TemplateName
        {
            get
            {
                return DisplayedTemplate.Name;
            }

            set
            {
                if (DisplayedTemplate.Name != value)
                {
                    DisplayedTemplate.Name = value;
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }

        public IEnumerable<LabelStripTemplate> BasedOnTemplates
        {
            get
            {
                return Globals.Templates;
            }
        }

        protected LabelStripTemplate _BasedOnTemplateSelection;

        public LabelStripTemplate BasedOnTemplateSelection
        {
            get
            {
                return _BasedOnTemplateSelection;
            }

            set
            {
                if (_BasedOnTemplateSelection != value)
                {
                    _BasedOnTemplateSelection = value;
                    DisplayedTemplate = new LabelStripTemplate(_BasedOnTemplateSelection);

                    // Notify other Controls.
                    OnBasedOnTemplateSelectionChanged();

                    // Notify.
                    OnPropertyChanged(nameof(BasedOnTemplateSelection));
                }
            }
        }

        public double StripWidth
        {
            get
            {
                return DisplayedTemplate.StripWidth / UnitConversionRatio;
            }

            set
            {
                if (DisplayedTemplate.StripWidth / UnitConversionRatio != value)
                {
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        StripWidth = value * UnitConversionRatio
                    };
                }
            }
        }

        public double StripHeight
        {
            get
            {
                return DisplayedTemplate.StripHeight / UnitConversionRatio;
            }
            set
            {
                if (DisplayedTemplate.StripHeight / UnitConversionRatio != value)
                {
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        StripHeight = value * UnitConversionRatio
                    };
                }
            }
        }

        public IEnumerable<LabelStripMode> StripModes
        {
            get
            {
                return new LabelStripMode[] { LabelStripMode.Dual, LabelStripMode.Single };
                
            }
        }

        public LabelStripMode SelectedStripMode
        {
            get
            {
                return DisplayedTemplate.StripMode;
            }
            set
            {
                if (DisplayedTemplate.StripMode != value)
                {
                    DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                    {
                        Name = TemplateName,
                        StripMode = value
                    };
                }
            }
        }

        public IEnumerable<CellDataMode> CellDataModes
        {
            get
            {
                return new CellDataMode[] { CellDataMode.MixedField, CellDataMode.SingleField };

            }
        }

        public CellDataMode SelectedUpperCellDataMode
        {
            get
            {
                return DisplayedUpperCellTemplate.CellDataMode;
            }
            set
            {
                if (DisplayedUpperCellTemplate.CellDataMode != value)
                {
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        CellDataMode = value
                    };

                    // Set SingleField Enable State.
                    UpperSingleFieldModeEnabled = value == CellDataMode.SingleField ? true : false;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperCellDataMode));
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

        public CellRowHeightMode SelectedUpperRowHeightMode
        {
            get
            {
                return DisplayedUpperCellTemplate.RowHeightMode;
            }
            set
            {
                if (DisplayedUpperCellTemplate.RowHeightMode != value)
                {
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        RowHeightMode = value
                    };

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperRowHeightMode));
                }
            }
        }

        public Typeface UpperSingleFieldFont
        {
            get
            {
                return DisplayedUpperCellTemplate.SingleFieldFont;
            }
            set
            {
                if (DisplayedUpperCellTemplate.SingleFieldFont != value)
                {
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        SingleFieldFont = value
                    };

                    // Notify.
                    OnPropertyChanged(nameof(UpperSingleFieldFont));
                }
            }
        }

        public double UpperSingleFieldFontSize
        {
            get
            {
                return DisplayedUpperCellTemplate.SingleFieldDesiredFontSize;
            }
            set
            {
                if (DisplayedUpperCellTemplate.SingleFieldDesiredFontSize != value)
                {
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        SingleFieldDesiredFontSize = value
                    };

                    // Notify.
                    OnPropertyChanged(nameof(UpperSingleFieldFontSize));
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

        public LabelField UpperSingleFieldDataField
        {
            get
            {
                return DisplayedUpperCellTemplate.SingleFieldDataField;
            }
            set
            {
                if (DisplayedUpperCellTemplate.SingleFieldDataField != value)
                {
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        SingleFieldDataField = value
                    };

                    // Notify.
                    OnPropertyChanged(nameof(UpperSingleFieldDataField));
                }
            }
        }

        protected bool _UpperSingleFieldModeEnabled = true;
        public bool UpperSingleFieldModeEnabled
        {
            get
            {
                return _UpperSingleFieldModeEnabled;
            }
            set
            {
                if (_UpperSingleFieldModeEnabled != value)
                {
                    _UpperSingleFieldModeEnabled = value;

                    // Notify.
                    OnPropertyChanged(nameof(UpperSingleFieldModeEnabled));
                }
            }
        }

        private ObservableCollection<CellRowTemplate> _UpperRowTemplates = new ObservableCollection<CellRowTemplate>();

        public ObservableCollection<CellRowTemplate> UpperRowTemplates
        {
            get { return _UpperRowTemplates; }
            set { _UpperRowTemplates = value; }
        }

        private CellRowTemplate _SelectedUpperRowTemplate;

        public CellRowTemplate SelectedUpperRowTemplate
        {
            get { return _SelectedUpperRowTemplate; }
            set
            {
                if (_SelectedUpperRowTemplate != value)
                {
                    _SelectedUpperRowTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperRowTemplate));
                    _MoveUpperRowTemplateUpCommand.CheckCanExecute();
                    
                }
            }
        }



        #endregion

        #region Commands
        private RelayCommand _MoveUpperRowTemplateUpCommand;
        public ICommand MoveUpperRowTemplateUpCommand
        {
            get
            {
                return _MoveUpperRowTemplateUpCommand;
            }
        }

        protected void MoveUpperRowTemplateUpCommandExecute(object parameter)
        {
            var rowTemplate = SelectedUpperRowTemplate;
            var collection = UpperRowTemplates;

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

        protected bool MoveUpperRowTemplateUpCommandCanExecute(object parameter)
        {
            var rowTemplate = SelectedUpperRowTemplate;
            var collection = UpperRowTemplates;

            if (collection.IndexOf(rowTemplate) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Methods
        private void OnBasedOnTemplateSelectionChanged()
        {
            // Set Values.
            TemplateName = string.Empty;

            UpperSingleFieldModeEnabled = BasedOnTemplateSelection.UpperCellTemplate.CellDataMode ==
                CellDataMode.SingleField ? true : false;

            if (UpperRowTemplates.Count != BasedOnTemplateSelection.UpperCellTemplate.CellRowTemplates.Count())
            {
                // Clear.
                UpperRowTemplates.Clear();

                // Add new Cell Templates.
                foreach (var element in BasedOnTemplateSelection.UpperCellTemplate.CellRowTemplates)
                {
                    UpperRowTemplates.Add(element);
                }
            }

            // Raise Property Changed Notifications.
            OnPropertyChanged(nameof(StripWidth));
            OnPropertyChanged(nameof(StripHeight));
            OnPropertyChanged(nameof(SelectedStripMode));
        }

        private List<DimmerDistroUnit> GenerateExampleUnits()
        {
            var exampleUnit = new DimmerDistroUnit()
            {
                ChannelNumber = "Chan",
                InstrumentName = "Instr",
                Position = "Position",
                MulticoreName = "Multi",
                UserField1 = "User Field 1",
                UserField2 = "User Field 2",
                UserField3 = "User Field 3",
                UserField4 = "User Field 4",
                Custom = "Custom",
            };

            var examples = new List<DimmerDistroUnit>();

            for (int count = 1; count <= 12; count++)
            {
                examples.Add(exampleUnit);
            }

            return examples;
        }
        #endregion

        #region Event Handlers
        private void UpperRowTemplates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<CellRowTemplate>;

            // Push state of collection to DisplayedUpperCellTemplate.
            if (DisplayedUpperCellTemplate.CellRowTemplates.SequenceEqual(collection) == false)
            {
                DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                {
                    CellRowTemplates = collection.ToList()
                };
            }
        }
        #endregion
    }
}
