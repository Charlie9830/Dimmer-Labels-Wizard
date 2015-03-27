using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class DimDistroUnit
    {
        // Imported Data
        public string channel_number { get; set; }
        
        public int dimmer_number { get; set; }
        public string instrument_type { get; set; }
        public string multicore_name { get; set; }
        public string cabinet_number { get; set; }

        
        public string dimmer_number_string { get; set; } // Used only for data import, Value gets later parsed into Int.

        // Application running data
        public int global_id { get; set; }

        // Inferred Data
        public string rack_unit_type { get; set; }
        public int universe_number { get; set; }
        public int absolute_dmx_address { get; set; }


        // Calculates if RackUnit is a Distro or Dimmer.
        public void CalculateRackUnitType()
        {
            if (this.dimmer_number_string.Contains("N"))
            {
                this.rack_unit_type = "Distro";
                this.ParseDistroNumber();
            }
            
            else if (this.dimmer_number_string.Contains("/"))
            {
                this.rack_unit_type = "Dimmer";
                ParseDimmerNumber();
            }

            else
            {
                this.rack_unit_type = "Unknown";
                ParseGlobalDMXAddress();
            }
        }

        private void ParseDistroNumber()
        {
            // define a string to perform multiple actions on
            string working_string = String.Copy(this.dimmer_number_string);

            // Find the Index of the "N" Character.
            int n_index = working_string.IndexOf("N");

            // Remove the first two characters
            working_string = working_string.Remove(0, n_index + 1);

            // Remove any whitespace.
            working_string = working_string.Trim();

            // Explcitly convert to int and assign value to dimmer_number.
            this.dimmer_number = Convert.ToInt32(working_string);
        }

        private void ParseDimmerNumber()
        { 
            int slash_index = this.dimmer_number_string.IndexOf("/");

            // Copy the contents into 2 working strings.
            string universe_string = String.Copy(this.dimmer_number_string); // Will get trimed to the No# before the slash.
            
            string address_string = String.Copy(this.dimmer_number_string); // Will get trimed to the No# after the slash.

            // Trim from the Slash to end of the string, leaving only the universe number.
            universe_string = universe_string.Remove(slash_index, (universe_string.Length - 1));

            // Trim from the Begining of the string to the Slash.
            address_string = address_string.Remove(0, (slash_index + 1));

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

            // Trim Whitespace
            working_string.Trim();

            // Assign result back to Object converted to Int.
            this.absolute_dmx_address = Convert.ToInt32(working_string);

        }
        
    }

    

        
    
}
