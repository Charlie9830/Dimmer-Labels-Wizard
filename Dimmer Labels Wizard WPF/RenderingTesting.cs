using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class DrawTest : ContentControl
    {
        public DrawTest()
        {
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            var brush = new SolidColorBrush(Colors.White);
            var penBrush = new SolidColorBrush(Colors.Black);
            var pen = new Pen(penBrush, 1);
            var rect = new Rect(new Size(ActualWidth,ActualHeight));
            drawingContext.DrawRectangle(brush, pen, rect);
        }
    }
}
