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
            FileOut.Output_Workbook = Output_Application.Workbooks.Open(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Output Files\Test Output.xlsx");
            FileOut.Output_Worksheet = Output_Workbook.Sheets[1];
        }

        public static void PrintToExcel()
        {
            // Init Excel Object
            FileOut.InitExcel();

            Output_Worksheet.Cells[1, 1] = "test";
            
            
            // Save and Close the File
            Output_Workbook.Save();
            Output_Workbook.Close();
        }

        public static void DebugOutputToExcel()
        {
            // Init Excel Object
            FileOut.InitExcel();

            int entry_count = 0;
            int total_count = Globals.DimDistroUnits.Count;

            int col_index = 1;
            int row_index = 1;

            foreach (var element in Globals.DimDistroUnits)
            {
                Output_Worksheet.Cells[row_index,col_index] = element.multicore_name;
                row_index += 1;
                Output_Worksheet.Cells[row_index, col_index] = element.channel_number;
                row_index += 1;
                Output_Worksheet.Cells[row_index, col_index] = element.cabinet_number;
                row_index += 1;
                Output_Worksheet.Cells[row_index, col_index] = element.rack_number;
                row_index += 1;
                Output_Worksheet.Cells[row_index, col_index] = element.instrument_type;

                
                col_index += 2;
                row_index = 1;

                entry_count += 1;

                Console.WriteLine("Entry  {0}  of  {1}  Complete.",entry_count,total_count);
                
            }


            // Save and Close the File
            Output_Workbook.Save();
            Output_Workbook.Close();
            Console.WriteLine("Excel Closed");
           
        }

    }
}
