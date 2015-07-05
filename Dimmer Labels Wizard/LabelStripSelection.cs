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

        public void MakeSelection(object selectionOutline)
        {
            Border outline = (Border)selectionOutline;

            if (outline.Tag.GetType() == typeof(HeaderCell))
            {
                HeaderCell headerCell = (HeaderCell)outline.Tag;

                if (SelectedHeaders.Contains(headerCell) == false)
                {
                    // Add it as a Selection.
                    SelectedHeaders.Add(headerCell);
                    outline.BorderBrush = SystemColors.HighlightBrush;
                }

                else
                {
                    // Remove it from Selections.
                    SelectedHeaders.Remove(headerCell);
                    outline.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }

            if (outline.Tag.GetType() == typeof(FooterCell))
            {
                FooterCell footerCell = (FooterCell)outline.Tag;

                if (SelectedFooters.Contains(footerCell) == false)
                {
                    // Add it as a Selection.
                    SelectedFooters.Add(footerCell);
                    outline.BorderBrush = SystemColors.HighlightBrush;
                }

                else
                {
                    // Remove it from Selections.
                    SelectedFooters.Remove(footerCell);
                    outline.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }

        public void ClearSelections()
        {
            SelectedHeaders.Clear();
            SelectedFooters.Clear();
        }
    }
}
