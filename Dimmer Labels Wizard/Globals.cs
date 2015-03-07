using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class Globals
    {
        // List to hold Header Cell Objects.
        public static List<HeaderCell> HeaderCells = new List<HeaderCell>();

        // List Cell to footer cell objects.
        public static List<FooterCell> FooterCells = new List<FooterCell>();

        // List to Hold a Dimmer Range to Sort Label Output.
        public static List<int> DimmerRanges = new List<int>();

    }
}
