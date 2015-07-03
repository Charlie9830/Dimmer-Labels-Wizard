using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Globalization;

namespace Dimmer_Labels_Wizard
{
    [Serializable]
    public class LabelStrip : IComparable<LabelStrip>
    {
        // Properties.
        public List<HeaderCell> Headers = new List<HeaderCell>();
        public List<FooterCell> Footers = new List<FooterCell>();

        // Render Outlines.
        public List<Border> HeaderOutlines = new List<Border>();
        public List<Border> FooterOutlines = new List<Border>();

        public int LabelWidthInMM { get; set; } // Width of each Cell in Millimeters
        public int LabelHeightInMM { get; set; } // Height of each Cell in Millimeters

        public float LineWeight { get; set; }

        public RackType RackUnitType { get; set; } // Imported from DimmerDistroUnit or User Selection of Labels.
        public int RackNumber { get; set; } // Imported from DimDistroUnit for User selection of Labels.

        // Methods.
        public void PrintToConsole()
        {
            int count = 0;
            for (int i = 0; i < Headers.Count; i++)
            {
                Console.Write(Headers[i].Data);
                Console.Write(" | ");
                count++;
            }
            Console.Write("    ");
            Console.Write(count);
            count = 0;

            Console.WriteLine();

            for (int i = 0; i < Footers.Count; i++)
            {
                Console.Write(Footers[i].MiddleData);
                Console.Write(" | ");
                count++;
            }
            Console.Write("    ");
            Console.Write(count);
        }

        // Sets the Background Colour of Each Header and Footer Cell
        public void SetBackgroundColor(System.Drawing.Color desiredColor)
        {
            foreach (var element in Headers)
            {
                element.BackgroundColor = new SolidBrush(desiredColor);
            }

            foreach (var element in Footers)
            {
                element.BackgroundColor = new SolidBrush(desiredColor);
            }
        }

        // Overload: Sets the Background Color of Header and Cell Cells Matching a Multicore Name
        public void SetBackgroundColor(string multicoreName, System.Drawing.Color desiredColor)
        {
            for (int list_index = 0; list_index < 12; list_index++)
            {
                if (Headers[list_index].Data == multicoreName)
                {
                    Headers[list_index].BackgroundColor = new SolidBrush(desiredColor);
                    Footers[list_index].BackgroundColor = new SolidBrush(desiredColor);
                }
            }
        }

        // Render Header and Footers to Display
        public void RenderToDisplay(Canvas canvas, Point origin)
        {
            // Resources.
            SolidColorBrush outlineColor = new SolidColorBrush(Colors.Black);
            Thickness headerOutlineThickness = new Thickness(2);

            double labelWidth = this.LabelWidthInMM;
            double labelHeight = this.LabelHeightInMM;

            double headerXOffset = origin.X;
            int rightBoundPosition = 1;

            char stringDelimiter = ' ';

            // Clear Lists.
            if (HeaderOutlines.Count > 0)
            {
                HeaderOutlines.Clear();
            }

            if (FooterOutlines.Count > 0)
            {
                FooterOutlines.Clear();
            }

            // Header Rendering.
            for (int index = 0; index < Headers.Count; index++)
            {
                Canvas textCanvas = new Canvas();
                List<TextBlock> textBlocks = new List<TextBlock>();

                HeaderOutlines.Add(new Border());

                // Determine Right Bound Position.
                string currentData = Headers[index].Data;
                for (int j = index + 1; j < Headers.Count; j++)
                {
                    if (currentData == Headers[j].Data)
                    {
                        rightBoundPosition++;
                    }

                    else
                    {
                        break;
                    }
                }

                // Set HeaderOutline Size.
                HeaderOutlines[index].Width = labelWidth * rightBoundPosition;
                HeaderOutlines[index].Height = labelHeight;

                // Set HeaderOutline Position.
                if (index == 0)
                {
                    Canvas.SetTop(HeaderOutlines[index], origin.X);
                    Canvas.SetLeft(HeaderOutlines[index], origin.Y);
                }

                else
                {
                    Canvas.SetTop(HeaderOutlines[index], headerXOffset);
                    Canvas.SetLeft(HeaderOutlines[index], origin.Y);
                }

                // Set Colors.
                HeaderOutlines[index].Background = Headers[index].BackgroundColor;
                HeaderOutlines[index].BorderBrush = outlineColor;

                // Set Outline Thickness.
                HeaderOutlines[index].BorderThickness = headerOutlineThickness;

                // Setup Text.
                textCanvas.Width = HeaderOutlines[index].Width - HeaderOutlines[index].BorderThickness.Left -
                    HeaderOutlines[index].BorderThickness.Right;
                textCanvas.Height = HeaderOutlines[index].Height - HeaderOutlines[index].BorderThickness.Top -
                    HeaderOutlines[index].BorderThickness.Bottom;

                Size textCanvasSize = new Size(textCanvas.Width, textCanvas.Height);

                string headerData = Headers[index].Data;
                Typeface headerTypeface = Headers[index].Font;
                double headerFontSize = Headers[index].FontSize;

                // Measure the string.
                Size downSizeRatio = MeasureText(headerData, headerTypeface, headerFontSize);

                // Can the string Fit into onto the Canvas as is?
                if (downSizeRatio.Width == 1)
                {
                    // GenerateTextBlock()
                }

                else
                {
                    // Is the String Splitable
                    string[] splitStrings = headerData.Split(stringDelimiter);

                    if (splitStrings.Length > 1)
                    {
                        // Check If Vertical Downsizing is required.
                            // Apply Downsize back to HeaderCell.

                        // Generate TextBlocks.
                    }

                    else
                    {
                        Headers[index].FontSize *= downSizeRatio.Width;

                        // Generate TextBlocks.
                    }
                }

                // Add to canvas.
                canvas.Children.Add(HeaderOutlines[index]);

                // Iterate and Reset.
                headerXOffset += HeaderOutlines[index].Width;
                index += rightBoundPosition;
                rightBoundPosition = 1;
            }

        }
      
