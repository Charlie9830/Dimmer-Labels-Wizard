using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Windows;
using Dimmer_Labels_Wizard_WPF.Repositories;

namespace Dimmer_Labels_Wizard_WPF
{
    public class UnitImporter
    {
        public UnitImporter(string filePath, ImportConfiguration importConfiguration)
        {
            FilePath = filePath;
            _ImportConfiguration = importConfiguration;
        }


        #region Fields
        // Main Working List.
        public List<DimmerDistroUnit> Units = new List<DimmerDistroUnit>();

        protected string FilePath = string.Empty;

        protected ImportConfiguration _ImportConfiguration;
        #endregion

        #region Properties.
        public ImportConfiguration ImportConfiguration
        {
            get
            {
                return _ImportConfiguration;
            }
        }

        public IEnumerable<DimmerDistroUnit> ConflictingUnits
        {
            get
            {
                return from unit in Units
                       where unit.RackUnitType == RackType.ConflictingRange
                       select unit;
            }
        }

        public IEnumerable<DimmerDistroUnit> UnResolveableUnits
        {
            get
            {
                return from unit in Units
                       where unit.RackUnitType == RackType.Unparseable
                       select unit;
            }
        }

        public bool AllUnitsValid
        {
            get
            {
                return ConflictingUnits.Count() == 0 &&
                    UnResolveableUnits.Count() == 0;
            }
        }
        #endregion

        #region Public Methods.
        public void CommitToDatabase(UnitRepository unitRepo, UnitImportMergeType mergeType)
        {
            if (mergeType == UnitImportMergeType.BlindMerge)
            {
                foreach (var newUnit in Units)
                {
                    var existingUnit = unitRepo.GetUnit(newUnit.RackUnitType, newUnit.UniverseNumber, newUnit.DimmerNumber);

                    if (existingUnit == null)
                    {
                        // No existing Unit.
                        unitRepo.InsertUnit(newUnit);
                    }

                    else
                    {
                        // Replace the Unit.
                        unitRepo.ReplaceUnit(existingUnit, newUnit);
                    }
                }
            }

            if (mergeType == UnitImportMergeType.PreserveShortNames)
            {
                foreach (var newUnit in Units)
                {
                    var oldUnit = unitRepo.GetUnit(newUnit.RackUnitType, newUnit.UniverseNumber, newUnit.DimmerNumber);

                    if (oldUnit == null)
                    {
                        // No existing Unit.
                        unitRepo.InsertUnit(newUnit);
                    }

                    else
                    {
                        // Walk the Units Properties of the Existing and New Units, if LastImported Data matches the new Unit
                        // carry the oldUnit's Short Name Over to the new Unit.

                        // Channel Number
                        if (oldUnit.LastImportedChannelNumber == newUnit.LastImportedChannelNumber)
                        {
                            newUnit.ChannelNumber = oldUnit.ChannelNumber;
                        }

                        // Instrument Name
                        if (oldUnit.LastImportedInstrumentName == newUnit.LastImportedInstrumentName)
                        {
                            newUnit.InstrumentName = oldUnit.InstrumentName;
                        }

                        // MulticoreName
                        if (oldUnit.LastImportedMulticoreName == newUnit.LastImportedMulticoreName)
                        {
                            newUnit.MulticoreName = oldUnit.MulticoreName;
                        }

                        // Position.
                        if (oldUnit.LastImportedPosition == newUnit.LastImportedPosition)
                        {
                            newUnit.Position = oldUnit.Position;
                        }

                        // User Field 1.
                        if (oldUnit.LastImportedUserField1 == newUnit.LastImportedUserField1)
                        {
                            newUnit.UserField1 = oldUnit.UserField1;
                        }

                        // User Field 2.
                        if (oldUnit.LastImportedUserField2 == newUnit.LastImportedUserField2)
                        {
                            newUnit.UserField2 = oldUnit.UserField2;
                        }

                        // User Field 3.
                        if (oldUnit.LastImportedUserField3 == newUnit.LastImportedUserField3)
                        {
                            newUnit.UserField3 = oldUnit.UserField3;
                        }

                        // User Field 4.
                        if (oldUnit.LastImportedUserField4 == newUnit.LastImportedUserField4)
                        {
                            newUnit.UserField4 = oldUnit.UserField4;
                        }

                        // Replace the Unit in the DB.
                        unitRepo.ReplaceUnit(oldUnit, newUnit);
                    }
                }
            }

            if (mergeType == UnitImportMergeType.Overwrite)
            {
                // Overwrite Data.
                unitRepo.RemoveAllUnits();
                unitRepo.Save();

                unitRepo.InsertUnitRange(Units);
            }

            // Save.
            unitRepo.Save();
        }

