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
        public List<HeaderCell> SelectedHeaderCells = new List<HeaderCell>();

        // Populated by RenderToDisplay()'s return Value.
        public List<RectangleF> HeaderOutlines = new List<RectangleF>();
        public List<RectangleF> FooterOutlines = new List<RectangleF>();
        
        // Populate SeletedHeaderCells List based off Mouse Click Location.
        // Returns False If a Selection Could Not be found.
        public bool UpdateSelectedHeaderCells(Point mouseClickLocation)
        {
            bool selectionFound = false;

            foreach (var element in HeaderOutlines)
            {
                if (element.Contains(mouseClickLocation))
                {
                    int elementIndex = HeaderOutlines.IndexOf(element);

                    // Remove Selection from List if Already existing.
                    if (SelectedHeaderCells.Contains(LabelStrip.Headers[elementIndex]))
                    {
                        SelectedHeaderCells.Remove(LabelStrip.Headers[elementIndex]);
                        selectionFound = true;
                        break;
                    }

                    // Add it If it hasn't already been selected.
                    SelectedHeaderCells.Add(LabelStrip.Headers[elementIndex]);
                    selectionFound = true;
                    break;
                }
            }

            // User has clicked on the Canvas Outside the Rectangles. Clear the selections.
            if (selectionFound == false)
            {
                SelectedHeaderCells.Clear();
            }

            return selectionFound;
        }

        // Populate SelectedFooterCells List based off Mouse Click Location
        // Returns False if a Selection Could not be found.
        public bool UpdateSelectedFooterCells(Point mouseClickLocation)
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

            return selectionFound;
        }

        public void ClearSelections()
        {
            SelectedFooterCells.Clear();
            SelectedHeaderCells.Clear();
        }
    }

    public struct HeaderSelectionZone
    {
        public HeaderSelectionZone(RectangleF outline,HeaderCell header)
        {
            Outline = outline;
            Header = header;
        }

        public RectangleF Outline;
        public HeaderCell Header;
    }

    public struct FooterSelectionZone
    {
        public FooterSelectionZone(RectangleF outline, FooterCell footer)
        {
            Outline = outline;
            Footer = footer;
        }

        public RectangleF Outline;
        public RectangleF Footer;
    }
}
