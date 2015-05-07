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

            ResolveBlankChannels(UserParameters.StartDistroNumber,UserParameters.EndDistroNumber);

            return deleteCount;
        }

        // Sort the DimDistoUnits List into A Sortorder set by the Globals.DimDistroSortOrder Enumeration.
        private static void SortDimDistroUnits()
        {
            Globals.DimmerDistroUnits.Sort();
        }

        // Remove All Instances of Cabinet number 0. Returns: The amount of items removed.
        private static int RemoveNonCabinetData()
        {
            int index = 0;
            int deleteCount = 0;

            for (index = 0; index < Globals.DimmerDistroUnits.Count;)
            {
                // Remove object from List if Cabinet Number equals 0.
                if (Globals.DimmerDistroUnits[index].CabinetNumber == 0)
                {
                    Globals.DimmerDistroUnits.RemoveAt(index);
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
            int index = 0;
            int deleteCount = 0;
            char seperatingCharacter = ' ';

            for (index = 0; index < Globals.DimmerDistroUnits.Count; )
            {
                // Check if Dimmer numbers are the same.
                if ((index + 1) < Globals.DimmerDistroUnits.Count && Globals.DimmerDistroUnits[index].DimmerNumber == Globals.DimmerDistroUnits[index + 1].DimmerNumber)
                {
                    // If So check if Multicore names are the same. Concatenate if so.
                    if (Globals.DimmerDistroUnits[index].MulticoreName != Globals.DimmerDistroUnits[index + 1].MulticoreName)
                    {
                        Globals.DimmerDistroUnits[index].MulticoreName += seperatingCharacter + Globals.DimmerDistroUnits[index + 1].MulticoreName;
                    }

                    // Remove object.
                    Globals.DimmerDistroUnits.RemoveAt(index + 1);
                    index++;
                    deleteCount++;
 
                }
                
                else
                {
                    index++;
                }
            }
            return deleteCount++;
        }

        // Generates Blank DimmerDistroObjects.
        private static void ResolveBlankChannels(int startIndex,int endIndex)
        {
         for (int index = 0; index < Globals.DimmerDistroUnits.Count; index++)
         {
             // Are we looking at a Distro?
             if (Globals.DimmerDistroUnits[index].RackUnitType == RackType.Distro)
             {
                 // Dont try and look out of bounds.
                 if (index + 1 != Globals.DimmerDistroUnits.Count)
                 {
                     // Are the Dimmer_numbers Consequtive?
                     if (Globals.DimmerDistroUnits[index].DimmerNumber != Globals.DimmerDistroUnits[index + 1].DimmerNumber - 1)
                     {
                         // Create a new Blank DimDistro Object
                         Globals.DimmerDistroUnits.Insert(index + 1, new DimmerDistroUnit());
                         // Populate it
                         Globals.DimmerDistroUnits[index + 1].RackUnitType = RackType.Distro;
                         Globals.DimmerDistroUnits[index + 1].DimmerNumber = Globals.DimmerDistroUnits[index].DimmerNumber + 1;
                         Globals.DimmerDistroUnits[index + 1].ChannelNumber = "Blank";
                         Globals.DimmerDistroUnits[index + 1].MulticoreName = "Blank Blank Blank";
                         Globals.DimmerDistroUnits[index + 1].InstrumentType = "Blank";
                         // Atempt to Resolve it's Cabinet Address.
                         ResolveDistroCabRackNo(Globals.DimmerDistroUnits[index + 1]);
                     }
                     
                 }
             }

             else if (Globals.DimmerDistroUnits[index].RackUnitType == RackType.Dimmer)
             {
                 // Don't try and look out of bound.
                 if (index + 1 != Globals.DimmerDistroUnits.Count)
                 {
                     // Are the Dimmer_numbers Consecutive, Within the Dimmer Range & in the dimmer Universe?
                     if (Globals.DimmerDistroUnits[index].DimmerNumber != Globals.DimmerDistroUnits[index + 1].DimmerNumber - 1 &&
                         UserParameters.IsInDimmerUniverses(Globals.DimmerDistroUnits[index].UniverseNumber) == true &&
                         Globals.DimmerDistroUnits[index].DimmerNumber <= UserParameters.EndDimmerNumber)
                     {
                         // Create a new Blank Object
                         Globals.DimmerDistroUnits.Insert(index + 1, new DimmerDistroUnit());
                         // Populate it
                         Globals.DimmerDistroUnits[index + 1].RackUnitType = RackType.Dimmer;
                         Globals.DimmerDistroUnits[index + 1].UniverseNumber = Globals.DimmerDistroUnits[index].UniverseNumber;
                         Globals.DimmerDistroUnits[index + 1].DimmerNumber = Globals.DimmerDistroUnits[index].DimmerNumber + 1;
                         Globals.DimmerDistroUnits[index + 1].ChannelNumber = "Blank";
                         Globals.DimmerDistroUnits[index + 1].MulticoreName = "Blank";
                         Globals.DimmerDistroUnits[index + 1].InstrumentType = "Blank";
                         // Attempt to Resolve it's cabinet number.
                         ResolveDimmerCabRackNo(Globals.DimmerDistroUnits[index + 1]);
                     }
                 }
             }
         }

        }

        // Atempt to Resolve Missing Cabinet Numbers based on Contexual Infomation.
        public static void ResolveCabinetRackNumbers()
        {
            // Re-sort the DimDistroList to match Method Requirements.
            Globals.DimmerDistroSortOrder = SortOrder.DimmerAndDistroNumber;
            Globals.DimmerDistroUnits.Sort();

            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (element.CabinetNumber == 0 || element.RackNumber == 0)
                {
                    if (element.RackUnitType == RackType.Dimmer)
                    {
                        ResolveDimmerCabRackNo(element);
                        
                    }

                    else if (element.RackUnitType == RackType.Distro)
                    {
                        ResolveDistroCabRackNo(element);
                    }
                }
            }

            // Return Sort Order to Default.
            Globals.DimmerDistroSortOrder = SortOrder.Default;
            Globals.DimmerDistroUnits.Sort();
        }

        // Helper Method for ResolveDistroCabinetNumbers. Checks if 2 DimDistroObjects reside in the same Distro Rack.
        // Returns true if they do.
        private static bool AreInSameRack(int inputIndex1, int inputIndex2, RackType rack_type, bool is5k)
        {
            // Check If Distro or Dimmer.
            if (rack_type == RackType.Distro)
            {
                int rackSize = 12;

                int distroIndex1 = 1;
                int distroIndex2 = 2;

                // Determine the UserParameters.DistroStartingAddress Index of inputIndex1
                for (int i = 0; i < UserParameters.DistroStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DistroStartAddresses.Count)
                    {
                        if (Globals.DimmerDistroUnits[inputIndex1].DimmerNumber >= UserParameters.DistroStartAddresses[i] &&
                            Globals.DimmerDistroUnits[inputIndex1].DimmerNumber <= UserParameters.DistroStartAddresses[i] + (rackSize - 1))
                        {
                            distroIndex1 = i;
                            break;
                        }
                    }
                }
                // Determine the UserParameters.DistroStartingAddress Index of inputIndex2
                for (int i = 0; i < UserParameters.DistroStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DistroStartAddresses.Count)
                    {
                        if (Globals.DimmerDistroUnits[inputIndex2].DimmerNumber >= UserParameters.DistroStartAddresses[i] &&
                            Globals.DimmerDistroUnits[inputIndex2].DimmerNumber <= UserParameters.DistroStartAddresses[i] + (rackSize - 1))
                        {
                            distroIndex2 = i;
                            break;
                        }
                    }
                }

                if (distroIndex1 == distroIndex2)
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
                int rackSize = 12;

                if (is5k == true)
                {
                    rackSize = 6;

                }

                int dimmerIndex1 = 1;
                int dimmerIndex2 = 2;

                // Determine the UserParameters.DimmerStartingAddress Index of input_index1
                for (int i = 0; i < UserParameters.DimmerStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DimmerStartAddresses.Count)
                    {
                        if (Globals.DimmerDistroUnits[inputIndex1].UniverseNumber == UserParameters.DimmerStartAddresses[i].universe &&
                            Globals.DimmerDistroUnits[inputIndex1].DimmerNumber >= UserParameters.DimmerStartAddresses[i].channel &&
                            Globals.DimmerDistroUnits[inputIndex1].DimmerNumber <= UserParameters.DimmerStartAddresses[i].channel + (rackSize - 1))
                        {
                            dimmerIndex1 = i;
                            break;
                        }
                    }
                }
                // Determine the UserParameters.DistroStartingAddress Index of input_index2
                for (int i = 0; i < UserParameters.DimmerStartAddresses.Count; i++)
                {
                    if (i != UserParameters.DimmerStartAddresses.Count)
                    {
                        if (Globals.DimmerDistroUnits[inputIndex2].UniverseNumber == UserParameters.DimmerStartAddresses[i].universe &&
                            Globals.DimmerDistroUnits[inputIndex2].DimmerNumber >= UserParameters.DimmerStartAddresses[i].channel &&
                            Globals.DimmerDistroUnits[inputIndex2].DimmerNumber <= UserParameters.DimmerStartAddresses[i].channel + (rackSize - 1))
                        {
                            dimmerIndex2 = i;
                            break;
                        }
                    }
                }

                if (dimmerIndex1 == dimmerIndex2)
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
        private static int FindPrecedingCabRackNo(int searchStartIndex,bool is5k)
        {
            int rackSize = 12;
            if (is5k == true)
            {
                rackSize = 6;
            }

            int outputIndex = -1;
            int i = 1;

            for (int index = searchStartIndex - 1; index >= 0 && i <= (rackSize - 1); index--)
            {
                if (Globals.DimmerDistroUnits[index].CabinetNumber != 0 &&
                    Globals.DimmerDistroUnits[index].RackNumber != 0)
                {
                    outputIndex = index;
                    break;
                }
                i++;
            }

            return outputIndex;

        }

        // Helper Method for Resolve DistroCabinetNumbers. Searches for the closest Preceding DimDistro Object with
        // Cabinet/Rack Data. Returns the Index of the unit or -1 if unit cannot be found.
        private static int FindSucceedingCabRackNo(int searchStartIndex,bool is5k)
        {
            int rackSize = 12;
            if (is5k == true)
            {
                rackSize = 6;
            }

            int outputIndex = -1;
            int i = 1;

            for (int listIndex = searchStartIndex + 1; listIndex <= Globals.DimmerDistroUnits.Count && i <= (rackSize - 1); listIndex++)
            {
                if (Globals.DimmerDistroUnits[listIndex].CabinetNumber != 0 &&
                    Globals.DimmerDistroUnits[listIndex].RackNumber != 0)
                {
                    outputIndex = listIndex;
                    break;
                }
                i++;
            }
            return outputIndex;
        }

        // Helper Method. Returs True if DimDistroUnit Resides in a 5kw rack.
        private static bool IsIn5kRack(DimmerDistroUnit unit)
        {
           for (int list_index = 0; list_index < UserParameters.FiveKDimmerAddresses.Count; list_index++)
           {
               if (unit.UniverseNumber == UserParameters.FiveKDimmerAddresses[list_index].universe &&
                   unit.DimmerNumber >= UserParameters.FiveKDimmerAddresses[list_index].channel &&
                   unit.DimmerNumber <= UserParameters.FiveKDimmerAddresses[list_index].channel + 5)
               {
                   return true;
               }
           }
           return false;
        }

        private static void ResolveDistroCabRackNo(DimmerDistroUnit unit)
        {
            int inputIndex = Globals.DimmerDistroUnits.IndexOf(unit);

            // Find index of a suitable preceeding DimDistroUnit.
            int precedingIndex = FindPrecedingCabRackNo(inputIndex,false);

            // Check if Unit exists
            if (precedingIndex != -1)
            {
                // Calcualte if Units are in the same Distro Rack.
                if (AreInSameRack(inputIndex, precedingIndex,RackType.Distro,false) == true)
                {
                    Globals.DimmerDistroUnits[inputIndex].CabinetNumber = Globals.DimmerDistroUnits[precedingIndex].CabinetNumber;
                    Globals.DimmerDistroUnits[inputIndex].RackNumber = Globals.DimmerDistroUnits[precedingIndex].RackNumber;

                    Globals.ResolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                }

                // if not in the same distro rack try a succeeding unit.
                else
                {
                    int succeedingIndex = FindSucceedingCabRackNo(inputIndex,false);

                    if (succeedingIndex != -1)
                    {
                        if (AreInSameRack(inputIndex, succeedingIndex,RackType.Distro,false) == true)
                        {
                            Globals.DimmerDistroUnits[inputIndex].CabinetNumber = Globals.DimmerDistroUnits[succeedingIndex].CabinetNumber;
                            Globals.DimmerDistroUnits[inputIndex].RackNumber = Globals.DimmerDistroUnits[succeedingIndex].RackNumber;

                            Globals.ResolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                        }
                        else
                        {
                            // Unit is Unresolvable
                            Globals.UnresolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                        }
                    }
                }
            }

            else
            {
                int succeedingIndex = FindSucceedingCabRackNo(inputIndex,false);

                if (succeedingIndex != -1)
                {
                    if (AreInSameRack(inputIndex, succeedingIndex,RackType.Distro,false) == true)
                    {
                        Globals.DimmerDistroUnits[inputIndex].CabinetNumber = Globals.DimmerDistroUnits[succeedingIndex].CabinetNumber;
                        Globals.DimmerDistroUnits[inputIndex].RackNumber = Globals.DimmerDistroUnits[succeedingIndex].RackNumber;

                        Globals.ResolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                    }

                    else
                    {
                        Globals.UnresolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                    }
                }
            }
        }

        private static void ResolveDimmerCabRackNo(DimmerDistroUnit unit)
        {
            // Check if unit resides in a Dimmer Universe
            if (UserParameters.IsInDimmerUniverses(unit.UniverseNumber) == true)
            {
                bool isA5kRack = IsIn5kRack(unit);
                int inputIndex = Globals.DimmerDistroUnits.IndexOf(unit);

                // Find index of a suitable preceeding DimDistroUnit.
                int precedingIndex = FindPrecedingCabRackNo(inputIndex, isA5kRack);

                // Check if Unit exists
                if (precedingIndex != -1)
                {
                    // Calcualte if Units are in the same Distro Rack.
                    if (AreInSameRack(inputIndex, precedingIndex, RackType.Dimmer, isA5kRack) == true)
                    {
                        Globals.DimmerDistroUnits[inputIndex].CabinetNumber = Globals.DimmerDistroUnits[precedingIndex].CabinetNumber;
                        Globals.DimmerDistroUnits[inputIndex].RackNumber = Globals.DimmerDistroUnits[precedingIndex].RackNumber;

                        Globals.ResolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                    }

                    // if not in the same distro rack try a succeeding unit.
                    else
                    {
                        int succeedingIndex = FindSucceedingCabRackNo(inputIndex, isA5kRack);

                        if (succeedingIndex != -1)
                        {
                            if (AreInSameRack(inputIndex, succeedingIndex, RackType.Dimmer, isA5kRack) == true)
                            {
                                Globals.DimmerDistroUnits[inputIndex].CabinetNumber = Globals.DimmerDistroUnits[succeedingIndex].CabinetNumber;
                                Globals.DimmerDistroUnits[inputIndex].RackNumber = Globals.DimmerDistroUnits[succeedingIndex].RackNumber;

                                Globals.ResolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                            }
                            else
                            {
                                // Unit is Unresolvable
                                Globals.UnresolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                            }
                        }
                    }
                }

                else
                {
                    int succeedingIndex = FindSucceedingCabRackNo(inputIndex, isA5kRack);

                    if (succeedingIndex != -1)
                    {
                        if (AreInSameRack(inputIndex, succeedingIndex, RackType.Dimmer, isA5kRack) == true)
                        {
                            Globals.DimmerDistroUnits[inputIndex].CabinetNumber = Globals.DimmerDistroUnits[succeedingIndex].CabinetNumber;
                            Globals.DimmerDistroUnits[inputIndex].RackNumber = Globals.DimmerDistroUnits[succeedingIndex].RackNumber;

                            Globals.ResolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                        }

                        else
                        {
                            Globals.UnresolvedCabinetRackNumbers.Add(Globals.DimmerDistroUnits[inputIndex]);
                        }
                    }
                }
            }

        }
        // Removes all Units of RackType Unkown 
        private static int RemoveUnknownUnits()
        {
            int deleteCount = 0;
            for (int index = 0; index < Globals.DimmerDistroUnits.Count;)
            {
                if (Globals.DimmerDistroUnits[index].RackUnitType == RackType.Unknown)
                {
                    Globals.DimmerDistroUnits.RemoveAt(index);
                    deleteCount++;
                }

                else
                {
                    index++;
                }
            }
            return deleteCount;
        }
    }
}
