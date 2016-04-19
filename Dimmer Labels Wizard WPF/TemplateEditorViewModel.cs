using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                if (_SelectedExistingTemplate != value)
                {
                    _SelectedExistingTemplate = value;
                    _UpperCellTemplate = value.UpperCellTemplate;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedExistingTemplate));
                    OnPropertyChanged(nameof(DisplayedStyle));
                    OnPropertyChanged(nameof(AssignedStrips));


                    // Command Executes.
                    _RemoveExistingTemplateCommand.CheckCanExecute();
                    _RenameTemplateCommand.CheckCanExecute();
                }
            }
        }


        protected LabelCellTemplate _UpperCellTemplate;

        public LabelCellTemplate UpperCellTemplate
        {
            get { return _UpperCellTemplate; }
            set
            {
                if (_UpperCellTemplate != value)
                {
                    _UpperCellTemplate = value;
                    _SelectedExistingTemplate.UpperCellTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(UpperCellTemplate));
                }

                // Notify Regardless of if the Value has changed. Updates to Properties of UpperCellTemplate
                // require the Displayed Style to Update.
                OnPropertyChanged(nameof(DisplayedStyle));
            }
        }

        public List<DimmerDistroUnit> ExampleUnits
        {
            get { return GenerateExampleUnits(); }
        }
        
        public Style DisplayedStyle
        {
            get
            {
                return SelectedExistingTemplate?.Style;
            }
        }

        public string TemplateName
        {
            get
            {
                return SelectedExistingTemplate.Name;
            }

            set
            {
                if (SelectedExistingTemplate.Name != value)
                {
                    SelectedExistingTemplate.Name = value;
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
                if (SelectedExistingTemplate != null)
                {
                    
                    return Math.Round(SelectedExistingTemplate.StripWidth / UnitConversionRatio, 2);
                }

                else
                {
                    return 0;
                }
                
            }
            set
            {
                if (SelectedExistingTemplate != null &&
                    SelectedExistingTemplate.StripWidth != value)
                {
                    SelectedExistingTemplate.StripWidth = value;

                    // Notify.
                    OnPropertyChanged(nameof(StripWidth));
                    OnPropertyChanged(nameof(DisplayedStyle));
                }
            }
        }

        public double StripHeight
        {
            get
            {
                if (SelectedExistingTemplate != null)
                {
                    return Math.Round(SelectedExistingTemplate.StripHeight / UnitConversionRatio, 2);
                }

                else
                {
                    return 0;
                }
                
            }
            set
            {
                if (SelectedExistingTemplate != null &&
                    SelectedExistingTemplate.StripHeight != value)
                {
                    SelectedExistingTemplate.StripHeight = value;

                    // Notify.
                    OnPropertyChanged(nameof(StripHeight));
                    OnPropertyChanged(nameof(DisplayedStyle));
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
                if (SelectedExistingTemplate != null)
                {
                    return SelectedExistingTemplate.StripMode;
                }

                else
                {
                    return LabelStripMode.Dual;
                }
            }
            set
            {
                if (SelectedExistingTemplate != null && SelectedExistingTemplate.StripMode != value)
                {
                    SelectedExistingTemplate.StripMode = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedStripMode));
                    OnPropertyChanged(nameof(DisplayedStyle));
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
            //var dialog = new RenameTemplate();
            //var viewModel = dialog.DataContext as RenameTemplateViewModel;

            //if (SelectedExistingTemplate != null)
            //{
            //    viewModel.Name = SelectedExistingTemplate.Name;

            //    if (dialog.ShowDialog() == true)
            //    {
            //        TemplateHelper.RenameTemplate(SelectedExistingTemplate, viewModel.Name);

            //        // Notify.
            //        OnPropertyChanged(nameof(TemplateName));
            //    }
            //}

            throw new NotImplementedException();
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
            //TemplateHelper.RemoveExistingTemplate(SelectedExistingTemplate);
            //SelectedExistingTemplate = Globals.DefaultTemplate;

            throw new NotImplementedException();
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

            // TemplateHelper.ModifyExistingTemplate(SelectedExistingTemplate, DisplayedStyle);
            throw new NotImplementedException();

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
    }
}
