using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard
{
    public class FooterCell : LabelCell
    {
        // Top Line Properties
        public string TopData { get; set; }
        public FontFamily TopFont { get; set; }

        // Middle Line Properties
        public string MiddleData { get; set; }
        public FontFamily MiddleFont { get; set; }

        //Bottom line Properties
        public string BottomData { get; set; }
        public FontFamily BottomFont { get; set; }
    }
}
