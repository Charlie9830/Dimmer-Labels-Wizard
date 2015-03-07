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
        /// <summary>
        /// Import a CSV File. Process into Objects and adds those Objects to the HeaderCells/FooterCells List.
        /// Paramters: Nothing
        /// Returns: Nothing
        /// </summary>
        public static void ImportFile()
        {

            // Creae new CSV Object and Point it to the file location.
            CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Data.csv");
            file.SetDelimiters(",");

            // Read the First line to Throw out Coloum header values.
            file.ReadLine();

            // Keep track of and Assign HeaderCells/FooterCells list Indices
            int list_index = 0;

            while (!file.EndOfData)
            {
                
                
                // Capture Each CSV File Line.
                string[] fields = file.ReadFields();

                // Check if a value exists in the Dimmer Cell.
                if (fields[1] != "")
                {
                    // Iterate through File Line, Create and Assign Values to HeaderCells/FooterCells Objects.
                    for (int i = 0; i < fields.Length; i++)
                    {
                        // Init Objects Here
                        Globals.HeaderCells.Insert(list_index, new HeaderCell());
                        Globals.FooterCells.Insert(list_index, new FooterCell());

                        // Populate them here
                        Globals.FooterCells[list_index].global_id = list_index;
                        Globals.FooterCells[list_index].top_data = fields[0];
                        Globals.FooterCells[list_index].bot_data = fields[2];

                        Globals.HeaderCells[list_index].global_id = list_index;
                        Globals.HeaderCells[list_index].data = fields[3];


                    }

                    list_index++;
                }
            }

            file.Close();

        }

    }
}
