using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class FooterCell : LabelCell
    {
        // Top Line Properties
        public string top_data { get; set; }

        public bool top_isBold { get; set; }
        public bool top_isItalics { get; set; }
        public bool top_isUnderline { get; set; }

        public string top_font { get; set; }
        public int top_size { get; set; }
        public string top_justification { get; set; }


        //Bottom line Properties
        public string bot_data { get; set; }

        public bool bot_isBold { get; set; }
        public bool bot_isItalics { get; set; }
        public bool bot_isUnderline { get; set; }

        public string bot_font { get; set; }
        public int bot_size { get; set; }
        public string bot_justification { get; set; }
    }
}
