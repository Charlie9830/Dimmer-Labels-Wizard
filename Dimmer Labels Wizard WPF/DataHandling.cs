using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class DataHandling
    {

        // Main Method. Sorts and Sanitzes Data. RETURNS: int amount of objects sanitzed.
        public static void SanitizeDimDistroUnits()
        {
            SortDimDistroUnits();
            Console.WriteLine("Sort Complete");


            ResolvePiggybacks();
            Console.WriteLine("PiggyBack Resolution Complete");

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

            // Find the count of the First Distro Unit in DimmerDistroUnits.
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

            // Find the count of the First and Last Dimmer Items within the parameter Universe in DimmerDistroUnits.
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
