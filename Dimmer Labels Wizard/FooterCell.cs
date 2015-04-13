using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dimmer_Labels_Wizard
{
    public class FooterCell : LabelCell
    {
        // Top Line Properties
        public string top_data { get; set; }
        public Font top_font { get; set; }
        public StringFormat top_format { get; set; }


        //Bottom line Properties
        public string bot_data { get; set; }
        public Font bot_font { get; set; }
        public StringFormat bot_format { get; set; }

       
    }
}
