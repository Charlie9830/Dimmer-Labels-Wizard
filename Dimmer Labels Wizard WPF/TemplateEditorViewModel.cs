using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            // Command Binding
            _CreateNewTemplateCommand = new RelayCommand(CreateNewTemplateCommandExecute);
            
            _MoveUpperRowTemplateUpCommand = new RelayCommand(MoveUpperRowTemplateUpCommandExecute,
                MoveUpperRowTemplateUpCommandCanExecute);

            _MoveLowerRowTemplateUpCommand = new RelayCommand(MoveLowerRowTemplateUpCommandExecute,
                MoveLowerRowTemplateUpCommandCanExecute);

            _MoveUpperRowTemplateDownCommand = new RelayCommand(MoveUpperRowTemplateDownCommandExecute,
                MoveUpperRowTemplateDownCommandCanExecute);

            _MoveLowerRowTemplateDownCommand = new RelayCommand(MoveLowerRowTemplateDownCommandExecute,
                MoveLowerRowTemplateDownCommandCanExecute);

            _AddUpperRowTemplateCommand = new RelayCommand(AddUpperRowTemplateCommandExecute);

            _AddLowerRowTemplateCommand = new RelayCommand(AddLowerRowTemplateCommandExecute);

            _RemoveUpperRowTemplateCommand = new RelayCommand(RemoveUpperRowTemplateCommandExecute,
                RemoveUpperRowTemplateCommandCanExecute);

            _RemoveLowerRowTemplateCommand = new RelayCommand(RemoveLowerRowTemplateCommandExecute,
                RemoveLowerRowTemplateCommandCanExecute);

            _ShowUpperCellManualRowDialogCommand = new RelayCommand(ShowUpperCellManualRowDialogCommandExecute,
                ShowUpperCellManualRowDialogComandCanExecute);

            _ShowLowerCellManualRowDialogCommand = new RelayCommand(ShowLowerCellManualRowDialogCommandExecute,
                ShowLowerCellManualRowDialogComandCanExecute);

            _OkCommand = new RelayCommand(OkCommandExecute);

            // Event Subscriptions
            UpperRowTemplates.CollectionChanged += UpperRowTemplates_CollectionChanged;
            LowerRowTemplates.CollectionChanged += LowerRowTemplates_CollectionChanged;

            // Initialization.
            SelectedExistingTemplate = Globals.DefaultTemplate;
        }
        #endregion

        #region Fields.
        // Constants
        protected const double UnitConversionRatio = 96d / 25.4d;

        // Variables.
        protected bool IsInUpperRowCollectionChangedEvent = false;
        protected bool IsInLowerRowCollectionChangedEvent = false;
        #endregion

        #region Binding Properties.
        protected LabelStripTemplate _SelectedExistingTemplate;

        public LabelStripTemplate SelectedExistingTemplate
        {
            get { return _SelectedExistingTemplate; }
            set
            {
                if (_SelectedExistingTemplate != value)
                {
                    // Store Current state.
                    var previousSelectedExistingTemplate = _SelectedExistingTemplate;
                    var previousDisplayedTemplate = DisplayedTemplate;

                    // Execute Switch.
                    _SelectedExistingTemplate = value;
                    DisplayedTemplate = value;

                    //// Modify Existing Template.
                    //if (previousDisplayedTemplate != null && previousSelectedExistingTemplate != null)
                    //{
                    //    TemplateHelper.ModifyExistingTemplate(ExistingTemplates[templateIndex], previousDisplayedTemplate);
                    //}

                    // Notify.
                    OnPropertyChanged(nameof(SelectedExistingTemplate));
                }
            }
        }

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

                    // Notify.
                    OnPropertyChanged(nameof(DisplayedTemplate));
                    OnPropertyChanged(nameof(TemplateName));
                    OnPropertyChanged(nameof(DisplayedUpperCellTemplate));
                    OnPropertyChanged(nameof(StripWidth));
                    OnPropertyChanged(nameof(StripHeight));
                    OnPropertyChanged(nameof(SelectedStripMode));
                    OnPropertyChanged(nameof(SelectedUpperCellDataMode));
                    OnPropertyChanged(nameof(SelectedLowerCellDataMode));
                    OnPropertyChanged(nameof(SelectedUpperRowHeightMode));
                    OnPropertyChanged(nameof(SelectedLowerRowHeightMode));
                    OnPropertyChanged(nameof(SelectedUpperRowFont));
                    OnPropertyChanged(nameof(SelectedLowerRowFont));
                    OnPropertyChanged(nameof(SelectedUpperRowFontSize));
                    OnPropertyChanged(nameof(SelectedLowerRowFontSize));
                    OnPropertyChanged(nameof(SelectedUpperRowDataField));
                    OnPropertyChanged(nameof(SelectedLowerRowDataField));
                    OnPropertyChanged(nameof(UpperSingleFieldFont));
                    OnPropertyChanged(nameof(LowerSingleFieldFont));
                    OnPropertyChanged(nameof(UpperSingleFieldFontSize));
                    OnPropertyChanged(nameof(LowerSingleFieldFontSize));
                    OnPropertyChanged(nameof(UpperSingleFieldDataField));
                    OnPropertyChanged(nameof(LowerSingleFieldDataField));

                    // Enables
                    OnPropertyChanged(nameof(UpperMultiFieldModeEnable));
                    OnPropertyChanged(nameof(LowerMultiFieldModeEnable));
                    OnPropertyChanged(nameof(UpperSingleFieldModeEnable));
                    OnPropertyChanged(nameof(LowerSingleFieldModeEnable));

                    // Executes.
                    _ShowUpperCellManualRowDialogCommand.CheckCanExecute();
                    _ShowLowerCellManualRowDialogCommand.CheckCanExecute();


                    // UpperRowTemplates Collection.
                    if (IsInUpperRowCollectionChangedEvent == false)
                    {
                        UpperRowTemplates.Clear();
                        foreach (var element in value.UpperCellTemplate.CellRowTemplates)
                        {
                            UpperRowTemplates.Add(element);
                        }
                    }

                    // LowerRowTemplates Collection.
                    if (IsInLowerRowCollectionChangedEvent == false)
                    {
                        LowerRowTemplates.Clear();
                        foreach (var element in value.LowerCellTemplate.CellRowTemplates)
                        {
                            LowerRowTemplates.Add(element);
                        }
                    }
                }
            }

        }

        public LabelCellTemplate DisplayedUpperCellTemplate
        {
            get
            {
                return DisplayedTemplate.UpperCellTemplate;
            }
            
            set
            {
                DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                {
                    UpperCellTemplate = value
                };
            }
        }

        public LabelCellTemplate DisplayedLowerCellTemplate
        {
            get
            {
                return DisplayedTemplate.LowerCellTemplate;
            }
            set
            {
                DisplayedTemplate = new LabelStripTemplate(DisplayedTemplate)
                {
                    LowerCellTemplate = value
                };
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

        public ObservableCollection<LabelStripTemplate> ExistingTemplates
        {
            get
            {
                return Globals.Templates;
            }
        }

        public double StripWidth
        {
            get
            {
                return Math.Round(DisplayedTemplate.StripWidth / UnitConversionRatio, 2);
            }

            set
            {
                if (Math.Round(DisplayedTemplate.StripWidth / UnitConversionRatio, 2) != value)
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
                return Math.Round(DisplayedTemplate.StripHeight / UnitConversionRatio, 2);
            }
            set
            {
                if (Math.Round(DisplayedTemplate.StripHeight / UnitConversionRatio, 2) != value)
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
                return new CellDataMode[] { CellDataMode.MultiField, CellDataMode.SingleField };

            }
        }

        public CellDataMode SelectedUpperCellDataMode
        {
            get
            {
                return DisplayedTemplate.UpperCellTemplate.CellDataMode;
            }
            set
            {
                if (DisplayedUpperCellTemplate.CellDataMode != value)
                {
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        CellDataMode = value
                    };
                }
            }
        }

        public CellDataMode SelectedLowerCellDataMode
        {
            get
            {
                return DisplayedTemplate.LowerCellTemplate.CellDataMode;
            }
            set
            {
                if (DisplayedLowerCellTemplate.CellDataMode != value)
                {
                    DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                    {
                        CellDataMode = value
                    };
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
                }
            }
        }

        public CellRowHeightMode SelectedLowerRowHeightMode
        {
            get
            {
                return DisplayedLowerCellTemplate.RowHeightMode;
            }
            set
            {
                if (DisplayedLowerCellTemplate.RowHeightMode != value)
                {
                    DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                    {
                        RowHeightMode = value
                    };
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

                }
            }
        }

        public Typeface LowerSingleFieldFont
        {
            get
            {
                return DisplayedLowerCellTemplate.SingleFieldFont;
            }
            set
            {
                if (DisplayedLowerCellTemplate.SingleFieldFont != value)
                {
                    DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                    {
                        SingleFieldFont = value
                    };
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

        public double LowerSingleFieldFontSize
        {
            get
            {
                return DisplayedLowerCellTemplate.SingleFieldDesiredFontSize;
            }
            set
            {
                if (DisplayedLowerCellTemplate.SingleFieldDesiredFontSize != value)
                {
                    DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                    {
                        SingleFieldDesiredFontSize = value
                    };
                }

                // Notify.
                OnPropertyChanged(nameof(LowerSingleFieldFontSize));
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
                }
            }
        }

       public LabelField LowerSingleFieldDataField
        {
            get
            {
                return DisplayedLowerCellTemplate.SingleFieldDataField;
            }
            set
            {
                if (DisplayedLowerCellTemplate.SingleFieldDataField != value)
                {
                    DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                    {
                        SingleFieldDataField = value
                    };
                }
            }
        }

        private ObservableCollection<CellRowTemplate> _UpperRowTemplates = new ObservableCollection<CellRowTemplate>();

        public ObservableCollection<CellRowTemplate> UpperRowTemplates
        {
            get { return _UpperRowTemplates; }
            set { _UpperRowTemplates = value; }
        }

        private ObservableCollection<CellRowTemplate> _LowerRowTemplates = new ObservableCollection<CellRowTemplate>();

        public ObservableCollection<CellRowTemplate> LowerRowTemplates
        {
            get { return _LowerRowTemplates; }
            set { _LowerRowTemplates = value; }
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

                    _SelectedUpperRowFont = value == null ? null : value.Font;
                    _SelectedUpperRowFontSize = value == null ? 0 : value.DesiredFontSize;
                    _SelectedUpperRowDataField = value == null ? LabelField.NoAssignment : value.DataField;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperRowTemplate));
                    OnPropertyChanged(nameof(SelectedUpperRowFont));
                    OnPropertyChanged(nameof(SelectedUpperRowFontSize));
                    OnPropertyChanged(nameof(SelectedUpperRowDataField));

                    // Check Executes.
                    _MoveUpperRowTemplateUpCommand.CheckCanExecute();
                    _MoveUpperRowTemplateDownCommand.CheckCanExecute();
                    _RemoveUpperRowTemplateCommand.CheckCanExecute();
                }
            }
        }

        private CellRowTemplate _SelectedLowerRowTemplate;

        public CellRowTemplate SelectedLowerRowTemplate
        {
            get { return _SelectedLowerRowTemplate; }
            set
            {
                if (_SelectedLowerRowTemplate != value)
                {
                    _SelectedLowerRowTemplate = value;

                    _SelectedLowerRowFont = value == null ? null : value.Font;
                    _SelectedLowerRowFontSize = value == null ? 0 : value.DesiredFontSize;
                    _SelectedLowerRowDataField = value == null ? LabelField.NoAssignment : value.DataField;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLowerRowTemplate));
                    OnPropertyChanged(nameof(SelectedLowerRowFont));
                    OnPropertyChanged(nameof(SelectedLowerRowFontSize));
                    OnPropertyChanged(nameof(SelectedLowerRowDataField));

                    // Check Executes.
                    _MoveLowerRowTemplateUpCommand.CheckCanExecute();
                    _MoveLowerRowTemplateDownCommand.CheckCanExecute();
                    _RemoveLowerRowTemplateCommand.CheckCanExecute();
                }
            }
        }

        private Typeface _SelectedUpperRowFont = null;

        public Typeface SelectedUpperRowFont
        {
            get { return _SelectedUpperRowFont; }
            set
            {
               if (_SelectedUpperRowFont != value)
                {
                    _SelectedUpperRowFont = value;

                    if (SelectedUpperRowTemplate != null)
                    {
                        // Find the SelectedUpperRowTemplate in the UpperRowTemplates collection and modify it's value there,
                        // this will trigger the CollectionChanged Event so the change is propagated into the LabelStrip Template.
                        // Then Reset the SelectedUpperRowTemplate as it's reference will be broken.
                        int index = UpperRowTemplates.IndexOf(SelectedUpperRowTemplate);
                        UpperRowTemplates[index] = new CellRowTemplate(SelectedUpperRowTemplate)
                        {
                            Font = value
                        };

                        // Preserve Selection.
                        SelectedUpperRowTemplate = UpperRowTemplates[index];
                    }
                    
                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperRowFont));
                    
                }
            }
        }

        private Typeface _SelectedLowerRowFont = null;

        public Typeface SelectedLowerRowFont
        {
            get { return _SelectedLowerRowFont; }
            set
            {
                if (_SelectedLowerRowFont != value)
                {
                    _SelectedLowerRowFont = value;

                    if (SelectedLowerRowTemplate != null)
                    {
                        // Find the SelectedLowerRowTemplate in the LowerRowTemplates collection and modify it's value there,
                        // this will trigger the CollectionChanged Event so the change is propagated into the LabelStrip Template.
                        // Then Reset the SelectedLowerRowTemplate as it's reference will be broken.
                        int index = LowerRowTemplates.IndexOf(SelectedLowerRowTemplate);
                        LowerRowTemplates[index] = new CellRowTemplate(SelectedLowerRowTemplate)
                        {
                            Font = value
                        };

                        // Preserve Selection.
                        SelectedLowerRowTemplate = LowerRowTemplates[index];
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLowerRowFont));

                }
            }
        }

        private double _SelectedUpperRowFontSize;

        public double SelectedUpperRowFontSize
        {
            get { return _SelectedUpperRowFontSize; }
            set
            {
                if (_SelectedUpperRowFontSize != value)
                {
                    _SelectedUpperRowFontSize = value;

                    if (SelectedUpperRowTemplate != null)
                    {
                        // Find the SelectedUpperRowTemplate in the UpperRowTemplates collection and modify it's value there,
                        // this will trigger the CollectionChanged Event so the change is propagated into the LabelStrip Template.
                        // Then Reset the SelectedUpperRowTemplate as it's reference will be broken.
                        int index = UpperRowTemplates.IndexOf(SelectedUpperRowTemplate);
                        UpperRowTemplates[index] = new CellRowTemplate(SelectedUpperRowTemplate)
                        {
                            DesiredFontSize = value
                        };

                        // Preserve Selection.
                        SelectedUpperRowTemplate = UpperRowTemplates[index];
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperRowFontSize));
                }
            }
        }

        private double _SelectedLowerRowFontSize;

        public double SelectedLowerRowFontSize
        {
            get { return _SelectedLowerRowFontSize; }
            set
            {
                if (_SelectedLowerRowFontSize != value)
                {
                    _SelectedLowerRowFontSize = value;

                    if (SelectedLowerRowTemplate != null)
                    {
                        // Find the SelectedLowerRowTemplate in the LowerRowTemplates collection and modify it's value there,
                        // this will trigger the CollectionChanged Event so the change is propagated into the LabelStrip Template.
                        // Then Reset the SelectedLowerRowTemplate as it's reference will be broken.
                        int index = LowerRowTemplates.IndexOf(SelectedLowerRowTemplate);
                        LowerRowTemplates[index] = new CellRowTemplate(SelectedLowerRowTemplate)
                        {
                            DesiredFontSize = value
                        };

                        // Preserve Selection.
                        SelectedLowerRowTemplate = LowerRowTemplates[index];
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLowerRowFontSize));
                }
            }
        }

        private LabelField _SelectedUpperRowDataField;

        public LabelField SelectedUpperRowDataField
        {
            get { return _SelectedUpperRowDataField; }
            set
            {
                if (_SelectedUpperRowDataField != value)
                {
                    _SelectedUpperRowDataField = value;

                    if (SelectedUpperRowTemplate != null)
                    {
                        // Find the SelectedUpperRowTemplate in the UpperRowTemplates collection and modify it's value there,
                        // this will trigger the CollectionChanged Event so the change is propagated into the LabelStrip Template.
                        // Then Reset the SelectedUpperRowTemplate as it's reference will be broken.
                        int index = UpperRowTemplates.IndexOf(SelectedUpperRowTemplate);
                        UpperRowTemplates[index] = new CellRowTemplate(SelectedUpperRowTemplate)
                        {
                            DataField = value
                        };

                        // Preserve Selection.
                        SelectedUpperRowTemplate = UpperRowTemplates[index];
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperRowDataField));
                }
            }
        }

        private LabelField _SelectedLowerRowDataField;

        public LabelField SelectedLowerRowDataField
        {
            get { return _SelectedLowerRowDataField; }
            set
            {
                if (_SelectedLowerRowDataField != value)
                {
                    _SelectedLowerRowDataField = value;

                    if (SelectedLowerRowTemplate != null)
                    {
                        // Find the SelectedLowerRowTemplate in the LowerRowTemplates collection and modify it's value there,
                        // this will trigger the CollectionChanged Event so the change is propagated into the LabelStrip Template.
                        // Then Reset the SelectedLowerRowTemplate as it's reference will be broken.
                        int index = LowerRowTemplates.IndexOf(SelectedLowerRowTemplate);
                        LowerRowTemplates[index] = new CellRowTemplate(SelectedLowerRowTemplate)
                        {
                            DataField = value
                        };

                        // Preserve Selection.
                        SelectedLowerRowTemplate = LowerRowTemplates[index];
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedLowerRowDataField));
                }
            }
        }

        protected bool _UpperRowHeightEditorOpen = false;

        public bool UpperRowHeightEditorOpen
        {
            get { return _UpperRowHeightEditorOpen; }
            set
            {
                if (_UpperRowHeightEditorOpen != value)
                {
                    _UpperRowHeightEditorOpen = value;

                    // Notify.
                    OnPropertyChanged(nameof(UpperRowHeightEditorOpen));
                }
            }
        }

        protected bool _LowerRowHeightEditorOpen = false;

        public bool LowerRowHeightEditorOpen
        {
            get { return _LowerRowHeightEditorOpen; }
            set
            {
                if (_LowerRowHeightEditorOpen != value)
                {
                    _LowerRowHeightEditorOpen = value;

                    // Notify.
                    OnPropertyChanged(nameof(LowerRowHeightEditorOpen));
                }
            }
        }

        protected List<double> _UpperRowHeightProportions = new List<double>();

        public List<double> UpperRowHeightProportions
        {
            get { return _UpperRowHeightProportions; }
            set
            {
                if (value != null && _UpperRowHeightProportions.SequenceEqual(value) == false)
                {
                    _UpperRowHeightProportions = value;

                    var newCellTemplates = new List<CellRowTemplate>();

                    // Iterate through both the Proportions Collection and the existing CellRowTemplates collection.
                    var rowTemplateEnumerator = DisplayedUpperCellTemplate.CellRowTemplates.GetEnumerator();
                    var proportionsEnumerator = value.GetEnumerator();

                    while (rowTemplateEnumerator.MoveNext() && proportionsEnumerator.MoveNext())
                    {
                        newCellTemplates.Add(new CellRowTemplate(rowTemplateEnumerator.Current)
                        { ManualRowHeight = proportionsEnumerator.Current });
                    }

                    // Write back to DisplayedUpperCellTemplate.
                    DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                    {
                        CellRowTemplates = newCellTemplates
                    };

                }
            }
        }

        protected List<double> _LowerRowHeightProportions = new List<double>();

        public List<double> LowerRowHeightProportions
        {
            get { return _LowerRowHeightProportions; }
            set
            {
                if (value != null && _LowerRowHeightProportions.SequenceEqual(value) == false)
                {
                    _LowerRowHeightProportions = value;

                    var newCellTemplates = new List<CellRowTemplate>();

                    // Iterate through both the Proportions Collection and the existing CellRowTemplates collection.
                    var rowTemplateEnumerator = DisplayedLowerCellTemplate.CellRowTemplates.GetEnumerator();
                    var proportionsEnumerator = value.GetEnumerator();

                    while (rowTemplateEnumerator.MoveNext() && proportionsEnumerator.MoveNext())
                    {
                        newCellTemplates.Add(new CellRowTemplate(rowTemplateEnumerator.Current)
                        { ManualRowHeight = proportionsEnumerator.Current });
                    }

                    // Write back to DisplayedLowerCellTemplate.
                    DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                    {
                        CellRowTemplates = newCellTemplates
                    };

                }
            }
        }


        public bool UpperSingleFieldModeEnable
        {
            get
            {
                return SelectedUpperCellDataMode == CellDataMode.SingleField;
            }
        }

        public bool LowerSingleFieldModeEnable
        {
            get
            {
                return SelectedLowerCellDataMode == CellDataMode.SingleField;
            }
        }

        public bool UpperMultiFieldModeEnable
        {
            get
            {
                return SelectedUpperCellDataMode == CellDataMode.MultiField;
            }
        }

        public bool LowerMultiFieldModeEnable
        {
            get
            {
                return SelectedLowerCellDataMode == CellDataMode.MultiField;
            }
        }

        #endregion

        #region Commands
        private RelayCommand _ShowUpperCellManualRowDialogCommand;
        public ICommand ShowUpperCellManualRowDialogCommand
        {
            get
            {
                return _ShowUpperCellManualRowDialogCommand;
            }
        }

        protected void ShowUpperCellManualRowDialogCommandExecute(object parameter)
        {
            UpperRowHeightEditorOpen = !UpperRowHeightEditorOpen;
        }

        protected bool ShowUpperCellManualRowDialogComandCanExecute(object parameter)
        {
            return SelectedUpperRowHeightMode == CellRowHeightMode.Manual;
        }

        private RelayCommand _ShowLowerCellManualRowDialogCommand;
        public ICommand ShowLowerCellManualRowDialogCommand
        {
            get
            {
                return _ShowLowerCellManualRowDialogCommand;
            }
        }

        protected void ShowLowerCellManualRowDialogCommandExecute(object parameter)
        {
            LowerRowHeightEditorOpen = !LowerRowHeightEditorOpen;
        }

        protected bool ShowLowerCellManualRowDialogComandCanExecute(object parameter)
        {
            return SelectedLowerRowHeightMode == CellRowHeightMode.Manual;
        }


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

        private RelayCommand _MoveLowerRowTemplateUpCommand;
        public ICommand MoveLowerRowTemplateUpCommand
        {
            get
            {
                return _MoveLowerRowTemplateUpCommand;
            }
        }

        protected void MoveLowerRowTemplateUpCommandExecute(object parameter)
        {
            var rowTemplate = SelectedLowerRowTemplate;
            var collection = LowerRowTemplates;

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

        protected bool MoveLowerRowTemplateUpCommandCanExecute(object parameter)
        {
            var rowTemplate = SelectedLowerRowTemplate;
            var collection = LowerRowTemplates;

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

        private RelayCommand _MoveUpperRowTemplateDownCommand;
        public ICommand MoveUpperRowTemplateDownCommand
        {
            get
            {
                return _MoveUpperRowTemplateDownCommand;
            }
        }

        protected void MoveUpperRowTemplateDownCommandExecute(object parameter)
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
            if (currentIndex != collection.Count - 1)
            {
                collection.Move(currentIndex, currentIndex + 1);
            }
        }

        protected bool MoveUpperRowTemplateDownCommandCanExecute(object parameter)
        {
            var rowTemplate = SelectedUpperRowTemplate;
            var collection = UpperRowTemplates;

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

        private RelayCommand _MoveLowerRowTemplateDownCommand;
        public ICommand MoveLowerRowTemplateDownCommand
        {
            get
            {
                return _MoveLowerRowTemplateDownCommand;
            }
        }

        protected void MoveLowerRowTemplateDownCommandExecute(object parameter)
        {
            var rowTemplate = SelectedLowerRowTemplate;
            var collection = LowerRowTemplates;

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

        protected bool MoveLowerRowTemplateDownCommandCanExecute(object parameter)
        {
            var rowTemplate = SelectedLowerRowTemplate;
            var collection = LowerRowTemplates;

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

        private RelayCommand _AddUpperRowTemplateCommand;
        public ICommand AddUpperRowTemplateCommand
        {
            get
            {
                return _AddUpperRowTemplateCommand;
            }
        }

        protected void AddUpperRowTemplateCommandExecute(object parameter)
        {
            // Create a new Template, Add it to the collection and Select it.
            CellRowTemplate rowTemplate = new CellRowTemplate();
            UpperRowTemplates.Add(rowTemplate);
            SelectedUpperRowTemplate = rowTemplate;
        }

        private RelayCommand _AddLowerRowTemplateCommand;
        public ICommand AddLowerRowTemplateCommand
        {
            get
            {
                return _AddLowerRowTemplateCommand;
            }
        }

        protected void AddLowerRowTemplateCommandExecute(object parameter)
        {
            // Create a new Template, Add it to the collection and Select it.
            CellRowTemplate rowTemplate = new CellRowTemplate();
            LowerRowTemplates.Add(rowTemplate);
            SelectedLowerRowTemplate = rowTemplate;
        }

        private RelayCommand _RemoveUpperRowTemplateCommand;
        public ICommand RemoveUpperRowTemplateCommand
        {
            get
            {
                return _RemoveUpperRowTemplateCommand;
            }
        }

        protected void RemoveUpperRowTemplateCommandExecute(object parameter)
        {
            var collection = UpperRowTemplates;
            var selectedItem = SelectedUpperRowTemplate;
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
                        SelectedUpperRowTemplate = collection[0];
                    }

                    else if (selectedItemIndex == originalCollectionCount - 1)
                    {
                        // Bottom most Item was Removed.
                        SelectedUpperRowTemplate = collection[collectionCount - 1];
                    }

                    else
                    {
                        // Middle Item was removed.
                        if (selectedItemIndex < collectionCount)
                        {
                            SelectedUpperRowTemplate = collection[selectedItemIndex];
                        }

                        else
                        {
                            SelectedUpperRowTemplate = collection[selectedItemIndex - 1];
                        }
                    }

                }
            }
        }

        protected bool RemoveUpperRowTemplateCommandCanExecute(object parameter)
        {
            var collection = UpperRowTemplates;
            var selectedItem = SelectedUpperRowTemplate;

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

        private RelayCommand _RemoveLowerRowTemplateCommand;
        public ICommand RemoveLowerRowTemplateCommand
        {
            get
            {
                return _RemoveLowerRowTemplateCommand;
            }
        }

        protected void RemoveLowerRowTemplateCommandExecute(object parameter)
        {
            var collection = LowerRowTemplates;
            var selectedItem = SelectedLowerRowTemplate;
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
                        SelectedLowerRowTemplate = collection[0];
                    }

                    else if (selectedItemIndex == originalCollectionCount - 1)
                    {
                        // Bottom most Item was Removed.
                        SelectedLowerRowTemplate = collection[collectionCount - 1];
                    }

                    else
                    {
                        // Middle Item was removed.
                        if (selectedItemIndex < collectionCount)
                        {
                            SelectedLowerRowTemplate = collection[selectedItemIndex];
                        }

                        else
                        {
                            SelectedLowerRowTemplate = collection[selectedItemIndex - 1];
                        }
                    }

                }
            }
        }

        protected bool RemoveLowerRowTemplateCommandCanExecute(object parameter)
        {
            var collection = LowerRowTemplates;
            var selectedItem = SelectedLowerRowTemplate;

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

        protected RelayCommand _CreateNewTemplateCommand;

        public ICommand CreateNewTemplateCommand
        {
            get
            {
                return _CreateNewTemplateCommand;
            }
        }

        protected void CreateNewTemplateCommandExecute(object parameter)
        {
            var dialog = new NewTemplate();
            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult == true)
            {
                SelectedExistingTemplate = Globals.Templates.Last();
            }
        }

        protected RelayCommand _OkCommand;

        public ICommand OkCommand
        {
            get
            {
                return _OkCommand;
            }
        }

        protected void OkCommandExecute(object parameter)
        {
            var window = parameter as TemplateEditor;

            TemplateHelper.ModifyExistingTemplate(SelectedExistingTemplate, DisplayedTemplate);

            //window.Close();
        }
        #endregion

        #region Methods

        private List<DimmerDistroUnit> GenerateExampleUnits()
        {
            var exampleUnit = new DimmerDistroUnit()
            {
                ChannelNumber = "Chan",
                InstrumentName = "Instr",
                Position = "Row0 Row1 Row2",
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

        protected void BeginRowTemplateCollectionChanged(CellVerticalPosition verticalPosition)
        {
            if (verticalPosition == CellVerticalPosition.Upper)
            {
                IsInUpperRowCollectionChangedEvent = true;
            }

            if (verticalPosition == CellVerticalPosition.Lower)
            {
                IsInLowerRowCollectionChangedEvent = true;
            }
        }

        protected void EndRowTemplateCollectionChanged(CellVerticalPosition verticalPosition)
        {
            if (verticalPosition == CellVerticalPosition.Upper)
            {
                IsInUpperRowCollectionChangedEvent = false;
            }

            if (verticalPosition == CellVerticalPosition.Lower)
            {
                IsInLowerRowCollectionChangedEvent = false;
            }
        }
        #endregion

        #region Event Handlers
        private void UpperRowTemplates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Flag Collection event has begun.
            BeginRowTemplateCollectionChanged(CellVerticalPosition.Upper);

            var collection = sender as ObservableCollection<CellRowTemplate>;

            // Push state of collection to DisplayedUpperCellTemplate.
            if (DisplayedUpperCellTemplate.CellRowTemplates.SequenceEqual(collection) == false)
            {
                DisplayedUpperCellTemplate = new LabelCellTemplate(DisplayedUpperCellTemplate)
                {
                    CellRowTemplates = collection.ToList()
                };
            }

            // Notify.
            _RemoveUpperRowTemplateCommand.CheckCanExecute();
            _MoveUpperRowTemplateUpCommand.CheckCanExecute();
            _MoveUpperRowTemplateDownCommand.CheckCanExecute();

            // Reset collection Event Flags.
            EndRowTemplateCollectionChanged(CellVerticalPosition.Upper);
        }

        private void LowerRowTemplates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Flag Collection event has begun.
            BeginRowTemplateCollectionChanged(CellVerticalPosition.Lower);

            var collection = sender as ObservableCollection<CellRowTemplate>;

            // Push state of collection to DisplayedLowerCellTemplate.
            if (DisplayedLowerCellTemplate.CellRowTemplates.SequenceEqual(collection) == false)
            {
                DisplayedLowerCellTemplate = new LabelCellTemplate(DisplayedLowerCellTemplate)
                {
                    CellRowTemplates = collection.ToList()
                };
            }

            // Notify.
            _RemoveLowerRowTemplateCommand.CheckCanExecute();
            _MoveLowerRowTemplateUpCommand.CheckCanExecute();
            _MoveLowerRowTemplateDownCommand.CheckCanExecute();

            // Reset collection Event Flags.
            EndRowTemplateCollectionChanged(CellVerticalPosition.Lower);
        }
        #endregion
    }
}
