using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Dimmer_Labels_Wizard
{
    public partial class FORM_InstrumentNameEntry : Form
    {
        private List<string> ImportedInstrumentNames = new List<string>();

        private int importedInstrumentNameColumnIndex = 0;
        private int labelInstrumentNameColumnIndex = 1;
        private int characterCountColumnIndex = 2;

        private Dictionary<string, DimmerDistroUnit> UserSelectionDictionary = new Dictionary<string, DimmerDistroUnit>();

        public FORM_InstrumentNameEntry()
        {
            InitializeComponent();
        }

        private void FORM_InstrumentNameEntry_Load(object sender, EventArgs e)
        {
            PopulateInstrumentNamesTable();
        }

        private void InstrumentNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        // Add InstrumentNames to Imported instrument names list.
        private void PopulateImportedInstrumentNames()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (ImportedInstrumentNames.Contains(element.InstrumentName) != true)
                {
                    // Add Instrument Name to List.
                    ImportedInstrumentNames.Add(element.InstrumentName);
                        
                    // Add a Tracking KeyPair to the User selection dictionary.
                    UserSelectionDictionary.Add(element.InstrumentName, element);
                }
                
            }
        }

        private void PopulateInstrumentNamesTable()
        {
            PopulateImportedInstrumentNames();

            int rowIndex = 0;
            foreach (var element in ImportedInstrumentNames)
            {
                InstrumentNamesTable.Rows.Add(new DataGridViewRow());

                InstrumentNamesTable.Rows[rowIndex].Cells[characterCountColumnIndex].Style.Alignment = 
                    DataGridViewContentAlignment.MiddleCenter;

                InstrumentNamesTable.Rows[rowIndex].Cells[importedInstrumentNameColumnIndex].Value = element;
                InstrumentNamesTable.Rows[rowIndex].Cells[characterCountColumnIndex].Value = element.Length;
                
                if (element.Length > 8)
                {
                    InstrumentNamesTable.Rows[rowIndex].Cells[characterCountColumnIndex].Style.BackColor = Color.Orange;
                }

                rowIndex++;
            }
        }

        private void UpdateInstrumentNames()
        {
            for (int index = 0; index < InstrumentNamesTable.Rows.Count; index++)
            {
                DataGridViewRow row = InstrumentNamesTable.Rows[index];

                if (row.Cells[labelInstrumentNameColumnIndex].Value != null)
                {
                    // Collect the selected DimmerDistroUnit
                    DimmerDistroUnit currentUnit = UserSelectionDictionary[row.Cells[0].Value.ToString()];

                    if (currentUnit.InstrumentName != row.Cells[labelInstrumentNameColumnIndex].Value.ToString())
                    {
                        currentUnit.InstrumentName = row.Cells[labelInstrumentNameColumnIndex].Value.ToString();
                        
                        // Search for and Update Like DimmerDistroUnits.
                        foreach (var element in Globals.DimmerDistroUnits)
                        {
                            if (element.InstrumentName == row.Cells[importedInstrumentNameColumnIndex].Value.ToString())
                            {
                                element.InstrumentName = currentUnit.InstrumentName;
                            }
                        }
                        
                    }
                }
                
            }
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            UpdateInstrumentNames();
            this.Hide();
            Forms.LabelSetup = new FORM_LabelSetup();
            Forms.LabelSetup.Show();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to go Back?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
                if (Forms.UserParameterEntry == null)
                {
                    Forms.UserParameterEntry = new FORM_UserParameterEntry();
                }
                Forms.UserParameterEntry.Show();
            }
        }
    }
}
