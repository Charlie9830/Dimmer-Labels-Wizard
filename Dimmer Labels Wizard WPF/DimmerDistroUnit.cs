using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    
    public class DimmerDistroUnit : ViewModelBase, IComparable<DimmerDistroUnit>, INotifyModification
    {
        public DimmerDistroUnit()
        {

        }

        public DimmerDistroUnit(DimmerDistroUnitStorage storage)
        {
            ChannelNumber = storage.ChannelNumber;
            InstrumentName = storage.InstrumentName;
            MulticoreName = storage.MulticoreName;
            Position = storage.Position;
            UserField1 = storage.UserField1;
            UserField2 = storage.UserField2;
            UserField3 = storage.UserField3;
            UserField4 = storage.UserField4;

            DimmerNumberText = storage.DimmerNumberText;
            DMXAddressText = storage.DMXAddressText;

            ImportIndex = storage.ImportIndex;

            RackUnitType = storage.RackUnitType;
            UniverseNumber = storage.UniverseNumber;
            AbsoluteDMXAddress = storage.AbsoluteDMXAddress;
            DimmerNumber = storage.DimmerNumber;
    }

        // Imported Data
        protected string _ChannelNumber = string.Empty;
        protected string _InstrumentName = string.Empty;
        protected string _MulticoreName = string.Empty;
        protected string _Position = string.Empty;
        protected string _UserField1 = string.Empty;
        protected string _UserField2 = string.Empty;
        protected string _UserField3 = string.Empty;
        protected string _UserField4 = string.Empty;
        protected string _Custom = string.Empty;

        #region Imported Data Public Accsesors.
        public string ChannelNumber
        {
            get
            {
                return _ChannelNumber;
            }
            set
            {
                if (value != _ChannelNumber)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(ChannelNumber), _ChannelNumber);

                    // Modify Value.
                    _ChannelNumber = value;
                    OnPropertyChanged("ChannelNumber");
                }
            }
        }

        public string InstrumentName
        {
            get
            {
                return _InstrumentName;
            }
            set
            {
                if (value != _InstrumentName)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(InstrumentName), _InstrumentName);

                    // Modify Value.
                    _InstrumentName = value;
                    OnPropertyChanged("InstrumentName");
                }
            }
        }

        public string MulticoreName
        {
            get
            {
                return _MulticoreName;
            }
            set
            {
                if (value != _MulticoreName)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(MulticoreName), _MulticoreName);

                    // Modify Value.
                    _MulticoreName = value;
                    OnPropertyChanged("MulticoreName");
                }
            }
        }

        public string Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (value != _Position)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(Position), _Position);

                    // Modify Value.
                    _Position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        public string UserField1
        {
            get
            {
                return _UserField1;
            }
            set
            {
                if (value != _UserField1)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(UserField1), _UserField1);

                    // Modify Value.
                    _UserField1 = value;
                    OnPropertyChanged("UserField1");
                }
            }
        }

        public string UserField2
        {
            get
            {
                return _UserField2;
            }
            set
            {
                if (value != _UserField2)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(UserField2), _UserField2);

                    // Modify Value.
                    _UserField2 = value;
                    OnPropertyChanged("UserField2");
                }
            }
        }

        public string UserField3
        {
            get
            {
                return _UserField3;
            }
            set
            {
                if (value != _UserField3)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(UserField3), _UserField3);

                    // Modify Value.
                    _UserField3 = value;
                    OnPropertyChanged("UserField3");
                }
            }
        }

        public string UserField4
        {
            get
            {
                return _UserField4;
            }
            set
            {
                if (value != _UserField4)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(UserField4), _UserField4);

                    // Modify Value.
                    _UserField4 = value;
                    OnPropertyChanged("UserField4");
                }
            }
        }

        public string Custom
        {
            get
            {
                return _Custom;
            }
            set
            {
                if (value != _Custom)
                {
                    // Notify Modification.
                    OnNotifyModification(this, nameof(Custom), _Custom);

                    // Modify Value.
                    _Custom = value;
                    OnPropertyChanged("Custom");
                }
            }
        }

        public Color LabelColor
        {
            get
            {
                return GetLabelColor();
            }

            set
            {
                SetLabelColor(value);
            }
        }
        

        #endregion

        // Imported Temporary Data
        protected string _DimmerNumberText = string.Empty;

        public string DimmerNumberText
        { get { return _DimmerNumberText; } set { _DimmerNumberText = value; } }

        public string DMXAddressText { get; set; }

        // Application running data
        public int ImportIndex;

        // Inferred Data
        public RackType RackUnitType { get; set; }
        public int UniverseNumber { get; set; }
        public int AbsoluteDMXAddress { get; set; }
        public int DimmerNumber { get; set; }


        public override string ToString()
        {
            return "Dimmer Number: " + DimmerNumber.ToString();
        }

        private Color GetLabelColor()
        {
            Color color;

            // Select Search Space.
            var dictionary = RackUnitType == RackType.Dimmer ? Globals.DimmerLabelColors : Globals.DistroLabelColors;

            if (dictionary.TryGetValue(DimmerNumber, out color))
            {
                return color;
            }

            else
            {
                return Colors.White;
            }

        }

        private void SetLabelColor(Color desiredColor)
        {
            Color color;

            // Select Search Space.
            var dictionary = RackUnitType == RackType.Dimmer ? Globals.DimmerLabelColors : Globals.DistroLabelColors;

            if (dictionary.TryGetValue(DimmerNumber, out color))
            {
                // Dictionary Entry exists, Modify Value if required.
                if (color != desiredColor)
                {
                    dictionary[DimmerNumber] = desiredColor;
                }
            }

            else
            {
                // Make a new Entry.
                dictionary.Add(DimmerNumber, desiredColor);
            }
        }

        // Provides easier accsess when using Switch Statements to GetData Based on LabelField.
        public string GetData(LabelField labelField)
        {
            switch (labelField)
            {
                case LabelField.NoAssignment:
                    return "No Assignment";
                case LabelField.ChannelNumber:
                    return ChannelNumber;
                case LabelField.InstrumentName:
                    return InstrumentName;
                case LabelField.MulticoreName:
                    return MulticoreName;
                case LabelField.Position:
                    return Position;
                case LabelField.UserField1:
                    return UserField1;
                case LabelField.UserField2:
                    return UserField2;
                case LabelField.UserField3:
                    return UserField3;
                case LabelField.UserField4:
                    return UserField4;
                case LabelField.Custom:
                    return Custom;
                default:
                    return "DimmerDistroUnit.GetData() Error";
            }
        }

        public string GetData(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ChannelNumber):
                    return ChannelNumber;

                case nameof(InstrumentName):
                    return InstrumentName;

                case nameof(MulticoreName):
                    return MulticoreName;

                case nameof(Position):
                    return Position;

                case nameof(UserField1):
                    return UserField1;

                case nameof(UserField2):
                    return UserField2;

                case nameof(UserField3):
                    return UserField3;

                case nameof(UserField4):
                    return UserField4;

                case nameof(Custom):
                    return Custom;

                default:
                    return "DimmerDistroUnit.GetData(propertyName) Error.";
            }
        }

        public void SetData(string data, LabelField labelField)
        {
            switch (labelField)
            {
                case LabelField.NoAssignment:
                    break;
                case LabelField.ChannelNumber:
                    ChannelNumber = data;
                    break;
                case LabelField.InstrumentName:
                    InstrumentName = data;
                    break;
                case LabelField.MulticoreName:
                    MulticoreName = data;
                    break;
                case LabelField.Position:
                    Position = data;
                    break;
                case LabelField.UserField1:
                    UserField1 = data;
                    break;
                case LabelField.UserField2:
                    UserField2 = data;
                    break;
                case LabelField.UserField3:
                    UserField3 = data;
                    break;
                case LabelField.UserField4:
                    UserField4 = data;
                    break;
                case LabelField.Custom:
                    Custom = data;
                    break;
                default:
                    break;
            }
        }

        public void SetData(string data, string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ChannelNumber):
                    ChannelNumber = data;
                    break;

                case nameof(InstrumentName):
                    InstrumentName = data;
                    break;

                case nameof(MulticoreName):
                    MulticoreName = data;
                    break;

                case nameof(Position):
                    Position = data;
                    break;

                case nameof(UserField1):
                    UserField1 = data;
                    break;

                case nameof(UserField2):
                    UserField2 = data;
                    break;

                case nameof(UserField3):
                    UserField3 = data;
                    break;

                case nameof(UserField4):
                     UserField4 = data;
                    break;

                case nameof(Custom):
                    Custom = data;
                    break;
            }
        }

        // Control Method for Parse Methods. Determines Rack Type and calls Parse Methods. Adds unit to Unparseable
        // List if string format does not Match the User set option.
        public void ParseUnitData(UnitImport unitImportParent, ImportConfiguration importConfiguration)
        {
            
            // Determine RackType.
            RackType rackType = DetermineUnitType(importConfiguration);

            switch (rackType)
            {
                case RackType.Dimmer:
                    int universeNumber;
                    DimmerNumber = ParseDimmerNumber(importConfiguration,out universeNumber);
                    UniverseNumber = universeNumber;
                    RackUnitType = RackType.Dimmer;
                    break;
                case RackType.Distro:
                    ParseDistroNumber(importConfiguration);
                    RackUnitType = RackType.Distro;
                    break;
                case RackType.OutsideLabelRange:
                    RackUnitType = RackType.OutsideLabelRange;
                    break;
                case RackType.Unparseable:
                    unitImportParent.UnResolveableUnits.Add(this);
                    RackUnitType = RackType.Unparseable;
                    break;
                case RackType.ClashingRange:
                    unitImportParent.ConflictingUnits.Add(this);
                    RackUnitType = RackType.ClashingRange;
                    break;
                default:
                    break;
            }
        }

        // Returns Parsed Dimmer Number Value.
        private int ParseDistroNumber(ImportConfiguration importConfiguration)
        {
            switch (importConfiguration.DistroImportFormat)
            {
                case ImportFormat.Format1:
                    return Convert.ToInt32(RemoveLetters(DimmerNumberText).Trim());
                    
                case ImportFormat.Format2:
                    return Convert.ToInt32(DimmerNumberText.Trim());
                    
                case ImportFormat.Format3:
                    // Split and Discard Number preceeding Slash.
                    return Convert.ToInt32(SplitBySlash(DimmerNumberText)[1].Trim());
                    
                case ImportFormat.Format4:
                    // Split and Discard Letter preceeding Slash.
                    return Convert.ToInt32(SplitBySlash(DimmerNumberText)[1].Trim());
                    
                default:
                    throw new ArgumentException("Import Format couldn't be found.");
            }
            
        }


        // Returns Parsed Dimmer Number value, Universe Number returned via out Parameter.
        private int ParseDimmerNumber(ImportConfiguration importConfiguration, out int universeNumber)
        {
            switch (importConfiguration.DimmerImportFormat)
            {
                case ImportFormat.Format1:
                    universeNumber = Convert.ToInt32(SplitBySlash(DimmerNumberText)[0].Trim());
                    return Convert.ToInt32(SplitBySlash(DimmerNumberText)[1].Trim());

                    
                case ImportFormat.Format2:
                    universeNumber = ExtractUniverseNumber(DMXAddressText, importConfiguration);
                    return Convert.ToInt32(DimmerNumberText.Trim());
                    
                case ImportFormat.Format3:
                    universeNumber = ConvertStreamLetterToNumber(RemoveNumbers(DimmerNumberText).ToCharArray()[0]);
                    return Convert.ToInt32(RemoveLetters(DimmerNumberText).Trim());
                    
                case ImportFormat.Format4:
                    universeNumber = Convert.ToInt32(ConvertStreamLetterToNumber(SplitBySlash(DimmerNumberText)[0].Trim().ToCharArray()[0]));
                    return Convert.ToInt32(RemoveSlash(RemoveLetters(DimmerNumberText).Trim()));
                    
                default:
                    throw new ArgumentException("Dimmer Import Format not found.");
            }
        }


        // Helper Method for Parse Methods. Returns a string with all Letter Characters Removed.
        private string RemoveLetters(string input)
        {
            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

            string outputText = input;

            bool breakLoop = false;
            int letterIndex = 0;

            while (breakLoop == false)
            {
                letterIndex = outputText.IndexOfAny(alphabet);

                if (letterIndex != -1)
                {
                    outputText = outputText.Remove(letterIndex, 1);
                }

                else
                {
                    breakLoop = true;
                }
            }

            return outputText;
        }

        // Helper Method for Parse Methods. Returns a string with all Number Characters Removed.
        private string RemoveNumbers(string input)
        {
            char[] numbers = "0123456789".ToCharArray();

            string outputText = input;

            bool breakLoop = false;
            int numberIndex = 0;

            while (breakLoop == false)
            {
                numberIndex = outputText.IndexOfAny(numbers);

                if (numberIndex != -1)
                {
                    outputText = outputText.Remove(numberIndex, 1);
                }

                else
                {
                    breakLoop = true;
                }
            }

            return outputText;
        }

        // Helper Method for Parse Methods. Removes Occurances of Slashes.
        private string RemoveSlash(string input)
        {
            char slash = '/';
            if (input.IndexOf(slash) == -1)
            {
                return input;
            }

            else
            {
                return input.Remove(input.IndexOf(slash), 1);
            }
        }
        // Helper Method for Parse Methods. Splits string by Slash and Returns results as String Array.

        private string[] SplitBySlash(string input)
        {
            char delimitingCharacter = '/';
            string[] outputArray = input.Split(delimitingCharacter);
            return outputArray;
        }

        // Helper Method for Parse Methods. Returns a Number coresponding to Char inputs Alphabetical position.
        private int ConvertStreamLetterToNumber(char letter)
        {
            char[] outputValue = letter.ToString().ToUpper().ToCharArray();
            return outputValue[0] - 64;
        }

        // Helper Method for ParseUnitData(). Determines Rack Type. Returns RackType.Unparseable If Data
        // cannot be sucseffully assinged a Type.
        private RackType DetermineUnitType(ImportConfiguration importConfiguration)
        {
            ImportFormat dimmerFormat = importConfiguration.DimmerImportFormat;
            ImportFormat distroFormat = importConfiguration.DistroImportFormat;

            bool verifiedDimmer = false;
            bool verifiedDistro = false;

            // Test as Dimmer.
            if (dimmerFormat != ImportFormat.NoAssignment &&
                VerifyStringFormat(DimmerNumberText, importConfiguration, RackType.Dimmer,dimmerFormat) == true)
            {
                verifiedDimmer = true;
            }

            // Test as Distro
            if (distroFormat != ImportFormat.NoAssignment &&
                VerifyStringFormat(DimmerNumberText, importConfiguration, RackType.Distro,distroFormat) == true)
            {
                verifiedDistro = true;
            }

            // Compare Results.
            // Verified Dimmer.
            if (verifiedDimmer == true && verifiedDistro == false)
            {
                // Parse Values into integers.
                int universe;
                int dimmerNumber = ParseDimmerNumber(importConfiguration, out universe);

                if (IsInDimmersRange(importConfiguration, universe, dimmerNumber))
                {
                    return RackType.Dimmer;
                }

                else
                {
                    return RackType.OutsideLabelRange;
                }
            }

            // Verified Distro.
            if (verifiedDimmer == false && verifiedDistro == true)
            {
                if (IsInDistroRange(importConfiguration, ParseDistroNumber(importConfiguration)))
                {
                    return RackType.Distro;
                }

                else
                {
                    return RackType.OutsideLabelRange;
                }
            }

            // Both failed Verification.
            if (verifiedDimmer == false && verifiedDistro == false)
            {
                return RackType.Unparseable;
            }

            // Both Passed Verification.
            if (verifiedDimmer == true && verifiedDistro == true)
            {
                bool inDimmerRange = false;
                bool inDistroRange = false;

                // Test Both Ranges
                int universeNumber;
                int dimmerNumber = ParseDimmerNumber(importConfiguration, out universeNumber);
                if (IsInDimmersRange(importConfiguration, universeNumber, dimmerNumber))
                {
                    inDimmerRange = true;
                }

                if (IsInDistroRange(importConfiguration, ParseDistroNumber(importConfiguration)))
                {
                    inDistroRange = true;
                }

                // Compare Results
                // In Dimmer Range.
                if (inDimmerRange == true && inDistroRange == false)
                {
                    return RackType.Dimmer;
                }

                // In Distro Range.
                if (inDimmerRange == false && inDistroRange == true)
                {
                    return RackType.Distro;
                }

                // Not in either Range.
                if (inDimmerRange == false && inDistroRange == false)
                {
                    return RackType.OutsideLabelRange;
                }

                // In Both Ranges.
                if (inDimmerRange == true && inDistroRange == true)
                {
                    return RackType.ClashingRange;
                }
            }

            return RackType.Unparseable;
        }

        private bool IsInDistroRange(ImportConfiguration importConfiguration, int dimmerNumber)
        {
            // Search the DistroRanges for a matching Range. Return true if found.
            var query = from range in importConfiguration.DistroRanges
                        where range.FirstDimmerNumber <= DimmerNumber &&
                        range.LastDimmerNumber >= dimmerNumber
                        select range;

            return query.Count() > 0;
        }

        private bool IsInDimmersRange(ImportConfiguration importConfiguration, int universeNumber, int dimmerNumber)
        {
            var query = from range in importConfiguration.DimmerRanges
                        where range.Universe == universeNumber &&
                        range.FirstDimmerNumber <= dimmerNumber &&
                        range.LastDimmerNumber >= dimmerNumber
                        select range;

            return query.Count() > 0;
        }

        // Verifies if data Matches its inferred Format.
        private bool VerifyStringFormat(string data, ImportConfiguration importConfiguration,
            RackType rackType, ImportFormat inferredFormat)
        {
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] numbers = "1234567890".ToCharArray();
            char[] slash = {'/'};
            
            // Verify Dimmer Formatting
            if (rackType == RackType.Dimmer)
            {
                switch (inferredFormat)
                {
                    case ImportFormat.Format1:
                        // Does the Data contain a Slash, Numbers AND Not contain any Letters?
                        if (data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.Format2:
                        // Does the Data Contain ONLY Numeric Characters?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(slash) == -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.Format3:
                        // Does the data Contain Numbers and Letters But NOT Slashes?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.Format4:
                        // Does the Data Contain Letters, slashes and Numbers?
                        if (data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.NoAssignment:
                        return false;

                    default:
                        return false;
                }
            
            }

            else if (rackType == RackType.Distro)
            {
                switch (inferredFormat)
                {
                    case ImportFormat.Format1:
                        // Does the data Contain Numbers and Letters But NOT Slashes, And Contains the Correct Distro Prefix?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) == -1 &&
                            data.Contains(importConfiguration.DistroNumberPrefix))
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.Format2:
                        // Does the Data Contain ONLY Numeric Characters?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(slash) == -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.Format3:
                        // Does the Data contain a Slash, Numbers AND Not contain any Letters?
                        if (data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.Format4:
                        // Does the Data Contain Letters, slashes and Numbers?
                        if (data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormat.NoAssignment:
                        return false;
                    default:
                        break;
                }
            }
            return false;
        }

        // Provides Modification (UndoRedo) Events.
        public event NotifyModificationEventHandler NotifyModification;

        protected void OnNotifyModification(object sender, string propertyName, string oldValue)
        {
            if (NotifyModification != null)
            {
                NotifyModification(this, new NotifyModificationEventArgs(this, propertyName, oldValue));
            }
        }

        // Provides the comparator functionality to the list.Sort() Method. Sorts by Rack Unit Type, Then by Rack Number, then By Dimmer Number.
        public int CompareTo(DimmerDistroUnit other)
        {
            if (RackUnitType == other.RackUnitType)
            {
                if (UniverseNumber == other.UniverseNumber)
                {
                    return DimmerNumber - other.DimmerNumber;
                }
                return UniverseNumber - other.UniverseNumber;
            }
            return RackUnitType - other.RackUnitType;
        }

        private int ExtractUniverseNumber(string text, ImportConfiguration importConfiguration)
        {
            ImportFormat DMXaddressColumnFormat = importConfiguration.DMXAddressImportFormat;

            // Overide the Function if User has chosen to overide Universe Infomation.
            if (DMXaddressColumnFormat == ImportFormat.NoUniverseData)
            {
                return 0;
            }

            if (VerifyStringFormat(text, importConfiguration, RackType.Dimmer, DMXaddressColumnFormat) == true)
            {
                switch (DMXaddressColumnFormat)
                {
                    case ImportFormat.Format1:
                        return Convert.ToInt32(SplitBySlash(text)[0].Trim());
                    case ImportFormat.Format2:
                        return Convert.ToInt32(text.Trim());
                    case ImportFormat.Format3:
                        return ConvertStreamLetterToNumber(RemoveNumbers(text).ToCharArray()[0]);
                    case ImportFormat.Format4:
                        return Convert.ToInt32(ConvertStreamLetterToNumber(SplitBySlash(text)[0].Trim().ToCharArray()[0]));
                    case ImportFormat.NoUniverseData:
                        return 0;
                    default:
                        return -2;
                }
            }

            else
            {
                Console.WriteLine("Could Not Read DMX Address Column");
                return 0;
            }
        }

        #region Serialization
        public DimmerDistroUnitStorage GenerateStorage()
        {
            DimmerDistroUnitStorage storage = new DimmerDistroUnitStorage();

            storage.ChannelNumber = ChannelNumber;
            storage.InstrumentName = InstrumentName;
            storage.MulticoreName = MulticoreName;
            storage.Position = Position;
            storage.UserField1 = UserField1;
            storage.UserField2 = UserField2;
            storage.UserField3 = UserField3;
            storage.UserField4 = UserField4;

            storage.DimmerNumberText = DimmerNumberText;
            storage.DMXAddressText = DMXAddressText;

            storage.ImportIndex = ImportIndex;

            storage.RackUnitType = RackUnitType;
            storage.UniverseNumber = UniverseNumber;
            storage.AbsoluteDMXAddress = AbsoluteDMXAddress;
            storage.DimmerNumber = DimmerNumber;

            return storage;

        }
        #endregion
    }

    
    public class DimmerDistroUnitStorage
    {
        public string ChannelNumber;
        public string InstrumentName;
        public string MulticoreName;
        public string Position;
        public string UserField1;
        public string UserField2;
        public string UserField3;
        public string UserField4;

        public string DimmerNumberText;
        public string DMXAddressText;

        public int ImportIndex;

        public RackType RackUnitType;
        public int UniverseNumber;
        public int AbsoluteDMXAddress;
        public int DimmerNumber;
        public int RackNumber;


    }
}