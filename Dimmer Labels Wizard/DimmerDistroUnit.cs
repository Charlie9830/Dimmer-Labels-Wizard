using System;
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
        

        // Imported Temporary Data
        public string DimmerNumberText { get; set; } // Used only for data import, Value gets later parsed into Int.
        public string CabinetNumberText { get; set; } // Later parsed into Cabinet Number + Rack Number Properties

        // Application running data
        public int GlobalIdentifier { get; set; }

        // Inferred Data
        public RackType RackUnitType { get; set; }
        public int UniverseNumber { get; set; }
        public int AbsoluteDMXAddress { get; set; }
        public int DimmerNumber { get; set; }
        public int CabinetNumber { get; set; }
        public int RackNumber { get; set; }


        // Parse and Convert Dimmer, Distro, Cabinet and Global DMX Values.
        public void ParseUnitData()
        {
            // Calculate Unit Type and Parse relevant data.
            if (this.DimmerNumberText.Contains('N'))
            {
                this.RackUnitType = RackType.Distro;
                this.ParseDistroNumber();
            }
            
            else if (this.DimmerNumberText.Contains('/'))
            {
                this.RackUnitType = RackType.Dimmer;
                this.ParseDimmerNumber();
            }

            else
            {
                this.RackUnitType = RackType.Unknown;
                this.ParseGlobalDMXAddress();
            }

            // Parse Cabinet Number regardless of Unit Type.
            this.ParseCabinetNumber();
        }


        private void ParseDistroNumber()
        {
            // Define a string to perform multiple actions on
            string workingString = String.Copy(this.DimmerNumberText);

            // Find the Index of the "N" Character.
            int nIndex = workingString.IndexOf('N');

            // Remove the first two characters
            workingString = workingString.Remove(0, nIndex + 1);

            // Remove any whitespace.
            workingString = workingString.Trim();

            // Explcitly convert to int and assign value to dimmer_number.
            this.DimmerNumber = Convert.ToInt32(workingString);
        }

        private void ParseDimmerNumber()
        { 
            int delimiterIndex = this.DimmerNumberText.IndexOf('/');

            // Copy the contents into 2 working strings.
            string universeText = String.Copy(this.DimmerNumberText); // Will get trimed to the No# before the slash.
            string addressText = String.Copy(this.DimmerNumberText); // Will get trimed to the No# after the slash.

            // Trim from the delimiter (slash) to end of the string, leaving only the universe number.
            universeText = universeText.Remove(delimiterIndex, (universeText.Length - delimiterIndex));

            // Trim from the Begining of the string to the delimiter (slash).
            addressText = addressText.Remove(0, (delimiterIndex + 1));

            // Trim any remaining whitespace.
            universeText = universeText.Trim();
            addressText = addressText.Trim();
            
            // Assign values back to object properties
            this.UniverseNumber = Convert.ToInt32(universeText);
            this.DimmerNumber = Convert.ToInt32(addressText);
            
            
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

        private void ParseCabinetNumber()
        {
            // Check if Cabinet_number been assigned a Value.
            if (this.CabinetNumberText.Equals(null))
            {
                this.CabinetNumber = 0;
                this.RackNumber = 0;
            }

            else
            {
                // Declare an Array of Characters to be Trimmed from each string.
                char[] trimChars = new char[] { 'R', 'a', 'c', 'k' };

                // Copy contents into two working strings.
                string workingString = String.Copy(CabinetNumberText);
                

                // Trim "Rack" out of Both Strings.
                workingString = workingString.TrimStart(trimChars);
                
                // Split String into an Array of 2 Strings around Deliminter character.
                string[] splitStrings = workingString.Split('-');

                // Check if string array isn't empty and doesn't exceed two elements.
                if (splitStrings != null && splitStrings.Length >= 2)
                {
                    this.CabinetNumber = Convert.ToInt32(splitStrings[0].Trim());
                    this.RackNumber = Convert.ToInt32(splitStrings[1].Trim());
                    
                }
                
                else
                {
                    this.CabinetNumber = 0;
                    this.RackNumber = 0;
                }
            }
        }


        // Provides the comparator functionality to the list.Sort() Method. Sorts by Rack Unit Type, Cabinet Number, Then by Rack Number, then By Dimmer Number.
        public int CompareTo(DimmerDistroUnit other)
        {
            if (Globals.DimmerDistroSortOrder == SortOrder.Default)
            {
                if (RackUnitType == other.RackUnitType)
                {
                    if (CabinetNumber == other.CabinetNumber)
                    {
                        if (RackNumber == other.RackNumber)
                        {
                            return DimmerNumber - other.DimmerNumber;
                        }
                        return RackNumber - other.RackNumber;
                    }
                    return CabinetNumber - other.CabinetNumber;
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


    }

}
