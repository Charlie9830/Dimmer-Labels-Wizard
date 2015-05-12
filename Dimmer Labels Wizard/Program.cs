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
            
            //UserParameters.StartDimmerNumber = 1;
            //UserParameters.EndDimmerNumber = 208;
            //UserParameters.DimmerUniverses.Add(1);  
            //UserParameters.StartDistroNumber = 1;
            //UserParameters.EndDistroNumber = 124;

            UserParameters.LabelWidth = 50;
            UserParameters.LabelHeight = 60;

            FORM_UserParameterEntry UserParameterEntry = new FORM_UserParameterEntry();
            UserParameterEntry.ShowDialog();

            // Debug Call Method to Populate DistroStartAddresses.
            UserParameters.HardCodeRackStartAddresses();
            

            FileImport.ImportFile();

            int deleteCount = DataHandling.SanitizeDimDistroUnits();

            Console.WriteLine("Data Handling Complete.");
            Console.WriteLine("{0} Objects Sanitzed", deleteCount);
            Console.WriteLine();
            Console.Write("{0} Resolved Cabinets / {1} Unresolved Cabinets", Globals.ResolvedCabinetRackNumbers.Count, Globals.UnresolvedCabinetRackNumbers.Count);

            

            FORM_CabinetAddressResolution CabinetAddressResolution = new FORM_CabinetAddressResolution();
            CabinetAddressResolution.ShowDialog();

            Output.ExportToRackLabel();
            UserParameters.SetDefaultRackLabelSettings();

            FORM_LabelEditor NextWindow = new FORM_LabelEditor();
            NextWindow.ShowDialog();
        }
    }
}
