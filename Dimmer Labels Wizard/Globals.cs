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
        public static List<DimmerDistroUnit> DimmerDistroUnits = new List<DimmerDistroUnit>();
        public static SortOrder DimmerDistroSortOrder { get; set; }

        // List to Hold LabelStrip Objects
        public static List<LabelStrip> LabelStrips = new List<LabelStrip>();

        // List to Hold DimDistroUnits that have had their Cabinet/Rack Numbers Sucsefully resolved.
        public static List<DimmerDistroUnit> ResolvedCabinetRackNumbers = new List<DimmerDistroUnit>();

        // List to Hold DimDistroUnits that had failed to have their Cabinet/Rack Numbers Resolved.
        public static List<DimmerDistroUnit> UnresolvedCabinetRackNumbers = new List<DimmerDistroUnit>();

        // Represents DMX Addresses.
        public struct DMX
        {
            public int Universe;
            public int Channel;
        }
    }
}