        public void ImportData()
        {
            // Import Data from CSV.
            Import(_ImportConfiguration);
        }

        public void SanitizeData()
        {
            if (AllUnitsValid != true)
            {
                throw new NotSupportedException("Data must be Valid before it can be Sanitized");
            }
            // Remove Units that aren't within the Label Range.
            CullOutOfRangeUnits();

            // Resolve PiggyBacks.
            ResolvePiggybacks();

            // Resolve Missing Dimmer Numbers.
            ResolveMissingDimmerNumbers();

            // Sort.
            Units.Sort();
        }

        public void RetryValidation()
        {
            // Remove any units marked to be Ommited.
            var query = from unit in Units
                        where unit.OmitUnit == true
                        select unit;

            foreach (var element in query.ToList())
            {
                Units.Remove(element);
            }

            // Attempt to Validate UnResolveable Collection.
            foreach (var element in UnResolveableUnits.ToList())
            {
                element.ParseUnitData(this, _ImportConfiguration);
            }
            
            // Attempt to Validate Conflicting Units Collection.
            foreach (var element in ConflictingUnits.ToList())
            {
                element.ParseUnitData(this, _ImportConfiguration);
            }
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

        protected void ResolveMissingDimmerNumbers()
        {
            // Dimmers.
            if (_ImportConfiguration.DimmerRanges.Count != 0)
            {
                // Compare the data to the Ranges Provided. If a unit Should exist, as dictated by the Range,
                // add it to a seperate list, to be merged with the working lists after the Query is executed (Avoids
                // Enumerator throwing an Exception).
                var pendingDimmers = new List<DimmerDistroUnit>();

                foreach (var range in _ImportConfiguration.DimmerRanges)
                {
                    // Get a collection of existing Dimmer Numbers that fall within the current Range.
                    var existingDimmerNumbers = from unit in Units
                                where unit.RackUnitType == RackType.Dimmer
                                where unit.UniverseNumber == range.Universe &&
                                unit.DimmerNumber >= range.FirstDimmerNumber &&
                                unit.DimmerNumber <= range.LastDimmerNumber
                                select unit.DimmerNumber;

                    // Compare with a consequtive range of Integers.
                    var missingDimmerNumbers = range.Range.Except(existingDimmerNumbers);

                    // Create a new DimmerDistroUnit representing the Missing Number,
                    // add it to the pendingUnits list.
                    foreach (var dimmerNumber in missingDimmerNumbers)
                    {
                        pendingDimmers.Add(new DimmerDistroUnit()
                        {
                            UniverseNumber = range.Universe,
                            DimmerNumber = dimmerNumber,
                            RackUnitType = RackType.Dimmer
                        });
                    }
                }

                // Commit newly created Units to Main Units List.
                Units.AddRange(pendingDimmers);
            }

            // Distros.
            if (_ImportConfiguration.DistroRanges.Count != 0)
            {
                var pendingDistros = new List<DimmerDistroUnit>();

                foreach (var range in _ImportConfiguration.DistroRanges)
                {
                    // Get a collection of existing Dimmer Numbers that fall within the current Range.
                    var existingDimmerNumbers = from unit in Units
                                                where unit.RackUnitType == RackType.Distro
                                                where unit.DimmerNumber >= range.FirstDimmerNumber &&
                                                unit.DimmerNumber <= range.LastDimmerNumber
                                                select unit.DimmerNumber;

                    // Compare with a consequtive range of Integers.
                    var missingDimmerNumbers = range.Range.Except(existingDimmerNumbers);

                    // Create a new DimmerDistroUnit representing the Missing Number,
                    // add it to the pendingUnits list.
                    foreach (var dimmerNumber in missingDimmerNumbers)
                    {
                        pendingDistros.Add(new DimmerDistroUnit()
                        {
                            DimmerNumber = dimmerNumber,
                            RackUnitType = RackType.Distro
                        });
                    }
                }

                // Commit newly created Units to Main Units List.
                Units.AddRange(pendingDistros);
            }
        }

        protected void Import(ImportConfiguration importSettings)
        {
            // Create new CSV Object and Point it to the file location.
            TextFieldParser file = FileImport.CreateTextFieldParser(FilePath);

            file.SetDelimiters(",");

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

            // Clear Local Collection.
            Units.Clear();

            // Begin Importing Data.
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

                    // Copy Imported Data into LastImported Data Properties.
                    newUnit.CopyShortNamesToLastImportNames();

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
        #endregion
    }

}
