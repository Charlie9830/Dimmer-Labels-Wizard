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
using Dimmer_Labels_Wizard_WPF.Repositories;
using System.ComponentModel;
using Microsoft.Win32;

namespace Dimmer_Labels_Wizard_WPF
{
    public class TemplateEditorViewModel : ViewModelBase
    {
        #region Constructor
        public TemplateEditorViewModel()
        {
            // Repositories.
            PrimaryDB context = new PrimaryDB();
            _TemplateRepository = new TemplateRepository(context);
            _StripRepository = new StripRepository(context);

            // Loads.
            _StripRepository.Load();

            // Command Binding
            _CreateNewTemplateCommand = new RelayCommand(CreateNewTemplateCommandExecute);
            _RemoveExistingTemplateCommand = new RelayCommand(RemoveExistingTemplateCommandExecute,
                RemoveExistingTemplateTemplateCommandCanExecute);
            _AssignRightCommand = new RelayCommand(AssignRightCommandExecute);
            _AssignLeftCommand = new RelayCommand(AssignLeftCommandExecute);
            _OkCommand = new RelayCommand(OkCommandExecute);
            _RenameTemplateCommand = new RelayCommand(RenameTemplateCommandExecute, RenameTemplateCommandCanExecute);
            _ImportTemplateCommand = new RelayCommand(ImportTemplateCommandExecute);
            _ExportTemplateCommand = new RelayCommand(ExportTemplateCommandExecute, ExportTemplateCommandCanExecute);

            // Event Handling.
            StripSpacers.CollectionChanged += StripSpacers_CollectionChanged;

            // Initialization.
            SelectedExistingTemplate = null;
        }

        #endregion

        #region Fields.

        // Constants
        protected const double _UnitConversionRatio = 96d / 25.4d;
        protected const string _FileExt = "labt";
        protected const string _ExtFilter = "Label Template Files (.labt) |*.labt";

        // Database Repositories.
        protected TemplateRepository _TemplateRepository;
        protected StripRepository _StripRepository;
        
        protected bool _IsInUpperRowCollectionChangedEvent = false;
        protected bool _IsInLowerRowCollectionChangedEvent = false;
        protected bool _SelectedExistingTemplateChanging = false;
        #endregion

        #region Non Bound Properties.
        protected string _SelectedTemplateName;

        public string SelectedTemplateName
        {
            get { return _SelectedTemplateName; }
            set
            {
                if (_SelectedTemplateName != value)
                {
                    _SelectedTemplateName = value;
                    SelectedExistingTemplate = _TemplateRepository.GetTemplate(value);

                    // Notify.
                    OnPropertyChanged(nameof(SelectedTemplateName));
                }
            }
        }

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
                    BeginSelectedExistingTemplateChange();

                    _SelectedExistingTemplate = value;
                    _UpperCellTemplate = value.UpperCellTemplate;
                    _LowerCellTemplate = value.LowerCellTemplate;
                    _StripWidth = value.StripWidth;
                    _StripHeight = value.StripHeight;
                    
                    // Clear Strip Spacers.
                    while (StripSpacers.Count > 0)
                    {
                        StripSpacers.RemoveAt(StripSpacers.Count - 1);
                    }

                    foreach (var element in value.StripSpacers)
                    {
                        _StripSpacers.Add(element);
                    }

                    // Notify.
                    OnPropertyChanged(nameof(SelectedExistingTemplate));
                    OnPropertyChanged(nameof(StripWidth));
                    OnPropertyChanged(nameof(StripHeight));
                    OnPropertyChanged(nameof(DisplayedStyle));
                    OnPropertyChanged(nameof(AssignedStrips));
                    OnPropertyChanged(nameof(UpperCellTemplate));
                    OnPropertyChanged(nameof(LowerCellTemplate));
                    OnPropertyChanged(nameof(TemplateName));
                    OnPropertyChanged(nameof(StripSpacers));

                    // Command Executes.
                    _RemoveExistingTemplateCommand.CheckCanExecute();
                    _RenameTemplateCommand.CheckCanExecute();
                    _ExportTemplateCommand.CheckCanExecute();

