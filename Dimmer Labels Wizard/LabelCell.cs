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
        public float height { get; set; }
        public float width { get; set; }

        protected SolidBrush text_color_field;
        protected SolidBrush back_color_field;

        public SolidBrush text_color
        {
            get
            {
                return text_color_field;
            }

            private set     // Set By Back_colour
            {
                text_color_field = value;
            }
        }
        
        public SolidBrush back_color
        {
            get
            {
                return back_color_field;
            }

            set
            {

                back_color_field = value;
                // Calculate Luminance of Color and set fore_colour to White or Black based on this luminance result.
                if ((0.299 * value.Color.R) + (0.587 * value.Color.G) + (0.114 * value.Color.B) > 128)
                {
                    text_color_field = new SolidBrush(Color.Black);
                }

                else
                {
                    text_color_field = new SolidBrush(Color.White);
                }

                
            }
        }

        

    }
}
