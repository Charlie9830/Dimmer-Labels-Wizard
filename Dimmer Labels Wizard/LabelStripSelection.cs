using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dimmer_Labels_Wizard
{
    public class LabelStripSelection
    {
        public LabelStrip LabelStrip;
        public List<HeaderCell> SelectedHeaders = new List<HeaderCell>();
        public List<FooterCell> SelectedFooters = new List<FooterCell>();

        public void ClearSelections()
        {
            SelectedHeaders.Clear();
            SelectedFooters.Clear();
        }
    }
}
