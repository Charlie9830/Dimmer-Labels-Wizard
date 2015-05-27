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
        public static string[] CollectHeaders()
        {
            // Create new CSV object Pointed to File Location.
            CSVRead.TextFieldParser file = CreateTextFieldParser();
            file.SetDelimiters(",");
            
            
            // Read the First line to Collect the Headers.
            string[] headers = file.ReadFields();

            // Close the File to return Cursor to Top.
            file.Close();

            return headers;
        }

        public static void ImportFile()
        {
            // Create new CSV Object and Point it to the file location.
            CSVRead.TextFieldParser file = CreateTextFieldParser();
            
            file.SetDelimiters(",");

            // Read the First line to Throw out Coloum headerCell values.
            file.ReadLine();

            // Keep track of and Assign HeaderCells/FooterCells list Indices
            int index = 0;

            // Collect Column Indexes
            int channelColumn = UserParameters.ChannelNumberColumnIndex;
            int dimmerColumn = UserParameters.DimmerNumberColumnIndex;
            int instrumentTypeColumn = UserParameters.InstrumentTypeColumnIndex;
            int multicoreNameColumn = UserParameters.MulticoreNameColumnIndex;
            int positionColumn = UserParameters.PositionColumnIndex;
            int DMXaddressColumn = UserParameters.UniverseDataColumnIndex;

            while (!file.EndOfData)
            {
                // Capture Each CSV File Line.
                string[] fields = file.ReadFields();

                // Check if a value exists in the Dimmer Cell.
                if (fields[dimmerColumn] != "")
                {
                    // Init Object Representing a Dimmer Or Distro Channel Here
                    Globals.DimmerDistroUnits.Insert(index, new DimmerDistroUnit());

                    // Populate object 
                            //Directly Imported Data
                    Globals.DimmerDistroUnits[index].ChannelNumber = fields[channelColumn];

                    Globals.DimmerDistroUnits[index].DimmerNumberText = fields[dimmerColumn];
                    Globals.DimmerDistroUnits[index].InstrumentName = fields[instrumentTypeColumn];
                    Globals.DimmerDistroUnits[index].MulticoreName = fields[multicoreNameColumn];

                    // Application running data.
                    Globals.DimmerDistroUnits[index].ImportIndex = index;

                    // Import Format specific Data.
                    if (UserParameters.DimmerImportFormat == ImportFormatting.Format2)
                    {
                        Globals.DimmerDistroUnits[index].DMXAddressText = fields[DMXaddressColumn];
                    }

                    // Parse Unit Data.
                    Globals.DimmerDistroUnits[index].ParseUnitData();

                    index++;
                }
            }

            file.Close();

        }

        private static CSVRead.TextFieldParser CreateTextFieldParser()
        {   
            if (Environment.MachineName == "CHARLIESAMSUNG")
            {
                CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\General Test Data.csv");
                return file;
            }

            else if (Environment.MachineName == "CHARLIE-METABOX")
            {
                CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\General Test Data.csv");
                return file;
            }

            else
            {
                Console.WriteLine("Unrecognized Computer: Please add a Condition for this Computer Name, and a Filepath to FileImport.cs");
                return null;
            }
        }

        private static void DetermineColumnIndexes()
        {

        }

    }
}
