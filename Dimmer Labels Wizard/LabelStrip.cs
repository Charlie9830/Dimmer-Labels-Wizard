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

        private double unitConversionRatio = 96d / 25.4d;

        // Updates HeaderCell Data that is Associated with the input headerCell, so Cells don't split unexpectatly.
        public void UpdateHeaderData(string newData, HeaderCell headerCell)
        {
            // Determine headerCell Index.
            int headerCellIndex = Headers.IndexOf(headerCell);
            // Init a List of headerCells to Update.
            List<HeaderCell> headersToUpdate = new List<HeaderCell>();

            // Itterate Forward through Headers. If Data matches old Data, add it to Update List.
            for (int index = headerCellIndex; index < Headers.Count; index++)
        {
                if (Headers[index].Data == headerCell.Data)
                {
                    headersToUpdate.Add(Headers[index]);
                }

                else
                {
                    break;
                }
                
            }
            
            // Apply Updates to everything in the Updates List.
            foreach (var element in headersToUpdate)
            {
                element.Data = newData;
            }
        }

        // Sets the Background Colour of Each Header and Footer Cell
        public void SetBackgroundColor(Color desiredColor)
        {
            foreach (var element in Headers)
            {
                element.BackgroundColor = new SolidColorBrush(desiredColor);
            }

            foreach (var element in Footers)
            {
                element.BackgroundColor = new SolidColorBrush(desiredColor);
            }
        }


        // Control Method for Render Methods.
        public void RenderToDisplay(Canvas canvas, Point offset)
        {
            RenderHeader(canvas, offset);

            offset.Y += (this.LabelHeightInMM * unitConversionRatio) * 1.5;

            RenderFooters(canvas, offset);
        }

        private void RenderHeader(Canvas canvas, Point offset)
        {
            // Resources.
            SolidColorBrush outlineColor = new SolidColorBrush(Colors.Black);
            Thickness headerOutlineThickness = new Thickness(2);

            // Convert to WPF Units (Inches)
            double labelWidth = this.LabelWidthInMM * unitConversionRatio;
            double labelHeight = this.LabelHeightInMM * unitConversionRatio;

            double headerXOffset = offset.X;
            int rightBoundPosition = 1;

            char stringDelimiter = ' ';

            // Clear List.
            if (HeaderOutlines.Count > 0)
            {
                HeaderOutlines.Clear();
            }

            // Header Rendering Loop.
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
                HeaderOutlines.Last().Width = labelWidth * rightBoundPosition;
                HeaderOutlines.Last().Height = labelHeight;

                // Set HeaderOutline Position.
                if (index == 0)
                {
                    Canvas.SetTop(HeaderOutlines.Last(), offset.Y);
                    Canvas.SetLeft(HeaderOutlines.Last(), offset.X);
                }

                else
                {
                    Canvas.SetTop(HeaderOutlines.Last(), offset.Y);
                    Canvas.SetLeft(HeaderOutlines.Last(), headerXOffset);
                }

                // Set Colors.
                HeaderOutlines.Last().Background = Headers[index].BackgroundColor;
                HeaderOutlines.Last().BorderBrush = outlineColor;

                // Set Outline Thickness.
                HeaderOutlines.Last().BorderThickness = headerOutlineThickness;

                // Setup Text.
                textCanvas.Width = HeaderOutlines.Last().Width - HeaderOutlines.Last().BorderThickness.Left -
                    HeaderOutlines.Last().BorderThickness.Right;

                textCanvas.Height = HeaderOutlines.Last().Height - HeaderOutlines.Last().BorderThickness.Top -
                    HeaderOutlines.Last().BorderThickness.Bottom;

                Size textCanvasSize = new Size(textCanvas.Width, textCanvas.Height);

                string headerData = Headers[index].Data;
                Typeface headerTypeface = Headers[index].Font;
                double headerFontSize = Headers[index].FontSize;

                // Measure the string.
                Size fontDimensions = MeasureText(headerData, headerTypeface, headerFontSize);
                Size downSizeRatio = GetDifferenceRatio(fontDimensions, textCanvasSize);

                // Can the string Fit into onto the Canvas as is?
                if (downSizeRatio.Width == 1)
                {
                    if (downSizeRatio.Height == 1)
                    {
                        textCanvas.Children.Add
                            (GenerateTextBlocks(textCanvasSize, headerData, headerTypeface, headerFontSize,
                            Headers[index].TextColor, fontDimensions));
                    }

                    else
                    {
                        // Downsize Vertically and Send to Canvas.
                        Headers[index].FontSize *= downSizeRatio.Height;
                        textCanvas.Children.Add
                            (GenerateTextBlocks(textCanvasSize, headerData, headerTypeface, headerFontSize,
                            Headers[index].TextColor, fontDimensions));
                    }
                }

                else
                {
                    // Is the String Splitable
                    string[] splitStrings = headerData.Split(stringDelimiter);

                    if (splitStrings.Length > 1)
                    {
                        // Find the Longest String, Check if it Fits within the Canvas Width.
                        int longestStringIndex = GetLongestStringIndex(splitStrings, headerTypeface, Headers[index].FontSize);

                        // Is Width Downsizing Required?
                        double horizontalDownSizeRatio = GetDifferenceRatio(MeasureText(splitStrings[longestStringIndex], headerTypeface, Headers[index].FontSize),
                            textCanvasSize).Width;
                        if (horizontalDownSizeRatio != 1)
                        {
                            Headers[index].FontSize *= horizontalDownSizeRatio;
                        }

                        // (re)measure string based off Tallest Index. Any strings that are too long have been Downsized at this point.
                        int tallestStringIndex = GetTallestStringIndex(splitStrings, headerTypeface, Headers[index].FontSize);
                        fontDimensions = MeasureText(splitStrings[tallestStringIndex], headerTypeface, Headers[index].FontSize);

                        // Check if Vertical Downsizing is required.
                        double verticalDownsizeRatio = textCanvas.Height > fontDimensions.Height * splitStrings.Length ?
                            1 : textCanvas.Height / (fontDimensions.Height * splitStrings.Length);

                        if (verticalDownsizeRatio != 1)
                        {
                            // Downsize Further.
                            Headers[index].FontSize *= verticalDownsizeRatio;
                        }

                        // Recalulate Font Dimensions.
                        Size verticallyDownsizedFontDimensions = MeasureText(splitStrings.First(),headerTypeface,Headers[index].FontSize);
                        
                        // Generate TextBlocks.
                        TextBlock[] textBlockQueue = GenerateTextBlocks(textCanvasSize,splitStrings,headerTypeface,
                            Headers[index].FontSize, Headers[index].TextColor, verticallyDownsizedFontDimensions);

                        foreach (var element in textBlockQueue)
                        {
                            textCanvas.Children.Add(element);
                        }
                    }

                    else
                    {
                        Headers[index].FontSize *= downSizeRatio.Width;

                        // Recalculate Font Dimensions.
                        fontDimensions = MeasureText(headerData, headerTypeface, Headers[index].FontSize);

                        textCanvas.Children.Add
                        (GenerateTextBlocks(textCanvasSize,headerData,headerTypeface,Headers[index].FontSize,
                        Headers[index].TextColor,fontDimensions));
                    }
                }

                // Tag the HeaderOutline.
                HeaderOutlines.Last().Tag = Headers[index];

                // Add to labelCanvas.
                HeaderOutlines.Last().Child = textCanvas;
                canvas.Children.Add(HeaderOutlines.Last());

                // Iterate and Reset.
                headerXOffset += HeaderOutlines.Last().Width;
                index += rightBoundPosition - 1;
                rightBoundPosition = 1;
            }
        }

        private void RenderFooters(Canvas canvas, Point offset)
        {
            // Resources.
            SolidColorBrush outlineColor = new SolidColorBrush(Colors.Black);
            Thickness footerOutlineThickness = new Thickness(2);

            // Convert to WPF Units (inches).
            double labelWidth = this.LabelWidthInMM * unitConversionRatio;
            double labelHeight = this.LabelHeightInMM * unitConversionRatio;

            double footerXOffset = offset.X;

            // Clear List.
            if (FooterOutlines.Count > 0)
            {
                FooterOutlines.Clear();
            }

            // Footer Rendering Loop.
            for (int index = 0; index < Footers.Count; index++)
            {
                Canvas textCanvas = new Canvas();

                FooterOutlines.Add(new Border());

                // Set FooterOutline Size.
                FooterOutlines.Last().Width = labelWidth;
                FooterOutlines.Last().Height = labelHeight;

                // Set FooterOutline Position.
                if (index == 0)
                {
                    Canvas.SetTop(FooterOutlines.Last(), offset.Y);
                    Canvas.SetLeft(FooterOutlines.Last(), offset.X);
                }

                else
                {
                    Canvas.SetTop(FooterOutlines.Last(), offset.Y);
                    Canvas.SetLeft(FooterOutlines.Last(), footerXOffset);
                }

                // Set Colors.
                FooterOutlines.Last().Background = Footers[index].BackgroundColor;
                FooterOutlines.Last().BorderBrush = outlineColor;

                // Set Outline Thickness.
                FooterOutlines.Last().BorderThickness = footerOutlineThickness;

                // Setup Text Canvas.
                textCanvas.Width = FooterOutlines.Last().Width - FooterOutlines.Last().BorderThickness.Left -
                    FooterOutlines.Last().BorderThickness.Right;

                textCanvas.Height = FooterOutlines.Last().Height - FooterOutlines.Last().BorderThickness.Top -
                    FooterOutlines.Last().BorderThickness.Bottom;

                // Analyze and Minimize Fonts if Requried.
                Size textCanvasSize = new Size(textCanvas.Width,textCanvas.Height);

                Size topDifferenceRatio = GetDifferenceRatio(MeasureText(Footers[index].TopData, Footers[index].TopFont,
                    Footers[index].TopFontSize), textCanvasSize);

                Size middleDifferenceRatio = GetDifferenceRatio(MeasureText(Footers[index].MiddleData, Footers[index].MiddleFont,
                    Footers[index].MiddleFontSize), textCanvasSize);

                Size bottomDifferenceRatio = GetDifferenceRatio(MeasureText(Footers[index].BottomData, Footers[index].BottomFont,
                    Footers[index].BottomFontSize), textCanvasSize);
                
                // Scale Down heights if Required.
                if (topDifferenceRatio.Width != 1)
                {
                    Footers[index].TopFontSize *= topDifferenceRatio.Width;
                }

                if (middleDifferenceRatio.Width != 1)
                {
                    Footers[index].MiddleFontSize *= middleDifferenceRatio.Width;
                }

                if (bottomDifferenceRatio.Width != 1)
                {
                    Footers[index].BottomFontSize *= bottomDifferenceRatio.Width;
                }

                // Generate blank TextBlocks.
                TextBlock[] textBlocks = GenerateFooterTextBlocks(textCanvas,Footers[index].TextColor);

                TextBlock topTextBlock = textBlocks[0];
                TextBlock middleTextBlock = textBlocks[1];
                TextBlock bottomTextBlock = textBlocks[2];

                // Populate TextBlocks.
                topTextBlock.Text = Footers[index].TopData;
                topTextBlock.FontFamily = Footers[index].TopFont.FontFamily;
                topTextBlock.FontSize = Footers[index].TopFontSize;
                topTextBlock.FontStyle = Footers[index].TopFont.Style;
                topTextBlock.FontWeight = Footers[index].TopFont.Weight;
                topTextBlock.FontStretch = Footers[index].TopFont.Stretch;

                middleTextBlock.Text = Footers[index].MiddleData;
                middleTextBlock.FontFamily = Footers[index].MiddleFont.FontFamily;
                middleTextBlock.FontSize = Footers[index].MiddleFontSize;
                middleTextBlock.FontStyle = Footers[index].MiddleFont.Style;
                middleTextBlock.FontWeight = Footers[index].MiddleFont.Weight;
                middleTextBlock.FontStretch = Footers[index].MiddleFont.Stretch;

                bottomTextBlock.Text = Footers[index].BottomData;
                bottomTextBlock.FontFamily = Footers[index].BottomFont.FontFamily;
                bottomTextBlock.FontSize = Footers[index].BottomFontSize;
                bottomTextBlock.FontStyle = Footers[index].BottomFont.Style;
                bottomTextBlock.FontWeight = Footers[index].BottomFont.Weight;
                bottomTextBlock.FontStretch = Footers[index].BottomFont.Stretch;

                // Add TextBlocks to textCanvas.
                textCanvas.Children.Add(topTextBlock);
                textCanvas.Children.Add(middleTextBlock);
                textCanvas.Children.Add(bottomTextBlock);

                // Add Canvas to Outline.
                FooterOutlines.Last().Child = textCanvas;

                // Tag Outline.
                FooterOutlines.Last().Tag = Footers[index];

                // Add Outline to Label Canvas.
                canvas.Children.Add(FooterOutlines.Last());

                // Iterate and Reset.
                footerXOffset += FooterOutlines.Last().Width;
            }

        }

        // Returns the index of the Longest String in an Array. (Typefaces Measurements).
        private int GetLongestStringIndex(string[] strings, Typeface typeface, double emSize)
        {
            List<double> widths = new List<double>();
            List<string> stringList = strings.ToList();

            foreach (var element in stringList)
            {
                widths.Add(MeasureText(element, typeface, emSize).Width);
            }

            return widths.IndexOf(widths.Max());
            
        }

        private int GetTallestStringIndex(string[] strings, Typeface typeface, double emSize)
        {
            List<double> heights = new List<double>();
            List<string> stringList = strings.ToList();

            foreach (var element in stringList)
            {
                heights.Add(MeasureText(element, typeface, emSize).Height);
            }

            return heights.IndexOf(heights.Max());

        }

        private Size MeasureText(string text, Typeface typeface, double emSize)
        {
            if (text == null)
            {
                return new Size(1, 1);
            }

            FormattedText formatter = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                typeface, emSize, Brushes.Black);

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
        
        private TextBlock[] GenerateTextBlocks(Size canvasSize, string[] strings, Typeface typeface, double fontSize, 
            SolidColorBrush textColor, Size fontDimensions)
        {
            List<TextBlock> textBlocks = new List<TextBlock>();

            int textBlockQTY = strings.Length;

            
            double topOffset = 1;
            double leftOffset = 0;

            for (int count = 0; count < textBlockQTY; count++)
            {
                textBlocks.Add(new TextBlock());

                textBlocks.Last().Width = canvasSize.Width;
                textBlocks.Last().Height = fontDimensions.Height;

                textBlocks.Last().TextAlignment = TextAlignment.Center;

                if (count == 0)
                {
                    Canvas.SetTop(textBlocks.Last(), topOffset);
                    Canvas.SetLeft(textBlocks.Last(), leftOffset);
                }

                else
                {
                    Canvas.SetTop(textBlocks.Last(), textBlocks.Last().Height * count);
                    Canvas.SetLeft(textBlocks.Last(), leftOffset);
                }
            }

            // Add Text and set Parameters.
            int index = 0;
            foreach (var element in textBlocks)
            {
                element.Text = strings[index];
                element.FontFamily = typeface.FontFamily;
                element.FontStyle = typeface.Style;
                element.FontStretch = typeface.Stretch;
                element.FontWeight = typeface.Weight;
                element.FontSize = fontSize;
                element.Foreground = textColor;

                index++;
            }

            return textBlocks.ToArray();
        }

        // Overload, For Single String use.
        private TextBlock GenerateTextBlocks(Size canvasSize, string text, Typeface typeface, double fontSize,
            SolidColorBrush textColor, Size fontDimensions)
        {
            TextBlock returnBlock = new TextBlock();

            double yOffset = (canvasSize.Height - fontDimensions.Height) / 2;
            double xOffset = (canvasSize.Width - fontDimensions.Width) / 2;

            Canvas.SetTop(returnBlock, yOffset);
            Canvas.SetLeft(returnBlock, xOffset);

            // Add Text and set Parameters.
            returnBlock.Text = text;
            returnBlock.TextAlignment = TextAlignment.Center;
            returnBlock.FontFamily = typeface.FontFamily;
            returnBlock.FontStyle = typeface.Style;
            returnBlock.FontStretch = typeface.Stretch;
            returnBlock.FontWeight = typeface.Weight;
            returnBlock.FontSize = fontSize;
            returnBlock.Foreground = textColor;

            return returnBlock;
        }

        // Generates Blank TextBlocks for Footer Rendering. Top to Bottom.
        private TextBlock[] GenerateFooterTextBlocks(Canvas canvas, SolidColorBrush textColor)
        {
            List<TextBlock> textBlocks = new List<TextBlock>();

            double textBlockWidth = canvas.Width;
            double textBlockHeight = canvas.Height / 3;

            for (int count = 0; count < 3; count++)
            {
                textBlocks.Add(new TextBlock());

                textBlocks.Last().Width = textBlockWidth;
                textBlocks.Last().Height = textBlockHeight;

                textBlocks.Last().TextAlignment = TextAlignment.Center;

                if (count == 0)
                {
                    Canvas.SetTop(textBlocks.Last(), 0);
                }

                else
                {
                    Canvas.SetTop(textBlocks.Last(), textBlockHeight * count);
                }
            }

            return textBlocks.ToArray();
            
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
