using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class Globals
    {
        // Debug Mode
        public static bool DebugActive = false;

        // List to hold DimDistroUnit Objects
        public static List<DimmerDistroUnit> DimmerDistroUnits = new List<DimmerDistroUnit>();
        public static SortOrder DimmerDistroSortOrder { get; set; }

        // List to Hold LabelStrip Objects
        public static List<LabelStrip> LabelStrips = new List<LabelStrip>();

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
                return null;
            }
        }

        // List to Hold Imported Elements with Dimmer Data that does not Match Formatting Styles.
        public static List<DimmerDistroUnit> UnresolvableUnits = new List<DimmerDistroUnit>();

        // List to hold Imported Elements that passed both Dimmer and Distro String Verifcation,
        // and reside in the same Range.
        public static List<DimmerDistroUnit> ClashingRangeData = new List<DimmerDistroUnit>();

        // Represents DMX Addresses.
        [Serializable()]
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

