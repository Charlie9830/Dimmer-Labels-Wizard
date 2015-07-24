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
        protected double _TopFontSize;

        public string TopData { get; set; }
        public Typeface TopFont { get; set; }
        public double TopFontSize
        {
            get
            {
                return _TopFontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _TopFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }

        // Middle Line Properties
        protected double _MiddleFontSize;

        public string MiddleData { get; set; }
        public Typeface MiddleFont { get; set; }
        public double MiddleFontSize
        {
            get
            {
                return _MiddleFontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _MiddleFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }

        //Bottom line Properties
        protected double _BottomFontSize;

        public string BottomData { get; set; }
        public Typeface BottomFont { get; set; }
        public double BottomFontSize
        {
            get
            {
                return _BottomFontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _BottomFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }
    }
}
