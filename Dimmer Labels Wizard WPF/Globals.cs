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
            RowCount = 0,
            Width = 70d,
            RowHeightMode = CellRowHeightMode.Static,
            CellRowTemplates = new List<CellRowTemplate>() as IEnumerable<CellRowTemplate>,
        };

        public static LabelStripTemplate BaseLabelStripTemplate = new LabelStripTemplate()
        {
            StripHeight = 70d,
            StripMode = LabelStripMode.Dual,
            UpperCellsTemplate = BaseLabelCellTemplate,
            LowerCellsTemplate = BaseLabelCellTemplate,
            Name = "Base LabelStrip Template"
        };

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

