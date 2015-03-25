using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class DimDistroUnit
    {
        // Imported Data
        public string channel_number { get; set; }
        public string dimmer_number { get; set; }
        public string instrument_type { get; set; }
        public string multicore_name { get; set; }
        public string cabinet_number { get; set; }

        // Application running data
        public int global_id { get; set; }

    }

    

        
    
}
