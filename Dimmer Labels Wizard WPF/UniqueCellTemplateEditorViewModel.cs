using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class UniqueCellTemplateEditorViewModel : ViewModelBase
    {
        public UniqueCellTemplateEditorViewModel()
        {
            _OkCommand = new RelayCommand(OkCommandExecute);
            _CancelCommand = new RelayCommand(CancelCommandExecute);
        }


        #region Binding Source Properties.
        protected LabelCellTemplate _DisplayedTemplate = Globals.DefaultTemplate.UpperCellTemplate;

        public LabelCellTemplate DisplayedTemplate
        {
            get { return _DisplayedTemplate; }
            set
            {
                if (_DisplayedTemplate != value)
                {
                    _DisplayedTemplate = value;

                    // Notify.
                    OnPropertyChanged(nameof(DisplayedTemplate));
                }
            }
        }


        protected DimmerDistroUnit _DataReference;

        public DimmerDistroUnit DataReference
        {
            get { return _DataReference; }
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
    }
}
