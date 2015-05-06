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
        // Currently not Used. Although Multiple Word Strings Split into Top and Bottom Data During Render.
        public string TopData { get; set; }
        public Font TopFont { get; set; }
        public StringFormat TopFormat { get; set; }

        public string MiddleData { get; set; }
        public Font MiddleFont { get; set; }
        public StringFormat MiddleFormat { get; set; }

        // Currently not Used. Although Multiple Word Strings Split into Top and Bottom Data During Render.
        public string BottomData { get; set; }
        public Font BottomFont { get; set; }
        public StringFormat BottomFormat { get; set; }
    }
}
