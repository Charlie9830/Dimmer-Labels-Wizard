using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drawing_Test
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Drawing Code cannot be placed here.
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            // Overide OnPaint method to have the form not Clip the graphics object

            System.Drawing.Graphics graphics = this.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Red);
            Rectangle myRectangle = new Rectangle(0, 0, 20, 20);
            string test_string = "LX1X";
            System.Drawing.Font testFont = new Font("Arial", 12);
            PointF myPoint = new PointF(0, 0);


            for (int i = 0; i < 12; i++)
            {
                myRectangle.X += myRectangle.Width;
                graphics.DrawRectangle(System.Drawing.Pens.Red, myRectangle);
                myRectangle.Width += 10;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Prints the Header Labels

            // Initialize Test Strings
            string[] headers = new string[12];
            headers[0] = "LX1X";
            headers[1] = "LX1X";
            headers[2] = "LX1X";
            headers[3] = "LX1X";
            headers[4] = "LX1A";
            headers[5] = "LX1A";
            headers[6] = "LX1Y";
            headers[7] = "LX1Y";
            headers[8] = "LX1Y";
            headers[9] = "LX1Y";
            headers[10] = "PSU's";
            headers[11] = "PSU's";

            int label_width = 20;
            int label_height = 20;

            int strip_length = 12;
            int right_bound_pos = 1;
            int x_pos = 0;

            System.Drawing.Graphics canvas = this.CreateGraphics();

            SolidBrush BlackBrush = new SolidBrush(Color.Black);
            SolidBrush Colour1 = new SolidBrush(Color.GreenYellow);
            SolidBrush Colour2 = new SolidBrush(Color.Red);
            SolidBrush Colour3 = new SolidBrush(Color.Blue);
            SolidBrush Colour4 = new SolidBrush(Color.Pink);

            Point OriginPoint = new Point(0, 0);
            System.Drawing.Font LabelFont = new Font("Arial", 10);
            Rectangle LabelRec = new Rectangle(0, 0, label_width, label_height);
            Rectangle FillRec = new Rectangle(0, 0, 0, 0);
            System.Drawing.StringFormat LabelFormat = new StringFormat();
            SizeF StringSize = new SizeF();

            LabelFormat.Alignment = StringAlignment.Center;
            LabelFormat.LineAlignment = StringAlignment.Center;

            // Iterate through each Label in the strip.
            for (int i = 0; i < strip_length; i++)
            {
                // Figure out The end point of the current Label Box.
                for (int j = i; j < headers.Length; j++)
                {
                    if (j + 1 < headers.Length && headers[j] == headers[j + 1])     // Check that J is not Out of Index Before trying to retreive that index.
                    {
                        right_bound_pos += 1;
                    }

                    else
                    {
                        break;
                    }
                }
                // Assign the Size values to the Label Rectangle.
                LabelRec.Width = label_width * right_bound_pos;

                // Position the Rectangle.
                LabelRec.X = x_pos;

                // Draw the Background Colour
                canvas.FillRectangle(Colour1, LabelRec);


                // Draw the Rectangle
                canvas.DrawRectangle(System.Drawing.Pens.Black, LabelRec);

                // Store the Right hand Boundry of the current label AFTER It has been drawn.
                x_pos = LabelRec.Right;

                // Measure the Size of the String.
                StringSize = canvas.MeasureString(headers[i], LabelFont);

                // Check if the string is small enough to fit in the rectangle.
                if (StringSize.Width > LabelRec.Width)
                {
                    // If it is too big. Figure out the Ratio how much bigger it is.
                    // Can Become a Negative Value that will cause an Exception to be thrown. Ratios Maybe better.
                    float difference_ratio = StringSize.Width - LabelRec.Width;

                    // Draw the String with a New Font Initalized at Smaller Size
                    canvas.DrawString(headers[i], new Font("Arial", 10 - difference_ratio), BlackBrush, LabelRec, LabelFormat);

                }

                else
                {
                    // Otherwise Draw the string into the Rectangle with the default font size.
                    canvas.DrawString(headers[i], LabelFont, BlackBrush, LabelRec, LabelFormat);
                }

                //Reset Everthing.
                i += (right_bound_pos - 1);
                right_bound_pos = 1;
                LabelRec.Width = label_width;

            }




        }
    }
}
