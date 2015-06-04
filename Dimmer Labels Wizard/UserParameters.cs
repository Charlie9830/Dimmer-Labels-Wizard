using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class UserParameters
    {
        public static ImportFormatting DimmerImportFormat { get; set; }
        public static ImportFormatting DMXAddressImportFormat { get; set; }

        public static ImportFormatting DistroImportFormat { get; set; }
        public static string DistroNumberPrefix { get; set; }

        public static int ChannelNumberColumnIndex { get; set; }
        public static int DimmerNumberColumnIndex { get; set; }
        public static int InstrumentTypeColumnIndex { get; set; }
        public static int MulticoreNameColumnIndex { get; set; }
        public static int PositionColumnIndex { get; set; }
        public static int UniverseDataColumnIndex { get; set; }

        public static int StartDimmerNumber { get; set; }
        public static int EndDimmerNumber { get; set; }

        public static List<int> DimmerUniverses = new List<int>();

        public static int StartDistroNumber { get; set; }
        public static int EndDistroNumber { get; set; }

        public static List<DistroRack> DistroRacks = new List<DistroRack>();
        public static List<DimmerRack> DimmerRacks = new List<DimmerRack>();

        public static int LabelWidthInMM { get; set; } // Width in Pixels
        public static int LabelHeightInMM { get; set; } // Height in Pixels

        // Create Hardcoded Distro Start Address Values
        public static void HardCodeRackNumbers()
        {
            // Distro Starting Address. NOT the Very Last Distro Channel.
            int firstDistroAddress = StartDistroNumber;
            int lastDistroAddress = EndDistroNumber - 12;

            // Dimmer Starting Addresses. NOT the Very Last Distro Channel
            int dimmerUniverse = 1;
            int firstDimmerAddress = StartDimmerNumber;
            int lastDimmerAddress = EndDimmerNumber - 12;


            int distroRackNumberCounter = 1;
            int distroOutputIndex = 0;
            int distroRackCounter = 12;

            for (int channel = firstDistroAddress; channel <= lastDistroAddress + 12; channel++)
            {
                if (distroRackCounter == 12)
                {
                    DistroRacks.Insert(distroOutputIndex, new DistroRack());
                    DistroRacks[distroOutputIndex].StartingAddress = channel;
                    DistroRacks[distroOutputIndex].EndingAddress = channel + 11;
                    DistroRacks[distroOutputIndex].RackNumber = distroRackNumberCounter;

                    distroRackNumberCounter++;
                    distroOutputIndex++;
                    distroRackCounter = 1;
                }

                else
                {
                    distroRackCounter++;
                }
            }

            int dimmerOutputIndex = 0;
            int dimmerRackCounter = 12;
            int dimmerRackNumberCounter = 1;

            for (int channel = firstDimmerAddress; channel <= lastDimmerAddress + 12; channel++)
            {
                if (dimmerRackCounter == 12)
                {
                    DimmerRacks.Insert(dimmerOutputIndex,new DimmerRack());
                    
                    Globals.DMX startAddress = new Globals.DMX();
                    startAddress.Channel = channel;
                    startAddress.Universe = dimmerUniverse;
                    Globals.DMX endAddress = new Globals.DMX();
                    endAddress.Channel = channel + 11;
                    endAddress.Universe = dimmerUniverse;

                    DimmerRacks[dimmerOutputIndex].StartingAddress = startAddress;
                    DimmerRacks[dimmerOutputIndex].EndingAddress = endAddress;
                    DimmerRacks[dimmerOutputIndex].RackNumber = dimmerRackNumberCounter;

                    dimmerRackCounter = 1;
                    dimmerOutputIndex++;
                    dimmerRackNumberCounter++;
                }

                else
                {
                    dimmerRackCounter++;
                }
            }

            DimmerUniverses.Add(1);
        }

       // Checks if a Universe exists in Dimmer Universes list.
        public static bool IsInDimmerUniverses(int inputUniverse)
       {
           for (int index = 0; index < DimmerUniverses.Count; index++ )
           {
               if (DimmerUniverses[index] == inputUniverse)
               {
                   return true;
               }
           }

           return false;
       }

        public static void SetDefaultRackLabelSettings()
        {
            
            foreach (var element in Globals.LabelStrips)
            {
                System.Drawing.StringFormat nearStringFormat = new System.Drawing.StringFormat();
                nearStringFormat.Alignment = System.Drawing.StringAlignment.Center;
                nearStringFormat.LineAlignment = System.Drawing.StringAlignment.Near;

                System.Drawing.StringFormat centerStringFormat = new System.Drawing.StringFormat();
                centerStringFormat.Alignment = System.Drawing.StringAlignment.Center;
                centerStringFormat.LineAlignment = System.Drawing.StringAlignment.Center;

                System.Drawing.StringFormat farStringFormat = new System.Drawing.StringFormat();
                farStringFormat.Alignment = System.Drawing.StringAlignment.Center;
                farStringFormat.LineAlignment = System.Drawing.StringAlignment.Far;

                element.SetBackgroundColor(System.Drawing.Color.White);
                
                // Set Default Fonts,FontStyles and StringFormat Alignments.
                for (int listIndex = 0; listIndex < element.Headers.Count; listIndex++)
                {
                    element.Headers[listIndex].Font = new System.Drawing.Font("Arial",16,System.Drawing.FontStyle.Bold);
                    element.Headers[listIndex].Format = centerStringFormat;

                    element.Footers[listIndex].TopFont = new System.Drawing.Font("Arial", 10);
                    element.Footers[listIndex].TopFormat = nearStringFormat;

                    element.Footers[listIndex].MiddleFont = new System.Drawing.Font("Arial", 18,System.Drawing.FontStyle.Bold);
                    element.Footers[listIndex].MiddleFormat = centerStringFormat;

                    element.Footers[listIndex].BottomFont = new System.Drawing.Font("Arial", 10);
                    element.Footers[listIndex].BottomFormat = farStringFormat;
                    

                }

                element.LabelHeightInMM = UserParameters.LabelHeightInMM;
                element.LabelWidthInMM = UserParameters.LabelWidthInMM;
            }
        }


    }
}
