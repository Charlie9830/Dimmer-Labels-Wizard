using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dimmer_Labels_Wizard
{
    public class LabelStripSelection
    {
        public LabelStrip LabelStrip
        {
            get
            {
                return label;
            }

            set
            {
                label = value;
                SelectedFooterCells.Clear();
            }
        }

        private LabelStrip label = new LabelStrip();

        // Public List. Populated by UpdateSelectedFooterCells() Method.
        public List<FooterCell> SelectedFooterCells = new List<FooterCell>();

        // Populated by RenderToDisplay()'s return Value.
        public List<RectangleF> HeaderOutlines = new List<RectangleF>();
        public List<RectangleF> FooterOutlines = new List<RectangleF>();
        
        

        // Populate FooterCellSelectionIndexes based off Mouse Click Location.
        public void UpdateSelectedFooterCells(Point mouseClickLocation)
        {
            bool selectionFound = false;

            foreach (var element in FooterOutlines)
            {
                if (element.Contains(mouseClickLocation))
                {
                    int elementIndex = FooterOutlines.IndexOf(element);

                    // Remove Selection from List if Already existing.
                    if (SelectedFooterCells.Contains(LabelStrip.Footers[elementIndex]))
                    {
                        SelectedFooterCells.Remove(LabelStrip.Footers[elementIndex]);
                        selectionFound = true;
                        break;
                    }

                    // Add it If it hasn't already been selected.
                    SelectedFooterCells.Add(LabelStrip.Footers[elementIndex]);
                    selectionFound = true;
                    break;
                }
            }

            // User has clicked on the Canvas Outside the Rectangles. Clear the selections.
            if (selectionFound == false)
            {
                SelectedFooterCells.Clear();
            }
        }

        public void ClearSelections()
        {
            SelectedFooterCells.Clear();
        }
    }
}
