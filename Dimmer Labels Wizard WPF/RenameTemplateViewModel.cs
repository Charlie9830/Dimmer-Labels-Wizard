using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class RenameTemplateViewModel : ViewModelBase
    {
        public RenameTemplateViewModel()
        {
            // Commands.
            _OkCommand = new RelayCommand(OkCommandExecute, OkCommandCanExecute);
            _CancelCommand = new RelayCommand(CancelCommandExecute);
        }


        #region Constants.
        protected const string EnterNewName = "Enter new Name";
        protected const string InvalidName = "Invalid Name";
        protected const string NameInUse = "Name already in Use";
        protected const string ValidName = "";
        #endregion

        #region Fields
        protected bool IsValidName = true;
        #endregion

        #region Binding Sources.

        protected string _Name = EnterNewName;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    // Validates, Sets Validation Properties and calls _OkCommand.CheckCanExecute().
                    ValidateText();

                    // Notify.
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        protected string _ValidationText = InvalidName;

        public string ValidationText
        {
            get { return _ValidationText; }
            set
            {
                if (_ValidationText != value)
                {
                    _ValidationText = value;

                    // Notify.
                    OnPropertyChanged(nameof(ValidationText));
                }
            }
        }
        #endregion

        #region Commands.
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
            if (IsValidName)
            {
                var window = parameter as RenameTemplate;

                window.DialogResult = true;

                window.Close();
            }
        }

        protected bool OkCommandCanExecute(object parameter)
        {
            return IsValidName;
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
            var window = parameter as RenameTemplate;

            window.DialogResult = false;

            window.Close();
            
        }
        #endregion

        #region Methods.
        protected void ValidateText()
        {
            if (Name == EnterNewName)
            {
                ValidationText = InvalidName;
                IsValidName = false;
            }

            else if (TemplateHelper.ValidateTemplateName(Name) == false)
            {
                ValidationText = NameInUse;
                IsValidName = false;
            }

            else
            {
                ValidationText = ValidName;
                IsValidName = true;
            }

            _OkCommand.CheckCanExecute();
        }

        #endregion
    }
}
