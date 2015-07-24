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
        protected double _FontSize;

        public string Data { get; set; }
        public Typeface Font { get; set; }
        public double FontSize 
        {
            get
            {
                return _FontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _FontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }
    }

    public class HeaderCellWrapper
    {
        // Wraps a List of FooterCells so they can be Tagged to outlines during Rendering.
        public List<HeaderCell> Cells = new List<HeaderCell>();
    }
}
