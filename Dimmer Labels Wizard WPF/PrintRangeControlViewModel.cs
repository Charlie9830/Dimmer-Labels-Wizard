using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public class PrintRangeControlViewModel : ViewModelBase
    {
        protected List<LabelStrip> _SelectedStripsToPrint = new List<LabelStrip>();
        protected List<LabelStrip> _LabelStrips = new List<LabelStrip>();
        protected RackType _RackType;
        protected RadioButtonSelection _SelectedRadioButton = RadioButtonSelection.All;
        protected int _RackLowerRange = 1;
        protected int _RackUpperRange = 2;
        protected string _SelectionText = string.Empty;
        protected string _RadioButtonGroup = string.Empty;

        protected char _delimiter = ',';
        protected char _hypen = '-';
        protected char _space = ' ';

        #region Getters/Setters
        public List<LabelStrip> LabelStrips
        {
            get
            {
                return _LabelStrips;
            }
            set
            {
                _LabelStrips = value;
            }
        }

        public RackType RackType
        {
            get
            {
                return _RackType;
            }
            set
            {
                _RackType = value;
                OnPropertyChanged("RackType");
            }
        }

        public RadioButtonSelection SelectedRadioButton
        {
            get
            {
                return _SelectedRadioButton;
            }
            set
            {
                _SelectedRadioButton = value;
                OnPropertyChanged("SelectedRadioButton");
            }
        }

        public int RackLowerRange
        {
            get
            {
                return _RackLowerRange;
            }
            set
            {
                
                _RackLowerRange = value;
                OnPropertyChanged("RackLowerRange");

                if (ValidateRackRange(value, _RackUpperRange) == false)
                {
                    throw new ArgumentException("Left hand number must be less than or equal to Right hand number.");
                }
                
            }
        }

        public int RackUpperRange
        {
            get
            {
                return _RackUpperRange;
            }
            set
            {
                
                _RackUpperRange = value;
                OnPropertyChanged("RackUpperRange");

                if (ValidateRackRange(_RackLowerRange,value) == false)
                {
                    throw new ArgumentException("Left hand number must be less than or equal to Right hand number.");
                }
            }
        }

        public string SelectionText
        {
            get
            {
                return _SelectionText;
            }
            set
            {
                if (ValidateSelectionText(value) == false)
                {
                    throw new ArgumentException("Invalid Characters found in Selection.");
                }
                _SelectionText = value;
            }
        }

        public string RadioButtonGroup
        {
            get
            {
                return _RadioButtonGroup;
            }
            set
            {
                _RadioButtonGroup = value;
                OnPropertyChanged("RadioButtonGroup");
            }
        }
        #endregion

        #region Validation Methods
        protected bool ValidateRackRange(int lowerRange, int upperRange)
        {
            if (lowerRange > upperRange)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateSelectionText(string text)
        {
            // Remove Hyphens, Delimiters and Spaces from Text.
            string testString = text;
            testString = testString.Replace(_delimiter.ToString(), string.Empty);
            testString = testString.Replace(_hypen.ToString(), string.Empty);
            testString = testString.Replace(_space.ToString(), string.Empty);

            // Attempt to Parse into Integer.
            int tryParseOutput = 0;
            bool tryParseResult = int.TryParse(testString, out tryParseOutput);

            // Return Result of Parse attempt.
            return tryParseResult;
        }
        #endregion

        #region Setup Methods
        public void SetupControl(List<LabelStrip> labelStrips, RackType rackUnitType, string radioButtonGroup)
        {
            _RackType = rackUnitType;

            foreach (var element in labelStrips)
            {
                if (element.RackUnitType == rackUnitType)
                {
                    _LabelStrips.Add(element);
                }
            }
        }

        #endregion

        #region Update and Processing Methods
        public List<LabelStrip> GetPrintRange(out bool validationResult)
        {
            if (_SelectedRadioButton == RadioButtonSelection.All
                || _SelectedRadioButton == RadioButtonSelection.None)
            {
                validationResult = true;
            }

            else if (_SelectedRadioButton == RadioButtonSelection.Rack)
            {
                validationResult = ValidateRackRange(_RackLowerRange, _RackUpperRange);
            }

            else
            {
                validationResult = ValidateSelectionText(SelectionText);
            }
                

            if (validationResult == true)
            {
                GeneratePrintRange();
            }

            return _SelectedStripsToPrint;
        }

        void GeneratePrintRange()
        {
            if (_LabelStrips.Count == 0)
            {
                return;
            }

            _SelectedStripsToPrint.Clear();

            switch (_SelectedRadioButton)   
            {
                case RadioButtonSelection.None:
                    break;
                case RadioButtonSelection.All:
                    _SelectedStripsToPrint.AddRange(_LabelStrips);
                    break;
                case RadioButtonSelection.Rack:
                    _SelectedStripsToPrint = GetRackRange();
                    break;
                case RadioButtonSelection.Selection:
                    _SelectedStripsToPrint = GetSelectionRange();
                    break;
                default:
                    break;
            }
        }

        List<LabelStrip> GetRackRange()
        {
            List<LabelStrip> returnList = new List<LabelStrip>();

            int maxRackNumber = _LabelStrips.Last().RackNumber;
            int minRackNumber = _LabelStrips.Last().RackNumber;

            int startIndex = 0;
            int endIndex = _LabelStrips.Count - 1;

            if (_LabelStrips.Exists(item => item.RackNumber == _RackLowerRange))
            {
                startIndex =
                _LabelStrips.IndexOf(_LabelStrips.Find(item => item.RackNumber == _RackLowerRange));
            }

            if (_LabelStrips.Exists(item => item.RackNumber == _RackUpperRange))
            {
                endIndex =
                _LabelStrips.IndexOf(_LabelStrips.Find(item => item.RackNumber == _RackUpperRange));
            }
            

            for (int index = startIndex; index <= endIndex; index++)
            {
                returnList.Add(_LabelStrips[index]);
            }

            return returnList;
        }

        List<LabelStrip> GetSelectionRange()
        {
            List<LabelStrip> returnList = new List<LabelStrip>();     

            string selectionText = SelectionText;
            string[] selections = selectionText.Split(_delimiter);

            foreach (var element in selections)
            {
                if (element != string.Empty)
                {
                    // Range Selection
                    if (element.Contains(_hypen))
                    {
                        string[] rackNumbers = element.Split(_hypen);
                        int lowerRange = Convert.ToInt32(rackNumbers.First().Trim());
                        int upperRange = Convert.ToInt32(rackNumbers.Last().Trim());

                        returnList.AddRange(_LabelStrips.FindAll(item => item.RackNumber >= lowerRange &&
                            item.RackNumber <= upperRange));
                    }

                    // Single Selection
                    else
                    {
                        element.Trim();
                        int rackNumber = Convert.ToInt32(element);

                        LabelStrip selection = _LabelStrips.Find(item => item.RackNumber == rackNumber);
                        if (returnList.Contains(selection) != true)
                        {
                            returnList.Add(selection);
                        }
                    }
                }
            }

            return returnList;
        }
        #endregion
    }

    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.Equals(value))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (((bool)value) == true)
            {
                return parameter;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }


}
