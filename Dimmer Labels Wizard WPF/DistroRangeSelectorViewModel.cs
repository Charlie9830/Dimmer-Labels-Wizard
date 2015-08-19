using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DistroRangeSelectorViewModel : ViewModelBase
    {
        protected DistroRange _DistroRange = new DistroRange(1, 12);

        #region Getters/Setters
        public int StartNumber
        {
            get
            {
                return _DistroRange.FirstChannel;
            }
            set
            {
                _DistroRange.FirstChannel = value;
                OnPropertyChanged("StartNumber");
            }
        }

        public int EndNumber
        {
            get
            {
                return _DistroRange.LastChannel;
            }
            set
            {
                _DistroRange.LastChannel = value;
                OnPropertyChanged("EndNumber");
            }
        }

        public DistroRange DistroRange
        {
            get
            {
                return _DistroRange;
            }
        }

        #endregion
    }
}
