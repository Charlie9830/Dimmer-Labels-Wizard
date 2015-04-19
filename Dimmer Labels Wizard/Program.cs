using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

/*
 * Working Notes:
 * Datahandling is still trying to Resolve Scenic Tower Dimmers. 
 * Test DataHandling.CalculateBlankDistroChannel()
 * Code CalculateBlankDimmerChannel
*/


namespace Dimmer_Labels_Wizard
{
    class Program
    {
       
        static void Main(string[] args)
        {
            // ***************************************************
            // Hard Coded user Preferences
            UserParameters.start_dimmer_number = 1;
            UserParameters.end_dimmer_number = 208;
            UserParameters.start_distro_number = 1;
            UserParameters.end_distro_number = 124;

            // Debug Call Method to Populate DistroStartAddresses.
            UserParameters.PopulateRackStartAddresses();
            // ***************************************************

            Console.BufferHeight = 8000;
            Console.BufferWidth += 30;

            FileIn.ImportFile();

            int delete_count = DataHandling.SanitizeDimDistroUnits();

            Console.WriteLine("Data Handling Complete.");
            Console.WriteLine("{0} Objects Sanitzed", delete_count);
            Console.WriteLine();
            Console.Write("{0} Resolved Cabinets / {1} Unresolved Cabinets", Globals.ResolvedCabinetRacks.Count, Globals.UnresolvedCabinetRacks.Count);

            FORM_CabinetAddressResolution CabinetAddressResolution = new FORM_CabinetAddressResolution();
            CabinetAddressResolution.ShowDialog();

            Output.ExportToRackLabel();

            Debug.PrintRackLabelsToConsole();

            

        }
    }
}
