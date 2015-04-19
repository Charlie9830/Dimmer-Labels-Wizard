using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class DimDistroUnit : IComparable<DimDistroUnit>
    {

        // Imported Data
        public string channel_number { get; set; }
        public string instrument_type { get; set; }
        public string multicore_name { get; set; }
        

        // Imported Temporary Data
        public string dimmer_number_string { get; set; } // Used only for data import, Value gets later parsed into Int.
        public string cabinet_number_string { get; set; } // Later parsed into Cabinet Number + Rack Number Properties

        // Application running data
        public int global_id { get; set; }

        // Inferred Data
        public RackType rack_unit_type { get; set; }
        public int universe_number { get; set; }
        public int absolute_dmx_address { get; set; }
        public int dimmer_number { get; set; }
        public int cabinet_number { get; set; }
        public int rack_number { get; set; }


        // Parse and Convert Dimmer, Distro, Cabinet and Global DMX Values.
        public void ParseUnitData()
        {
            // Calculate Unit Type and Parse relevant data.
            if (this.dimmer_number_string.Contains('N'))
            {
                this.rack_unit_type = RackType.Distro;
                this.ParseDistroNumber();
            }
            
            else if (this.dimmer_number_string.Contains('/'))
            {
                this.rack_unit_type = RackType.Dimmer;
                this.ParseDimmerNumber();
            }

            else
            {
                this.rack_unit_type = RackType.Unknown;
                this.ParseGlobalDMXAddress();
            }

            // Parse Cabinet Number regardless of Unit Type.
            this.ParseCabinetNumber();
        }


        private void ParseDistroNumber()
        {
            // define a string to perform multiple actions on
            string working_string = String.Copy(this.dimmer_number_string);

            // Find the Index of the "N" Character.
            int n_index = working_string.IndexOf('N');

            // Remove the first two characters
            working_string = working_string.Remove(0, n_index + 1);

            // Remove any whitespace.
            working_string = working_string.Trim();

            // Explcitly convert to int and assign value to dimmer_number.
            this.dimmer_number = Convert.ToInt32(working_string);
        }

        private void ParseDimmerNumber()
        { 
            int delimiter_index = this.dimmer_number_string.IndexOf('/');

            // Copy the contents into 2 working strings.
            string universe_string = String.Copy(this.dimmer_number_string); // Will get trimed to the No# before the slash.
            string address_string = String.Copy(this.dimmer_number_string); // Will get trimed to the No# after the slash.

            // Trim from the delimiter (slash) to end of the string, leaving only the universe number.
            universe_string = universe_string.Remove(delimiter_index, (universe_string.Length - delimiter_index));

            // Trim from the Begining of the string to the delimiter (slash).
            address_string = address_string.Remove(0, (delimiter_index + 1));

            // Trim any remaining whitespace.
            universe_string = universe_string.Trim();
            address_string = address_string.Trim();
            
            // Assign values back to object properties
            this.universe_number = Convert.ToInt32(universe_string);
            this.dimmer_number = Convert.ToInt32(address_string);
            
            
        }

        private void ParseGlobalDMXAddress()
        {
            // Copy the Contents into a Working string.
            string working_string = String.Copy(this.dimmer_number_string);

            // Trim Whitespace.
            working_string.Trim();

            // Assign result back to Object converted to Int.
            this.absolute_dmx_address = Convert.ToInt32(working_string);

        }

        private void ParseCabinetNumber()
        {
            // Check if Cabinet_number been assigned a Value.
            if (this.cabinet_number_string.Equals(null))
            {
                this.cabinet_number = 0;
                this.rack_number = 0;
            }

            else
            {
                // Declare an Array of Characters to be Trimmed from each string.
                char[] trim_chars = new char[] { 'R', 'a', 'c', 'k' };

                // Copy contents into two working strings.
                string working_string = String.Copy(cabinet_number_string);
                

                // Trim "Rack" out of Both Strings.
                working_string = working_string.TrimStart(trim_chars);
                
                // Split String into an Array of 2 Strings around Deliminter character.
                string[] split_strings = working_string.Split('-');

                // Check if string array isn't empty and doesn't exceed two elements.
                if (split_strings != null && split_strings.Length >= 2)
                {
                    this.cabinet_number = Convert.ToInt32(split_strings[0].Trim());
                    this.rack_number = Convert.ToInt32(split_strings[1].Trim());
                    
                }
                
                else
                {
                    this.cabinet_number = 0;
                    this.rack_number = 0;
                }
            }
        }


        // Provides the comparator functionality to the list.Sort() Method. Sorts by Rack Unit Type, Cabinet Number, Then by Rack Number, then By Dimmer Number.
        public int CompareTo(DimDistroUnit other)
        {
            if (Globals.DimDistroSortOrder == SortOrder.Default)
            {
                if (rack_unit_type == other.rack_unit_type)
                {
                    if (cabinet_number == other.cabinet_number)
                    {
                        if (rack_number == other.rack_number)
                        {
                            return dimmer_number - other.dimmer_number;
                        }
                        return rack_number - other.rack_number;
                    }
                    return cabinet_number - other.cabinet_number;
                }
                return rack_unit_type - other.rack_unit_type;
            }

            else
                if (rack_unit_type == other.rack_unit_type)
                {
                        return dimmer_number - other.dimmer_number;
                }
            return rack_unit_type - other.rack_unit_type;
        }


    }

}