        private Size MeasureText(string text,Typeface typeface, double emSize)
        {
            FormattedText formatter = new FormattedText(text, CultureInfo.CurrentCulture,FlowDirection.LeftToRight,
                typeface,emSize,Brushes.Black);

            Size returnSize = new Size(formatter.Width, formatter.Height);

            return returnSize;
        }

        private Size GetDifferenceRatio(Size textSize, Size containerSize)
        {
            Size returnSize = new Size();

            // Width
            if (textSize.Width > containerSize.Width)
            {
                returnSize.Width = containerSize.Width / textSize.Width;
            }

            else
            {
                returnSize.Width = 1;
            }

            // Height
            if (textSize.Height > containerSize.Height)
            {
                returnSize.Height = containerSize.Height / textSize.Height;
            }

            else
            {
                returnSize.Height = 1;
            }

            return returnSize;
        }

        private TextBlock[] GenerateTextBlocks(Canvas canvas, string[] strings, Typeface typeface, Size fontSize)
        {
            List<TextBlock> textBlocks = new List<TextBlock>();

        }

        // Helper Method for Render. Determines if a String is too large to fit within a rectangle.
        // Returns SizeF Ratio.
        //private SizeF StringRatio(string inputText, Font inputFont, Rectangle inputRec, Graphics graphics)
        //{
        //    var returnValue = new SizeF();

        //    // Measure the width and height of the string.
        //    SizeF stringSize = graphics.MeasureString(inputText, inputFont);

        //    // Determine Width Ratio.
        //    if (stringSize.Width > inputRec.Width)
        //    {
        //        // It's too big. Calculate the ratio.
        //        returnValue.Width = inputRec.Width / stringSize.Width;
                
        //    }

        //    else
        //    {
        //        returnValue.Width = 1;
        //    }

        //    // Determine Height Ratio
        //    if (stringSize.Height > inputRec.Height)
        //    {
        //        // It's too big. Calculate the Ratio.
        //        returnValue.Height = inputRec.Height / stringSize.Height;
        //    }

        //    else
        //    {
        //        returnValue.Height = 1;
        //    }

        //    return returnValue;
        //}

        // Helper Method for Render. Provides advanced String Drawing functionality. Truncates Split strings to Max
        // of 3 lines currently. Only Suitable for Header Cell Text Rendering.
        //private void RenderText(HeaderCell cell, Rectangle rectangle, Graphics graphics,float scaleRatio)
        //{
        //    // String Formating Positions.
        //    StringFormat topPosition = new StringFormat();
        //    topPosition.Alignment = StringAlignment.Center;
        //    topPosition.LineAlignment = StringAlignment.Near;
        //    topPosition.FormatFlags = StringFormatFlags.NoWrap;

        //    StringFormat middlePosition = new StringFormat();
        //    middlePosition.Alignment = StringAlignment.Center;
        //    middlePosition.LineAlignment = StringAlignment.Center;
        //    middlePosition.FormatFlags = StringFormatFlags.NoWrap;

