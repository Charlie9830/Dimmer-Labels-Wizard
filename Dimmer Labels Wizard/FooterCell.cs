using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
namespace Dimmer_Labels_Wizard
{
    public class FooterCell : LabelCell
    {
        // Top Line Properties
        public string TopData { get; set; }
        public Typeface TopFont { get; set; }
        public double TopFontSize { get; set; }

        // Middle Line Properties
        public string MiddleData { get; set; }
        public Typeface MiddleFont { get; set; }
        public double MiddleFontSize { get; set; }
        //Bottom line Properties
        public string BottomData { get; set; }
        public Typeface BottomFont { get; set; }
        public double BottomFontSize { get; set; }
    }
}
