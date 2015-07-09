using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard
{
    public class LabelStripSelection
    {
        public LabelStrip LabelStrip;
        public List<HeaderCell> SelectedHeaders = new List<HeaderCell>();
        public List<FooterCell> SelectedFooters = new List<FooterCell>();

        public void MakeSelection(object selectionOutline, Canvas labelCanvas)
        {
            Border outline = (Border)selectionOutline;

            if (outline.Tag.GetType() == typeof(HeaderCellWrapper))
            {
                HeaderCellWrapper wrapper = (HeaderCellWrapper)outline.Tag;
                List<HeaderCell> headerCells = wrapper.Cells;

                foreach (var element in headerCells)
                {
                    if (SelectedHeaders.Contains(element) == false)
                    {
                        // Add it as a Selection.
                        SelectedHeaders.Add(element);
                    }

                    else
                    {
                        // Remove it from Selections.
                        SelectedHeaders.Remove(element);
                    }
                }

                OnSelectedHeadersChanged(new EventArgs());
            }

            if (outline.Tag.GetType() == typeof(FooterCell))
            {
                FooterCell footerCell = (FooterCell)outline.Tag;

                if (SelectedFooters.Contains(footerCell) == false)
                {
                    // Add it as a Selection.
                    SelectedFooters.Add(footerCell);
                }

                else
                {
                    // Remove it from Selections.
                    SelectedFooters.Remove(footerCell);
                }

                OnSelectedFootersChanged(new EventArgs());
            }

            RenderSelections(labelCanvas);
        }

        public void RenderSelections(Canvas labelCanvas)
        {
            foreach (var element in labelCanvas.Children)
            {
                Border outline = (Border)element;
                if (outline.Tag.GetType() == typeof(HeaderCellWrapper))
                {
                    HeaderCellWrapper wrapper = (HeaderCellWrapper)outline.Tag;
                    List<HeaderCell> headerCells = wrapper.Cells;

                    foreach (var cell in headerCells)
                    if (SelectedHeaders.Contains(cell))
                    {
                        outline.BorderBrush = SystemColors.HighlightBrush;
                    }

                    else
                    {
                        outline.BorderBrush = new SolidColorBrush(Colors.Black);
                    }
                }

                if (outline.Tag.GetType() == typeof(FooterCell))
                {
                    if (SelectedFooters.Contains(outline.Tag))
                    {
                        outline.BorderBrush = SystemColors.HighlightBrush;
                    }

                    else
                    {
                        outline.BorderBrush = new SolidColorBrush(Colors.Black);
                    }
                }


            }
        }

        public void ClearSelections()
        {
            SelectedHeaders.Clear();
            OnSelectedHeadersChanged(new EventArgs());

            SelectedFooters.Clear();
            OnSelectedFootersChanged(new EventArgs());
        }

        #region External Events
        public event EventHandler SelectedHeadersChanged;
        public event EventHandler SelectedFootersChanged;

        protected void OnSelectedHeadersChanged(EventArgs e)
        {
            SelectedHeadersChanged(this, e);
        }

        protected void OnSelectedFootersChanged(EventArgs e)
        {
            SelectedFootersChanged(this, e);
        }
        #endregion
    }
}
