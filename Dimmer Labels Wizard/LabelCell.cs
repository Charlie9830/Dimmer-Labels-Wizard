using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dimmer_Labels_Wizard
{
    public class LabelCell
    {
        // Properties
        public DimmerDistroUnit PreviousReference { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

        protected SolidBrush _textColor;
        protected SolidBrush _backgroundColor;

        public SolidBrush TextColor
        {
            get
            {
                return _textColor;
            }

            private set     // Set By BackgroundColor Setter
            {
                _textColor = value;
            }
        }
        
        public SolidBrush BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }

            set
            {

                _backgroundColor = value;

                // Calculate Luminance of Color and set _textColor to White or Black based on this luminance result.
                if ((0.299 * value.Color.R) + (0.587 * value.Color.G) + (0.114 * value.Color.B) > 128)
                {
                    _textColor = new SolidBrush(Color.Black);
                }

                else
                {
                    _textColor = new SolidBrush(Color.White);
                }

                
            }
        }

        

    }
}
