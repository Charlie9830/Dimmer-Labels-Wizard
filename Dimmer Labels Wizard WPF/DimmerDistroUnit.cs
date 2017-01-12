﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dimmer_Labels_Wizard_WPF.Repositories;
using System.Runtime.Serialization;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    [DataContract]
    public class DimmerDistroUnit : ViewModelBase, IComparable<DimmerDistroUnit>, INotifyModification
    {
        public DimmerDistroUnit()
        {
        }

        // Database and Navigation Properties.
        [InverseProperty("PrimaryUnit")]
        public virtual ICollection<Merge> MergePrimaryUnit { get; set; }

        [InverseProperty("ConsumedUnits")]
        public virtual ICollection<Merge> MergeConsumedUnits { get; set; }

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

        #region Public Accessors.

        protected bool _IsSelected = false;
        [NotMapped]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;

                    // Notify.
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        [DataMember]
        public  string ChannelNumber
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
                    OnPropertyChanged(nameof(ChannelNumber));
                }
            }
        }

        protected string _LastImportedChannelNumber = string.Empty;
        [DataMember]
        public string LastImportedChannelNumber
        {
            get { return _LastImportedChannelNumber; }
            set
            {
                if (_LastImportedChannelNumber != value)
                {
                    _LastImportedChannelNumber = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedChannelNumber));
                }
            }
        }

        [DataMember]
        public  string InstrumentName
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


        protected string _LastImportedInstrumentName = string.Empty;
        [DataMember]
        public string LastImportedInstrumentName
        {
            get { return _LastImportedInstrumentName; }
            set
            {
                if (_LastImportedInstrumentName != value)
                {
                    _LastImportedInstrumentName = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedInstrumentName));
                }
            }
        }

        [DataMember]
        public  string MulticoreName
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


        protected string _LastImportedMulticoreName = string.Empty;

        [DataMember]
        public string LastImportedMulticoreName
        {
            get { return _LastImportedMulticoreName; }
            set
            {
                if (_LastImportedMulticoreName != value)
                {
                    _LastImportedMulticoreName = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedMulticoreName));
                }
            }
        }

        [DataMember]
        public  string Position
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


        protected string _LastImportedPosition = string.Empty;
        [DataMember]
        public string LastImportedPosition
        {
            get { return _LastImportedPosition; }
            set
            {
                if (_LastImportedPosition != value)
                {
                    _LastImportedPosition = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedPosition));
                }
            }
        }

        [DataMember]
        public  string UserField1
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


        protected string _LastImportedUserField1 = string.Empty;
        [DataMember]
        public string LastImportedUserField1
        {
            get { return _LastImportedUserField1; }
            set
            {
                if (_LastImportedUserField1 != value)
                {
                    _LastImportedUserField1 = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedUserField1));
                }
            }
        }

        [DataMember]
        public  string UserField2
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


        protected string _LastImportedUserField2 = string.Empty;
        [DataMember]
        public string LastImportedUserField2
        {
            get { return _LastImportedUserField2; }
            set
            {
                if (_LastImportedUserField2 != value)
                {
                    _LastImportedUserField2 = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedUserField2));
                }
            }
        }

        [DataMember]
        public  string UserField3
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


        protected string _LastImportedUserField3;

        [DataMember]
        public string LastImportedUserField3
        {
            get { return _LastImportedUserField3; }
            set
            {
                if (_LastImportedUserField3 != value)
                {
                    _LastImportedUserField3 = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedUserField3));
                }
            }
        }

        [DataMember]
        public  string UserField4
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


        protected string _LastImportedUserField4 = string.Empty;
        [DataMember]
        public string LastImportedUserField4
        {
            get { return _LastImportedUserField4; }
            set
            {
                if (_LastImportedUserField4 != value)
                {
                    _LastImportedUserField4 = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedUserField4));
                }
            }
        }

        [DataMember]
        public  string Custom
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


        protected string _LastImportedCustom = string.Empty;
        [DataMember]
        public string LastImportedCustom
        {
            get { return _LastImportedCustom; }
            set
            {
                if (_LastImportedCustom != value)
                {
                    _LastImportedCustom = value;

                    // Notify.
                    OnPropertyChanged(nameof(LastImportedCustom));
                }
            }
        }

        protected int _DimmerNumber = 0;

        [DataMember]
        [Key]
        [Column(Order = 3)]
        public  int DimmerNumber
        {
            get
            {
                return _DimmerNumber;
            }
            set
            {
                if (value != _DimmerNumber)
                {
                    _DimmerNumber = value;
                    OnPropertyChanged(nameof(DimmerNumber));
                }
            }
        }

        protected RackType _RackUnitType;

        [DataMember]
        [Key]
        [Column(Order = 1)]
        public RackType RackUnitType
        {
            get
            {
                return _RackUnitType;
            }

            set
            {
                if (_RackUnitType != value)
                {
                    _RackUnitType = value;

                    OnPropertyChanged(nameof(RackUnitType));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(IsUniverseEntryEnabled));
                }
            }
        }


        

        [NotMapped]
        public Color LabelColor
        {
            get
            {
                return GetLabelColor();
            }
        }

        // Window Specific Properties.
        // Invalid Units Window.
        protected bool _OmitUnit = false;
        [NotMapped]
        public bool OmitUnit
        {
            get { return _OmitUnit; }
            set
            {
                if (_OmitUnit != value)
                {
                    _OmitUnit = value;

                    // Notify.
                    OnPropertyChanged(nameof(OmitUnit));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        // Import Units Window.
        [NotMapped]
        public bool IsValid
        {
            get
            {
                if (OmitUnit)
                {
                    return true;
                }

                return RackUnitType == RackType.Dimmer ||
                    RackUnitType == RackType.Distro;
            }
        }

        // Database Manager Window.
        public bool IsUniverseEntryEnabled
        {
            get
            {
                return RackUnitType == RackType.Dimmer;
            }
        }

        #endregion

        // Imported Temporary Data
        protected string _DimmerNumberText = string.Empty;

        [NotMapped]
        public string DimmerNumberText { get; set; }

        [NotMapped]
        public string DMXAddressText { get; set; }

        // Inferred Data
        protected int _UniverseNumber;
        [DataMember]
        [Key]
        [Column(Order = 2)]
        public int UniverseNumber
        {
            get
            {
                return _UniverseNumber;
            }

            set
            {
                if (_UniverseNumber != value)
                {
                    _UniverseNumber = value;

                    OnPropertyChanged(nameof(UniverseNumber));
                }
            }

        }


        #region Methods.
        public void CopyShortNamesToLastImportNames()
        {
            _LastImportedChannelNumber = ChannelNumber;
            _LastImportedInstrumentName = InstrumentName;
            _LastImportedMulticoreName = MulticoreName;
            _LastImportedPosition = Position;
            _LastImportedUserField1 = UserField1;
            _LastImportedUserField2 = UserField2;
            _LastImportedUserField3 = UserField3;
            _LastImportedUserField4 = UserField4;
            _LastImportedCustom = Custom;
        }

        private Color GetLabelColor()
        {
            // Establish Connection to DB.
            ColorDictionaryRepository repo = new ColorDictionaryRepository(new PrimaryDB());

            // Select Search Space.
            ColorDictionary dictionary = RackUnitType == RackType.Dimmer ?
                repo.DimmerColorDictionary : repo.DistroColorDictionary;

            Color color;
            if (dictionary.TryGetColor(UniverseNumber, DimmerNumber, out color ) == true)
            {
                repo.Dispose();
                return color;
            }         

            else
            {
                repo.Dispose();
                return Colors.White;
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

        // Provides easier accsess when using Switch Statements to GetData Based on LabelField.
        public string GetOriginalData(LabelField labelField)
        {
            switch (labelField)
            {
                case LabelField.NoAssignment:
                    return "No Assignment";
                case LabelField.ChannelNumber:
                    return LastImportedChannelNumber;
                case LabelField.InstrumentName:
                    return LastImportedInstrumentName;
                case LabelField.MulticoreName:
                    return LastImportedMulticoreName;
                case LabelField.Position:
                    return LastImportedPosition;
                case LabelField.UserField1:
                    return LastImportedUserField1;
                case LabelField.UserField2:
                    return LastImportedUserField2;
                case LabelField.UserField3:
                    return LastImportedUserField3;
                case LabelField.UserField4:
                    return LastImportedUserField4;
                case LabelField.Custom:
                    return LastImportedCustom;
                default:
                    return "DimmerDistroUnit.GetOriginalData() Error";
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
        public void ParseUnitData(UnitImporter unitImporterParent, ImportConfiguration importConfiguration)
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
                    DimmerNumber = ParseDistroNumber(importConfiguration);
                    RackUnitType = RackType.Distro;
                    break;
                case RackType.OutsideLabelRange:
                    RackUnitType = RackType.OutsideLabelRange;
                    break;
                case RackType.Unparseable:
                    RackUnitType = RackType.Unparseable;
                    break;
                case RackType.ConflictingRange:
                    RackUnitType = RackType.ConflictingRange;
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
                    return RackType.ConflictingRange;
                }
            }

            return RackType.Unparseable;
        }

        private bool IsInDistroRange(ImportConfiguration importConfiguration, int dimmerNumber)
        {
            // Search the DistroRanges for a matching Range. Return true if found.
            var query = from range in importConfiguration.DistroRanges
                        where range.FirstDimmerNumber <= dimmerNumber &&
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
            ImportFormat DMXaddressColumnFormat = importConfiguration.UniverseImportFormat;

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

        public void InvalidateLabelColor()
        {
            OnPropertyChanged(nameof(LabelColor));
        }

        #endregion
    }


    public class DimmerDistroUnitEqualityComparer : IEqualityComparer<DimmerDistroUnit>
    {
        public bool Equals(DimmerDistroUnit x, DimmerDistroUnit y)
        {
            if (x.RackUnitType == y.RackUnitType)
            {
                // Dimmer.
                if (x.RackUnitType == RackType.Dimmer)
                {
                    return x.UniverseNumber == y.UniverseNumber && 
                        x.DimmerNumber == y.DimmerNumber;
                }

                if (x.RackUnitType == RackType.Distro)
                {
                    return x.DimmerNumber == y.DimmerNumber;
                }

                else
                {
                    throw new NotSupportedException("Equality Comparison is only supported on Units of RackType Dimmer or Distro)");
                }
            }

            else
            {
                return false;
            }
        }

        public int GetHashCode(DimmerDistroUnit obj)
        {
            var hCode = obj.UniverseNumber ^ obj.DimmerNumber;

            return hCode.GetHashCode();
        }
    }
}