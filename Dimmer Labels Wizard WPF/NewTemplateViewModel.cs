using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class NewTemplateViewModel : ViewModelBase
    {
        public NewTemplateViewModel()
        {
            // Command Binding.
            _CreateCommand = new RelayCommand(CreateCommandExecute, CreateCommandCanExecute);
            _CancelCommand = new RelayCommand(CancelCommandExecute);
        }

        protected const string _EnterTemplateName = "Enter Template Name";

        #region Binding Sources
        public IEnumerable<LabelStripTemplate> ExistingTemplates
        {
            get
            {
                return Globals.Templates;
            }
        }


        protected LabelStripTemplate _BasedOnTemplateSelection = Globals.DefaultTemplate;

        public LabelStripTemplate BasedOnTemplateSelection
        {
            get { return _BasedOnTemplateSelection; }
            set
            {
                if (_BasedOnTemplateSelection != value)
                {
                    _BasedOnTemplateSelection = value;

                    // Notify.
                    OnPropertyChanged(nameof(BasedOnTemplateSelection));
                }
            }
        }


        protected string _TemplateName = _EnterTemplateName;

        public string TemplateName
        {
            get { return _TemplateName; }
            set
            {
                if (_TemplateName != value)
                {
                    _TemplateName = value;

                    // Validate.
                    IsValidTemplateName = ValidateTemplateName(value);
                    _CreateCommand.CheckCanExecute();

                    // Notify.
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }


        protected bool _IsValidTemplateName = false;

        public bool IsValidTemplateName
        {
            get { return _IsValidTemplateName; }
            set
            {
                if (_IsValidTemplateName != value)
                {
                    _IsValidTemplateName = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsValidTemplateName));
                    OnPropertyChanged(nameof(IsValidationMessageVisible));
                }
            }
        }

        public bool IsValidationMessageVisible
        {
            get
            {
                return !IsValidTemplateName;
            }
        }
        #endregion

        #region Commands
        protected RelayCommand _CreateCommand;

        public ICommand CreateCommand
        {
            get
            {
                return _CreateCommand;
            }
        }

        protected void CreateCommandExecute(object parameter)
        {
            var window = parameter as NewTemplate;

            if (IsValidTemplateName)
            {
                //Globals.Templates.Add(new LabelStripTemplate(BasedOnTemplateSelection) { Name = TemplateName });
                throw new NotImplementedException();

                // Close Window.
                window.DialogResult = true;
                window.Close();
            }
        }

        protected bool CreateCommandCanExecute(object parameter)
        {
            return IsValidTemplateName;
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
            var window = parameter as NewTemplate;

            // Close Window.
            window.DialogResult = false;
            window.Close();
        }
        #endregion

        #region Methods
        protected bool ValidateTemplateName(string name)
        {
            // Collect existing names.
            var existingNames = from template in ExistingTemplates
                                select template.Name;

            if (existingNames.Contains(name) || name == _EnterTemplateName)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        #endregion
    }
}
