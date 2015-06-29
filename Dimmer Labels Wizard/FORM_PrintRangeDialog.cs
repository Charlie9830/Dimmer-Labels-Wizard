using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_PrintRangeDialog : Form
    {
        public List<int> DimmerPrintRange = new List<int>();
        public List<int> DistroPrintRange = new List<int>();

        public FORM_PrintRangeDialog()
        {
            InitializeComponent();
        }

        private void FORM_PrintRangeDialog_Load(object sender, EventArgs e)
        {
            InitializePrintRangeSelectionControls();
        }

        private void InitializePrintRangeSelectionControls()
        {
            int DimmerRackQty = Globals.LabelStrips.Count(item => item.RackUnitType == RackType.Dimmer);
            if (DimmerRackQty == 0)
            {
                DimmerPrintRangeSelection.Enabled = false;
            }

            else
            {
                DimmerPrintRangeSelection.RackQty = DimmerRackQty;
            }

            int DistroRackQty = Globals.LabelStrips.Count(item => item.RackUnitType == RackType.Distro);
            if (DistroRackQty == 0)
            {
                DistroPrintRangeSelection.Enabled = false;
            }

            else
            {
                DistroPrintRangeSelection.RackQty = DistroRackQty;
            }
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            DimmerPrintRangeSelection.GenerateRackRange();
            DimmerPrintRange.AddRange(DimmerPrintRangeSelection.RackRange);

            DistroPrintRangeSelection.GenerateRackRange();
            DistroPrintRange.AddRange(DistroPrintRangeSelection.RackRange);
        }
    }
}
