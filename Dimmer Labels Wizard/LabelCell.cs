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

        public Pen fore_colour { get; private set; } // Can be Set only by the back_colour set acsessor.
        
        public SolidBrush back_colour
        {
            get
            {
                return back_colour;
            }

            set
            {
                back_colour = value;

                // Calculate Luminance of Color and set fore_colour to White or Black based on this luminance result.
                if ((0.299 * fore_colour.Color.R) + (0.587 * fore_colour.Color.G) + (0.114 * fore_colour.Color.B) > 128)
                {
                    fore_colour = new Pen(Color.Black);
                }

                else
                {
                    fore_colour = new Pen(Color.White);
                }
            }
        }

        

    }
}
