using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

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
                Part2.Visible = true;
            }

            else if (Part2.Visible == true)
            {
                // Set any Undefined Colors to White
                Part2.SetUndefinedColors();

                this.Hide();

                UserParameters.SetDefaultRackLabelSettings();
                Forms.LabelEditor = new LabelEditor();
                ElementHost.EnableModelessKeyboardInterop(Forms.LabelEditor);
                Forms.LabelEditor.Show();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (Part1.Visible == true)
            {
                this.Close();
                if (Forms.InstrumentNameEntry == null)
                {
                    Forms.InstrumentNameEntry = new FORM_InstrumentNameEntry();
                }

                Forms.InstrumentNameEntry.Show();
            }

            else if (Part2.Visible == true)
            {
                if (MessageBox.Show("Are you sure you want to go back? Going back now will cause you to lose your Colour selections."
                    , "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Part2.Visible = false;
                    Part1.Visible = true;
                }
            }
        }
    }
}
