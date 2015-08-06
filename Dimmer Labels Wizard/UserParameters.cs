using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard
{
    public static class UserParameters
    {
        public static ImportFormatting DimmerImportFormat { get; set; }
        public static ImportFormatting DMXAddressImportFormat { get; set; }

        public static ImportFormatting DistroImportFormat { get; set; }
        public static string DistroNumberPrefix { get; set; }

        public static int ChannelNumberColumnIndex { get; set; }
        public static int DimmerNumberColumnIndex { get; set; }
        public static int InstrumentTypeColumnIndex { get; set; }
        public static int MulticoreNameColumnIndex { get; set; }
        public static int PositionColumnIndex { get; set; }
        public static int UniverseDataColumnIndex { get; set; }
        public static int UserField1ColumnIndex { get; set; }
        public static int UserField2ColumnIndex { get; set; }
        public static int UserField3ColumnIndex { get; set; }
        public static int UserField4ColumnIndex { get; set; }

        public static bool CreateDistroObjects { get; set; }
        public static bool CreateDimmerObjects { get; set; }

        public static int StartDistroNumber { get; set; }
        public static int EndDistroNumber { get; set; }

        // Gets Sorted by FORM_UserParamaterEntry.UpdateUserParameters().
        public static List<Globals.DimmerRange> DimmerRanges = new List<Globals.DimmerRange>();

        public static List<DistroRack> DistroRacks = new List<DistroRack>();
        public static List<DimmerRack> DimmerRacks = new List<DimmerRack>();

        public static double DimmerLabelWidthInMM { get; set; } 
        public static double DimmerLabelHeightInMM { get; set; }

        public static double DistroLabelWidthInMM { get; set; }
        public static double DistroLabelHeightInMM { get; set; }

        // Label Type
        public static bool SingleLabel = false;
        public static bool HeaderBackGroundColourOnly = false;

        // Label Field Assignments
        public static LabelField HeaderField { get; set; }
        public static LabelField FooterTopField { get; set; }
        public static LabelField FooterMiddleField { get; set; }
        public static LabelField FooterBottomField { get; set; }

        // Create Hardcoded Distro Start Address Values
        public static void GenerateDistroRange()
        {
            // Distro Starting Address. NOT the Very Last Distro Channel.
            int firstDistroAddress = StartDistroNumber;
            int lastDistroAddress = EndDistroNumber - 12;

            int distroRackNumberCounter = 1;
            int distroOutputIndex = 0;
            int distroRackCounter = 12;

            for (int channel = firstDistroAddress; channel <= lastDistroAddress + 12; channel++)
            {
                if (distroRackCounter == 12)
                {
                    DistroRacks.Insert(distroOutputIndex, new DistroRack());
                    DistroRacks[distroOutputIndex].StartingAddress = channel;
                    DistroRacks[distroOutputIndex].EndingAddress = channel + 11;
                    DistroRacks[distroOutputIndex].RackNumber = distroRackNumberCounter;

                    distroRackNumberCounter++;
                    distroOutputIndex++;
                    distroRackCounter = 1;
                }

                else
                {
                    distroRackCounter++;
                }
            }

            int dimmerOutputIndex = 0;
            int dimmerRackCounter = 12;
            int dimmerRackNumberCounter = 1;

            foreach (var element in UserParameters.DimmerRanges)
            {
                // Dimmer Starting Addresses. NOT the Very Last Distro Channel
                int dimmerUniverse = element.Universe;
                int firstDimmerAddress = element.FirstChannel;
                int lastDimmerAddress = element.LastChannel - 12;


                for (int channel = firstDimmerAddress; channel <= lastDimmerAddress + 12; channel++)
                {
                    if (dimmerRackCounter == 12)
                    {
                        DimmerRacks.Insert(dimmerOutputIndex, new DimmerRack());

                        Globals.DMX startAddress = new Globals.DMX();
                        startAddress.Channel = channel;
                        startAddress.Universe = dimmerUniverse;
                        Globals.DMX endAddress = new Globals.DMX();
                        endAddress.Channel = channel + 11;
                        endAddress.Universe = dimmerUniverse;

                        DimmerRacks[dimmerOutputIndex].StartingAddress = startAddress;
                        DimmerRacks[dimmerOutputIndex].EndingAddress = endAddress;
                        DimmerRacks[dimmerOutputIndex].RackNumber = dimmerRackNumberCounter;

                        dimmerRackCounter = 1;
                        dimmerOutputIndex++;
                        dimmerRackNumberCounter++;
                    }

                    else
                    {
                        dimmerRackCounter++;
                    }
                }

                dimmerRackCounter = 12;
            }
        }

        public static void SetDefaultRackLabelSettings()
        {

            foreach (var element in Globals.LabelStrips)
            {
                element.LineWeight = 1.25d;

                // Set Default Fonts,FontStyles and StringFormat Alignments.
                for (int listIndex = 0; listIndex < element.Headers.Count; listIndex++)
                {
                    if (SingleLabel == true)
                    {
                        element.Headers[listIndex].Font = new Typeface("Arial");
                        element.Headers[listIndex].FontSize = 24;
                    }

                    else
                    {
  
                        element.Headers[listIndex].Font = new Typeface("Arial");
                        element.Headers[listIndex].FontSize = 24;
                    }

                    element.Footers[listIndex].TopFont = new Typeface("Arial");
                    element.Footers[listIndex].TopFontSize = 12;

                    element.Footers[listIndex].MiddleFont = new Typeface("Arial");
                    element.Footers[listIndex].MiddleFontSize = 16;

                    element.Footers[listIndex].BottomFont = new Typeface("Arial");
                    element.Footers[listIndex].BottomFontSize = 14;
                }
            }
        }

        #region Serialization
        public static UserParametersStorage GenerateStorage()
        {
            UserParametersStorage storage = new UserParametersStorage();

            storage.DimmerImportFormat = DimmerImportFormat;
            storage.DMXAddressImportFormat = DMXAddressImportFormat;

            storage.DistroImportFormat = DistroImportFormat;
            storage.DistroNumberPrefix = DistroNumberPrefix;

            storage.ChannelNumberColumnIndex = ChannelNumberColumnIndex;
            storage.DimmerNumberColumnIndex = DimmerNumberColumnIndex;
            storage.InstrumentTypeColumnIndex = InstrumentTypeColumnIndex;
            storage.MulticoreNameColumnIndex = MulticoreNameColumnIndex;
            storage.PositionColumnIndex = PositionColumnIndex;
            storage.UniverseDataColumnIndex = UniverseDataColumnIndex;
            storage.UserField1ColumnIndex = UserField1ColumnIndex;
            storage.UserField2ColumnIndex = UserField2ColumnIndex;
            storage.UserField3ColumnIndex = UserField3ColumnIndex;
            storage.UserField4ColumnIndex = UserField4ColumnIndex;

            storage.CreateDistroObjects = CreateDistroObjects;
            storage.CreateDimmerObjects = CreateDimmerObjects;

            storage.StartDistroNumber = StartDistroNumber;
            storage.EndDistroNumber = EndDistroNumber;

            storage.DimmerRanges.AddRange(DimmerRanges);

            storage.DistroRacks.AddRange(DistroRacks);
            storage.DimmerRacks.AddRange(DimmerRacks);

            storage.DimmerLabelWidthInMM = DimmerLabelWidthInMM;
            storage.DimmerLabelHeightInMM = DimmerLabelHeightInMM;

            storage.DistroLabelWidthInMM = DistroLabelWidthInMM;
            storage.DistroLabelHeightInMM = DistroLabelHeightInMM;

            storage.SingleLabel = SingleLabel;
            storage.HeaderBackGroundColourOnly = HeaderBackGroundColourOnly;
                    
            storage.HeaderField = HeaderField;
            storage.FooterTopField = FooterTopField;
            storage.FooterMiddleField = FooterMiddleField;
            storage.FooterBottomField = FooterBottomField;

            return storage;
    }

        public static void Rebuild(UserParametersStorage storage)
        {
            DimmerImportFormat = storage.DimmerImportFormat;
            DMXAddressImportFormat = storage.DMXAddressImportFormat;

            DistroImportFormat = storage.DistroImportFormat;
            DistroNumberPrefix = storage.DistroNumberPrefix;

            ChannelNumberColumnIndex = storage.ChannelNumberColumnIndex;
            DimmerNumberColumnIndex = storage.DimmerNumberColumnIndex;
            InstrumentTypeColumnIndex = storage.InstrumentTypeColumnIndex;
            MulticoreNameColumnIndex = storage.MulticoreNameColumnIndex;
            PositionColumnIndex = storage.PositionColumnIndex;
            UniverseDataColumnIndex = storage.UniverseDataColumnIndex;

            UserField1ColumnIndex = storage.UserField1ColumnIndex;
            UserField2ColumnIndex = storage.UserField2ColumnIndex;
            UserField3ColumnIndex = storage.UserField3ColumnIndex;
            UserField4ColumnIndex = storage.UserField4ColumnIndex;

            CreateDistroObjects = storage.CreateDistroObjects;
            CreateDimmerObjects = storage.CreateDimmerObjects;

            StartDistroNumber = storage.StartDistroNumber;
            EndDistroNumber = storage.EndDistroNumber;

            DimmerRanges.AddRange(storage.DimmerRanges);

            DistroRacks.AddRange(storage.DistroRacks);
            DimmerRacks.AddRange(storage.DimmerRacks);

            DimmerLabelWidthInMM = storage.DimmerLabelWidthInMM;
            DimmerLabelHeightInMM = storage.DimmerLabelHeightInMM;

            DistroLabelWidthInMM = storage.DistroLabelWidthInMM;
            DistroLabelHeightInMM = storage.DistroLabelHeightInMM;

            SingleLabel = storage.SingleLabel;
            HeaderBackGroundColourOnly = storage.HeaderBackGroundColourOnly;

            HeaderField = storage.HeaderField;
            FooterTopField = storage.FooterTopField;
            FooterMiddleField = storage.FooterMiddleField;
            FooterBottomField = storage.FooterBottomField;
        }

        #endregion
    }

    [Serializable()]
    public class UserParametersStorage
    {
        public ImportFormatting DimmerImportFormat;
        public ImportFormatting DMXAddressImportFormat;

        public ImportFormatting DistroImportFormat;
        public string DistroNumberPrefix;

        public int ChannelNumberColumnIndex;
        public int DimmerNumberColumnIndex;
        public int InstrumentTypeColumnIndex;
        public int MulticoreNameColumnIndex;
        public int PositionColumnIndex;
        public int UniverseDataColumnIndex;
        public int UserField1ColumnIndex;
        public int UserField2ColumnIndex;
        public int UserField3ColumnIndex;
        public int UserField4ColumnIndex;

        public bool CreateDistroObjects;
        public bool CreateDimmerObjects;

        public int StartDistroNumber;
        public int EndDistroNumber;

        public List<Globals.DimmerRange> DimmerRanges = new List<Globals.DimmerRange>();

        public List<DistroRack> DistroRacks = new List<DistroRack>();
        public List<DimmerRack> DimmerRacks = new List<DimmerRack>();

        public double DimmerLabelWidthInMM;
        public double DimmerLabelHeightInMM;

        public double DistroLabelWidthInMM;
        public double DistroLabelHeightInMM;

        public bool SingleLabel;
        public bool HeaderBackGroundColourOnly;

        public LabelField HeaderField;
        public LabelField FooterTopField;
        public LabelField FooterMiddleField;
        public LabelField FooterBottomField;
    }

}
