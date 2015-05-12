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
    public partial class FORM_InstrumentNameEntry : Form
    {
        private List<string> ImportedInstrumentNames = new List<string>();

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
                if (element.CabinetNumber != 0)
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
        }

        private void PopulateInstrumentNamesTable()
        {
            PopulateImportedInstrumentNames();
            
            foreach (var element in ImportedInstrumentNames)
            {
                InstrumentNamesTable.Rows.Add(element);
            }
        }

        private void UpdateInstrumentNames()
        {
            for (int index = 0; index < InstrumentNamesTable.Rows.Count; index++)
            {
                DataGridViewRow row = InstrumentNamesTable.Rows[index];

                if (row.Cells[1].Value != null)
                {
                    // Collect the selected DimmerDistroUnit
                    DimmerDistroUnit currentUnit = UserSelectionDictionary[row.Cells[0].Value.ToString()];

                    if (currentUnit.InstrumentName != row.Cells[1].Value.ToString())
                    {
                        currentUnit.InstrumentName = row.Cells[1].Value.ToString();
                        
                        // Search for and Update Like DimmerDistroUnits.
                        foreach (var element in Globals.DimmerDistroUnits)
                        {
                            if (element.InstrumentName == row.Cells[0].Value.ToString())
                            {
                                element.InstrumentName = currentUnit.InstrumentName;
                            }
                        }
                        
                    }
                }
                
            }
        }

        private void TruncateInstrumentNames()
        {
            // Truncate by Manufacturer

            // If Above Fails. Force Truncate. Decide on a character length, then cut down by the nearest
            // whitespcae.

        }


        private void ContinueButton_Click(object sender, EventArgs e)
        {
            UpdateInstrumentNames();
            this.Close();
        }
    }
}
