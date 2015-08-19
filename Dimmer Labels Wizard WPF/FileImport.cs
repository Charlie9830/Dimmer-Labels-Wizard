using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVRead = Microsoft.VisualBasic.FileIO;


namespace Dimmer_Labels_Wizard_WPF
{
    public static class FileImport
    {
        public static string FilePath;

        public static string[] CollectHeaders()
        {
            // Create new CSV object Pointed to File Location.
            CSVRead.TextFieldParser file = CreateTextFieldParser();
            file.SetDelimiters(",");


            // Read the First line to Collect the Cells.
            string[] headers = file.ReadFields();

            // Close the File to return Cursor to Top.
            file.Close();

            return headers;
        }

        public static bool ImportFile()
        {
            // Create new CSV Object and Point it to the file location.
            CSVRead.TextFieldParser file = CreateTextFieldParser();

            file.SetDelimiters(",");

            // Read the First line to Throw out Coloum headerCell values.
            file.ReadLine();

            // Keep track of and Assign FooterCells/FooterCells list Indices
            int index = 0;

            // Collect Column Indexes
            int channelColumn = UserParameters.ChannelNumberColumnIndex;
            int dimmerColumn = UserParameters.DimmerNumberColumnIndex;
            int instrumentNameColumn = UserParameters.InstrumentTypeColumnIndex;
            int multicoreNameColumn = UserParameters.MulticoreNameColumnIndex;
            int positionColumn = UserParameters.PositionColumnIndex;
            int DMXaddressColumn = UserParameters.UniverseDataColumnIndex;
            int userField1Column = UserParameters.UserField1ColumnIndex;
            int userField2Column = UserParameters.UserField2ColumnIndex;
            int userField3Column = UserParameters.UserField3ColumnIndex;
            int userField4Column = UserParameters.UserField4ColumnIndex;

            // Clear DimmerDistroUnits and Unparseable Data Lists if they have already been Populated.
            if (Globals.DimmerDistroUnits.Count > 0)
            {
                Globals.DimmerDistroUnits.Clear();
            }

            if (Globals.UnresolvableUnits.Count > 0)
            {
                Globals.UnresolvableUnits.Clear();
            }

            while (!file.EndOfData)
            {
                // Capture Each CSV File Line.
                string[] fields = file.ReadFields();

                // Check if a value exists in the Dimmer Cell.
                if (fields[dimmerColumn] != "")
                {
                    // Init Object Representing a Dimmer Or Distro Channel Here
                    Globals.DimmerDistroUnits.Insert(index, new DimmerDistroUnit());

                    // Populate object if Columns have been assigned Indexes.
                    //Directly Imported Data
                    Globals.DimmerDistroUnits[index].ChannelNumber = channelColumn == -1 ? "" : fields[channelColumn];
                    Globals.DimmerDistroUnits[index].DimmerNumberText = dimmerColumn == -1 ? "" : fields[dimmerColumn];
                    Globals.DimmerDistroUnits[index].InstrumentName = instrumentNameColumn == -1 ? "" : fields[instrumentNameColumn];
                    Globals.DimmerDistroUnits[index].MulticoreName = multicoreNameColumn == -1 ? "" : fields[multicoreNameColumn];
                    Globals.DimmerDistroUnits[index].Position = positionColumn == -1 ? "" : fields[positionColumn];
                    Globals.DimmerDistroUnits[index].UserField1 = userField1Column == -1 ? "" : fields[userField1Column];
                    Globals.DimmerDistroUnits[index].UserField2 = userField2Column == -1 ? "" : fields[userField2Column];
                    Globals.DimmerDistroUnits[index].UserField3 = userField3Column == -1 ? "" : fields[userField3Column];
                    Globals.DimmerDistroUnits[index].UserField4 = userField4Column == -1 ? "" : fields[userField4Column];

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

            return true;
        }

        private static CSVRead.TextFieldParser CreateTextFieldParser()
        {
            if (FilePath != null)
            {
                Console.WriteLine("Loading from User Selected File");
                CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(FilePath);
                return file;
            }

            else if (Environment.MachineName == "CHARLIESAMSUNG")
            {
                Console.WriteLine("Loading from Hardcoded File Path");
                CSVRead.TextFieldParser file = new CSVRead.TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\General Test Data.csv");
                return file;
            }

            else if (Environment.MachineName == "CHARLIE-METABOX")
            {
                Console.WriteLine("Loading from Hardcoded File Path");
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