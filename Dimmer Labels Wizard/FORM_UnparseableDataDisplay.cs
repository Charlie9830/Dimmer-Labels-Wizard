﻿using System;
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
    public partial class FORM_UnparseableDataDisplay : Form
    {
        private Dictionary<int, DimmerDistroUnit> TableObjectTracking = new Dictionary<int, DimmerDistroUnit>();

        private List<DimmerDistroUnit> UserChangedData = new List<DimmerDistroUnit>();  

        public FORM_UnparseableDataDisplay()
        {
            InitializeComponent();
        }

        private void FORM_UnparseableDataDisplay_Load(object sender, EventArgs e)
        {
            PopulateUnparseableDataGridView();

            // Assign Event Handles to Events.
            UnparseableDataGridView.CellValueChanged += UnparseableDataGridView_CellValueChanged;
        }

        private void PopulateUnparseableDataGridView()
        {
            int rowIndex = 0;

            foreach(var element in Globals.UnParseableData)
            {
                UnparseableDataGridView.Rows.Add(element.ChannelNumber,element.DimmerNumberText,element.InstrumentName,element.MulticoreName,element.ImportIndex);
                
                // Add Element to Tracking Dictionary
                TableObjectTracking.Add(rowIndex, element);
                rowIndex++;
            }
        }

        // Returns true if All Unparseable data has been accounted for by the User. False if Data still exists.
        private bool UpdateDimmerDistroUnits()
        {
            foreach (var element in UserChangedData)
            {
                // Remove it from Unparseable Data. If it fails parsing it will be re-added to this list.
                Globals.UnParseableData.Remove(element);

                // Attempt to parse it's data.
                element.ParseUnitData();
            }

            if (Globals.UnParseableData.Count == 0)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        private void RemoveOmitCheckedUnits()
        {
            for (int index = 0; index < UnparseableDataGridView.Rows.Count; index++)
            {
                DataGridViewRow currentRow = UnparseableDataGridView.Rows[index];

                // Check if Data exists in Omit Column.
                if (currentRow.Cells[OmitColumn.Index].Value != null)
                {
                    // If the CheckBox has been checked.
                    if ((bool)currentRow.Cells[OmitColumn.Index].Value == true)
                    {
                        // Delete that DimmerDistroUnits from the Global List and from unparseable data list.
                        Globals.DimmerDistroUnits.Remove(TableObjectTracking[currentRow.Index]);
                        Globals.UnParseableData.Remove(TableObjectTracking[currentRow.Index]);
                        UserChangedData.Remove(TableObjectTracking[currentRow.Index]);
                    }
                }
                
            }
        }

        private void UnparseableDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update the Object with New User Inputted Data.
            TableObjectTracking[e.RowIndex].DimmerNumberText = (string) UnparseableDataGridView.Rows[e.RowIndex].Cells[1].Value;

            // Add it to the list.
            if (UserChangedData.Contains(TableObjectTracking[e.RowIndex]) == false)
            {
                UserChangedData.Add(TableObjectTracking[e.RowIndex]);
            }
        }


        private void ContinueButton_Click(object sender, EventArgs e)
        {
            RemoveOmitCheckedUnits();
            bool continueState = UpdateDimmerDistroUnits();

            if (continueState == false)
            {
                UnparseableDataGridView.Rows.Clear();
                TableObjectTracking.Clear();
                PopulateUnparseableDataGridView();
            }

            else
            {
                this.Close();
            }
        }

        private void OmitAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OmitAllCheckBox.Checked == true)
            {
                for (int index = 0; index < UnparseableDataGridView.Rows.Count; index++)
                {
                    DataGridViewRow currentRow = UnparseableDataGridView.Rows[index];

                    currentRow.Cells[OmitColumn.Index].Value = true;
                }
            }

            else
            {
                for (int index = 0; index < UnparseableDataGridView.Rows.Count; index++)
                {
                    DataGridViewRow currentRow = UnparseableDataGridView.Rows[index];

                    currentRow.Cells[OmitColumn.Index].Value = false;
                }
            }
        }
    }
}
