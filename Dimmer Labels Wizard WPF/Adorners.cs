using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class SelectionAdorner : Adorner
    {
        public SelectionAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.DesiredSize);

            Brush selectionBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));
            Pen selectionPen = new Pen();

            drawingContext.DrawRectangle(selectionBrush, selectionPen, adornedElementRect);
        }

        public void ForceInvalidate()
        {
            InvalidateVisual();
        }
    }
}
