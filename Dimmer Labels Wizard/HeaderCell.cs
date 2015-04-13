using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dimmer_Labels_Wizard
{
    public class HeaderCell : LabelCell
    {
        public string data { get; set; }
        public Font font { get; set; }
        public StringFormat format { get; set; }
    }
}
