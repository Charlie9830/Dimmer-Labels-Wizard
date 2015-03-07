using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Dimmer_Labels_Wizard
{
    public class FileOut
    {
        public static Excel.Workbook Output_Workbook = null;
        public static Excel.Application Output_Application = null;
        public static Excel.Worksheet Output_Worksheet = null;

        public static void InitExcel()
        {
            FileOut.Output_Application = new Excel.Application();
            //FileOut.Output_Application.Visible = false;
            FileOut.Output_Workbook = Output_Application.Workbooks.Open(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Output.xlsx");
            FileOut.Output_Worksheet = Output_Workbook.Sheets[1];
        }

        public void PrintToExcel()
        {
            // Init Excel Object
            FileOut.InitExcel();

            // Add Stuff Here
                // Dump Data into Document

                // Format it.
            
            // Save and Close the File
            Output_Workbook.Save();
            Output_Workbook.Close();
        }

    }
}