        //    StringFormat bottomPosition = new StringFormat();
        //    bottomPosition.Alignment = StringAlignment.Center;
        //    bottomPosition.LineAlignment = StringAlignment.Far;
        //    bottomPosition.FormatFlags = StringFormatFlags.NoWrap;

        //    char[] stringDelimiter = {' '};

        //    // Collect Font Value and apply Scaling Ratio.
        //    var fontBuffer = new Font(cell.Font.Name, cell.Font.Size * scaleRatio, cell.Font.Style, graphics.PageUnit);

        //    // Determine Difference Ratio of the String.
        //    SizeF differenceRatio = StringRatio(cell.Data, fontBuffer, rectangle, graphics); // REFACTOR ME TO widthDifferenceRatio
            
        //    // Does the String Width fit within the rectangle?
        //    if (differenceRatio.Width != 1)
        //    {
        //        // Can the String be Split?
        //        string[] splitStrings = cell.Data.Split(stringDelimiter);
        //        if (splitStrings.Length > 1)
        //        {
        //            int numberOfLines = splitStrings.Length;

        //            // Determine differenceRatios of multiple strings.
        //            float[] widthDifferenceRatios = new float[splitStrings.Length];

        //            for (int index = 0; index < splitStrings.Length; index++)
        //            {
        //                widthDifferenceRatios[index] = StringRatio(splitStrings[index], fontBuffer, rectangle, graphics).Width;
        //            }

        //            int widthDownsizeIndex = DetermineDownsizingIndex(widthDifferenceRatios);

        //            if (widthDownsizeIndex == -1)
        //            {
        //                // Check if Vertical downsizing is required
        //                if (graphics.MeasureString(cell.Data, fontBuffer).Height * numberOfLines > rectangle.Height)
        //                {
        //                    // Measure the Size of the text
        //                    SizeF textSize = graphics.MeasureString(cell.Data, fontBuffer);

        //                    // Determine the difference ratio.
        //                    float verticalDownsizeRatio = rectangle.Height / (textSize.Height * numberOfLines);

        //                    // Initialize a new smaller font.
        //                    Font verticallyDownsizedFont = new Font(fontBuffer.Name, fontBuffer.Size * verticalDownsizeRatio, fontBuffer.Style, graphics.PageUnit);

        //                    // Generate Text Rectangles.
        //                    Rectangle[] textRectangles = GenerateTextRectangles(splitStrings, cell.Data, verticallyDownsizedFont, rectangle, graphics);

        //                    // Draw Text to Rectangles.
        //                    for (int index = 0; index < splitStrings.Length; index++)
        //                    {
        //                        graphics.DrawString(splitStrings[index], verticallyDownsizedFont, cell.TextColor, textRectangles[index], middlePosition);
        //                    }
        //                }

        //                // Otherwise Draw the text with current Font Size.
        //                else
        //                {
        //                    // Generate Text Rectangles.
        //                    Rectangle[] textRectangles = GenerateTextRectangles(splitStrings, cell.Data, fontBuffer, rectangle, graphics);

        //                    // Draw Text to Rectangles.
        //                    for (int index = 0; index < splitStrings.Length; index++)
        //                    {
        //                        graphics.DrawString(splitStrings[index], fontBuffer, cell.TextColor, textRectangles[index], middlePosition);
        //                    }
        //                }
        //            }

        //            else
        //            {
        //                // Initialize a new downsized Font.
        //                var downsizedFont = new Font(fontBuffer.Name, fontBuffer.Size * widthDifferenceRatios[widthDownsizeIndex], fontBuffer.Style, graphics.PageUnit);

        //                SizeF textSize = graphics.MeasureString(cell.Data, downsizedFont);

        //                // Check if Vertical Downsizing is required.
        //                if (textSize.Height * numberOfLines > rectangle.Height)
        //                {
        //                    // Determine the Ratio
        //                    float verticalDownsizeRatio = rectangle.Height / (textSize.Height * numberOfLines);

        //                    // Initialize a new further downsized font.
        //                    Font verticallyDownsizedFont = new Font(downsizedFont.Name, downsizedFont.Size * verticalDownsizeRatio, downsizedFont.Style, GraphicsUnit.Pixel);
        //                    // Generate Text Rectangles.
        //                    Rectangle[] textRectangles = GenerateTextRectangles(splitStrings, cell.Data, verticallyDownsizedFont, rectangle, graphics);

        //                    // Draw text to Rectangles.
        //                    for (int index = 0; index < splitStrings.Length; index++)
        //                    {
        //                        graphics.DrawString(splitStrings[index], verticallyDownsizedFont, cell.TextColor, textRectangles[index], middlePosition);
        //                    }
        //                }

