using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dimmer_Labels_Wizard
{
    public class LabelCell
    {
        // Properties
        public int strip_identifier { get; set; }
        public int col_index { get; set; }
        public int global_id { get; set; }

        public int height { get; set; }
        public int width { get; set; }

        public string back_colour { get; set; }
        public string fore_colour { get; set; }

    }
}
