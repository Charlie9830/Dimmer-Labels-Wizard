using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class DistroRack
    {
        public int RackNumber { get; set; }

        public int StartingAddress { get; set; }
        public int EndingAddress { get; set; }

    }

    public class DimmerRack
    {
        public int RackNumber { get; set; }

        public bool Is5k { get; set; }

        public Globals.DMX StartingAddress { get; set; }
        public Globals.DMX EndingAddress { get; set; }
    }
}
