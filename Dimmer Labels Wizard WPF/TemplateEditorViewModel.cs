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

            _RemoveExistingTemplateCommand = new RelayCommand(RemoveExistingTemplateCommandExecute,
                RemoveExistingTemplateTemplateCommandCanExecute);

            _AssignRightCommand = new RelayCommand(AssignRightCommandExecute);

            _AssignLeftCommand = new RelayCommand(AssignLeftCommandExecute);

            _OkCommand = new RelayCommand(OkCommandExecute);

            _RenameTemplateCommand = new RelayCommand(RenameTemplateCommandExecute, RenameTemplateCommandCanExecute);

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
                if (_SelectedExistingTemplate != value && value != null)
                {
                    // Modify Existing Template.
                    TemplateHelper.ModifyExistingTemplate(_SelectedExistingTemplate, DisplayedTemplate);

                    // Execute Switch.
                    _SelectedExistingTemplate = value;
                    DisplayedTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedExistingTemplate));
                    OnPropertyChanged(nameof(AssignedStrips));

                    // Command Executes.
                    _RemoveExistingTemplateCommand.CheckCanExecute();
                    _RenameTemplateCommand.CheckCanExecute();
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
                    OnPropertyChanged(nameof(DisplayedLowerCellTemplate));
                    OnPropertyChanged(nameof(StripWidth));
                    OnPropertyChanged(nameof(StripHeight));
                    OnPropertyChanged(nameof(SelectedStripMode));

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
                    Name = DisplayedTemplate.Name,
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
                    Name = DisplayedTemplate.Name,
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

        public IEnumerable<Strip> AssignedStrips
        {
            get
            {
                return GetAssignedStrips();
            }
        }

        public IEnumerable<Strip> AllStrips
        {
            get
            {
                return Globals.Strips;
            }
        }

        public List<Strip> SelectedAllStrips
        {
            get
            {
                return (from strip in AllStrips
                       where strip.IsPoolSelected == true
                       select strip).ToList();
            }
        }

        public List<Strip> SelectedAssignedStrips
        {
            get
            {
                return (from strip in AssignedStrips
                       where strip.IsAssignedSelected == true
                       select strip).ToList();
            }
        }


        protected int _SelectedTabIndex = 0;

        public int SelectedTabIndex
        {
            get { return _SelectedTabIndex; }
            set
            {
                if (_SelectedTabIndex != value)
                {
                    _SelectedTabIndex = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedTabIndex));
                }
            }
        }

        #endregion

        #region Commands

        protected RelayCommand _RenameTemplateCommand;
        public ICommand RenameTemplateCommand
        {
            get
            {
                return _RenameTemplateCommand;
            }
        }

        protected void RenameTemplateCommandExecute(object parameter)
        {
            var dialog = new RenameTemplate();
            var viewModel = dialog.DataContext as RenameTemplateViewModel;

            if (SelectedExistingTemplate != null)
            {
                viewModel.Name = SelectedExistingTemplate.Name;

                if (dialog.ShowDialog() == true)
                {
                    TemplateHelper.RenameTemplate(SelectedExistingTemplate, viewModel.Name);

                    // Notify.
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }

        protected bool RenameTemplateCommandCanExecute(object parameter)
        {
            return SelectedExistingTemplate != null;
        }

        protected RelayCommand _AssignRightCommand;
        public ICommand AssignRightCommand
        {
            get
            {
                return _AssignRightCommand;
            }
        }

        protected void AssignRightCommandExecute(object parameter)
        {
            // DeAssign.. Assign Selected Strips to Default Template.
            foreach (var element in SelectedAssignedStrips)
            {
                element.AssignedTemplate = Globals.DefaultTemplate;
            }

            OnPropertyChanged(nameof(AssignedStrips));
        }

        protected RelayCommand _AssignLeftCommand;
        public ICommand AssignLeftCommand
        {
            get
            {
                return _AssignLeftCommand;
            }
        }

        protected void AssignLeftCommandExecute(object parameter)
        {
            if (SelectedExistingTemplate != null)
            {
                foreach (var element in SelectedAllStrips)
                {
                    element.AssignedTemplate = SelectedExistingTemplate;
                }

                OnPropertyChanged(nameof(AssignedStrips));
            }
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

        protected RelayCommand _RemoveExistingTemplateCommand;

        public ICommand RemoveExistingTemplateCommand
        {
            get
            {
                return _RemoveExistingTemplateCommand;
            }
        }

        protected void RemoveExistingTemplateCommandExecute(object parameter)
        {
            TemplateHelper.RemoveExistingTemplate(SelectedExistingTemplate);
            SelectedExistingTemplate = Globals.DefaultTemplate;
        }

        protected bool RemoveExistingTemplateTemplateCommandCanExecute(object parameter)
        {
            if (SelectedExistingTemplate == null)
            {
                return false;
            }

            return !SelectedExistingTemplate.IsBuiltIn;
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

            window.Close();
        }
        #endregion

        #region Methods

        private IEnumerable<Strip> GetAssignedStrips()
        {
            if (SelectedExistingTemplate == null)
            {
                return null;
            }

            // Query for a Collection of strips with the Selected Template assigned to them.
            var query = from strip in Globals.Strips
                        where strip.AssignedTemplate == SelectedExistingTemplate
                        select strip;

            return query;
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


        #endregion
    }
}
