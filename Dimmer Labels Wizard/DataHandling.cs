using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class DataHandling
    {

        // Main Method. Sorts and Sanitzes Data. RETURNS: int amount of objects sanitzed.
        public static int SanitizeDimDistroUnits()
        {
            int delete_count = 0;

            SortDimDistroUnits();
            delete_count += RemoveNonCabinetData();
            delete_count += RemovePiggybacks();

            return delete_count;
        }



        // Sort the DimDistoUnits List into Cabinet > Rack > Dimmer Number Order
        private static void SortDimDistroUnits()
        {
            Globals.DimDistroUnits.Sort();
        }

        // Remove All Instances of Cabinet number 0. Returns: The amount of items removed.
        private static int RemoveNonCabinetData()
        {
            int list_index = 0;
            int delete_count = 0;

            for (list_index = 0; list_index < Globals.DimDistroUnits.Count;) // Because list.RemoveAt Reorders the list, List_index Iteration has to be handled programatically.
            {
                // Remove object from List if Cabinet Number equals 0.
                if (Globals.DimDistroUnits[list_index].cabinet_number == 0)
                {
                    Globals.DimDistroUnits.RemoveAt(list_index);
                    delete_count += 1;
                }
                
                else
                {
                    list_index++;
                }
            }

            return delete_count;
        }

        // Remove Occurances of Piggybacks and Concatenates Multicore name if needed.

        private static int RemovePiggybacks()
        {
            int list_index = 0;
            int delete_count = 0;
            char seperating_character = ' ';

            for (list_index = 0; list_index < Globals.DimDistroUnits.Count; )
            {
                // Check if Dimmer numbers are the same.
                if ((list_index + 1) < Globals.DimDistroUnits.Count && Globals.DimDistroUnits[list_index].dimmer_number == Globals.DimDistroUnits[list_index + 1].dimmer_number)
                {
                    // If So check if Multicore names are the same. Concatenate if so.
                    if (Globals.DimDistroUnits[list_index].multicore_name != Globals.DimDistroUnits[list_index + 1].multicore_name)
                    {
                        Globals.DimDistroUnits[list_index].multicore_name += seperating_character + Globals.DimDistroUnits[list_index + 1].multicore_name;
                    }

                    // Remove object.
                    Globals.DimDistroUnits.RemoveAt(list_index + 1);
                    list_index++;
                    delete_count++;
 
                }
                
                else
                {
                    list_index++;
                }
            }
            return delete_count++;
        }
    }
}
