using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVRead = Microsoft.VisualBasic.FileIO;

namespace Dimmer_Labels_Wizard
{
    public static class FileIn
    {
        public static void ImportFile()
        {

            CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Data.csv");
            file.SetDelimiters(",");

            // Read the First line to Throw out Coloum header values.
            file.ReadLine();

            int list_index = 0;

            while (!file.EndOfData)
            {
                
                // Add the object to the lists, then get the reference to use in this part.

                string[] fields = file.ReadFields();

                for (int i = 0; i < fields.Length; i++ )
                {
                    // Init Objects Here
                    Globals.HeaderCells.Insert(list_index,new HeaderCell());
                    Globals.FooterCells.Insert(list_index,new FooterCell());

                    // Populate them here
                    Globals.FooterCells[list_index].top_data = fields[0];
                    Globals.FooterCells[list_index].bot_data = fields[2];

                    Globals.HeaderCells[list_index].data = fields[3];

                   
                }
                list_index++;
            }

            file.Close();

        }
    }
}
