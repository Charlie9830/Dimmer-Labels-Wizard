using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dimmer_Labels_Wizard
{
    public class LabelCell
    {
        // Properties
        public DimmerDistroUnit PreviousReference { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

        protected SolidColorBrush _textColor;
        protected SolidColorBrush _backgroundColor;

        public SolidColorBrush TextColor
        {
            get
            {
                return _textColor;
            }

            protected set     // Set By BackgroundColor Setter
            {
                _textColor = value;
            }
        }
        
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }

            set
            {    
                // Calculate Luminance of Color and set _textColor to White or Black based on this luminance result.
                if ((0.299 * value.Color.R) + (0.587 * value.Color.G) + (0.114 * value.Color.B) > 128)
                {
                    _textColor = new SolidColorBrush(Colors.Black);
                }

                else
                {
                    _textColor = new SolidColorBrush(Colors.White);
                }

                _backgroundColor = value;
                
            }
        }
    }
}