        //                else
        //                {
        //                    // Generate Text Rectangles.
        //                    Rectangle[] textRectangles = GenerateTextRectangles(splitStrings, cell.Data, downsizedFont, rectangle, graphics);

        //                    // Draw text to Rectangles.
        //                    for (int index = 0; index < splitStrings.Length; index++)
        //                    {
        //                        graphics.DrawString(splitStrings[index], downsizedFont, cell.TextColor, textRectangles[index], middlePosition);
        //                    }
        //                }
        //            }

        //        }
        //        // String Cannot be Split.
        //        else
        //        {
        //            var downsizedFont = new Font(fontBuffer.Name, fontBuffer.Size * differenceRatio.Width, fontBuffer.Style, graphics.PageUnit);
        //            graphics.DrawString(splitStrings[0], downsizedFont, cell.TextColor, rectangle, middlePosition);
        //        }
        //    }
        //    else
        //    {
        //        graphics.DrawString(cell.Data, fontBuffer, cell.TextColor, rectangle, cell.Format);
        //    }

        //}

        // Helper Method for RenderText. Determines if strings need to be downsized. Returns -1 if no downsizing is
        // needed, if downsizing is required, will return the count of the largest ratio from the array parameter.
        //private int DetermineDownsizingIndex(float[] differenceRatios)
        //{
        //    bool widthDownsizingRequried = false;

        //    // Determine if downsizing of Length is required.
        //    for (int index = 0; index < differenceRatios.Length; index++)
        //    {
        //        // Is Downsizing of any of the Strings required?
        //        if (differenceRatios[index] != 1)
        //        {
        //            widthDownsizingRequried = true;
        //            break;
        //        }
        //    }

        //    if (widthDownsizingRequried == true)
        //    {
        //        // Determine the Largest Ratio and returns the Index of that Ratio.
        //        return Array.IndexOf(differenceRatios, differenceRatios.Min());
        //    }

        //    return -1;
        //}

        // Helper Method for RenderText. Returns an Array of Rectangles, positioned and sized to fit within the paramter rectangle.
        //private Rectangle[] GenerateTextRectangles(string[] inputStrings, string cellData, Font font, Rectangle labelRectangle, Graphics graphics)
        //{
        //    int numberOfRectangles = inputStrings.Length;
        //    double textHeight = graphics.MeasureString(cellData, font).Height;

        //    Rectangle[] textRectangles = new Rectangle[numberOfRectangles];

        //    // Even Number of Rectangles.
        //    if (numberOfRectangles % 2 == 0)
        //    {
        //        // Determine Y Cordinate of first textRectangle.
        //        int yPos = (int)Math.Floor((double)(((labelRectangle.Height - (textHeight * numberOfRectangles)) / numberOfRectangles) + labelRectangle.Y));

        //        // Position and Store Rectangles.
        //        for (int i = 0; i < numberOfRectangles; i++)
        //        {
        //            // On First Iteration.
        //            if (i == 0)
        //            {
        //                textRectangles[i] = new Rectangle(labelRectangle.X, yPos, labelRectangle.Width, (int)Math.Floor(textHeight));
        //            }

        //            // Subsequent Iterations.
        //            else
        //            {
        //                textRectangles[i] = new Rectangle(labelRectangle.X, yPos + (textRectangles[i - 1].Height * i), labelRectangle.Width, (int)Math.Floor(textHeight));
        //            }

        //        }


        //    }
        //    // Odd number of Rectangles.
        //    else
        //    {
        //        int yPos = (int)Math.Floor((double)(((labelRectangle.Height - (textHeight * numberOfRectangles)) / numberOfRectangles) + labelRectangle.Y));

        //        // Position and Store Rectangles.
        //        for (int i = 0; i < numberOfRectangles; i++)
        //        {
        //            // On First Iteration.
        //            if (i == 0)
        //            {
        //                textRectangles[i] = new Rectangle(labelRectangle.X, yPos, labelRectangle.Width, (int)Math.Floor(textHeight));
        //            }

        //            // Subsequent Iterations.
        //            else
        //            {
        //                textRectangles[i] = new Rectangle(labelRectangle.X, yPos + (textRectangles[i - 1].Height * i), labelRectangle.Width, (int)Math.Floor(textHeight));
        //            }

        //        }
        //    }
        //    return textRectangles;
        //}


        public int CompareTo(LabelStrip other)
        {
            if (other.RackUnitType == RackUnitType)
            {
                return RackNumber - other.RackNumber;
            }

            return other.RackUnitType - RackUnitType;
        }
    }
}
