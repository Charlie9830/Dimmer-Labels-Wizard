using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_MainWindow : Form
    {
        public FORM_MainWindow()
        {
            InitializeComponent();
        }

        private void FORM_MainWindow_Load(object sender, EventArgs e)
        {
            // NextButton.Enabled = false;
            FileNameLabel.Visible = false;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            // C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\
            openFileDialog.InitialDirectory = @"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = "Browse";
            openFileDialog.DefaultExt = ".csv";
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileImport.FilePath = openFileDialog.FileName;
                FileNameLabel.Text = openFileDialog.SafeFileName;
                FileNameLabel.Visible = true;
                NextButton.Enabled = true;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            Globals.DebugActive = DebugModeCheckBox.Checked;
            this.Hide();
            Forms.UserParameterEntry = new FORM_UserParameterEntry();
            Forms.UserParameterEntry.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int count = 0; count < 12; count++)
            {
                Globals.LabelStrips.Add(new LabelStrip());
            }

            foreach (var element in Globals.LabelStrips)
            {
                element.RackUnitType = RackType.Dimmer;
            }

            FORM_PrintRangeDialog test = new FORM_PrintRangeDialog();
            test.ShowDialog();
        }
    }
}
