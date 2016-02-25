using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class AdvancedColorPickerViewModel : ViewModelBase
    {
        public AdvancedColorPickerViewModel()
        {
            // Commands.
            _OkCommand = new RelayCommand(OkCommandExecute);
        }

        #region Binding Source Properties

        protected Color _SelectedColor = Colors.White;

        public Color SelectedColor
        {
            get { return _SelectedColor; }
            set
            {
                if (_SelectedColor != value)
                {
                    _SelectedColor = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedColor));
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
            var window = parameter as AdvancedColorPicker;
            window.DialogResult = true;
            window.Close();
        }
        #endregion

    }
}
