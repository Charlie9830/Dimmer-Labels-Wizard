using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DimmerRange
    {
        public int Universe { get; set; }
        public int FirstDimmerNumber { get; set; }
        public int LastDimmerNumber { get; set; }
    }

    public class DistroRange
    {
        public int FirstDimmerNumber { get; set; }
        public int LastDimmerNumber { get; set; }
    }
}
