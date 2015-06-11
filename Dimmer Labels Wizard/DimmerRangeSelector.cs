using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class DimmerRangeSelector : UserControl
    {
        public Globals.DimmerRange Range;

        // Read only. Determines if Valid Input has been entered when acsessed.
        public bool ValidEntry
        {
            get
            {
                DetermineValidEntry();
                return validEntry;
            }
        }

        private bool validEntry;

        public DimmerRangeSelector()
        {
            InitializeComponent();
        }

        private void UniverseSelector_ValueChanged(object sender, EventArgs e)
        {
            Range.Universe = (int)UniverseSelector.Value;
        }

        private void FirstChannelSelector_ValueChanged(object sender, EventArgs e)
        {
            Range.FirstChannel = (int)FirstChannelSelector.Value;
        }

        private void LastChannelSelector_ValueChanged(object sender, EventArgs e)
        {
            Range.LastChannel = (int)LastChannelSelector.Value;
        }

        // Determines if Valid Data has been Entered. Called by ValidEntry Getter.
        private void DetermineValidEntry()
        {
            if (Range.FirstChannel <= Range.LastChannel)
            {
                validEntry = true;
            }

            else
            {
                validEntry = false;
            }
        }
    }
}
