using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVRead = Microsoft.VisualBasic.FileIO;


namespace Dimmer_Labels_Wizard
{
    public static class FileImport
    {
        
        public static void ImportFile()
        {

            // Create new CSV Object and Point it to the file location.
            CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\Advanced Test Data Les Mis Sydney.csv");
            file.SetDelimiters(",");

            // Read the First line to Throw out Coloum header values.
            file.ReadLine();

            // Keep track of and Assign HeaderCells/FooterCells list Indices
            int index = 0;

            // Column Indexes
            int channelColumn = 0;
            int dimmerColumn = 1;
            int instrumentTypeColumn = 2;
            int multicoreNameColumn = 3;
            int cabinetNumberColumn = 4;
            
            while (!file.EndOfData)
            {
                // Capture Each CSV File Line.
                string[] fields = file.ReadFields();

                // Check if a value exists in the Dimmer Cell.
                if (fields[1] != "")
                {
                    
                    
                        // Init Object Representing a Dimmer Or Distro Channel Here
                        Globals.DimmerDistroUnits.Insert(index, new DimmerDistroUnit());
                        

                        // Populate object 
                             //Directly Imported Data
                        Globals.DimmerDistroUnits[index].ChannelNumber = fields[channelColumn];

                        Globals.DimmerDistroUnits[index].DimmerNumberText = fields[dimmerColumn];
                        Globals.DimmerDistroUnits[index].InstrumentType = fields[instrumentTypeColumn];
                        Globals.DimmerDistroUnits[index].MulticoreName = fields[multicoreNameColumn];
                        Globals.DimmerDistroUnits[index].CabinetNumberText = fields[cabinetNumberColumn];

                        // Application running data.
                        Globals.DimmerDistroUnits[index].GlobalIdentifier = index;

                        // Call Method to Calculate what kind of RackUnit it is.
                        Globals.DimmerDistroUnits[index].ParseUnitData();
                    

                        index++;
                }
            }

            file.Close();

        }

    }
}
