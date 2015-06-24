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
        public Globals.DimmerRange Range
        {
            get
            {
                return GetRange();
            }
        }

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

        // Determines if Valid Data has been Entered. Called by ValidEntry Getter.
        private void DetermineValidEntry()
        {
            if (Range.Universe > 0 && Range.FirstChannel <= Range.LastChannel)
            {
                validEntry = true;
            }

            else
            {
                validEntry = false;
            }
        }

        private Globals.DimmerRange GetRange()
        {
            return new Globals.DimmerRange((int)UniverseSelector.Value, (int)FirstChannelSelector.Value,
                (int)LastChannelSelector.Value);
        }
    }
}
