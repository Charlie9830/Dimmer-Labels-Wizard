using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public abstract class UnitRangeBase
    {

    }

    public class DimmerRange : UnitRangeBase
    {
        public int Universe { get; set; }
        public int FirstDimmerNumber { get; set; }
        public int LastDimmerNumber { get; set; }
    }

    public class DistroRange : UnitRangeBase
    {
        public int FirstDimmerNumber { get; set; }
        public int LastDimmerNumber { get; set; }
    }
}
