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
        public string TopData { get; set; }
        public Font TopFont { get; set; }
        public StringFormat TopFormat { get; set; }

        // Middle Line Properties
        public string MiddleData { get; set; }
        public Font MiddleFont { get; set; }
        public StringFormat MiddleFormat { get; set; }

        //Bottom line Properties
        public string BottomData { get; set; }
        public Font BottomFont { get; set; }
        public StringFormat BottomFormat { get; set; }

        public bool IsSelected { get; set; }
    }
}
