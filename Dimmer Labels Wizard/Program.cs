using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace Dimmer_Labels_Wizard
{
    class Program
    {
       
        static void Main(string[] args)
        {
            FileIn.ImportFile();

            int delete_count = DataHandling.SanitizeDimDistroUnits();


            Console.WriteLine("Data Handling Complete.");
            Console.WriteLine("{0} Objects Sanitzed", delete_count);
            Console.WriteLine();

            Output.ExportToRackLabel();

            Debug.PrintRackLabelsToConsole();


        }
    }
}
