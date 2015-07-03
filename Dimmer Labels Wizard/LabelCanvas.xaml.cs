using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace Dimmer_Labels_Wizard
{
    /// <summary>
    /// Interaction logic for LabelCanvas.xaml
    /// </summary>
    public partial class LabelCanvas : UserControl
    {
        public List<Border> Outlines = new List<Border>();
        public List<Canvas> textCanvases = new List<Canvas>();

        public LabelCanvas()
        {
            InitializeComponent();

            Canvas.Background = Brushes.White;
            RenderStuff(Canvas);
        }

        public void RenderLabelStrip(Canvas canvas, Point origin)
        {
            // Initialize Resources

            // Header Rendering.
                // List of Outlines.

                // List of TextBlocks
                    // Will be Cleared after being added to canvas.Children after each Header
                    // cell has been rendered.

            // Header Render Loop.
                // Determine Right Bound Position.
                // Set Size and Position of outline Border.
                // Determine String Measurements.
                    // Analyze String Data. Determine:
                        // If it will Fit At it's current Size without Line Breaks within the Border.
                            // If not, Can Line Breaks be inserted?
                                // If So, Downsize Font based of Longest new Broken String.
                    // Based off how many Lines of Data are required, Create and Position TextBlocks.
        }

        public void RenderStuff(Canvas canvas)
        {
            SolidColorBrush outlineStroke = new SolidColorBrush(Colors.Black);
            SolidColorBrush fillColor = new SolidColorBrush(Colors.Yellow);

            int XPos = 40;
            int YPos = 10;
            int width = 75;
            int height = 75;

            for (int count = 1; count <= 12; count++)
            {

                List<TextBlock> textBlocks = new List<TextBlock>();

                Outlines.Add(new Border());

                double strokeThickness = 2;

                Outlines.Last().Width = width;
                Outlines.Last().Height = height;

                Canvas.SetLeft(Outlines.Last(), XPos + width + (strokeThickness * 2));
                Canvas.SetTop(Outlines.Last(), YPos);

                Outlines.Last().BorderBrush = outlineStroke;
                Outlines.Last().BorderThickness = new Thickness(strokeThickness);


                Outlines.Last().Background = fillColor;

                double textBlockQTY = 3;

                textBlocks.Add(new TextBlock());
                textBlocks.Last().Background = Brushes.Transparent;
                textBlocks.Last().Height = (height / textBlockQTY) - strokeThickness;
                textBlocks.Last().Width = width - strokeThickness;
                textBlocks.Last().Text = "Hello";
                textBlocks.Last().TextAlignment = TextAlignment.Center;
                textBlocks.Last().FontSize *= 2;
                Canvas.SetTop(textBlocks.Last(), 0);
                Canvas.SetLeft(textBlocks.Last(), 0);

                textBlocks.Add(new TextBlock());
                textBlocks.Last().Background = Brushes.Transparent;
                textBlocks.Last().Height = (height / textBlockQTY) - strokeThickness;
                textBlocks.Last().Width = width - strokeThickness;
                textBlocks.Last().Text = "World";
                textBlocks.Last().TextAlignment = TextAlignment.Center;
                textBlocks.Last().FontSize *= 2;
                Canvas.SetTop(textBlocks.Last(), 10);
                Canvas.SetLeft(textBlocks.Last(), 0);

                textBlocks.Add(new TextBlock());
                textBlocks.Last().Background = Brushes.Transparent;
                textBlocks.Last().Height = (height / textBlockQTY) - strokeThickness;
                textBlocks.Last().Width = width - strokeThickness;
                textBlocks.Last().Text = "Again";
                textBlocks.Last().TextAlignment = TextAlignment.Center;
                textBlocks.Last().FontSize *= 2;
                Canvas.SetTop(textBlocks.Last(), 20);
                Canvas.SetLeft(textBlocks.Last(), 0);

                textCanvases.Add(new Canvas());
                textCanvases.Last().Height = Outlines.Last().Height - 
                    BorderThickness.Top - BorderThickness.Bottom;

                textCanvases.Last().Width = Outlines.Last().Width - 
                    BorderThickness.Right - BorderThickness.Left;

                foreach (var element in textBlocks)
                {
                    textCanvases.Last().Children.Add(element);
                }

                Outlines.Last().Child = textCanvases.Last();

                Canvas.Children.Add(Outlines.Last());

                XPos += width;
            }
        }
        
    }
}
