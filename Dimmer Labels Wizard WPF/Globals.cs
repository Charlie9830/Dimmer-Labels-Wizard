using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class Globals
    {
        static Globals()
        {
            // Construct the Default LabelStrip Template.

            // Upper Strip is in Single Field Mode.
            // Lower Strip Cell Row Templates.
            var lowerTopRow = new CellRowTemplate()
            {
                DataField = LabelField.MulticoreName,
                Font = new Typeface("Arial"),
                DesiredFontSize = 12d,
            };

            var lowerMiddleRow = new CellRowTemplate()
            {
                DataField = LabelField.ChannelNumber,
                Font = new Typeface("Arial"),
                DesiredFontSize = 16d,
            };

            var lowerBottomRow = new CellRowTemplate()
            {
                DataField = LabelField.InstrumentName,
                Font = new Typeface("Arial"),
                DesiredFontSize = 12d,
            };

            // Upper Cell Template.
            var upperCellTemplate = new LabelCellTemplate()
            {
                CellDataMode = CellDataMode.SingleField,
                SingleFieldDataField = LabelField.Position,
                SingleFieldDesiredFontSize = 16d,
                SingleFieldFont = new Typeface("Arial"),
            };

            // Generate List of CellRowTemplates for construction of lowerCellTemplate.
            var lowerCellRowTemplates = new List<CellRowTemplate>();
            lowerCellRowTemplates.Add(lowerTopRow);
            lowerCellRowTemplates.Add(lowerMiddleRow);
            lowerCellRowTemplates.Add(lowerBottomRow);

            var lowerCellTemplate = new LabelCellTemplate()
            {
                CellDataMode = CellDataMode.MixedField,
                RowHeightMode = CellRowHeightMode.Automatic,
                CellRowTemplates = lowerCellRowTemplates,
            };

            // Construct Lists of UpperCell and LowerCell Templates before construction of LabelStrip Template.
            var upperCellTemplates = new List<LabelCellTemplate>();
            for (int count = 1; count <= 12; count++)
            {
                upperCellTemplates.Add(upperCellTemplate);
            }

            var lowerCellTemplates = new List<LabelCellTemplate>();
            for (int count = 1; count <= 12; count++)
            {
                lowerCellTemplates.Add(lowerCellTemplate);
            }

            // Generate LabelStripTemplate
            var labelStripTemplate = new LabelStripTemplate()
            {
                Name = "Default",
                StripMode = LabelStripMode.Dual,
                StripHeight = 70d,
                UpperCellTemplates = upperCellTemplates,
                LowerCellTemplates = lowerCellTemplates,
            };

            Templates.Add(labelStripTemplate);
            DefaultTemplate = labelStripTemplate;
        }

        // Debug Mode
        public static bool DebugActive = false;

        // List to hold DimDistroUnit Objects
        public static List<DimmerDistroUnit> DimmerDistroUnits = new List<DimmerDistroUnit>();

        public static SortOrder DimmerDistroSortOrder { get; set; }

        // List to Hold StripData Objects.
        public static ObservableCollection<Strip> Strips = new ObservableCollection<Strip>();

        public static CellRowTemplate BaseCellRowTemplate = new CellRowTemplate()
        {
            Font = new Typeface("Arial"),
            DesiredFontSize = 12d,
            DataField = LabelField.NoAssignment,
        };

        public static LabelCellTemplate BaseLabelCellTemplate = new LabelCellTemplate()
        {
            CellDataMode = CellDataMode.MixedField,
            SingleFieldFont = new Typeface("Arial"),
            SingleFieldDesiredFontSize = 12,
            SingleFieldDataField = LabelField.NoAssignment,
            RowHeightMode = CellRowHeightMode.Static,
            LeftWeight = 1d,
            TopWeight = 1d,
            RightWeight = 1d,
            BottomWeight = 1d,
            CellRowTemplates = new List<CellRowTemplate>() as IEnumerable<CellRowTemplate>,
        };

        // Not to be confused with the Default Strip Template. The Base LabelStripTemplates only exist to gaurantee
        // that the Property Accsessors of instances of LabelStripTemplate, LabelCellTemplate and CellRowTemplate will also
        // find a value. Otherwise they would return null, which is indistiguishable from a property value.
        public static LabelStripTemplate BaseLabelStripTemplate = new LabelStripTemplate()
        {
            StripHeight = 70d,
            StripWidth = 70d * 12,
            StripMode = LabelStripMode.Dual,
            UpperCellTemplates = new List<LabelCellTemplate>() as IEnumerable<LabelCellTemplate>,
            LowerCellTemplates = new List<LabelCellTemplate>() as IEnumerable<LabelCellTemplate>,
            Name = "Base Template"
        };

        // By Defualt, this is contained within the Templates Collection.
        public static LabelStripTemplate DefaultTemplate;

        public static ObservableCollection<LabelStripTemplate> Templates = 
            new ObservableCollection<LabelStripTemplate>();

        // Color Dictionary
        public static Dictionary<DimmerDistroUnit, SolidColorBrush> LabelColors = 
            new Dictionary<DimmerDistroUnit, SolidColorBrush>();

        public static SolidColorBrush GetLabelColor(DimmerDistroUnit unit)
        {
            SolidColorBrush outValue;
            if (Globals.LabelColors.TryGetValue(unit, out outValue) == true)
            {
                return outValue;
            }

            else
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        // List to Hold Imported Elements with Dimmer Data that does not Match Formatting Styles.
        public static List<DimmerDistroUnit> UnresolvableUnits = new List<DimmerDistroUnit>();

        // List to hold Imported Elements that passed both Dimmer and Distro String Verifcation,
        // and reside in the same Range.
        public static List<DimmerDistroUnit> ClashingRangeData = new List<DimmerDistroUnit>();

        // Represents DMX Addresses.
        
        public struct DMX
        {
            public int Universe;
            public int Channel;
        }

        public struct BoolString
        {
            public BoolString (bool sanityCheck, string errorMessage)
            {
                SanityCheck = sanityCheck;
                ErrorMessage = errorMessage;
            }

            public bool SanityCheck;
            public string ErrorMessage;
        }
    }
}