                    EndSelectedExistingTemplateChange();
                }
            }
        }


        protected LabelCellTemplate _UpperCellTemplate = new LabelCellTemplate();

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
                // invalidates Displayed Style.
                OnPropertyChanged(nameof(DisplayedStyle));
            }
        }


        protected LabelCellTemplate _LowerCellTemplate = new LabelCellTemplate();

        public LabelCellTemplate LowerCellTemplate
        {
            get { return _LowerCellTemplate; }
            set
            {
                if (_LowerCellTemplate != value)
                {
                    _LowerCellTemplate = value;
                    _SelectedExistingTemplate.LowerCellTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(LowerCellTemplate));
                }

                // Notify Regardless of if the Value has changed. Updates to Properties of UpperCellTemplate
                // invalidates Displayed Style.
                OnPropertyChanged(nameof(DisplayedStyle));
            }
        }

        public Style DisplayedStyle
        {
            get
            {
                return SelectedExistingTemplate?.Style;
            }
        }

        public List<DimmerDistroUnit> ExampleUnits
        {
            get { return GenerateExampleUnits(); }
        }
        


        public string TemplateName
        {
            get
            {
                return SelectedExistingTemplate?.Name;
            }

            set
            {
                if (SelectedExistingTemplate.Name != value && 
                    SelectedExistingTemplate != null)
                {
                    SelectedExistingTemplate.Name = value;
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }

        public IList<LabelStripTemplate> ExistingTemplates
        {
            get
            {
                if (_TemplateRepository == null)
                {
                    return new List<LabelStripTemplate>();
                }

                else
                {
                    return _TemplateRepository.GetTemplates();
                }
            }
        }


        protected double _StripWidth;

        public double StripWidth
        {
            get { return Math.Round(_StripWidth / _UnitConversionRatio,2); }
            set
            {
                if (_StripWidth != value * _UnitConversionRatio)
                {
                    _StripWidth = value * _UnitConversionRatio;
                    
                    if (SelectedExistingTemplate != null)
                    {
                        SelectedExistingTemplate.StripWidth = _StripWidth;

                        // Notify.
                        OnPropertyChanged(nameof(DisplayedStyle));
                    }

                    // Notify.
                    OnPropertyChanged(nameof(StripWidth));
                    
                }
            }
        }

        protected double _StripHeight;

        public double StripHeight
        {
            get { return Math.Round(_StripHeight / _UnitConversionRatio, 2); }
            set
            {
                if (_StripHeight != value * _UnitConversionRatio)
                {
                    _StripHeight = value * _UnitConversionRatio;

                    if (SelectedExistingTemplate != null)
                    {
                        SelectedExistingTemplate.StripHeight = _StripHeight;

                        // Notify.
                        OnPropertyChanged(nameof(DisplayedStyle));
                    }

                    // Notify.
                    OnPropertyChanged(nameof(StripHeight));

                }
            }
        }


        protected ObservableCollection<StripSpacer> _StripSpacers = new ObservableCollection<StripSpacer>();

        public ObservableCollection<StripSpacer> StripSpacers
        {
            get { return _StripSpacers; }
            set
            {
                if (_StripSpacers != value)
                {
                    _StripSpacers = value;

                    if (SelectedExistingTemplate != null)
                    {
                        SelectedExistingTemplate.StripSpacers = _StripSpacers.ToList();

                        // Notify.
                        OnPropertyChanged(nameof(DisplayedStyle));
                    }

                    // Notify.
                    OnPropertyChanged(nameof(StripSpacers));
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
                return _StripRepository.GetStrips();
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

        protected RelayCommand _ImportTemplateCommand;
        public ICommand ImportTemplateCommand
        {
            get
            {
                return _ImportTemplateCommand;
            }
        }

        protected void ImportTemplateCommandExecute(object parameter)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = _FileExt;
            fileDialog.Filter = _ExtFilter;

            if (fileDialog.ShowDialog() == true)
            {
                string filePath = fileDialog.FileName;
                var templateSaver = new TemplatePersitanceManager(filePath);

                // Deserialize.
                LabelStripTemplate incomingTemplate = templateSaver.DeserializeTemplate();

                if (incomingTemplate == null)
                {
                    MessageBox.Show("Something went wrong, Please check the file type and try again.",
                        "File Format Error", MessageBoxButton.OK);
                    return;
                }

                // Modify Template name if required.
                incomingTemplate.Name = ValidateAndAppendTemplateName(incomingTemplate.Name);

                // Insert into Database.
                _TemplateRepository.InsertTemplate(incomingTemplate);
                _TemplateRepository.Save();

                // Pre Select.
                SelectedExistingTemplate = _TemplateRepository.GetTemplates().Last();

                // Notify.
                OnPropertyChanged(nameof(ExistingTemplates));
            }
        }

        protected string ValidateAndAppendTemplateName(string incomingTemplateName)
        {
            var existingTemplates = ExistingTemplates.ToList();

            if (existingTemplates != null)
            {
                // Filter to just the Template Names.
                var templateNames = from template in existingTemplates
                                    select template.Name;

                // Find existing Identical Names.
                var existingIdenticalNames = (from name in templateNames
                                             where name == incomingTemplateName
                                             select name).ToList();

                if (existingIdenticalNames.Count > 0)
                {
                    // Existing Name found.
                    string existingName = existingIdenticalNames.First();

                    if (existingName == string.Empty)
                    {
                        return ValidateAndAppendTemplateName("Imported Template");
                    }

                    char lastChar = existingName.ToArray().Last();
                    if (char.IsNumber(lastChar) == true)
                    {
                        // Number has already been Appended, Iterate it.
                        int number = int.Parse(lastChar.ToString());
                        number++;
                        return ValidateAndAppendTemplateName(incomingTemplateName + " " + number);
                    }

                    else
                    {

                        return ValidateAndAppendTemplateName(incomingTemplateName + " " + 1);
                    }
                }

                else
                {
                    // No Existing name Found.
                    return incomingTemplateName;
                }
            }

            return incomingTemplateName;
        }

        protected RelayCommand _ExportTemplateCommand;
        public ICommand ExportTemplateCommand
        {
            get
            {
                return _ExportTemplateCommand;
            }
        }

        protected void ExportTemplateCommandExecute(object parameter)
        {
            if (SelectedExistingTemplate != null)
            {
                // Persist Data to Database so a new, Serializer Friendly Instance can be created.
                PersistData();

                var fileDialog = new SaveFileDialog();
                fileDialog.DefaultExt = _FileExt;
                fileDialog.Filter = _ExtFilter;
                fileDialog.FileName = SelectedExistingTemplate.Name;

                if (fileDialog.ShowDialog() == true)
                {
                    string filePath = fileDialog.FileName;
                    
                    var templateSaver = new TemplatePersitanceManager(filePath);

                    using (var context = new PrimaryDB(true))
                    {
                        var templateRepo = new TemplateRepository(context);
                        LabelStripTemplate outgoingTemplate = templateRepo.GetTemplate(SelectedTemplateName);

                        templateSaver.SerializeTemplate(outgoingTemplate);
                    }
                }
            }
        }

        protected bool ExportTemplateCommandCanExecute(object parameter)
        {
            return !(SelectedExistingTemplate == null);
        }

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
                element.AssignedTemplate = _TemplateRepository.GetDefaultTemplate();
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
                SelectedExistingTemplate = _TemplateRepository.GetTemplates().Last();
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
            int index = ExistingTemplates.IndexOf(SelectedExistingTemplate);

            // Remove Template.
            _TemplateRepository.RemoveTemplate(SelectedExistingTemplate);

            // Reset Selection.
            if (index < ExistingTemplates.Count)
            {
                SelectedExistingTemplate = ExistingTemplates[index];
            }

            else if (ExistingTemplates.Count == 0)
            {
                SelectedExistingTemplate = null;
            }

            else
            {
                SelectedExistingTemplate = ExistingTemplates.Last();
            }

            // Notify.
            OnPropertyChanged(nameof(ExistingTemplates));

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

            _TemplateRepository.UpdateTemplate(SelectedExistingTemplate);

            PersistData();

            _SelectedTemplateName = SelectedExistingTemplate.Name;

            window.Close();
        }
        #endregion

        #region Methods
        protected void BeginSelectedExistingTemplateChange()
        {
            _SelectedExistingTemplateChanging = true;
        }

        protected void EndSelectedExistingTemplateChange()
        {
            _SelectedExistingTemplateChanging = false;
        }

        protected void PersistData()
        {
            _TemplateRepository.Save();
            _StripRepository.Save();
        }

        private IEnumerable<Strip> GetAssignedStrips()
        {
            if (SelectedExistingTemplate == null)
            {
                return null;
            }

            // Query for a Collection of strips with the Selected Template assigned to them.
            var query = from strip in AllStrips
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

        private void RefreshStripSpacers()
        {
            if (SelectedExistingTemplate != null)
            {
                SelectedExistingTemplate.StripSpacers = StripSpacers.ToList();
            }

            // Notify.
            OnPropertyChanged(nameof(DisplayedStyle));
        }

        #endregion

        #region Event Handlers.
        private void StripSpacers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            var collection = sender as ObservableCollection<StripSpacer>;
            var newItems = e.NewItems;
            var oldItems = e.OldItems;

            if (oldItems != null)
            {
                // Disconnect Event Handlers.
                foreach (var element in oldItems)
                {
                    var item = element as INotifyPropertyChanged;
                    item.PropertyChanged -= StripSpacer_PropertyChanged;
                }

                if (_SelectedExistingTemplateChanging == false)
                {
                    // Refresh StripSpacers so LabelStrip will Re-Render.
                    RefreshStripSpacers();
                }
            }

            if (newItems != null)
            {
                // Connect Event Handler.
                foreach (var element in newItems)
                {
                    var item = element as INotifyPropertyChanged;

                    item.PropertyChanged += StripSpacer_PropertyChanged;
                }

                if (_SelectedExistingTemplateChanging == false)
                {
                    // Refresh StripSpacers so LabelStrip will Re-Render.
                    RefreshStripSpacers();
                }
            }
        }

        private void StripSpacer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StripSpacer.Index) || e.PropertyName == nameof(StripSpacer.Width))
            {
                // Refresh StripSpacers so LabelStrip will Re-Render.
                RefreshStripSpacers();
            }
        }
        #endregion
    }
}
