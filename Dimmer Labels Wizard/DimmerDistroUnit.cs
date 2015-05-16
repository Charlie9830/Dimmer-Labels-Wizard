﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class DimmerDistroUnit : IComparable<DimmerDistroUnit>
    {

        // Imported Data
        public string ChannelNumber { get; set; }
        public string InstrumentName { get; set; }
        public string MulticoreName { get; set; }
        public string Position { get; set; }

        // Imported Temporary Data
        public string DimmerNumberText { get; set; }
        public string DMXAddressText { get; set; }

        // Application running data
        public int ImportIndex { get; set; }

        // Inferred Data
        public RackType RackUnitType { get; set; }
        public int UniverseNumber { get; set; }
        public int AbsoluteDMXAddress { get; set; }
        public int DimmerNumber { get; set; }
        public int RackNumber { get; set; }


        // Control Method for Parse Methods. Determines Rack Type and calls Parse Methods. Adds unit to Unparseable
        // List if string format does not Match the User set option.
        public void ParseUnitData()
        {
            RackType rackType = DetermineUnitType(UserParameters.DimmerImportFormat, UserParameters.DistroImportFormat);
            switch (rackType)
            {
                case RackType.Dimmer:
                    ParseDimmerNumber();
                    RackUnitType = RackType.Dimmer;
                    break;
                case RackType.Distro:
                    ParseDistroNumber();
                    RackUnitType = RackType.Distro;
                    break;
                case RackType.OutsideLabelRange:
                    RackUnitType = RackType.OutsideLabelRange;
                    break;
                case RackType.Unparseable:
                    Globals.UnParseableData.Add(this);
                    RackUnitType = RackType.Unparseable;
                    break;
                default:
                    Console.WriteLine("I couldn't find a RackType!");
                    break;
            }
        }

        // Sets the Member Properties.
        private void ParseDistroNumber()
        {
            switch (UserParameters.DistroImportFormat)
            {
                case ImportFormatting.Format1:
                    DimmerNumber = Convert.ToInt32(RemoveLetters(DimmerNumberText).Trim());
                    break;
                case ImportFormatting.Format2:
                    DimmerNumber = Convert.ToInt32(DimmerNumberText.Trim());
                    break;
                case ImportFormatting.Format3:
                    // Split and Discard Number preceeding Slash.
                    DimmerNumber = Convert.ToInt32(SplitBySlash(DimmerNumberText)[1].Trim());
                    break;
                case ImportFormatting.Format4:
                    // Split and Discard Letter preceeding Slash.
                    DimmerNumber = Convert.ToInt32(SplitBySlash(DimmerNumberText)[1].Trim());
                    break;
                default:
                    Console.WriteLine("ParseDistroNumber is Hitting the Default Case!");
                    break;
            }
            
        }
        // Overload: Returns Value to Caller. Returns -1 If Control falls through the Format switch.
        private int ParseDistroNumber(string text, ImportFormatting format)
        {
                int returnValue = -1;
                switch (format)
                {
                    case ImportFormatting.Format1:
                        returnValue = Convert.ToInt32(RemoveLetters(text).Trim());
                        break;
                    case ImportFormatting.Format2:
                        returnValue = Convert.ToInt32(text.Trim());
                        break;
                    case ImportFormatting.Format3:
                        // Split and Discard Number preceeding Slash.
                        returnValue = Convert.ToInt32(SplitBySlash(text)[1].Trim());
                        break;
                    case ImportFormatting.Format4:
                        // Split and Discard Letter preceeding Slash.
                        returnValue = Convert.ToInt32(SplitBySlash(text)[1].Trim());
                        break;
                    default:
                        Console.WriteLine("ParseDistroNumber is Hitting the Default Case!");
                        break;
                }
                return returnValue;
        }

        // Sets the Member Properties
        private void ParseDimmerNumber()
        {
            switch (UserParameters.DimmerImportFormat)
            {
                case ImportFormatting.Format1:
                    DimmerNumber = Convert.ToInt32(SplitBySlash(DimmerNumberText)[1].Trim());
                    UniverseNumber = Convert.ToInt32(SplitBySlash(DimmerNumberText)[0].Trim());
                    break;
                case ImportFormatting.Format2:
                    DimmerNumber = Convert.ToInt32(DimmerNumberText.Trim());
                    UniverseNumber = ExtractUniverseNumber(DMXAddressText, UserParameters.DMXAddressImportFormat);
                    break;
                case ImportFormatting.Format3:
                    DimmerNumber = Convert.ToInt32(RemoveLetters(DimmerNumberText).Trim());
                    UniverseNumber = ConvertStreamLetterToNumber(RemoveNumbers(DimmerNumberText).ToCharArray()[0]);
                    break;
                case ImportFormatting.Format4:
                    UniverseNumber = Convert.ToInt32(ConvertStreamLetterToNumber(SplitBySlash(DimmerNumberText)[0].Trim().ToCharArray()[0]));
                    DimmerNumber = Convert.ToInt32(RemoveLetters(DimmerNumberText).Trim());
                    break;
                default:
                    Console.WriteLine("ParseDimmerNumber() is hiting the default case!");
                    break;
            }
        }

        // Overload Returns Value to Caller. Returns -1 if Controls Falls Through Format Switch.
        private Globals.DMX ParseDimmerNumber(string text, ImportFormatting format)
        {
            Globals.DMX returnValue;
            returnValue.Channel = -1;
            returnValue.Universe = -1;

                switch (format)
                {
                    case ImportFormatting.Format1:
                        returnValue.Channel = Convert.ToInt32(SplitBySlash(text)[1].Trim());
                        returnValue.Universe = Convert.ToInt32(SplitBySlash(text)[0].Trim());
                        break;
                    case ImportFormatting.Format2:
                        returnValue.Channel = Convert.ToInt32(text.Trim());
                        returnValue.Universe = ExtractUniverseNumber(text, UserParameters.DMXAddressImportFormat);
                        break;
                    case ImportFormatting.Format3:
                        returnValue.Channel = Convert.ToInt32(RemoveLetters(text).Trim());
                        returnValue.Universe = ConvertStreamLetterToNumber(RemoveNumbers(DimmerNumberText).ToCharArray()[0]);
                        break;
                    case ImportFormatting.Format4:
                        returnValue.Universe = Convert.ToInt32(ConvertStreamLetterToNumber(SplitBySlash(text)[0].Trim().ToCharArray()[0]));
                        returnValue.Channel = Convert.ToInt32(RemoveLetters(text).Trim());
                        break;
                    default:
                        Console.WriteLine("ParseDimmerNumber() is hiting the default case!");
                        break;
                }
                return returnValue;
        }

        private void ParseGlobalDMXAddress()
        {
            // Copy the Contents into a Working string.
            string workingString = String.Copy(this.DimmerNumberText);

            // Trim Whitespace.
            workingString.Trim();

            // Assign result back to Object converted to Int.
            this.AbsoluteDMXAddress = Convert.ToInt32(workingString);

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
        private RackType DetermineUnitType(ImportFormatting dimmerFormat, ImportFormatting distroFormat)
        {
            if (dimmerFormat == ImportFormatting.Format2)
            {
                Console.WriteLine("REMEMBER!! Dimmer Format2 Requires Seperate Universe Infomation be imported.");
            }
            // Assuming it's a dimmer for now. Can the data Format be Verified?
            if (VerifyStringFormat(DimmerNumberText, RackType.Dimmer, dimmerFormat) == true)
            {
                // It fits the Dimmer String Format, is it in the Dimmer Label Range?
                if (FindRackNumber(ParseDimmerNumber(DimmerNumberText, dimmerFormat)) != -1)
                {
                    return RackType.Dimmer;
                }

                else
                {
                    return RackType.OutsideLabelRange;
                }
                
            }
            
            // Let's Assume it is a Distro Now. Can the data format be Verified?
            else if (VerifyStringFormat(DimmerNumberText, RackType.Distro, distroFormat) == true)
            {
                // It fits the Distro String Format. Is it in the Distro Label Range?
                if (FindRackNumber(ParseDistroNumber(DimmerNumberText, distroFormat)) != -1)
                {
                    return RackType.Distro;
                }

                else
                {
                    return RackType.OutsideLabelRange;
                }
            }

            else
            {
                // Data Format cannot be Verified assuming Dimmer or Distro. User Input requried.
                return RackType.Unparseable;
            }
        }

        // Helper Method. Refined Version of Private Method DataHandling.FindRackNumber. Checks if Distro DimmerNumber
        // Resides in a Known Distro Rack. Returns -1 if rack cannot be found.
        private int FindRackNumber(int dimmerNumber)
        {
            foreach (var element in UserParameters.DistroRacks)
            {
                if (dimmerNumber >= element.StartingAddress &&
                    dimmerNumber <= element.EndingAddress)
                {
                    return element.RackNumber;
                }
            }
            return -1;
        }

        // Overload. Checks for Dimmer Racks instead.
        private int FindRackNumber(Globals.DMX dmxValue)
        {
            foreach (var element in UserParameters.DimmerRacks)
            {
                if (dmxValue.Universe == element.StartingAddress.Universe)
                {
                    if (dmxValue.Channel >= element.StartingAddress.Channel &&
                        dmxValue.Channel <= element.EndingAddress.Channel)
                    {
                        return element.RackNumber;
                    }
                }
            }
            return -1;
        }

        // Verifies if data Matches its inferred Format.
        private bool VerifyStringFormat(string data, RackType rackType, ImportFormatting format)
        {
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] numbers = "1234567890".ToCharArray();
            char[] slash = {'/'};
            
            // Verify Dimmer Formatting
            if (rackType == RackType.Dimmer)
            {
                switch (format)
                {
                    case ImportFormatting.Format1:
                        // Does the Data contain a Slash, Numbers AND Not contain any Letters?
                        if (data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormatting.Format2:
                        // Does the Data Contain ONLY Numeric Characters?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(slash) == -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormatting.Format3:
                        // Does the data Contain Numbers and Letters But NOT Slashes?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormatting.Format4:
                        // Does the Data Contain Letters, slashes and Numbers?
                        if (data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1)
                        {
                            return true;
                        }
                        return false;
                    default:
                        return false;
                }
            
            }

            else if (rackType == RackType.Distro)
            {
                switch (format)
                {
                    case ImportFormatting.Format1:
                        // Does the data Contain Numbers and Letters But NOT Slashes?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormatting.Format2:
                        // Does the Data Contain ONLY Numeric Characters?
                        if (data.IndexOfAny(numbers) != -1 && data.IndexOfAny(slash) == -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormatting.Format3:
                        // Does the Data contain a Slash, Numbers AND Not contain any Letters?
                        if (data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1 && data.IndexOfAny(letters) == -1)
                        {
                            return true;
                        }
                        return false;
                    case ImportFormatting.Format4:
                        // Does the Data Contain Letters, slashes and Numbers?
                        if (data.IndexOfAny(letters) != -1 && data.IndexOfAny(slash) != -1 && data.IndexOfAny(numbers) != -1)
                        {
                            return true;
                        }
                        return false;
                    default:
                        break;
                }
            }
            return false;
        }

        // Provides the comparator functionality to the list.Sort() Method. Sorts by Rack Unit Type, Then by Rack Number, then By Dimmer Number.
        public int CompareTo(DimmerDistroUnit other)
        {
            if (Globals.DimmerDistroSortOrder == SortOrder.Default)
            {
                if (RackUnitType == other.RackUnitType)
                {
                    if (RackNumber == other.RackNumber)
                    {
                        return DimmerNumber - other.DimmerNumber;
                    }
                    return RackNumber - other.RackNumber;
                }
                return RackUnitType - other.RackUnitType;
            }

            else
                if (RackUnitType == other.RackUnitType)
                {
                        return DimmerNumber - other.DimmerNumber;
                }
            return RackUnitType - other.RackUnitType;
        }

        private int ExtractUniverseNumber(string text, ImportFormatting dimmerFormat)
        {
            switch (dimmerFormat)
            {
                case ImportFormatting.Format1:
                    return Convert.ToInt32(SplitBySlash(text)[0].Trim());
                case ImportFormatting.Format2:
                    return Convert.ToInt32(text.Trim());
                case ImportFormatting.Format3:
                    return ConvertStreamLetterToNumber(RemoveNumbers(text).ToCharArray()[0]);
                case ImportFormatting.Format4:
                    return Convert.ToInt32(ConvertStreamLetterToNumber(SplitBySlash(text)[0].Trim().ToCharArray()[0]));
                default:
                    return -5;
            }
        }
    }
}