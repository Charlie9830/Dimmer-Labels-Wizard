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
        public string Data { get; set; }
        public Font Font { get; set; }
        public StringFormat Format { get; set; }
    }
}
