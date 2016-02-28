using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Dimmer_Labels_Wizard_WPF
{
    public class UnitImport
    {
        public UnitImport(string filePath, string csvDelimiter)
        {
            FilePath = filePath;
            CSVDelimiter = csvDelimiter;
        }


        #region Fields
        // Main Working List.
        public List<DimmerDistroUnit> Units = new List<DimmerDistroUnit>();

        public List<DimmerDistroUnit> ConflictingUnits = new List<DimmerDistroUnit>();

        public List<DimmerDistroUnit> UnResolveableUnits = new List<DimmerDistroUnit>();

        public string FilePath = string.Empty;

        public string CSVDelimiter = string.Empty;
        #endregion

        #region CLR Properties.
        public string[] ColumnHeaders
        {
            get
            {
                return PeekHeaders();
            }
        }

        #endregion

        #region Public Methods.
        public bool ValidateFile(out string errorMessage)
        {
            TextFieldParser file = new TextFieldParser(FilePath);
            file.SetDelimiters(CSVDelimiter);

            try
            {
                while (!file.EndOfData)
                {
                    file.ReadLine();
                }
            }

            catch (MalformedLineException e)
            {
                errorMessage = e.Message;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
        
        public bool ImportData(ImportConfiguration importSettings)
        {
            // Import Data from CSV.
            Import(importSettings);
            
            if (ConflictingUnits.Count == 0 && UnResolveableUnits.Count == 0)
            {
                // No further User Interaction is Required.
                return true;
            }

            else
            {
                // Further User Interaction is Required.
                return false;
            }
        }

        public void SanitizeData()
        {
            // Sort.
            Units.Sort();

            // Remove Units that aren't within the Label Range.
            CullOutOfRangeUnits();

            // Resolve PiggyBacks.
            ResolvePiggybacks();

            // Resolve Missing Distro Channels.

            // Resolve Missing Dimmer Channels.

        }



        #endregion.

        #region Private Methods.
        private void CullOutOfRangeUnits()
        {
            var query = (from unit in Units
                        where unit.RackUnitType == RackType.OutsideLabelRange
                        select unit).ToList();

            foreach (var element in query)
            {
                Units.Remove(element);
            }
        }

        private void ResolvePiggybacks()
        {
            // Query for Units with duplicate Dimmer Numbers.
            var duplicateDimmers = from unit in Units
                                   where unit.RackUnitType == RackType.Dimmer
                                   group unit by unit.DimmerNumber into newGroup
                                   select newGroup.ToList();

            var duplicateDistros = from unit in Units
                                   where unit.RackUnitType == RackType.Distro
                                   group unit by unit.DimmerNumber into newGroup
                                   select newGroup.ToList();

            // Execute Dimmer Query.
            foreach (var list in duplicateDimmers)
            {
                if (list.TrueForAll(item => item.MulticoreName == list.First().MulticoreName) == false)
                {
                    // Multicore name Concat required.
                    StringBuilder newMulticoreName = new StringBuilder();
                    foreach (var unit in list)
                    {
                        if (newMulticoreName.ToString().Contains(unit.MulticoreName) == false)
                        {
                            newMulticoreName.Append(unit.MulticoreName + " ");
                        }
                    }
                    
                    // Concat Multicore Names 
                    list.First().MulticoreName = newMulticoreName.ToString().Trim();
                }

                // Remove from Units Collection.
                var removalUnits = list.GetRange(1, list.Count - 1);
                foreach (var element in removalUnits)
                {
                    Units.Remove(element);
                }
            }


            // Execute Distro Query.
            foreach (var list in duplicateDistros)
            {
                if (list.TrueForAll(item => item.MulticoreName == list.First().MulticoreName) == false)
                {
                    // Multicore name Concat required.
                    StringBuilder newMulticoreName = new StringBuilder();
                    foreach (var unit in list)
                    {
                        if (newMulticoreName.ToString().Contains(unit.MulticoreName) == false)
                        {
                            newMulticoreName.Append(unit.MulticoreName + " ");
                        }
                    }

                    // Concat Multicore Names 
                    list.First().MulticoreName = newMulticoreName.ToString().Trim();
                }

                // Remove from Units Collection.
                var removalUnits = list.GetRange(1, list.Count - 1);
                foreach (var element in removalUnits)
                {
                    Units.Remove(element);
                }
            }

        }

        protected void Import(ImportConfiguration importSettings)
        {
            // Collect Data

            // Create new CSV Object and Point it to the file location.
            TextFieldParser file = CreateTextFieldParser();

            file.SetDelimiters(CSVDelimiter);

            // Read the First line to Throw out Column Headers.
            file.ReadLine();


            // Collect Column Indexes
            int channelColumn = importSettings.ChannelNumberColumnIndex;
            int dimmerColumn = importSettings.DimmerNumberColumnIndex;
            int instrumentNameColumn = importSettings.InstrumentTypeColumnIndex;
            int multicoreNameColumn = importSettings.MulticoreNameColumnIndex;
            int positionColumn = importSettings.PositionColumnIndex;
            int DMXaddressColumn = importSettings.UniverseDataColumnIndex;
            int userField1Column = importSettings.UserField1ColumnIndex;
            int userField2Column = importSettings.UserField2ColumnIndex;
            int userField3Column = importSettings.UserField3ColumnIndex;
            int userField4Column = importSettings.UserField4ColumnIndex;

            // Clear Working Lists.
            Units.Clear();
            UnResolveableUnits.Clear();
            ConflictingUnits.Clear();

            // Keep track of and Assign FooterCells/FooterCells list Indices
            int index = 0;
            while (!file.EndOfData)
            {
                // Capture Each CSV File Line.
                string[] fields = file.ReadFields();

                // Check if a value exists in the Dimmer Cell.
                if (fields[dimmerColumn] != string.Empty)
                {
                    // Generate new DimmerDistroUnit.
                    var newUnit = new DimmerDistroUnit();

                    Units.Insert(index, newUnit);

                    // Populate object if Columns have been assigned Indexes.
                    // Directly Imported Data
                    newUnit.ChannelNumber = channelColumn == -1 ? string.Empty : fields[channelColumn];
                    newUnit.DimmerNumberText = dimmerColumn == -1 ? string.Empty : fields[dimmerColumn];
                    newUnit.InstrumentName = instrumentNameColumn == -1 ? string.Empty : fields[instrumentNameColumn];
                    newUnit.MulticoreName = multicoreNameColumn == -1 ? string.Empty : fields[multicoreNameColumn];
                    newUnit.Position = positionColumn == -1 ? string.Empty : fields[positionColumn];
                    newUnit.UserField1 = userField1Column == -1 ? string.Empty : fields[userField1Column];
                    newUnit.UserField2 = userField2Column == -1 ? string.Empty : fields[userField2Column];
                    newUnit.UserField3 = userField3Column == -1 ? string.Empty : fields[userField3Column];
                    newUnit.UserField4 = userField4Column == -1 ? string.Empty : fields[userField4Column];

                    // Application running data.
                    newUnit.ImportIndex = index;

                    // Import Format specific Data.
                    if (importSettings.DimmerImportFormat == ImportFormat.Format2)
                    {
                        newUnit.DMXAddressText = fields[DMXaddressColumn];
                    }

                    // Parse Unit Data.
                    newUnit.ParseUnitData(this, importSettings);

                    index++;
                }
            }

            file.Close();
        }

        private string[] PeekHeaders()
        {
            // Create new CSV object Pointed to File Location.
            TextFieldParser file = CreateTextFieldParser();
            file.SetDelimiters(",");


            // Read the First line to Collect the Cells.
            string[] headers = file.ReadFields();

            // Close the File to return Cursor to Top.
            file.Close();

            return headers;
        }

        private TextFieldParser CreateTextFieldParser()
        {
            if (FilePath != null)
            {
                Console.WriteLine("Loading from User Selected File");
                TextFieldParser file = new TextFieldParser(FilePath);
                return file;
            }

            else if (Environment.MachineName == "CHARLIESAMSUNG")
            {
                Console.WriteLine("Loading from Hardcoded File Path");
                TextFieldParser file = new TextFieldParser(@"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\General Test Data.csv");
                return file;
            }

            else if (Environment.MachineName == "CHARLIE-METABOX")
            {
                Console.WriteLine("Loading from Hardcoded File Path");
                TextFieldParser file = new TextFieldParser(@"C:\Users\Charlie\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\General Test Data.csv");
                return file;
            }

            else
            {
                Console.WriteLine("Unrecognized Computer: Please add a Condition for this Computer Name, and a Filepath to FileImport.cs");
                return null;
            }
        }

        #endregion
    }

}
