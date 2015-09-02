using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Documents;
using System.Collections.ObjectModel;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelStripSelection
    {
        public LabelStrip LabelStrip;
        public List<HeaderCell> SelectedHeaders = new List<HeaderCell>();
        public List<FooterCell> SelectedFooters = new List<FooterCell>();

        // These lists are expicitly linked and Tracking SelectedHeaders and SelectedFooters
        private List<SelectionAdorner> _HeaderAdorners = new List<SelectionAdorner>();
        private List<SelectionAdorner> _FooterAdorners = new List<SelectionAdorner>();

        private Canvas _LabelStripCanvas;
        private AdornerLayer _AdornerLayer;

        public LabelStripSelection()
        {
        }

        public void SetSelectedLabelStrip(LabelStrip labelStrip, Canvas labelStripCanvas)
        {
            LabelStrip = labelStrip;
            _LabelStripCanvas = labelStripCanvas;
            _AdornerLayer = AdornerLayer.GetAdornerLayer(labelStripCanvas);
        }

        public void MakeSelection(object selectionOutline)
        {
            Border outline = (Border)selectionOutline;

            if (outline.Tag != null)
            {
                if (outline.Tag.GetType() == typeof(HeaderCellWrapper))
                {
                    HeaderCellWrapper wrapper = (HeaderCellWrapper)outline.Tag;
                    List<HeaderCell> headerCells = wrapper.Cells;

                    foreach (var element in headerCells)
                    {
                        if (SelectedHeaders.Contains(element) == false)
                        {
                            // Add it as a Selection.
                            AddHeaderAdorner(outline);
                            SelectedHeaders.Add(element);
                        }

                        else
                        {
                            // Remove it from Selections.
                            RemoveHeaderAdorner(outline);
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
                        AddFooterAdorner(outline);
                        SelectedFooters.Add(footerCell);
                    }

                    else
                    {
                        // Remove it from Selections.
                        RemoveFooterAdorner(outline);
                        SelectedFooters.Remove(footerCell);
                    }
                    OnSelectedFootersChanged(new EventArgs());
                }
            }
        }

        public void MakeSelections(List<Border> selectionOutlines)
        {
            bool headerUpdateRequired = false;
            bool footerUpdateRequired = false;

            foreach (var selection in selectionOutlines)
            {
                Border outline = selection;

                if (outline.Tag != null)
                {
                    if (outline.Tag.GetType() == typeof(HeaderCellWrapper))
                    {
                        HeaderCellWrapper wrapper = (HeaderCellWrapper)outline.Tag;
                        List<HeaderCell> headerCells = wrapper.Cells;

                        foreach (var element in headerCells)
                        {
                            if (SelectedHeaders.Contains(element) == false)
                            {
                                // Add it as a Selection.
                                AddHeaderAdorner(outline);
                                SelectedHeaders.Add(element);
                            }

                            else
                            {
                                // Remove it from Selections.
                                RemoveHeaderAdorner(outline);
                                SelectedHeaders.Remove(element);
                            }
                        }

                        headerUpdateRequired = true;
                    }

                    if (outline.Tag.GetType() == typeof(FooterCell))
                    {
                        FooterCell footerCell = (FooterCell)outline.Tag;

                        if (SelectedFooters.Contains(footerCell) == false)
                        {
                            // Add it as a Selection.
                            AddFooterAdorner(outline);
                            SelectedFooters.Add(footerCell);
                        }

                        else
                        {
                            // Remove it from Selections.
                            RemoveFooterAdorner(outline);
                            SelectedFooters.Remove(footerCell);
                        }

                        footerUpdateRequired = true;
                    }
                }
            }

            if (headerUpdateRequired)
            {
                OnSelectedHeadersChanged(new EventArgs());
            }

            if (footerUpdateRequired)
            {
                OnSelectedFootersChanged(new EventArgs());
            }
        }

        public void ClearSelections()
        {
            SelectedHeaders.Clear();
            ClearHeaderAdorners();
            OnSelectedHeadersChanged(new EventArgs());

            SelectedFooters.Clear();
            ClearFooterAdorners();
            OnSelectedFootersChanged(new EventArgs());
        }

        #region AdornerHandling
        public void ReAttachAdorners(Canvas labelCanvas)
        {
            foreach (var child in labelCanvas.Children)
            {
                if (child.GetType() == typeof(Border))
                {
                    Border outline = child as Border;

                    if (outline.Tag.GetType() == typeof(HeaderCellWrapper))
                    {
                        HeaderCellWrapper wrapper = outline.Tag as HeaderCellWrapper;
                        HeaderCell headerCell = wrapper.Cells.First();

                        if (SelectedHeaders.Contains(headerCell))
                        {
                            AddHeaderAdorner(outline);
                        }
                    }

                    if (outline.Tag.GetType() == typeof(FooterCell))
                    {
                        FooterCell footerCell = outline.Tag as FooterCell;

                        if (SelectedFooters.Contains(footerCell))
                        {
                            AddFooterAdorner(outline);
                        }
                    }
                }
            }
        }

        public void RefreshAdorners()
        {
            foreach (var element in _HeaderAdorners)
            {
                element.ForceInvalidate();
            }

            foreach (var element in _FooterAdorners)
            {
                element.ForceInvalidate();
            }
        }

        private void RemoveHeaderAdorner(Border outline)
        {
            SelectionAdorner adornerToRemove = _HeaderAdorners.Find(item => item.AdornedElement == outline);

            if (adornerToRemove != null)
            {
                _AdornerLayer.Remove(adornerToRemove);
                _HeaderAdorners.Remove(adornerToRemove);
            }
        }

        private void AddHeaderAdorner(Border outline)
        {
            if (_HeaderAdorners.Find(item => item.AdornedElement == outline) == null)
            {
                SelectionAdorner adornerToAdd = new SelectionAdorner(outline);
                adornerToAdd.IsHitTestVisible = false;
                _AdornerLayer.Add(adornerToAdd);
                _HeaderAdorners.Add(adornerToAdd);
            }
        }

        private void ClearHeaderAdorners()
        {
            foreach (var element in _HeaderAdorners)
            {
                _AdornerLayer.Remove(element);
            }

            _HeaderAdorners.Clear();
        }

        private void RemoveFooterAdorner(Border outline)
        {
            SelectionAdorner adornerToRemove = _FooterAdorners.Find(item => item.AdornedElement == outline);

            if (adornerToRemove != null)
            {
                _AdornerLayer.Remove(adornerToRemove);
                _FooterAdorners.Remove(adornerToRemove);
            }
        }

        private void AddFooterAdorner(Border outline)
        {
            if (_FooterAdorners.Find(item => item.AdornedElement == outline) == null)
            {
                SelectionAdorner adornerToAdd = new SelectionAdorner(outline);
                adornerToAdd.IsHitTestVisible = false;
                _AdornerLayer.Add(adornerToAdd);
                _FooterAdorners.Add(adornerToAdd);
            }
        }

        private void ClearFooterAdorners()
        {
            foreach (var element in _FooterAdorners)
            {
                _AdornerLayer.Remove(element);
            }

            _FooterAdorners.Clear();
        }

        #endregion

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
