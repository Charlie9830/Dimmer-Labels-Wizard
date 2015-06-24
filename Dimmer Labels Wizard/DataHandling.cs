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
            Console.WriteLine("Sort Complete");

            RemoveNonLabelRangeUnits();
            Console.WriteLine("Non Label Range Unit Removal Complete");

            ResolveRackNumbers();
            Console.WriteLine("Racknumber Resolution Complete");

            ResolvePiggybacks();
            Console.WriteLine("PiggyBack Resolution Complete");

            // Resolve Blank Distro Channels
            if (UserParameters.CreateDistroObjects == true)
            {
                ResolveBlankDistroChannels(UserParameters.StartDistroNumber, UserParameters.EndDistroNumber);
                Console.WriteLine("Blank Distro Channel Resolution Complete");
            }

            // Resolve Blank Dimmer Channels
            if (UserParameters.CreateDimmerObjects == true)
            {
                foreach (var element in UserParameters.DimmerRanges)
                {
                    ResolveBlankDimmerChannels(element.Universe,element.FirstChannel, element.LastChannel);
                }
                Console.WriteLine("Blank Dimmer Channel Resolution Complete");
            }
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
                if ((index + 1) < Globals.DimmerDistroUnits.Count && Globals.DimmerDistroUnits[index].DimmerNumber == Globals.DimmerDistroUnits[index + 1].DimmerNumber
                    && Globals.DimmerDistroUnits[index].UniverseNumber == Globals.DimmerDistroUnits[index + 1].UniverseNumber)
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


        private static void ResolveBlankDistroChannels(int firstDimmerNumber, int lastDimmerNumber)
        {
            // Make a list of Integers spanning from the first DistroNumber to the Last DistroNumber.
            int[] distroNumbers = GenerateNumberArray(firstDimmerNumber, lastDimmerNumber);

            // Find the index of the First Distro Unit in DimmerDistroUnits.
            int firstDistroIndex = Globals.DimmerDistroUnits.FindIndex(item => item.RackUnitType == RackType.Distro);

            if (firstDistroIndex == -1)
            {
                // No Distros have been found within the range. Return Early or Face an OutofIndex Exception.
                Console.WriteLine("No Suitable Distro Channels have been Found within this Range. Breaking out of ResolveBlankDistroChannels");
                return;
            }

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
                    Globals.DimmerDistroUnits[primaryIndex].ChannelNumber = " ";
                    Globals.DimmerDistroUnits[primaryIndex].MulticoreName = " ";
                    Globals.DimmerDistroUnits[primaryIndex].InstrumentName = " ";
                    Globals.DimmerDistroUnits[primaryIndex].Position = " ";

                    Globals.DimmerDistroUnits[primaryIndex].RackNumber = FindRackNumber(Globals.DimmerDistroUnits[primaryIndex]);
                }

                else
                {
                    primaryIndex++;
                    secondaryIndex++;
                }
            }
        }

        private static void ResolveBlankDimmerChannels(int universeNumber, int firstDimmerNumber, int lastDimmerNumber)
        {
         // Make a list of Integers spanning from the first DimmerNumber to the Last DimmerNumber.
            int[] dimmerNumbers = GenerateNumberArray(firstDimmerNumber, lastDimmerNumber);

            // Find the index of the First and Last Dimmer Items within the parameter Universe in DimmerDistroUnits.
            int firstDimmerIndex = Globals.DimmerDistroUnits.FindIndex(item => item.RackUnitType == RackType.Dimmer &&
                item.UniverseNumber == universeNumber);
            int lastDimmerIndex = Globals.DimmerDistroUnits.FindLastIndex(item => item.RackUnitType == RackType.Dimmer &&
                item.UniverseNumber == universeNumber);

            if (firstDimmerIndex == -1 || lastDimmerIndex == -1)
            {
                // Return early or face an OutofIndex exception.
                Console.WriteLine("The First or Last Dimmer indices could not be found.");
                return;
            }

            // Iterate through both lists simoultenously.
            int primaryIndex = 0;
            int secondaryIndex = 0;

            for (primaryIndex = firstDimmerIndex; primaryIndex < Globals.DimmerDistroUnits.Count &&
                secondaryIndex < dimmerNumbers.Length; )
            {
                if (Globals.DimmerDistroUnits[primaryIndex].DimmerNumber == -1)
                {
                    Console.WriteLine("The Priscilla Bus has Crashed!, A dimmer Number equals -1, See ResolveBlankDimmerNumbers()");
                    break;
                }

                if (Globals.DimmerDistroUnits[primaryIndex].DimmerNumber != dimmerNumbers[secondaryIndex])
                {
                    Globals.DimmerDistroUnits.Insert(primaryIndex, new DimmerDistroUnit());

                    // The Blank Unit is now the PrimaryIndex. "I am the captain now".
                    Globals.DimmerDistroUnits[primaryIndex].RackUnitType = RackType.Dimmer;
                    Globals.DimmerDistroUnits[primaryIndex].DimmerNumber = Globals.DimmerDistroUnits[primaryIndex + 1].DimmerNumber - 1;
                    Globals.DimmerDistroUnits[primaryIndex].UniverseNumber = Globals.DimmerDistroUnits[primaryIndex + 1].UniverseNumber;
                    Globals.DimmerDistroUnits[primaryIndex].ChannelNumber = " ";
                    Globals.DimmerDistroUnits[primaryIndex].MulticoreName = " ";
                    Globals.DimmerDistroUnits[primaryIndex].InstrumentName = " ";
                    Globals.DimmerDistroUnits[primaryIndex].Position = " ";

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
                if (IsInLabelRange(Globals.DimmerDistroUnits[index]) != true)
                {
                    Globals.DimmerDistroUnits.RemoveAt(index);
                }

                else
                {
                    index++;
                }
            }
        }

        // Helper Method for RemoveNonLabelRangeUnits.
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

        // Removes all Items of RackType Unkown 
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
