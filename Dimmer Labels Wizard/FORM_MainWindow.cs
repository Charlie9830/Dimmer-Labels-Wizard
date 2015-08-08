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
using System.Windows.Forms.Integration;

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
            FileNameLabel.Visible = false;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            // C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFileDialog.Multiselect = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = "Browse";
            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Filter = "Comma Seperated Values File (*.csv) | *.csv";
            openFileDialog.FilterIndex = 0;
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
            this.Hide();
            Forms.UserParameterEntry = new FORM_UserParameterEntry();
            Forms.UserParameterEntry.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Globals.DebugActive = true;
            this.Close();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Persistance test = new Persistance();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFileDialog.Multiselect = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = "Load";
            openFileDialog.DefaultExt = ".dlw";
            openFileDialog.Filter = "Dimmer Labels Wizard File (*.dlw) | *.dlw";
            openFileDialog.FilterIndex = 0;

            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != string.Empty)
            {
                test.LoadFromFile(openFileDialog.FileName);

                Forms.LabelEditor = new LabelEditor();
                ElementHost.EnableModelessKeyboardInterop(Forms.LabelEditor);
                Forms.LabelEditor.Show();
            }
        }
    }
}
