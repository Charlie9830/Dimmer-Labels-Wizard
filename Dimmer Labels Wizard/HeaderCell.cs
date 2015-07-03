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
    public class HeaderCell : LabelCell
    {
        public string Data { get; set; }
        public Typeface Font { get; set; }
        public double FontSize { get; set; }
    }
}
