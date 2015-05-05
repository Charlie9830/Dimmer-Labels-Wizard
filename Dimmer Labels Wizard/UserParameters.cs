using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class UserParameters
    {
        public static int start_dimmer_number { get; set; }
        public static int end_dimmer_number { get; set; }

        public static List<int> DimmerUniverses = new List<int>();

        public static int start_distro_number { get; set; }
        public static int end_distro_number { get; set; }

        public static List<int> DistroStartAddresses = new List<int>();
        public static List<Globals.DMX> DimmerStartAddresses = new List<Globals.DMX>();
        public static List<Globals.DMX> FiveKDimmerAddresses = new List<Globals.DMX>();

        public static int label_width { get; set; } // Width in Pixels
        public static int label_height { get; set; } // Height in Pixels

        // Create Hardcoded Distro Start Address Values
        public static void PopulateRackStartAddresses()
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
            address1.universe = 1;
            address1.channel = 1;
            address2.universe = 1;
            address2.channel = 13;
            address3.universe = 1;
            address3.channel = 37;

            FiveKDimmerAddresses.Add(address1);
            FiveKDimmerAddresses.Add(address2);
            FiveKDimmerAddresses.Add(address3);

            int var_address = 1;
            for (int counter = 1; counter <= 19; counter ++)
            {
                Globals.DMX dimmer_address;
                dimmer_address.universe = 1;
                dimmer_address.channel = var_address;
                var_address += 12;

                DimmerStartAddresses.Add(dimmer_address);
            }
        }
 
       // Checks if a Universe exists in Dimmer Universes list.
        public static bool IsInDimmerUniverses(int input_universe)
       {
           for (int list_index = 0; list_index < DimmerUniverses.Count; list_index++ )
           {
               if (DimmerUniverses[list_index] == input_universe)
               {
                   return true;
               }
           }

           return false;
       }

        public static void SetDefaultRackLabelSettings()
        {
            
            foreach (var element in Globals.RackLabels)
            {
                System.Drawing.StringFormat centerStringFormat = new System.Drawing.StringFormat();
                centerStringFormat.Alignment = System.Drawing.StringAlignment.Center;
                centerStringFormat.LineAlignment = System.Drawing.StringAlignment.Center;

                System.Drawing.StringFormat farStringFormat = new System.Drawing.StringFormat();
                farStringFormat.Alignment = System.Drawing.StringAlignment.Center;
                farStringFormat.LineAlignment = System.Drawing.StringAlignment.Far;


                element.SetBackgroundColour(System.Drawing.Color.White);
                
                // Set Default Fonts,FontStyles and StringFormat Alignments.
                for (int list_index = 0; list_index < element.headers.Count; list_index++)
                {
                    element.headers[list_index].font = new System.Drawing.Font("Arial",16,System.Drawing.FontStyle.Bold);
                    element.headers[list_index].format = centerStringFormat;
                    

                    element.footers[list_index].top_font = new System.Drawing.Font("Arial", 12,System.Drawing.FontStyle.Bold);
                    element.footers[list_index].top_format = centerStringFormat;

                    element.footers[list_index].bot_font = new System.Drawing.Font("Arial", 10);
                    element.footers[list_index].bot_format = farStringFormat;
                    

                }

                element.label_height = UserParameters.label_height;
                element.label_width = UserParameters.label_width;
            }
        }


    }
}
