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

        protected UnitRangeBase _Range;

        public UnitRangeBase Range
        {
            get { return _Range; }
            set
            {
                if (_Range != value)
                {
                    _Range = value;

                    // Push Values to other Properties.
                    if (value.GetType() == typeof(DimmerRange))
                    {
                        // Dimmer.
                        var range = value as DimmerRange;
                        
                        UniverseNumber = range.Universe;
                        FirstDimmerNumber = range.FirstDimmerNumber;
                        LastDimmerNumber = range.LastDimmerNumber;
                    }

                    else
                    {
                        // Distro.
                        var range = value as DistroRange;

                        FirstDimmerNumber = range.FirstDimmerNumber;
                        LastDimmerNumber = range.LastDimmerNumber;
                    }

                    // Notify.
                    OnPropertyChanged(nameof(Range));
                }
            }
        }

        protected int _UniverseNumber;

        public int UniverseNumber
        {
            get { return _UniverseNumber; }
            set
            {
                if (_UniverseNumber != value)
                {
                    _UniverseNumber = value;

                    UpdateRange(value, FirstDimmerNumber, LastDimmerNumber);

                    // Notify.
                    OnPropertyChanged(nameof(UniverseNumber));
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

                    UpdateRange(UniverseNumber, value, LastDimmerNumber);

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

                    UpdateRange(UniverseNumber, FirstDimmerNumber, value);

                    // Notify.
                    OnPropertyChanged(nameof(LastDimmerNumber));
                }
            }
        }

        #endregion

        #region Methods.
        protected void UpdateRange(int universe, int firstDimmer, int lastDimmer)
        {
            if (_Range.GetType() == typeof(DimmerRange))
            {
                // Dimmer.
                var range = _Range as DimmerRange;
                range.Universe = universe;
                range.FirstDimmerNumber = firstDimmer;
                range.LastDimmerNumber = lastDimmer;
            }

            else
            {
                // Distro.
                var range = _Range as DistroRange;
                range.FirstDimmerNumber = firstDimmer;
                range.LastDimmerNumber = lastDimmer;
            }
        }


        #endregion
    }
}
