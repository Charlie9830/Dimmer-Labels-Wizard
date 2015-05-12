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
    public partial class FORM_UserParameterEntry : Form
    {
        public FORM_UserParameterEntry()
        {
            InitializeComponent();
        }

        private void FORM_UserParameterEntry_Load(object sender, EventArgs e)
        {
            FiveKPanel.Enabled = false;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            UpdateUserParameters();
        }

        private void UpdateUserParameters()
        {
            UserParameters.StartDimmerNumber = Convert.ToInt16(FirstDimmerNumberSelector.Value);
            UserParameters.EndDimmerNumber = Convert.ToInt16(LastDimmerNumberSelector.Value);
            UserParameters.DimmerUniverses.Add(Convert.ToInt16(DimmersUniverseSelector.Value));

            UserParameters.StartDistroNumber = Convert.ToInt16(FirstDistroNumberSelector.Value);
            UserParameters.EndDistroNumber = Convert.ToInt16(LastDistroNumberSelector.Value);

            if (FiveKDimmerAddressDataGrid.Rows.Count > 0)
            {
                for (int index = 0; index < FiveKDimmerAddressDataGrid.Rows.Count; index++)
                {
                    DataGridViewRow row = FiveKDimmerAddressDataGrid.Rows[index];
                    Globals.DMX address;

                    address.Universe = Convert.ToInt16(row.Cells[0].Value);
                    address.Channel = Convert.ToInt16(row.Cells[1].Value);

                    UserParameters.FiveKDimmerAddresses.Add(address);
                }
            }

            UserParameters.PopulateRackStartAddresses();
        }

        private void FiveKDimmersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FiveKDimmersCheckBox.Checked == true)
            {
                FiveKPanel.Enabled = true;
            }

            else
            {
                FiveKPanel.Enabled = false;
            }
        }


        
    }
}
