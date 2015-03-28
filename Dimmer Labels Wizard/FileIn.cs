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

            // Create new CSV Object and Point it to the file location.
            CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\Advanced Test Data Les Mis Sydney.csv");
            file.SetDelimiters(",");

            // Read the First line to Throw out Coloum header values.
            file.ReadLine();

            // Keep track of and Assign HeaderCells/FooterCells list Indices
            int list_index = 0;

            // Column Indexes
            int channel_col = 0;
            int dimmer_col = 1;
            int instrument_type_col = 2;
            int multicore_name_col = 3;
            int cabinet_number_col = 4;
            
            while (!file.EndOfData)
            {
                
                
                // Capture Each CSV File Line.
                string[] fields = file.ReadFields();

                // Check if a value exists in the Dimmer Cell.
                if (fields[1] != "")
                {
                    
                    
                        // Init Object Representing a Dimmer Or Distro Channel Here
                        Globals.DimDistroUnits.Insert(list_index, new DimDistroUnit());
                        

                        // Populate object 
                             //Directly Imported Data
                        Globals.DimDistroUnits[list_index].channel_number = fields[channel_col];

                        Globals.DimDistroUnits[list_index].dimmer_number_string = fields[dimmer_col];
                        Globals.DimDistroUnits[list_index].instrument_type = fields[instrument_type_col];
                        Globals.DimDistroUnits[list_index].multicore_name = fields[multicore_name_col];
                        Globals.DimDistroUnits[list_index].cabinet_number_string = fields[cabinet_number_col];

                        // Application running data.
                        Globals.DimDistroUnits[list_index].global_id = list_index;

                        // Call Method to Calculate what kind of RackUnit it is.
                        Globals.DimDistroUnits[list_index].ParseUnitData();
                    

                        list_index++;
                }
            }

            file.Close();

        }

    }
}
