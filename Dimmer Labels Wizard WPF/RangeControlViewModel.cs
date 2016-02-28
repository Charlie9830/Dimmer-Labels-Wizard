using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class RangeControlViewModel : ViewModelBase
    {

        #region Binding Sources.

        protected int _UniverseNumber;

        public int UniverseNumeber
        {
            get { return _UniverseNumber; }
            set
            {
                if (_UniverseNumber != value)
                {
                    _UniverseNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(UniverseNumeber));
                }
            }
        }


        protected int _FirstDimmerNumber;

        public int FirstDimmerNumber
        {
            get { return _FirstDimmerNumber; }
            set
            {
                if (_FirstDimmerNumber != value)
                {
                    _FirstDimmerNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(FirstDimmerNumber));
                }
            }
        }


        protected int _LastDimmerNumber;

        public int LastDimmerNumber
        {
            get { return _LastDimmerNumber; }
            set
            {
                if (_LastDimmerNumber != value)
                {
                    _LastDimmerNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastDimmerNumber));
                }
            }
        }

        #endregion
    }
}
