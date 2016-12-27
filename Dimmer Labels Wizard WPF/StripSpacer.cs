using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.Serialization;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class StripSpacer : ViewModelBase
    {
        #region Constructors.
        public StripSpacer()
        {
            // Commands
            _MoveSpacerLeftCommand = new RelayCommand(MoveSpacerLeftCommandExecute, MoveSpacerLeftCommandCanExecute);
            _MoveSpacerRightCommand = new RelayCommand(MoveSpacerRightCommandExecute);
        }

        [OnDeserializing]
        protected void OnDeserialized(StreamingContext sc)
        {
            _MoveSpacerLeftCommand = new RelayCommand(MoveSpacerLeftCommandExecute, MoveSpacerLeftCommandCanExecute);
            _MoveSpacerRightCommand = new RelayCommand(MoveSpacerRightCommandExecute);
        }
        #endregion

        // Database Key and Navigation Properties.
        [Key]
        public int ID { get; set; }
        public LabelStripTemplate LabelStripTemplate { get; set; }

        // Base and ViewModel Properties.
        protected int _Index;
        [DataMember]
        public int Index
        {
            get { return _Index; }
            set
            {
                if (_Index != value)
                {
                    _Index = value;

                    // Executes.
                    _MoveSpacerLeftCommand.CheckCanExecute();

                    // Notify.
                    OnPropertyChanged(nameof(Index));
                }
            }
        }

        protected double _Width;
        [DataMember]
        public double Width
        {
            get { return _Width; }
            set
            {
                if (_Width != value)
                {
                    _Width = value;

                    // Notify.
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        // Commands
        [NotMapped]
        protected RelayCommand _MoveSpacerRightCommand;
        [NotMapped]
        public ICommand MoveSpacerRightCommand
        {
            get
            {
                return _MoveSpacerRightCommand;
            }
        }

        protected void MoveSpacerRightCommandExecute(object parameter)
        {
            Index++;
        }

        [NotMapped]
        protected RelayCommand _MoveSpacerLeftCommand;
        [NotMapped]
        public ICommand MoveSpacerLeftCommand
        {
            get
            {
                return _MoveSpacerLeftCommand;
            }
        }

        protected void MoveSpacerLeftCommandExecute(object parameter)
        {
            if (Index > 1)
            {
                Index--;
            }
        }

        protected bool MoveSpacerLeftCommandCanExecute(object parameter)
        {
            return !(Index == 1);
        }
    }
}
