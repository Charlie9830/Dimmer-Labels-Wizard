using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dimmer_Labels_Wizard_WPF.Repositories;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public class UniqueCellTemplateEditorViewModel : ViewModelBase
    {
        public UniqueCellTemplateEditorViewModel()
        {
            // Database.
            _UniqueCellTemplateRepo = new UniqueCellTemplateRepository(new PrimaryDB());

            // Commands.
            _CreateNewUniqueTemplateCommand = new RelayCommand(CreateNewUniqueTemplateCommandExecute);
            _RemoveUniqueTemplateCommand = new RelayCommand(RemoveUniqueTemplateCommandExecute, RemoveUniqueTemplateCommandCanExecute);
            _OkCommand = new RelayCommand(OkCommandExecute);
            _CancelCommand = new RelayCommand(CancelCommandExecute);
        }

        protected UniqueCellTemplateRepository _UniqueCellTemplateRepo;

        #region Binding Source Properties.

        protected LabelCellTemplate _SelectedExistingUniqueTemplate;

        public LabelCellTemplate SelectedExistingUniqueTemplate
        {
            get { return _SelectedExistingUniqueTemplate; }
            set
            {
                if (_SelectedExistingUniqueTemplate != value)
                {
                    _SelectedExistingUniqueTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedExistingUniqueTemplate));
                }
            }
        }

        public IEnumerable<LabelCellTemplate> ExistingUniqueCellTemplates
        {
            get
            {
                return _UniqueCellTemplateRepo.GetUniqueCellTemplates();
            }
        }

        public string TemplateName
        {
            get { return GetTemplateName(); }
            set
            {
                if (ValidateName(value) == true)
                {
                    SetTemplateName(value);
                    ValidTemplateName = true;

                    OnPropertyChanged(nameof(TemplateName));
                }

                else
                {
                    ValidTemplateName = false;
                }
            }
        }

        protected bool _ValidTemplateName;

        public bool ValidTemplateName
        {
            get { return _ValidTemplateName; }

            protected set
            {
                if (_ValidTemplateName != value)
                {
                    _ValidTemplateName = value;
                    OnPropertyChanged(nameof(ValidTemplateName));
                    OnPropertyChanged(nameof(ValidTemplateNameInverse));
                }
            }
        }

        public bool ValidTemplateNameInverse
        {
            get
            {
                return !ValidTemplateName;
            }
        }

        protected LabelCellTemplate _SelectedExistingTemplate = new LabelCellTemplate();

        public LabelCellTemplate SelectedExistingTemplate
        {
            get { return _SelectedExistingTemplate; }
            set
            {
                if (_SelectedExistingTemplate != value)
                {
                    _SelectedExistingTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedExistingTemplate));
                    OnPropertyChanged(nameof(TemplateName));

                    // Executes.
                    _RemoveUniqueTemplateCommand.CheckCanExecute();
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


        protected DimmerDistroUnit _DataReference;

        public virtual DimmerDistroUnit DataReference
        {
            get
            {
                if (_DataReference == null)
                {
                    // Return a Demo Unit.
                    return (DimmerDistroUnit)Application.Current.Resources["DemoUnit"];
                }

                else
                {
                    return _DataReference;
                }
            }
            set
            {
                if (_DataReference != value)
                {
                    _DataReference = value;

                    // Notify.
                    OnPropertyChanged(nameof(DataReference));
                }
            }
        }

        #endregion

        #region Non Binding CLR Properties.
        public List<string> ExistingNames { get; set; } = new List<string>();
        #endregion

        #region Commands.

        protected RelayCommand _CreateNewUniqueTemplateCommand;
        public ICommand CreateNewUniqueTemplateCommand
        {
            get
            {
                return _CreateNewUniqueTemplateCommand;
            }
        }

        protected void CreateNewUniqueTemplateCommandExecute(object parameter)
        {
            CreateNewUniqueTemplate(null);
        }


        protected RelayCommand _RemoveUniqueTemplateCommand;
        public ICommand RemoveUniqueTemplateCommand
        {
            get
            {
                return _RemoveUniqueTemplateCommand;
            }
        }

        protected void RemoveUniqueTemplateCommandExecute(object parameter)
        {
            if (SelectedExistingTemplate != null)
            {
                _UniqueCellTemplateRepo.RemoveUniqueTemplate(SelectedExistingTemplate);
                _UniqueCellTemplateRepo.Save();

                SelectedExistingTemplate = new LabelCellTemplate();

                // Notify.
                OnPropertyChanged(nameof(ExistingUniqueCellTemplates));
            }
        }

        protected bool RemoveUniqueTemplateCommandCanExecute(object parameter)
        {
            return !(SelectedExistingTemplate == null);
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
            // Persist Data.
            PersistData();

            var window = parameter as UniqueCellTemplateEditor;

            window.DialogResult = true;
            window.Close();
        }

        protected RelayCommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand;
            }
        }

        protected void CancelCommandExecute(object parameter)
        {
            var window = parameter as UniqueCellTemplateEditor;

            window.DialogResult = false;
            window.Close();
        }

        #endregion

        #region Methods.
        /// <summary>
        /// Creates a new Unique Template and Inserts it into the Database.
        /// </summary>
        /// <param name="basedOn">Use if you want to base this off an existing Template, leave null if it's a brand new template.</param>
        public void CreateNewUniqueTemplate(LabelCellTemplate basedOn)
        {
            LabelCellTemplate newTemplate;

            if (basedOn == null)
            {
                // Brand new Template.
                newTemplate = new LabelCellTemplate()
                {
                    IsUniqueTemplate = true,
                    UniqueCellName = "New Special Template".ValidateAndAppendName(ExistingNames),
                };
            }

            else
            {
                // Clone basedOn Template.
                newTemplate = (LabelCellTemplate)basedOn.Clone();
                newTemplate.IsUniqueTemplate = true;
                newTemplate.UniqueCellName = "New Special Template".ValidateAndAppendName(ExistingNames);
            }

            // Insert into DB.
            _UniqueCellTemplateRepo.InsertUniqueTemplate(newTemplate);
            _UniqueCellTemplateRepo.Save();

            // Collect the EF Dynamic Proxy Object from DB and select it.
            SelectedExistingTemplate = _UniqueCellTemplateRepo.GetUniqueCellTemplates().Last();

            // Notify.
            OnPropertyChanged(nameof(ExistingUniqueCellTemplates));
        }

        protected string GetTemplateName()
        {
            if (SelectedExistingTemplate == null)
            {
                return string.Empty;
            }

            else
            {
                return SelectedExistingTemplate.UniqueCellName;
            }
        }

        protected void SetTemplateName(string name)
        {
            if (SelectedExistingTemplate != null)
            {
                SelectedExistingTemplate.UniqueCellName = name;
            }
        }

        protected bool ValidateName(string desiredName)
        {
            if (ExistingNames == null)
            {
                return true;
            }

            else
            {
                return !ExistingNames.Contains(desiredName);
            }
        }

        private void PersistData()
        {
            _UniqueCellTemplateRepo.Save();
        }

        #endregion
    }
}
