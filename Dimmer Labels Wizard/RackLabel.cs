using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dimmer_Labels_Wizard
{
    public class RackLabel : IComparable<RackLabel>
    {
        // Properties.
        public List<HeaderCell> headers = new List<HeaderCell>();
        public List<FooterCell> footers = new List<FooterCell>();

        public int label_width { get; set; } // Width of each Cell in Pixels
        public int label_height { get; set; } // Height of each Cell in Pixels

        public int cabinet_number { get; set; } // Imported from DimDistroUnit for User selection of Labels
        public int rack_number { get; set; } // Imported from DimDistroUnit for User selection of Labels.

        // Methods.

        
        public void PrintToConsole()
        {
            int count = 0;
            for (int i = 0; i < headers.Count; i++)
            {
                Console.Write(headers[i].data);
                Console.Write(" | ");
                count++;
            }
            Console.Write("    ");
            Console.Write(count);
            count = 0;

            Console.WriteLine();

            for (int i = 0; i < footers.Count; i++)
            {
                Console.Write(footers[i].top_data);
                Console.Write(" | ");
                count++;
            }
            Console.Write("    ");
            Console.Write(count);
        }

        // Sets the Background Colour of Each Header and Footer Cell
        public void SetBackgroundColour(System.Drawing.Color DesiredColor)
        {
            foreach (var element in headers)
            {
                element.back_color = new SolidBrush(DesiredColor);
            }

            foreach (var element in footers)
            {
                element.back_color = new SolidBrush(DesiredColor);
            }
        }

        // Overload: Sets the Background Colour of Header and Footer Cells Matching a Multicore Name
        public void SetBackgroundColour(string MulticoreName, System.Drawing.Color DesiredColor)
        {
            for (int list_index = 0; list_index < 12; list_index++)
            {
                if (headers[list_index].data == MulticoreName)
                {
                    headers[list_index].back_color = new SolidBrush(DesiredColor);
                    footers[list_index].back_color = new SolidBrush(DesiredColor);
                }
            }
        }

        public void Render(System.Drawing.Graphics graphics, Point origin)
        {
            int default_label_width = this.label_width;
            int default_label_height = this.label_height;

            int units_in_strip = this.footers.Count;
            int right_bound_pos = 1;

            SolidBrush OutlineColour = new SolidBrush(Color.Black);

            Point HeaderOrigin = origin;
            Point HeaderPosition = new Point(HeaderOrigin.X, HeaderOrigin.Y);

            Point FooterOrigin = new Point(origin.X, default_label_height * 2); 
            Point FooterPosition = new Point(FooterOrigin.X, FooterOrigin.Y);

            Rectangle HeaderOutline = new Rectangle();
            Rectangle FooterOutline = new Rectangle();

            // Draw the Headers
            for (int i = 0; i < this.headers.Count; i++)
            {
                // Determime the Right Bound Position.
                for (int j = i; j < this.headers.Count; j++)
                    if (j + 1 < this.headers.Count &&
                        this.headers[j].data == this.headers[j + 1].data)
                    {
                        right_bound_pos += 1;
                    }
                    else
                    {
                        break;
                    }

                // Assign the Size Values to the Outline Rectangle
                HeaderOutline.Height = default_label_height;
                HeaderOutline.Width = default_label_width * right_bound_pos;


                // Position the Rectangle
                HeaderOutline.X = HeaderPosition.X;
                HeaderOutline.Y = HeaderPosition.Y;

                // Draw the Background Colour
                graphics.FillRectangle(this.headers[i].back_color, HeaderOutline);

                // Draw the Outline
                graphics.DrawRectangle(System.Drawing.Pens.Black, HeaderOutline);

                // Store the Right hand Boundry
                HeaderPosition.X = HeaderOutline.Right;

                // Collect the Objects Font Value
                Font FontBuffer = (Font) this.headers[i].font.Clone();

                // Will the String fit within the Outline Rectangle?
                float difference_ratio = StringRatio(this.headers[i].data, this.headers[i].font, HeaderOutline, graphics);
                if (difference_ratio != 1)
                {
                    FontBuffer = new Font(FontBuffer.Name, FontBuffer.Size * difference_ratio,FontBuffer.Style);
                }

                // Draw the String
                graphics.DrawString(this.headers[i].data, FontBuffer, this.headers[i].text_color, HeaderOutline, this.headers[i].format);

                // Reset Everything
                i += (right_bound_pos - 1);
                right_bound_pos = 1;
                HeaderOutline.Width = default_label_width;
            }

            // Draw the Footers
            for (int i = 0; i < footers.Count; i++)
            {
                // Assign the Size Values to the Outline Rectangle
                FooterOutline.Width = default_label_width;
                FooterOutline.Height = default_label_height;

                // Position the Rectangle
                FooterOutline.X = FooterPosition.X;
                FooterOutline.Y = FooterPosition.Y;

                // Draw the background Colour
                graphics.FillRectangle(footers[i].back_color, FooterOutline);

                // Draw the Outline
                graphics.DrawRectangle(System.Drawing.Pens.Black, FooterOutline);

                // Collect the Objects Font Values
                Font TopFontBuffer = (Font) this.footers[i].top_font.Clone();
                Font BotFontBuffer = (Font) this.footers[i].bot_font.Clone();

                // Will the Strings Fit within the Rectangle? If not Reintialize them at a smaller size.
                float top_difference_ratio = StringRatio(footers[i].top_data, TopFontBuffer, FooterOutline, graphics);
                float bot_difference_ratio = StringRatio(footers[i].bot_data, BotFontBuffer, FooterOutline, graphics);

                if (top_difference_ratio != 1)
                {
                    TopFontBuffer = new Font(TopFontBuffer.Name, TopFontBuffer.Size * top_difference_ratio);
                }

                if (bot_difference_ratio != 1)
                {
                    BotFontBuffer = new Font(BotFontBuffer.Name, BotFontBuffer.Size * bot_difference_ratio);
                }

                // Draw the Strings
                graphics.DrawString(footers[i].top_data, TopFontBuffer, footers[i].text_color, FooterOutline,footers[i].top_format);
                graphics.DrawString(footers[i].bot_data, BotFontBuffer, footers[i].text_color, FooterOutline,footers[i].bot_format);

                // Iterate and Reset
                FooterPosition.X += FooterOutline.Width;

            }
        }

        // Helper Method for Render. Determines if a String is too large to fit within a rectangle.
        // Returns Ratio of how much larger 
        private float StringRatio(string input_string, Font input_font, Rectangle input_rec, Graphics input_graphics)
        {
            // Measure the length of the string.
            SizeF StringSize = input_graphics.MeasureString(input_string, input_font);

            if (StringSize.Width > input_rec.Width)
            {
                // It's too big. Figure out the ratio.
                float difference_ratio = input_rec.Width / StringSize.Width;
                return difference_ratio;
            }
            else
            {
                return 1;
            }
        }


        public int CompareTo(RackLabel other)
        {
            if (other.cabinet_number == cabinet_number)
            {
                return rack_number - other.rack_number;
            }
            return cabinet_number - other.cabinet_number;
        }
    }
}
