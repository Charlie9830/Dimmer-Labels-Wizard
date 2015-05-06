// Draw the Headers
            for (int i = 0; i < this.Headers.Count; i++)
            {
                // Determime the Right Bound Position.
                for (int j = i; j < this.Headers.Count; j++)
                    if (j + 1 < this.Headers.Count &&
                        this.Headers[j].MiddleData == this.Headers[j + 1].MiddleData)
                    {
                        rightBoundPosition += 1;
                    }
                    else
                    {
                        break;
                    }

                // Assign the Size Values to the Outline Rectangle
                headerOutline.Height = defaultLabelHeight;
                headerOutline.Width = defaultLabelWidth * rightBoundPosition;


                // Position the Rectangle
                headerOutline.X = headerPosition.X;
                headerOutline.Y = headerPosition.Y;

                // Draw the Background Colour
                graphics.FillRectangle(this.Headers[i].BackgroundColor, headerOutline);

                // Draw the Outline
                graphics.DrawRectangle(outlineColor, headerOutline);

                // Store the Right hand Boundry
                headerPosition.X = headerOutline.Right;

                // Collect the Objects Font Value
                Font middleFontBuffer = (Font) this.Headers[i].MiddleFont.Clone();

                // Will the String fit within the Outline Rectangle?
                float differenceRatio = StringRatio(this.Headers[i].MiddleData, this.Headers[i].MiddleFont, headerOutline, graphics);
                if (differenceRatio != 1)
                {
                    middleFontBuffer = new Font(middleFontBuffer.Name, middleFontBuffer.Size * differenceRatio,middleFontBuffer.Style);
                }

                // Draw the String
                graphics.DrawString(this.Headers[i].MiddleData, middleFontBuffer, this.Headers[i].TextColor, headerOutline, this.Headers[i].MiddleFormat);

                // Reset Everything
                i += (rightBoundPosition - 1);
                rightBoundPosition = 1;
                headerOutline.Width = defaultLabelWidth;
            }
