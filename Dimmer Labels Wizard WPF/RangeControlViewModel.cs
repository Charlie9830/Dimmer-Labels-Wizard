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
                    _UniverseNumber = CoerceUniverseNumber(value);

                    UpdateRange(value, FirstDimmerNumber, LastDimmerNumber);

                    // Notify.
                    OnPropertyChanged(nameof(UniverseNumber));
                    OnPropertyChanged(nameof(IsValidDimmerRange));
                    OnPropertyChanged(nameof(IsValidDistroRange));
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
                    _FirstDimmerNumber = CoerceFirstDimmer(value);

                    UpdateRange(UniverseNumber, value, LastDimmerNumber);

                    // Notify.
                    OnPropertyChanged(nameof(FirstDimmerNumber));
                    OnPropertyChanged(nameof(IsValidDimmerRange));
                    OnPropertyChanged(nameof(IsValidDistroRange));
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
                    _LastDimmerNumber = CoerceLastDimmer(value);

                    UpdateRange(UniverseNumber, FirstDimmerNumber, value);

                    // Notify.
                    OnPropertyChanged(nameof(LastDimmerNumber));
                    OnPropertyChanged(nameof(IsValidDimmerRange));
                    OnPropertyChanged(nameof(IsValidDistroRange));
                }
            }
        }

        public bool IsValidDimmerRange
        {
            get
            {
                return UniverseNumber != 0 &&
                    FirstDimmerNumber != 0 &&
                    LastDimmerNumber != 0;
            }
        }

        public bool IsValidDistroRange
        {
            get
            {
                return FirstDimmerNumber != 0 &&
                    LastDimmerNumber != 0;
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

        protected int CoerceFirstDimmer(int newValue)
        { 
            if (newValue < 0)
            {
                newValue = 0;
            }

            if (newValue > 512)
            {
                newValue = 512;
            }

            // Cross Coerce with LastDimmerNumber.
            if (LastDimmerNumber == 0)
            {
                // Last Dimmer Number has not been Set yet.
                return newValue;
            }

            if (newValue > LastDimmerNumber)
            {
                return LastDimmerNumber;
            }

            else
            {
                return newValue;
            }
        }

        protected int CoerceLastDimmer(int newValue)
        {
            
            if (newValue < 0)
            {
                newValue = 0;
            }

            if (newValue > 512)
            {
                newValue = 512;
            }

            // Cross Coerce with FirstDimmerNumber.
            if (FirstDimmerNumber == 0)
            {
                // First Dimmer Number has not been set yet.
                return newValue;
            }

            if (newValue < FirstDimmerNumber)
            {
                return FirstDimmerNumber;
            }

            else
            {
                return newValue;
            }
        }

        protected int CoerceUniverseNumber(int newValue)
        {
            if (newValue < 0)
            {
                return 0;
            }

            else
            {
                return newValue;
            }
        }


        #endregion
    }
}
