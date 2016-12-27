using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dimmer_Labels_Wizard_WPF
{
    public class StripSpacerControlViewModel : ViewModelBase
    {
        public StripSpacerControlViewModel()
        {
            // Commands.
            _MoveSpacerLeftCommand = new RelayCommand(MoveSpacerLeftCommandExecute, MoveSpacerLeftCommandCanExecute);
            _MoveSpacerRightCommand = new RelayCommand(MoveSpacerRightCommandExecute);
        }


        #region Binding Sources.
        protected int _SelectedIndex = 1;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    // Coerce Value.
                    if (value < 1)
                    {
                        value = 1;
                    }

                    _SelectedIndex = value;

                    // Notify.
                    _MoveSpacerLeftCommand.CheckCanExecute();
                    OnPropertyChanged(nameof(SelectedIndex));
                }
            }
        }


        protected double _SelectedWidth = 10.0d;

        public double SelectedWidth
        {
            get { return _SelectedWidth; }
            set
            {
                if (_SelectedWidth != value)
                {
                    _SelectedWidth = value;

                    // Notify.
                    OnPropertyChanged(nameof(SelectedWidth));
                }
            }
        }

        #endregion

        #region Commands.
        protected RelayCommand _MoveSpacerRightCommand;
        public ICommand MoveSpacerRightCommand
        {
            get
            {
                return _MoveSpacerRightCommand;
            }
        }

        protected void MoveSpacerRightCommandExecute(object parameter)
        {
            SelectedIndex++;
        }


        protected RelayCommand _MoveSpacerLeftCommand;
        public ICommand MoveSpacerLeftCommand
        {
            get
            {
                return _MoveSpacerLeftCommand;
            }
        }

        protected void MoveSpacerLeftCommandExecute(object parameter)
        {
            if (SelectedIndex > 1)
            {
                SelectedIndex--;
            }
        }

        protected bool MoveSpacerLeftCommandCanExecute(object parameter)
        {
            return !(SelectedIndex == 1);
        }
        #endregion
    }
}
