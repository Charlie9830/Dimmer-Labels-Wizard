using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dimmer_Labels_Wizard
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

        // List to Hold Imported Elements with Dimmer Data that does not Match Formatting Styles.
        public static List<DimmerDistroUnit> UnParseableData = new List<DimmerDistroUnit>();

        // Represents DMX Addresses.
        public struct DMX
        {
            public int Universe;
            public int Channel;
        }

        public struct DimmerRange : IComparable<DimmerRange>
        {
            public DimmerRange(int universe, int firstChannel, int lastChannel)
            {
                this.Universe = universe;
                this.FirstChannel = firstChannel;
                this.LastChannel = lastChannel;
            }

            public int Universe;
            public int FirstChannel;
            public int LastChannel;


            public int CompareTo(DimmerRange other)
            {
                if (Universe == other.Universe)
                {
                    return FirstChannel - other.FirstChannel;
                }
                return Universe - other.Universe;
            }
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
