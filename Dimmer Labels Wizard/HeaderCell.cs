using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public class HeaderCell : LabelCell
    {
        public string data { get; set; }

        public bool isBold { get; set; }
        public bool isItalics { get; set; }
        public int isUnderlined { get; set; }

        public string font { get; set; }
        public int size { get; set; }
        public string justification { get; set; }
    }
}
