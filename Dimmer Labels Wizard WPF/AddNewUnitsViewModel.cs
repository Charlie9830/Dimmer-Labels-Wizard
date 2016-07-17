using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public class AddNewUnitsViewModel : ViewModelBase
    {
        public AddNewUnitsViewModel()
        {
            // Commands.
            _AddCommand = new RelayCommand(AddCommandExecute, AddCommandCanExecute);
            _CancelCommand = new RelayCommand(CancelCommandExecute);
        }


        #region Binding Sources.
        protected List<RackType> _RackTypes = new List<RackType> { RackType.Dimmer, RackType.Distro };

        public List<RackType> RackTypes
        {
            get { return _RackTypes; }
        }


        protected RackType _NewRackType;

        public RackType NewRackType
        {
            get { return _NewRackType; }
            set
            {
                if (_NewRackType != value)
                {
                    _NewRackType = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewRackType));
                    _AddCommand.CheckCanExecute();
                }
            }
        }


        protected int _NewUniverseNumber;

        public int NewUniverseNumber
        {
            get { return _NewUniverseNumber; }
            set
            {
                if (_NewUniverseNumber != value)
                {
                    _NewUniverseNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewUniverseNumber));
                    _AddCommand.CheckCanExecute();
                }
            }
        }



        protected int _NewFirstDimmerNumber;

        public int NewFirstDimmerNumber
        {
            get { return _NewFirstDimmerNumber; }
            set
            {
                if (_NewFirstDimmerNumber != value)
                {
                    _NewFirstDimmerNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewFirstDimmerNumber));
                    _AddCommand.CheckCanExecute();
                }
            }
        }


        protected int _NewLastDimmerNumber;

        public int NewLastDimmerNumber
        {
            get { return _NewLastDimmerNumber; }
            set
            {
                if (_NewLastDimmerNumber != value)
                {
                    _NewLastDimmerNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewLastDimmerNumber));
                }
            }
        }


        protected bool _DimmerNumberThrough;

        public bool DimmerNumberThrough
        {
            get { return _DimmerNumberThrough; }
            set
            {
                if (_DimmerNumberThrough != value)
                {
                    _DimmerNumberThrough = value;

                    // Notify.
                    OnPropertyChanged(nameof(DimmerNumberThrough));
                }
            }
        }


        protected string _NewChannelNumber;

        public string NewChannelNumber
        {
            get { return _NewChannelNumber; }
            set
            {
                if (_NewChannelNumber != value)
                {
                    _NewChannelNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewChannelNumber));
                }
            }
        }


        protected bool _EnumerateChannelNumber;

        public bool EnumerateChannelNumber
        {
            get { return _EnumerateChannelNumber; }
            set
            {
                if (_EnumerateChannelNumber != value)
                {
                    _EnumerateChannelNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(EnumerateChannelNumber));
                }
            }
        }

        protected string _NewInstrumentName;

        public string NewInstrumentName
        {
            get { return _NewInstrumentName; }
            set
            {
                if (_NewInstrumentName != value)
                {
                    _NewInstrumentName = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewInstrumentName));
                }
            }
        }


        protected string _NewPosition;

        public string NewPosition
        {
            get { return _NewPosition; }
            set
            {
                if (_NewPosition != value)
                {
                    _NewPosition = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewPosition));
                }
            }
        }


        protected string _NewMulticoreName;

        public string NewMulticoreName
        {
            get { return _NewMulticoreName; }
            set
            {
                if (_NewMulticoreName != value)
                {
                    _NewMulticoreName = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewMulticoreName));
                }
            }
        }


        protected string _NewUserField1;

        public string NewUserField1
        {
            get { return _NewUserField1; }
            set
            {
                if (_NewUserField1 != value)
                {
                    _NewUserField1 = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewUserField1));
                }
            }
        }

        protected string _NewUserField2;

        public string NewUserField2
        {
            get { return _NewUserField2; }
            set
            {
                if (_NewUserField2 != value)
                {
                    _NewUserField2 = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewUserField2));
                }
            }
        }

        protected string _NewUserField3;

        public string NewUserField3
        {
            get { return _NewUserField3; }
            set
            {
                if (_NewUserField3 != value)
                {
                    _NewUserField3 = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewUserField3));
                }
            }
        }

        protected string _NewUserField4;

        public string NewUserField4
        {
            get { return _NewUserField4; }
            set
            {
                if (_NewUserField4 != value)
                {
                    _NewUserField4 = value;

                    // Notify.
                    OnPropertyChanged(nameof(NewUserField4));
                }
            }
        }

        #endregion

        #region Commands.

        protected RelayCommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                return _AddCommand;
            }
        }

        protected void AddCommandExecute(object parameter)
        {
            var window = parameter as Window;

            GenerateUnits();

            window.DialogResult = true;
            window.Close();
        }

        protected bool AddCommandCanExecute(object parameter)
        {
            if (NewRackType == RackType.Dimmer)
            {
                if (NewUniverseNumber == 0 || NewFirstDimmerNumber == 0)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }

            if (NewRackType == RackType.Distro)
            {
                if (NewFirstDimmerNumber == 0)
                {
                    return false;
                }
                
                else
                {
                    return true;
                }
            }

            return false;
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
            var window = parameter as Window;
            window.DialogResult = false;
            window.Close();
        }

        #endregion

        #region Properties.
        public List<DimmerDistroUnit> Units { get; set; } = new List<DimmerDistroUnit>();
        #endregion

        #region Methods
        protected void GenerateUnits()
        {
            if (DimmerNumberThrough == true)
            {
                // Multiple Units.
                int enumerationCounter = NewLastDimmerNumber - NewFirstDimmerNumber;

                // Buffer.
                string channelNumberBuffer = NewChannelNumber;

                if (enumerationCounter >= 0)
                {
                    // Enumerating Up.
                    for (int counter = 0; counter <= enumerationCounter; counter++)
                    {
                        // Iterate Values.
                        int currentDimmerNumber = NewFirstDimmerNumber + counter;

                        // Determine the Current Channel Number Text.
                        string currentChannelNumber;
                        if (counter == 0)
                        {
                            // Execution is inside the first Loop Iteration.
                            currentChannelNumber = channelNumberBuffer;
                        }

                        else
                        {
                            if (EnumerateChannelNumber == true)
                            {
                                currentChannelNumber = IterateString(channelNumberBuffer, true);
                            }

                            else
                            {
                                currentChannelNumber = channelNumberBuffer;
                            }
                        }

                        // Generate Unit.
                        var newUnit = new DimmerDistroUnit()
                        {
                            RackUnitType = NewRackType,
                            UniverseNumber = NewUniverseNumber,
                            DimmerNumber = currentDimmerNumber,
                            ChannelNumber = currentChannelNumber,
                            InstrumentName = NewInstrumentName,
                            Position = NewPosition,
                            MulticoreName = NewMulticoreName,
                            UserField1 = NewUserField1,
                            UserField2 = NewUserField2,
                            UserField3 = NewUserField3,
                            UserField4 = NewUserField4
                        };

                        Units.Add(newUnit);

                        // Save to Buffer.
                        channelNumberBuffer = currentChannelNumber;
                    }
                }

                else
                {
                    // Enumerating Down.
                    for (int counter = Math.Abs(enumerationCounter); counter >= 0; counter--)
                    {
                        // Iterate Values.
                        int currentDimmerNumber = NewFirstDimmerNumber - counter;

                        // Determine the Current Channel Number Text.
                        string currentChannelNumber;
                        if (counter == Math.Abs(enumerationCounter))
                        {
                            // Execution is Inside the first Loop Iteration.
                            currentChannelNumber = channelNumberBuffer;
                        }

                        else
                        {
                            if (EnumerateChannelNumber == true)
                            {
                                currentChannelNumber = IterateString(channelNumberBuffer, false);
                            }

                            else
                            {
                                currentChannelNumber = channelNumberBuffer;
                            }
                        }

                        // Generate Unit.
                        var newUnit = new DimmerDistroUnit()
                        {
                            RackUnitType = NewRackType,
                            UniverseNumber = NewUniverseNumber,
                            DimmerNumber = currentDimmerNumber,
                            ChannelNumber = currentChannelNumber,
                            InstrumentName = NewInstrumentName,
                            Position = NewPosition,
                            MulticoreName = NewMulticoreName,
                            UserField1 = NewUserField1,
                            UserField2 = NewUserField2,
                            UserField3 = NewUserField3,
                            UserField4 = NewUserField4
                        };

                        Units.Add(newUnit);

                        // Save to Buffer.
                        channelNumberBuffer = currentChannelNumber;
                    }
                }
            }

            else
            {
                // Single Unit.
                var newUnit = new DimmerDistroUnit()
                {
                    RackUnitType = NewRackType,
                    UniverseNumber = NewUniverseNumber,
                    DimmerNumber = NewFirstDimmerNumber,
                    ChannelNumber = NewChannelNumber,
                    InstrumentName = NewInstrumentName,
                    Position = NewPosition,
                    MulticoreName = NewMulticoreName,
                    UserField1 = NewUserField1,
                    UserField2 = NewUserField2,
                    UserField3 = NewUserField3,
                    UserField4 = NewUserField4
                };

                Units.Add(newUnit);
            }

        }

        protected string IterateString(string currentValue, bool iterateUpwards)
        {
            // Extract Digits from input String. Iterate them, then re insert into String.

            if (ContainsNumbers(currentValue) == false)
            {
                return currentValue;
            }

            string extractedNumber = ExtractNumber(currentValue);
            int extractedCurrentNumber = int.Parse(extractedNumber);

            if (iterateUpwards)
            {
                // Iterate Up.
                return currentValue.Replace(extractedNumber, ((int.Parse(extractedNumber)) + 1).ToString());
            }

            else
            {
                // Iterate Down.
                return currentValue.Replace(extractedNumber, ((int.Parse(extractedNumber)) - 1).ToString());
            }
        }

        protected bool ContainsNumbers(string value)
        {
            return value.Any(char.IsDigit);
        }

        protected string ExtractNumber(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }
        #endregion.
    }
}
