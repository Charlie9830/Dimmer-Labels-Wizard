using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class Globals
    {
        // List to hold DimDistroUnit Objects
        public static List<DimDistroUnit> DimDistroUnits = new List<DimDistroUnit>();
        public static SortOrder DimDistroSortOrder { get; set; }

        // List to Hold LabelStrip Objects
        public static List<RackLabel> RackLabels = new List<RackLabel>();

        // List to Hold DimDistroUnits that have had their Cabinet/Rack Numbers Sucsefully resolved.
        public static List<DimDistroUnit> ResolvedCabinetRacks = new List<DimDistroUnit>();

        // List to Hold DimDistroUnits that had failed to have their Cabinet/Rack Numbers Resolved.
        public static List<DimDistroUnit> UnresolvedCabinetRacks = new List<DimDistroUnit>();

        // Represents DMX Addresses.
        public struct DMX
        {
            public int universe;
            public int channel;
        }
    }
}
