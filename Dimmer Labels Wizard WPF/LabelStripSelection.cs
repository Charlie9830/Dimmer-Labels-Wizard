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

        public List<FooterCellText> SelectedFooterCellText = new List<FooterCellText>();
        public List<HeaderCellWrapper> SelectedHeaderCellText = new List<HeaderCellWrapper>();

        // These lists are expicitly linked and Tracking SelectedHeaders and SelectedFooters
        private List<SelectionAdorner> _HeaderAdorners = new List<SelectionAdorner>();
        private List<SelectionAdorner> _FooterAdorners = new List<SelectionAdorner>();

        private Canvas _LabelStripCanvas;
        private AdornerLayer _AdornerLayer;

        #region Getters/Setters
        public bool HasSelectedItems
        {
            get
            {
                if (SelectedHeaders.Count > 0 || SelectedFooters.Count > 0 ||
                    SelectedHeaderCellText.Count > 0 || SelectedFooterCellText.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
        #endregion

        public LabelStripSelection()
        {
        }

        public void SetSelectedLabelStrip(LabelStrip labelStrip, Canvas labelStripCanvas)
        {
            LabelStrip = labelStrip;
            _LabelStripCanvas = labelStripCanvas;
            _AdornerLayer = AdornerLayer.GetAdornerLayer(labelStripCanvas);
        }

        public void MakeSelection(Border selectedBorder)
        {
            if (selectedBorder.Tag != null)
            {
                if (selectedBorder.Tag.GetType() == typeof(HeaderCellWrapper))
                {
                    HeaderCellWrapper wrapper = (HeaderCellWrapper)selectedBorder.Tag;
                    List<HeaderCell> headerCells = wrapper.Cells;

                    foreach (var element in headerCells)
                    {
                        if (SelectedHeaders.Contains(element) == false)
                        {
                            // Add it as a Selection.
                            AddHeaderAdorner(selectedBorder);
                            SelectedHeaders.Add(element);
                        }

                        else
                        {
                            // Remove it from Selections.
                            RemoveHeaderAdorner(selectedBorder);
                            SelectedHeaders.Remove(element);
                        }
                    }
                    OnSelectedHeadersChanged(new EventArgs());
                }

                if (selectedBorder.Tag.GetType() == typeof(FooterCell))
                {
                    FooterCell footerCell = (FooterCell)selectedBorder.Tag;

                    if (SelectedFooters.Contains(footerCell) == false)
                    {
                        // Add it as a Selection.
                        AddFooterAdorner(selectedBorder);
                        SelectedFooters.Add(footerCell);
                    }

                    else
                    {
                        // Remove it from Selections.
                        RemoveFooterAdorner(selectedBorder);
                        SelectedFooters.Remove(footerCell);
                    }
                    OnSelectedFootersChanged(new EventArgs());
                }
            }
        }

        public void MakeSelection(TextBlock selectedTextBlock)
        {
            if (selectedTextBlock.Tag != null)
            {
                if (selectedTextBlock.Tag is FooterCellText)
                {
                    var wrapper = selectedTextBlock.Tag as FooterCellText;

                    if (SelectedFooterCellText.Contains(wrapper) == false)
                    {
                        // Add it as a Selection.
                        AddFooterAdorner(selectedTextBlock);
                        SelectedFooterCellText.Add(wrapper);
                    }

                    else
                    {
                        // Remove it from Selections.
                        RemoveFooterAdorner(selectedTextBlock);
                        SelectedFooterCellText.Remove(wrapper);
                    }

                    OnSelectedFooterCellTextChanged(new EventArgs());
                }

                if (selectedTextBlock.Tag is HeaderCellWrapper)
                {
                    var wrapper = selectedTextBlock.Tag as HeaderCellWrapper;
                    if (SelectedHeaderCellText.Contains(wrapper) == false)
                    {
                        // Add all associated Textblocks as selections.
                        foreach (var element in wrapper.TextBlocks)
                        {
                            AddHeaderAdorner(element);
                        }

                        SelectedHeaderCellText.Add(wrapper);
                    }

                    else
                    {
                        // Remove all associated Textblocks from Selections.
                        foreach (var element in wrapper.TextBlocks)
                        {
                            RemoveHeaderAdorner(element);
                        }

                        SelectedHeaderCellText.Remove(wrapper);
                    }

                    OnSelectedHeaderCellTextChanged(new EventArgs());
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

        public void MakeSelections(List<TextBlock> selectedTextBlocks)
        {
            bool headerUpdateRequired = false;
            bool footerUpdateRequired = false;

            foreach (var selection in selectedTextBlocks)
            {
                if (selection.Tag != null)
                {
                    if (selection.Tag is FooterCellText)
                    {
                        var wrapper = selection.Tag as FooterCellText;

                        if (SelectedFooterCellText.Contains(wrapper) == false)
                        {
                            // Add it as Selection.
                            AddFooterAdorner(selection);
                            SelectedFooterCellText.Add(wrapper);
                        }

                        else
                        {
                            // Remove it From Selection.
                            RemoveFooterAdorner(selection);
                            SelectedFooterCellText.Remove(wrapper);
                        }

                        footerUpdateRequired = true;
                    }

                    if (selection.Tag is HeaderCellWrapper)
                    {
                        var wrapper = selection.Tag as HeaderCellWrapper;
                        if (SelectedHeaderCellText.Contains(wrapper) == false)
                        {
                            // Add all associated Textblocks as selections.
                            foreach (var element in wrapper.TextBlocks)
                            {
                                AddHeaderAdorner(element);
                            }

                            SelectedHeaderCellText.Add(wrapper);
                        }

                        else
                        {
                            // Remove all associated Textblocks from Selections.
                            foreach (var element in wrapper.TextBlocks)
                            {
                                RemoveHeaderAdorner(element);
                            }

                            SelectedHeaderCellText.Remove(wrapper);
                        }

                        headerUpdateRequired = true;
                    }
                }
            }

            if (headerUpdateRequired == true)
            {
                OnSelectedHeaderCellTextChanged(new EventArgs());
            }

            if (footerUpdateRequired == true)
            {
                OnSelectedFooterCellTextChanged(new EventArgs());
            }
        }

        public void ClearSelections()
        {
            SelectedHeaders.Clear();
            SelectedHeaderCellText.Clear();
            ClearHeaderAdorners();
            OnSelectedHeadersChanged(new EventArgs());
            OnSelectedHeaderCellTextChanged(new EventArgs());

            SelectedFooters.Clear();
            SelectedFooterCellText.Clear();
            ClearFooterAdorners();
            OnSelectedFootersChanged(new EventArgs());
            OnSelectedFooterCellTextChanged(new EventArgs());
        }

        #region AdornerHandling
        public void ReAttachAdorners(Canvas labelCanvas, CellSelectionMode selectionMode)
        {
            foreach (var child in labelCanvas.Children)
            {
                if (child is Border)
                {
                    Border outline = child as Border;

                    if (outline.Tag is HeaderCellWrapper)
                    {
                        HeaderCellWrapper wrapper = outline.Tag as HeaderCellWrapper;
                        HeaderCell headerCell = wrapper.Cells.First();

                        if (selectionMode == CellSelectionMode.Cell)
                        {
                            if (SelectedHeaders.Contains(headerCell))
                            {
                                AddHeaderAdorner(outline);
                            }
                        }

                        if (selectionMode == CellSelectionMode.Text)
                        {
                            Canvas textCanvas;
                            if ((textCanvas = outline.Child as Canvas) != null)
                            {
                                foreach (var textChild in textCanvas.Children)
                                {
                                    TextBlock textBlock;
                                    if ((textBlock = textChild as TextBlock) != null)
                                    {
                                        if (SelectedHeaderCellText.Contains((HeaderCellWrapper)textBlock.Tag))
                                        {
                                            HeaderCellWrapper textWrapper = textBlock.Tag as HeaderCellWrapper;
                                            foreach (var element in textWrapper.TextBlocks)
                                            {
                                                AddHeaderAdorner(element);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (outline.Tag is FooterCell)
                    {
                        FooterCell footerCell = outline.Tag as FooterCell;

                        if (selectionMode == CellSelectionMode.Cell)
                        {
                            if (SelectedFooters.Contains(footerCell))
                            {
                                AddFooterAdorner(outline);
                            }
                        }

                        if (selectionMode == CellSelectionMode.Text)
                        {
                            Canvas textCanvas;
                            if ((textCanvas = outline.Child as Canvas) != null)
                            {
                                foreach (var element in textCanvas.Children)
                                {
                                    TextBlock textBlock;
                                    if ((textBlock = element as TextBlock) != null)
                                    {
                                        if (SelectedFooterCellText.Contains((FooterCellText)textBlock.Tag))
                                        {
                                            AddFooterAdorner(textBlock);
                                        }
                                    }
                                }
                            }
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

        private void RemoveHeaderAdorner(TextBlock textBlock)
        {
            SelectionAdorner adornerToRemove = _HeaderAdorners.Find(item => item.AdornedElement == textBlock);
            
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

        private void AddHeaderAdorner(TextBlock textBlock)
        {
            if (_HeaderAdorners.Find(item => item.AdornedElement == textBlock) == null)
            {
                SelectionAdorner adornerToAdd = new SelectionAdorner(textBlock);
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

        private void RemoveFooterAdorner(TextBlock textBlock)
        {
            SelectionAdorner adornerToRemove = _FooterAdorners.Find(item => item.AdornedElement == textBlock);

            {
                if (adornerToRemove != null)
                {
                    _AdornerLayer.Remove(adornerToRemove);
                    _FooterAdorners.Remove(adornerToRemove);
                }
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

        private void AddFooterAdorner(TextBlock textBlock)
        {
            if (_FooterAdorners.Find(item => item.AdornedElement == textBlock) == null)
            {
                SelectionAdorner addornerToAdd = new SelectionAdorner(textBlock);
                addornerToAdd.IsHitTestVisible = false;
                _AdornerLayer.Add(addornerToAdd);
                _FooterAdorners.Add(addornerToAdd);
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

        public event EventHandler SelectedHeaderCellTextChanged;
        public event EventHandler SelectedFooterCellTextChanged;

        protected void OnSelectedHeadersChanged(EventArgs e)
        {
            SelectedHeadersChanged(this, e);
        }

        protected void OnSelectedFootersChanged(EventArgs e)
        {
            SelectedFootersChanged(this, e);
        }

        protected void OnSelectedHeaderCellTextChanged(EventArgs e)
        {
            SelectedHeaderCellTextChanged(this, e);
        }

        protected void OnSelectedFooterCellTextChanged(EventArgs e)
        {
            SelectedFooterCellTextChanged(this, e);
        }
        #endregion
    }
}
