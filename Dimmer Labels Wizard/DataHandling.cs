using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class DataHandling
    {

        // Main Method. Sorts and Sanitzes Data. RETURNS: int amount of objects sanitzed.
        public static int SanitizeDimDistroUnits()
        {
            int deleteCount = 0;

            deleteCount += RemoveUnknownUnits();

            ResolveCabinetRackNumbers();

            deleteCount += RemovePiggybacks();

            ResolveBlankChannels(UserParameters.start_distro_number,UserParameters.end_distro_number);

            return deleteCount;
        }

        // Sort the DimDistoUnits List into A Sortorder set by the Globals.DimDistroSortOrder Enumeration.
        private static void SortDimDistroUnits()
        {
            Globals.DimDistroUnits.Sort();
        }

        // Remove All Instances of Cabinet number 0. Returns: The amount of items removed.
        private static int RemoveNonCabinetData()
        {
            int index = 0;
            int deleteCount = 0;

            for (index = 0; index < Globals.DimDistroUnits.Count;)
            {
                // Remove object from List if Cabinet Number equals 0.
                if (Globals.DimDistroUnits[index].cabinet_number == 0)
                {
                    Globals.DimDistroUnits.RemoveAt(index);
                    deleteCount += 1;
                }
                
                else
                {
                    index++;
                }
            }

            return deleteCount;
        }

        // Remove Occurances of Piggybacks and Concatenates Multicore names if needed.
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

        // Needs Proper Testing after Cabinet/Rack Number Resolve methods have been implemented. So that Cabinet/Rack Numbers Can be Properly Assigned
        // this will make the ExportToRackLabels method Range the strips correctly.
        private static void ResolveBlankChannels(int start_index,int end_index)
        {
         for (int list_index = 0; list_index < Globals.DimDistroUnits.Count; list_index++)
         {
             // Are we looking at a Distro?
             if (Globals.DimDistroUnits[list_index].rack_unit_type == RackType.Distro)
             {
                 // Dont try and look out of bounds.
                 if (list_index + 1 != Globals.DimDistroUnits.Count)
                 {
                     // Are the Dimmer_numbers Consequtive?
                     if (Globals.DimDistroUnits[list_index].dimmer_number != Globals.DimDistroUnits[list_index + 1].dimmer_number - 1)
                     {
                         // Create a new Blank DimDistro Object
                         Globals.DimDistroUnits.Insert(list_index + 1, new DimDistroUnit());
                         // Populate it
                         Globals.DimDistroUnits[list_index + 1].rack_unit_type = RackType.Distro;
                         Globals.DimDistroUnits[list_index + 1].dimmer_number = Globals.DimDistroUnits[list_index].dimmer_number + 1;
                         Globals.DimDistroUnits[list_index + 1].channel_number = "Blank";
                         Globals.DimDistroUnits[list_index + 1].multicore_name = "Blank";
                         Globals.DimDistroUnits[list_index + 1].instrument_type = "Blank";
                         // Atempt to Resolve it's Cabinet Address.
                         ResolveDistroCabRackNo(Globals.DimDistroUnits[list_index + 1]);
                     }
                     
                 }
             }

             else if (Globals.DimDistroUnits[list_index].rack_unit_type == RackType.Dimmer)
             {
                 // Don't try and look out of bound.
                 if (list_index + 1 != Globals.DimDistroUnits.Count)
                 {
                     // Are the Dimmer_numbers Consecutive, Within the Dimmer Range & in the dimmer Universe?
                     if (Globals.DimDistroUnits[list_index].dimmer_number != Globals.DimDistroUnits[list_index + 1].dimmer_number - 1 &&
                         UserParameters.IsInDimmerUniverses(Globals.DimDistroUnits[list_index].universe_number) == true &&
                         Globals.DimDistroUnits[list_index].dimmer_number <= UserParameters.end_dimmer_number)
                     {
                         // Create a new Blank Object
                         Globals.DimDistroUnits.Insert(list_index + 1, new DimDistroUnit());
                         // Populate it
                         Globals.DimDistroUnits[list_index + 1].rack_unit_type = RackType.Dimmer;
                         Globals.DimDistroUnits[list_index + 1].universe_number = Globals.DimDistroUnits[list_index].universe_number;
                         Globals.DimDistroUnits[list_index + 1].dimmer_number = Globals.DimDistroUnits[list_index].dimmer_number + 1;
                         Globals.DimDistroUnits[list_index + 1].channel_number = "Blank";
                         Globals.DimDistroUnits[list_index + 1].multicore_name = "Blank";
                         Globals.DimDistroUnits[list_index + 1].instrument_type = "Blank";
                         // Attempt to Resolve it's cabinet number.
                         ResolveDimmerCabRackNo(Globals.DimDistroUnits[list_index + 1]);
                     }
                 }
             }
         }

        }

        // Atempt to Resolve Missing Cabinet Numbers based on Contexual Infomation.
        public static void ResolveCabinetRackNumbers()
        {
            // Resort the DimDistroList to match Method Requirements.
            Globals.DimDistroSortOrder = SortOrder.DimmerAndDistroNumber;
            Globals.DimDistroUnits.Sort();

            foreach (var element in Globals.DimDistroUnits)
            {
                if (element.cabinet_number == 0 || element.rack_number == 0)
                {
                    if (element.rack_unit_type == RackType.Dimmer)
                    {
                        ResolveDimmerCabRackNo(element);
                        
                    }

                    else if (element.rack_unit_type == RackType.Distro)
                    {
                        ResolveDistroCabRackNo(element);
                    }
                }
            }

            // Return Sort Order to Default.
            Globals.DimDistroSortOrder = SortOrder.Default;
            Globals.DimDistroUnits.Sort();
        }

        // Helper Method for ResolveDistroCabinetNumbers. Checks if 2 DimDistroObjects reside in the same Distro Rack.
        // Returns true if they do.
        private static bool AreInSameRack(int input_index1, int input_index2, RackType rack_type, bool is5k)
        {
            // Check If Distro or Dimmer.
            if (rack_type == RackType.Distro)
            {
                int rack_size = 12;

                int distro_index1 = 1;
                int distro_index2 = 2;

                // Determine the UserParameters.DistroStartingAddress Index of input_index1
                for (int i = 0; i < UserParameters.DistroStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DistroStartAddresses.Count)
                    {
                        if (Globals.DimDistroUnits[input_index1].dimmer_number >= UserParameters.DistroStartAddresses[i] &&
                            Globals.DimDistroUnits[input_index1].dimmer_number <= UserParameters.DistroStartAddresses[i] + (rack_size - 1))
                        {
                            distro_index1 = i;
                            break;
                        }
                    }
                }
                // Determine the UserParameters.DistroStartingAddress Index of input_index2
                for (int i = 0; i < UserParameters.DistroStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DistroStartAddresses.Count)
                    {
                        if (Globals.DimDistroUnits[input_index2].dimmer_number >= UserParameters.DistroStartAddresses[i] &&
                            Globals.DimDistroUnits[input_index2].dimmer_number <= UserParameters.DistroStartAddresses[i] + (rack_size - 1))
                        {
                            distro_index2 = i;
                            break;
                        }
                    }
                }

                if (distro_index1 == distro_index2)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
               
            // Rack Type is Dimmer.
            else
            {
                int rack_size = 12;

                if (is5k == true)
                {
                    rack_size = 6;

                }

                int dimmer_index1 = 1;
                int dimmer_index2 = 2;

                // Determine the UserParameters.DimmerStartingAddress Index of input_index1
                for (int i = 0; i < UserParameters.DimmerStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DimmerStartAddresses.Count)
                    {
                        if (Globals.DimDistroUnits[input_index1].universe_number == UserParameters.DimmerStartAddresses[i].universe &&
                            Globals.DimDistroUnits[input_index1].dimmer_number >= UserParameters.DimmerStartAddresses[i].channel &&
                            Globals.DimDistroUnits[input_index1].dimmer_number <= UserParameters.DimmerStartAddresses[i].channel + (rack_size - 1))
                        {
                            dimmer_index1 = i;
                            break;
                        }
                    }
                }
                // Determine the UserParameters.DistroStartingAddress Index of input_index2
                for (int i = 0; i < UserParameters.DimmerStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DimmerStartAddresses.Count)
                    {
                        if (Globals.DimDistroUnits[input_index2].universe_number == UserParameters.DimmerStartAddresses[i].universe &&
                            Globals.DimDistroUnits[input_index2].dimmer_number >= UserParameters.DimmerStartAddresses[i].channel &&
                            Globals.DimDistroUnits[input_index2].dimmer_number <= UserParameters.DimmerStartAddresses[i].channel + (rack_size - 1))
                        {
                            dimmer_index2 = i;
                            break;
                        }
                    }
                }

                if (dimmer_index1 == dimmer_index2)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }

        // Helper Method for Resolve DistroCabinetNumebrs. Searches for the closest Preceding DimDistro Object with
        // Cabinet/Rack Data. Returns the Index of the Unit or -1 if unit cannot be found.
        private static int FindPrecedingCabRackNo(int search_start_index,bool is5k)
        {
            int rack_size = 12;
            if (is5k == true)
            {
                rack_size = 6;
            }

            int output_index = -1;
            int i = 1;

            for (int list_index = search_start_index - 1; list_index >= 0 && i <= (rack_size - 1); list_index--)
            {
                if (Globals.DimDistroUnits[list_index].cabinet_number != 0 &&
                    Globals.DimDistroUnits[list_index].rack_number != 0)
                {
                    output_index = list_index;
                    break;
                }
                i++;
            }

            return output_index;

        }

        // Helper Method for Resolve DistroCabinetNumbers. Searches for the closest Preceding DimDistro Object with
        // Cabinet/Rack Data. Returns the Index of the unit or -1 if unit cannot be found.
        private static int FindSucceedingCabRackNo(int search_start_index,bool is5k)
        {
            int rack_size = 12;
            if (is5k == true)
            {
                rack_size = 6;
            }

            int output_index = -1;
            int i = 1;

            for (int list_index = search_start_index + 1; list_index <= Globals.DimDistroUnits.Count && i <= (rack_size - 1); list_index++)
            {
                if (Globals.DimDistroUnits[list_index].cabinet_number != 0 &&
                    Globals.DimDistroUnits[list_index].rack_number != 0)
                {
                    output_index = list_index;
                    break;
                }
                i++;
            }
            return output_index;
        }

        // Helper Method. Returs True if DimDistroUnit Resides in a 5kw rack.
        private static bool IsIn5kRack(DimDistroUnit input_object)
        {
           for (int list_index = 0; list_index < UserParameters.FiveKDimmerAddresses.Count; list_index++)
           {
               if (input_object.universe_number == UserParameters.FiveKDimmerAddresses[list_index].universe &&
                   input_object.dimmer_number >= UserParameters.FiveKDimmerAddresses[list_index].channel &&
                   input_object.dimmer_number <= UserParameters.FiveKDimmerAddresses[list_index].channel + 5)
               {
                   return true;
               }
           }
           return false;
        }

        private static void ResolveDistroCabRackNo(DimDistroUnit input_object)
        {
            int input_index = Globals.DimDistroUnits.IndexOf(input_object);

            // Find index of a suitable preceeding DimDistroUnit.
            int preceding_index = FindPrecedingCabRackNo(input_index,false);

            // Check if Unit exists
            if (preceding_index != -1)
            {
                // Calcualte if Units are in the same Distro Rack.
                if (AreInSameRack(input_index, preceding_index,RackType.Distro,false) == true)
                {
                    Globals.DimDistroUnits[input_index].cabinet_number = Globals.DimDistroUnits[preceding_index].cabinet_number;
                    Globals.DimDistroUnits[input_index].rack_number = Globals.DimDistroUnits[preceding_index].rack_number;

                    Globals.ResolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                }

                // if not in the same distro rack try a succeeding unit.
                else
                {
                    int succeeding_index = FindSucceedingCabRackNo(input_index,false);

                    if (succeeding_index != -1)
                    {
                        if (AreInSameRack(input_index, succeeding_index,RackType.Distro,false) == true)
                        {
                            Globals.DimDistroUnits[input_index].cabinet_number = Globals.DimDistroUnits[succeeding_index].cabinet_number;
                            Globals.DimDistroUnits[input_index].rack_number = Globals.DimDistroUnits[succeeding_index].rack_number;

                            Globals.ResolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                        }
                        else
                        {
                            // Unit is Unresolvable
                            Globals.UnresolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                        }
                    }
                }
            }

            else
            {
                int succeeding_index = FindSucceedingCabRackNo(input_index,false);

                if (succeeding_index != -1)
                {
                    if (AreInSameRack(input_index, succeeding_index,RackType.Distro,false) == true)
                    {
                        Globals.DimDistroUnits[input_index].cabinet_number = Globals.DimDistroUnits[succeeding_index].cabinet_number;
                        Globals.DimDistroUnits[input_index].rack_number = Globals.DimDistroUnits[succeeding_index].rack_number;

                        Globals.ResolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                    }

                    else
                    {
                        Globals.UnresolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                    }
                }
            }
        }

        private static void ResolveDimmerCabRackNo(DimDistroUnit input_object)
        {
            // Check if input_object resides in a Dimmer Universe
            if (UserParameters.IsInDimmerUniverses(input_object.universe_number) == true)
            {



                bool _is5k = IsIn5kRack(input_object);
                int input_index = Globals.DimDistroUnits.IndexOf(input_object);

                // Find index of a suitable preceeding DimDistroUnit.
                int preceding_index = FindPrecedingCabRackNo(input_index, _is5k);

                // Check if Unit exists
                if (preceding_index != -1)
                {
                    // Calcualte if Units are in the same Distro Rack.
                    if (AreInSameRack(input_index, preceding_index, RackType.Dimmer, _is5k) == true)
                    {
                        Globals.DimDistroUnits[input_index].cabinet_number = Globals.DimDistroUnits[preceding_index].cabinet_number;
                        Globals.DimDistroUnits[input_index].rack_number = Globals.DimDistroUnits[preceding_index].rack_number;

                        Globals.ResolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                    }

                    // if not in the same distro rack try a succeeding unit.
                    else
                    {
                        int succeeding_index = FindSucceedingCabRackNo(input_index, _is5k);

                        if (succeeding_index != -1)
                        {
                            if (AreInSameRack(input_index, succeeding_index, RackType.Dimmer, _is5k) == true)
                            {
                                Globals.DimDistroUnits[input_index].cabinet_number = Globals.DimDistroUnits[succeeding_index].cabinet_number;
                                Globals.DimDistroUnits[input_index].rack_number = Globals.DimDistroUnits[succeeding_index].rack_number;

                                Globals.ResolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                            }
                            else
                            {
                                // Unit is Unresolvable
                                Globals.UnresolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                            }
                        }
                    }
                }

                else
                {
                    int succeeding_index = FindSucceedingCabRackNo(input_index, _is5k);

                    if (succeeding_index != -1)
                    {
                        if (AreInSameRack(input_index, succeeding_index, RackType.Dimmer, _is5k) == true)
                        {
                            Globals.DimDistroUnits[input_index].cabinet_number = Globals.DimDistroUnits[succeeding_index].cabinet_number;
                            Globals.DimDistroUnits[input_index].rack_number = Globals.DimDistroUnits[succeeding_index].rack_number;

                            Globals.ResolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                        }

                        else
                        {
                            Globals.UnresolvedCabinetRacks.Add(Globals.DimDistroUnits[input_index]);
                        }
                    }
                }
            }

        }
        // Removes all Units of RackType Unkown 
        private static int RemoveUnknownUnits()
        {
            int delete_count = 0;
            for (int list_index = 0; list_index < Globals.DimDistroUnits.Count;)
            {
                if (Globals.DimDistroUnits[list_index].rack_unit_type == RackType.Unknown)
                {
                    Globals.DimDistroUnits.RemoveAt(list_index);
                    delete_count++;
                }

                else
                {
                    list_index++;
                }
            }
            return delete_count;
        }
    }
}
