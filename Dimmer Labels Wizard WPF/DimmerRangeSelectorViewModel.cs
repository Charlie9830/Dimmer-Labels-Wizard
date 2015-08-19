using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DimmerRangeSelectorViewModel : ViewModelBase
    {
        protected int _UniverseNumber = 1;
        protected int _StartNumber = 1;
        protected int _EndNumber = 12;

        protected DimmerRange _DimmerRange = new DimmerRange(1,1,12);

        #region Getters/Setters
        public int UniverseNumber
        {
            get
            {
                return _DimmerRange.Universe;
            }
            set
            {
                _DimmerRange.Universe = value;
                OnPropertyChanged("UniverseNumber");
            }
        }

        public int StartNumber
        {
            get
            {
                return _DimmerRange.FirstChannel;
            }
            set
            {
                _DimmerRange.FirstChannel = value;
                OnPropertyChanged("StartNumber");
            }
        }

        public int EndNumber
        {
            get
            {
                return _DimmerRange.LastChannel;
            }
            set
            {
                _DimmerRange.LastChannel = value;
                OnPropertyChanged("EndNumber");
            }
        }

        public DimmerRange DimmerRange
        {
            get
            {
                return _DimmerRange;
            }
        }

        #endregion
    }
}
