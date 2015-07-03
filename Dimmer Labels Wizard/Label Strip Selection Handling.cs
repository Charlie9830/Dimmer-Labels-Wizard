﻿using System;
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
                SelectedFooters.Clear();
            }
        }

        private LabelStrip label = new LabelStrip();

        public RectangleF HeaderStripOutline = new RectangleF();
        public RectangleF FooterStripOutline = new RectangleF();

        // Public List. Populated by SelectFooterCells() Method.
        public List<FooterSelection> SelectedFooters = new List<FooterSelection>();
        public List<HeaderSelection> SelectedHeaders = new List<HeaderSelection>();

        // Populated by FORM_LabelEditor Render(), Originally Generated by LabelStrip.RenderToDisplay().
        public List<HeaderSelection> RenderedHeaders = new List<HeaderSelection>();
        public List<FooterSelection> RenderedFooters = new List<FooterSelection>();

        // Used by the SplitCells Dialog.
        public int SelectedSplitIndex;

        // Populate/Update SeletedHeaderCells List based off Mouse Click Location.
        public void SelectHeaderCells(Point mouseClickLocation)
        {
            bool selectionFound = false;

            foreach (var element in RenderedHeaders)
            {
                if (element.Outline.Contains(mouseClickLocation))
                {
                    // Remove from Selected list if it is already selected.
                    if (SelectedHeaders.Contains(element))
                    {
                        element.IsSelected = false;
                        SelectedHeaders.Remove(element);
                        selectionFound = true;
                        break;
                    }

                    // Otherwise add it to the Selected List.
                    element.IsSelected = true;
                    SelectedHeaders.Add(element);
                    selectionFound = true;
                    break;
                }
            }
            // User has clicked on the labelCanvas Outside the Rectangles. Clear the selections.
            if (selectionFound == false)
            {
                foreach (var element in SelectedHeaders)
                {
                    element.IsSelected = false;
                }
                SelectedHeaders.Clear();
            }
        }

        // Joins Header Cell Selections if they reside between two Mouse Click Locations.
        public void SelectHeaderCells(Point previousMouseClickLocation, Point currentMouseClickLocation)
        {
            int previousIndex = RenderedHeaders.FindIndex(item => item.Outline.Contains(previousMouseClickLocation));
            int currentIndex = RenderedHeaders.FindIndex(item => item.Outline.Contains(currentMouseClickLocation));

            if (previousIndex == -1)
            {
                previousIndex = previousMouseClickLocation.X < HeaderStripOutline.Left ? 0 : RenderedHeaders.Count - 1;
            }

            if (currentIndex == -1)
            {
                currentIndex = currentMouseClickLocation.X > HeaderStripOutline.Right ? RenderedHeaders.Count - 1 : 0;
            }

            if (previousIndex < currentIndex)
            {
                for (int index = previousIndex; index <= currentIndex; index++)
                {
                    RenderedHeaders[index].IsSelected = true;
                    SelectedHeaders.Add(RenderedHeaders[index]);
                }
            }

            if (currentIndex < previousIndex)
            {
                for (int index = currentIndex; index <= previousIndex; index++)
                {
                    RenderedHeaders[index].IsSelected = true;
                    SelectedHeaders.Add(RenderedHeaders[index]);
                }
            }

            if (currentIndex == previousIndex && currentMouseClickLocation != previousMouseClickLocation)
            {
                RenderedHeaders[currentIndex].IsSelected = true;
                SelectedHeaders.Add(RenderedHeaders[currentIndex]);
            }
        }

        // Populate/Update SelectedFooters List based off Mouse Click Location
        public void SelectFooterCells(Point mouseClickLocation)
        {
            bool selectionFound = false;

            foreach (var element in RenderedFooters)
            {
                if (element.Outline.Contains(mouseClickLocation))
                {
                    // Remove Selection from List if Already existing.
                    if (SelectedFooters.Contains(element))
                    {
                        SelectedFooters.Remove(element);
                        element.IsSelected = false;
                        selectionFound = true;
                        break;
                    }

                    // Add it If it hasn't already been selected.
                    SelectedFooters.Add(element);
                    element.IsSelected = true;
                    selectionFound = true;
                    break;
                }
            }

            // User has clicked on the labelCanvas Outside the Rectangles. Clear the selections.
            if (selectionFound == false)
            {
                foreach(var element in SelectedFooters)
                {
                    element.IsSelected = false;
                }
                SelectedFooters.Clear();
            }
        }

        // Joins Footer Cell Selections if they Reside between to MouseClick Locations.
        public void SelectFooterCells(Point previousMouseClickLocation, Point currentMouseClickLocation)
        {
            int previousIndex = RenderedFooters.FindIndex(item => item.Outline.Contains(previousMouseClickLocation));
            int currentIndex = RenderedFooters.FindIndex(item => item.Outline.Contains(currentMouseClickLocation));

            if (previousIndex == -1)
            {
                previousIndex = previousMouseClickLocation.X < HeaderStripOutline.Left ? 0 : RenderedFooters.Count - 1;
            }

            if (currentIndex == -1)
            {
                currentIndex = currentMouseClickLocation.X > HeaderStripOutline.Right ? RenderedFooters.Count - 1 : 0;
            }

            if (previousIndex < currentIndex)
            {
                for (int index = previousIndex; index <= currentIndex; index++)
                {
                    RenderedFooters[index].IsSelected = true;
                    SelectedFooters.Add(RenderedFooters[index]);
                }
            }

            if (currentIndex < previousIndex)
            {
                for (int index = currentIndex; index <= previousIndex; index++)
                {
                    RenderedFooters[index].IsSelected = true;
                    SelectedFooters.Add(RenderedFooters[index]);
                }
            }

            if (currentIndex == previousIndex && currentMouseClickLocation != previousMouseClickLocation)
            {
                RenderedFooters[currentIndex].IsSelected = true;
                SelectedFooters.Add(RenderedFooters[currentIndex]);
            }
        }

        public void ClearSelections()
        {
            
            foreach (var element in SelectedFooters)
            {
                element.IsSelected = false;
            }
            SelectedFooters.Clear();
            
            foreach (var element in SelectedHeaders)
            {
                element.IsSelected = false;
            }
            SelectedHeaders.Clear();
        }

        // Returns the Location of Each Cell Split. Returns Points with Coords 0,0 
        // in place of existing cell Borders.
        public List<Point> DetermineCellSplitPoints()
        {
            List<Point> returnValues = new List<Point>();
            List<Point> existingCellSplits = new List<Point>();

            // Collect Existing Cell Split Points
            foreach (var element in RenderedHeaders)
            {
                existingCellSplits.Add(new Point((int)Math.Round(element.Outline.Right), 0));
            }

            // Collect Point Locations
            foreach (var element in RenderedFooters)
            {
                PointF splitPoint = new PointF(element.Outline.Right,RenderedHeaders.First().Outline.Location.Y);

                // Add the point to the Return Values List if a Drawn Cell Split does not already
                // exist in it's location.
                if (existingCellSplits.Exists(item => item.X == splitPoint.X ||
                    item.X - 3 <= splitPoint.X && item.X + 3 >= splitPoint.X) == false)
                {
                    returnValues.Add(Point.Round(splitPoint));
                }

                else
                {
                    returnValues.Add(new Point(0,0));
                }
            }

            return returnValues;
        }
    }

    public class HeaderSelection : IEquatable<HeaderSelection>
    {
        public RectangleF Outline;
        public List<HeaderCell> Cells = new List<HeaderCell>();
        public int HashCode;

        public HeaderSelection(int headerHashCode)
        {
            HashCode = headerHashCode;
        }

        public bool Equals(HeaderSelection other)
        {
            if (HashCode == other.HashCode)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                if (value == true)
                {
                    SetSelectionStates(true);
                    isSelected = true;
                }

                else
                {
                    SetSelectionStates(false);
                    isSelected = false;
                }
            }
        }
        
        private bool isSelected;

        private void SetSelectionStates(bool state)
        {
            foreach (var element in Cells)
            {
                element.IsSelected = state;
            }
        }
    }

    public class FooterSelection : IEquatable<FooterSelection>
    {
        public RectangleF Outline;
        public FooterCell Cell { get; set; }
        public int HashCode;

        public FooterSelection(int footerHashCode)
        {
            HashCode = footerHashCode;
        }

        public bool Equals(FooterSelection other)
        {
            if (HashCode == other.HashCode)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                if (value == true)
                {
                    SetSelectionStates(true);
                    isSelected = true;
                }

                else
                {
                    SetSelectionStates(false);
                    isSelected = false;
                }
            }
        }

        private bool isSelected;

        private void SetSelectionStates(bool state)
        {
            Cell.IsSelected = state;
        }
    }

    // Provides Multiple Type Return Functionaility to LabelStrip.RenderToDisplay() Method. Encapsulates
    // HeaderSelection and FooterSelection Objects.
    public class UserLabelSelection
    {
        public List<HeaderSelection> HeaderSelections = new List<HeaderSelection>();
        public List<FooterSelection> FooterSelections = new List<FooterSelection>();
    }
}
