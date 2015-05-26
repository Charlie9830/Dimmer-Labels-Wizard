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
        public static void SanitizeDimDistroUnits()
        {
            SortDimDistroUnits();

            RemoveNonLabelRangeUnits();
            ResolveRackNumbers();

            ResolvePiggybacks();

            // Resolve Blank Distro Channels
            ResolveBlankDistroChannels(UserParameters.StartDistroNumber, UserParameters.EndDistroNumber);
            // Resolve Blank Dimmer Channels
            ResolveBlankDimmerChannels(UserParameters.StartDimmerNumber, UserParameters.EndDimmerNumber);
        }

        // Sort the DimDistoUnits List into A Sortorder set by the Globals.DimDistroSortOrder Enumeration.
        private static void SortDimDistroUnits()
        {
            Globals.DimmerDistroUnits.Sort();
        }

        // Remove Multiple Occurances of Piggybacks and Concatenates Multicore names if required.
        private static int ResolvePiggybacks()
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
        private static void ResolveBlankChannels(int startDimmerNumber,int endDimmerNumber)
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
                         Globals.DimmerDistroUnits[index + 1].MulticoreName = "Blank";
                         Globals.DimmerDistroUnits[index + 1].InstrumentName = "Blank";
                         // Determine and Resolve it's Rack Number
                         Globals.DimmerDistroUnits[index + 1].RackNumber = FindRackNumber(Globals.DimmerDistroUnits[index + 1]);
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
                         Globals.DimmerDistroUnits[index + 1].InstrumentName = "Blank";
                         // Determine and Resolve it's RackNumber.
                         Globals.DimmerDistroUnits[index + 1].RackNumber = FindRackNumber(Globals.DimmerDistroUnits[index + 1]);
                     }
                 }
             }
         }

        }

        private static void ResolveBlankDistroChannels(int firstDimmerNumber, int lastDimmerNumber)
        {
            // Make a list of Integers spanning from the first DistroNumber to the Last DistroNumber.
            int[] distroNumbers = GenerateNumberArray(firstDimmerNumber, lastDimmerNumber);

            // Find the index of the First Distro Unit in DimmerDistroUnits.
            int firstDistroIndex = Globals.DimmerDistroUnits.FindIndex(delegate(DimmerDistroUnit unit) { return unit.RackUnitType == RackType.Distro; });

            // Iterate through both lists simoultenously.
            int primaryIndex = 0;
            int secondaryIndex = 0;

            for (primaryIndex = firstDistroIndex; primaryIndex < Globals.DimmerDistroUnits.Count && 
                secondaryIndex < distroNumbers.Length;)
            {
                if (Globals.DimmerDistroUnits[primaryIndex].DimmerNumber != distroNumbers[secondaryIndex])
                {
                    Globals.DimmerDistroUnits.Insert(primaryIndex, new DimmerDistroUnit());

                    // The Blank Unit is now the PrimaryIndex. "I am the captain now".
                    Globals.DimmerDistroUnits[primaryIndex].RackUnitType = RackType.Distro;
                    Globals.DimmerDistroUnits[primaryIndex].DimmerNumber = Globals.DimmerDistroUnits[primaryIndex + 1].DimmerNumber - 1;
                    Globals.DimmerDistroUnits[primaryIndex].ChannelNumber = "Blank";
                    Globals.DimmerDistroUnits[primaryIndex].MulticoreName = "Blank";
                    Globals.DimmerDistroUnits[primaryIndex].InstrumentName = "Blank";
                    Globals.DimmerDistroUnits[primaryIndex].Position = "Blank";

                    Globals.DimmerDistroUnits[primaryIndex].RackNumber = FindRackNumber(Globals.DimmerDistroUnits[primaryIndex]);
                }

                else
                {
                    primaryIndex++;
                    secondaryIndex++;
                }
            }
        }

        private static void ResolveBlankDimmerChannels(int firstDimmerNumber, int lastDimmerNumber)
        {
         // Make a list of Integers spanning from the first DistroNumber to the Last DistroNumber.
            int[] dimmerNumbers = GenerateNumberArray(firstDimmerNumber, lastDimmerNumber);

            // Find the index of the First Distro Unit in DimmerDistroUnits.
            int firstDimmerIndex = Globals.DimmerDistroUnits.FindIndex(delegate(DimmerDistroUnit unit) { return unit.RackUnitType == RackType.Dimmer; });
            int lastDimmerIndex = Globals.DimmerDistroUnits.FindLastIndex(delegate(DimmerDistroUnit unit) { return unit.RackUnitType == RackType.Dimmer; });

            // Iterate through both lists simoultenously.
            int primaryIndex = 0;
            int secondaryIndex = 0;

            for (primaryIndex = firstDimmerIndex; primaryIndex < Globals.DimmerDistroUnits.Count &&
                secondaryIndex < dimmerNumbers.Length; )
            {
                if (Globals.DimmerDistroUnits[primaryIndex].DimmerNumber != dimmerNumbers[secondaryIndex])
                {
                    Globals.DimmerDistroUnits.Insert(primaryIndex, new DimmerDistroUnit());

                    // The Blank Unit is now the PrimaryIndex. "I am the captain now".
                    Globals.DimmerDistroUnits[primaryIndex].RackUnitType = RackType.Dimmer;
                    Globals.DimmerDistroUnits[primaryIndex].DimmerNumber = Globals.DimmerDistroUnits[primaryIndex + 1].DimmerNumber - 1;
                    Globals.DimmerDistroUnits[primaryIndex].UniverseNumber = Globals.DimmerDistroUnits[primaryIndex + 1].UniverseNumber;
                    Globals.DimmerDistroUnits[primaryIndex].ChannelNumber = "Blank";
                    Globals.DimmerDistroUnits[primaryIndex].MulticoreName = "Blank";
                    Globals.DimmerDistroUnits[primaryIndex].InstrumentName = "Blank";
                    Globals.DimmerDistroUnits[primaryIndex].Position = "Blank";

                    Globals.DimmerDistroUnits[primaryIndex].RackNumber = FindRackNumber(Globals.DimmerDistroUnits[primaryIndex]);
                }

                else
                {
                    primaryIndex++;
                    secondaryIndex++;
                }
            }
        }

        private static int[] GenerateNumberArray(int firstNumber, int lastNumber)
        {
            List<int> returnList = new List<int>();

            for (int counter = firstNumber; counter <= lastNumber; counter++)
            {
                returnList.Add(counter);
            }

            return returnList.ToArray();
        }
        // Determine Rack Numbers for objects based of the User Inputed List of Rack Addresses.
        public static void ResolveRackNumbers()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                int rackNumber = FindRackNumber(element);
                if (rackNumber != -1)
                {
                    element.RackNumber = rackNumber;
                }

                else
                {
                    element.RackNumber = 0;
                }
            }
        }

        // Helper Method. Returns the Rack Number that a DimDistroUnit resides in. Returns -1 if a Rack cannot be found
        private static int FindRackNumber(DimmerDistroUnit inputUnit)
        {
            if (inputUnit.RackUnitType == RackType.Distro)
            {
                foreach (var element in UserParameters.DistroRacks)
                {
                    if (inputUnit.DimmerNumber >= element.StartingAddress &&
                        inputUnit.DimmerNumber <= element.EndingAddress)
                    {
                        return element.RackNumber;
                    }
                }
                return -1;
            }

            else if (inputUnit.RackUnitType == RackType.Dimmer)
            {
                foreach (var element in UserParameters.DimmerRacks)
                {
                    if (inputUnit.UniverseNumber == element.StartingAddress.Universe)
                    {
                        if (inputUnit.DimmerNumber >= element.StartingAddress.Channel &&
                            inputUnit.DimmerNumber <= element.EndingAddress.Channel)
                        {
                            return element.RackNumber;
                        }
                    }
                }
                return -1;
            }

            else
            {
                return -1;
            }
        }

        private static void RemoveNonLabelRangeUnits()
        {
            for (int index = 0; index < Globals.DimmerDistroUnits.Count;)
            {
                if (FindRackNumber(Globals.DimmerDistroUnits[index]) == -1)
                {
                    Globals.DimmerDistroUnits.RemoveAt(index);
                }

                else
                {
                    index++;
                }
            }
        }

        private static bool IsInLabelRange(DimmerDistroUnit unit)
        {
            if (FindRackNumber(unit) == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Removes all Units of RackType Unkown 
        private static int RemoveUnknownUnits()
        {
            int deleteCount = 0;
            for (int index = 0; index < Globals.DimmerDistroUnits.Count;)
            {
                if (Globals.DimmerDistroUnits[index].RackUnitType == RackType.OutsideLabelRange)
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
