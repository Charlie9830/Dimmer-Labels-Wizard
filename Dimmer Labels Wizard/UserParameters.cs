using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class UserParameters
    {
        public static int StartDimmerNumber { get; set; }
        public static int EndDimmerNumber { get; set; }

        public static List<int> DimmerUniverses = new List<int>();

        public static int StartDistroNumber { get; set; }
        public static int EndDistroNumber { get; set; }

        public static List<int> DistroStartAddresses = new List<int>();
        public static List<Globals.DMX> DimmerStartAddresses = new List<Globals.DMX>();
        public static List<Globals.DMX> FiveKDimmerAddresses = new List<Globals.DMX>();

        public static int LabelWidth { get; set; } // Width in Pixels
        public static int LabelHeight { get; set; } // Height in Pixels

        // Create Hardcoded Distro Start Address Values
        public static void HardCodeRackStartAddresses()
        {
            DistroStartAddresses.Add(1);
            DistroStartAddresses.Add(13);
            DistroStartAddresses.Add(25);
            DistroStartAddresses.Add(37);
            DistroStartAddresses.Add(49);
            DistroStartAddresses.Add(61);
            DistroStartAddresses.Add(73);
            DistroStartAddresses.Add(85);
            DistroStartAddresses.Add(97);
            DistroStartAddresses.Add(109);
            DistroStartAddresses.Add(121);

            Globals.DMX address1;
            Globals.DMX address2;
            Globals.DMX address3;
            address1.Universe = 1;
            address1.Channel = 1;
            address2.Universe = 1;
            address2.Channel = 13;
            address3.Universe = 1;
            address3.Channel = 37;

            FiveKDimmerAddresses.Add(address1);
            FiveKDimmerAddresses.Add(address2);
            FiveKDimmerAddresses.Add(address3);

            int variableAddress = 1;
            for (int counter = 1; counter <= 19; counter ++)
            {
                Globals.DMX dimmer_address;
                dimmer_address.Universe = 1;
                dimmer_address.Channel = variableAddress;
                variableAddress += 12;

                DimmerStartAddresses.Add(dimmer_address);
            }
        }

        public static void PopulateRackStartAddresses()
        {
            int listIndex = 0;
            for (int channel = StartDimmerNumber; channel <= EndDimmerNumber;)
            {
                Globals.DMX address;
                address.Universe = 1;
                address.Channel = channel;

                DimmerStartAddresses.Insert(listIndex, address);

                listIndex++;
                channel += 12;
            }

            listIndex = 0;
            for (int channel = StartDistroNumber; channel <= EndDistroNumber;)
            {
                DistroStartAddresses.Insert(listIndex, channel);

                listIndex++;
                channel += 12;
            }
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
                for (int list_index = 0; list_index < element.Headers.Count; list_index++)
                {
                    element.Headers[list_index].Font = new System.Drawing.Font("Arial",16,System.Drawing.FontStyle.Bold);
                    element.Headers[list_index].Format = centerStringFormat;
                    

                    element.Footers[list_index].MiddleFont = new System.Drawing.Font("Arial", 18,System.Drawing.FontStyle.Bold);
                    element.Footers[list_index].MiddleFormat = centerStringFormat;

                    element.Footers[list_index].bottomFont = new System.Drawing.Font("Arial", 10);
                    element.Footers[list_index].bottomFormat = farStringFormat;
                    

                }

                element.LabelHeight = UserParameters.LabelHeight;
                element.LabelWidth = UserParameters.LabelWidth;
            }
        }


    }
}
