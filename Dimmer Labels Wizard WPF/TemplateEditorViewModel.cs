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
            BasedOnTemplateSelection = Globals.Templates[1];

            // Command Binding
            _MoveUpperRowTemplateUpCommand = new RelayCommand(MoveUpperRowTemplateUpCommandExecute,
                MoveUpperRowTemplateUpCommandCanExecute);

            _MoveUpperRowTemplateDownCommand = new RelayCommand(MoveUpperRowTemplateDownCommandExecute,
                MoveUpperRowTemplateDownCommandCanExecute);

            _AddUpperRowTemplateCommand = new RelayCommand(AddUpperRowTemplateCommandExecute);

            _RemoveUpperRowTemplateCommand = new RelayCommand(RemoveUpperRowTemplateCommandExecute,
                RemoveUpperRowTemplateCommandCanExecute);

            _ShowUpperCellManualRowDialogCommand = new RelayCommand(ShowUpperCellManualRowDialogCommandExecute,
                ShowUpperCellManualRowDialogComandCanExecute);


            // Event Subscriptions
            UpperRowTemplates.CollectionChanged += UpperRowTemplates_CollectionChanged;
        }
        #endregion

        #region Fields.
        // Constants
        protected const double UnitConversionRatio = 96d / 25.4d;

        // Variables.
        protected bool IsInUpperRowCollectionChangedEvent = false;
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

                    SelectedUpperCellDataMode = value.CellDataMode;
                    SelectedUpperRowHeightMode = value.RowHeightMode;
                    UpperSingleFieldFont = value.SingleFieldFont;
                    UpperSingleFieldFontSize = value.SingleFieldDesiredFontSize;
                    UpperSingleFieldDataField = value.SingleFieldDataField;

                    UpperRowHeightProportions = (from rowTemplate in value.CellRowTemplates
                                                select rowTemplate.ManualRowHeight).ToList();

                    if (IsInUpperRowCollectionChangedEvent == false)
                    {
                        // Can cause ReEntrancy Collection Changed Exception in some cases.
                        UpperRowTemplates.Clear();
                        foreach (var element in value.CellRowTemplates)
                        {
                            UpperRowTemplates.Add(element);
                        }
                    }

                    // Notify.
                    OnPropertyChanged(nameof(DisplayedUpperCellTemplate));
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
                return new CellDataMode[] { CellDataMode.MultiField, CellDataMode.SingleField };

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

                    // Set additional Control States.
                    if (value == CellDataMode.SingleField)
                    {
                        if (UpperRowTemplates.Count > 0)
                        {
                            // Set SingleFieldDataField to first existing Row Template. Otherwise Channel.
                            UpperSingleFieldDataField = UpperRowTemplates.First().DataField;
                        }

                        else
                        {
                            UpperRowTemplates.Clear();
                            UpperSingleFieldDataField = LabelField.ChannelNumber;
                        }
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedUpperCellDataMode));
                    OnPropertyChanged(nameof(UpperSingleFieldModeEnable));
                    OnPropertyChanged(nameof(UpperMultiFieldModeEnable));
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

                    // Notify.
                    OnPropertyChanged(nameof(UpperRowHeightProportions));

                }
            }
        }

        #endregion

        public bool UpperSingleFieldModeEnable
        {
            get
            {
                return SelectedUpperCellDataMode == CellDataMode.SingleField;
            }
        }

        public bool UpperMultiFieldModeEnable
        {
            get
            {
                return SelectedUpperCellDataMode == CellDataMode.MultiField;
            }
        }

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
            return true;
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

        #endregion

        #region Methods
        private void OnBasedOnTemplateSelectionChanged()
        {
            // Set Values.
            TemplateName = string.Empty;

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
        }

        protected void EndRowTemplateCollectionChanged(CellVerticalPosition verticalPosition)
        {
            if (verticalPosition == CellVerticalPosition.Upper)
            {
                IsInUpperRowCollectionChangedEvent = false;
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
        #endregion
    }
}
