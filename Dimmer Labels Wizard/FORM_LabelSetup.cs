using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_LabelSetup : Form
    {
        public FORM_LabelSetup()
        {
            InitializeComponent();
        }

        private void FORM_LabelSetup_Load(object sender, EventArgs e)
        {

        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            // Tell Part1 Control to Update its user Parameters before proceeding.
            Part1.UpdateUserParameters();

            // Only Export on first Button Press.
            if (Part1.Visible == true)
            {
                Part1.Visible = false;
                Output.ExportToRackLabel();
                Part2.RenderControl();
            }

            else if (Part2.Visible == true)
            {
                // Set any Undefined Colors to White
                Part2.SetUndefinedColors();
                this.Close();
            }
        }
    }
}
