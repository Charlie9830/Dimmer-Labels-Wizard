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
            FiveKPanel.Visible = false;
            UniverseColumnSelectPanel.Visible = false;
            DistroNumberPrefixPanel.Visible = false;

            PopulateUniverseHeadersComboBox();
            PopulateColumnMappingComboBoxes();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            UpdateUserParameters();
            this.Close();
        }

        // Needs Sanity Checking. Have User selections actually been entered?
        private void UpdateUserParameters()
        {
            UserParameters.DimmerImportFormat = CollectDimmerFormatValue();
            UserParameters.DistroImportFormat = CollectDistroFormatValue();

            UserParameters.StartDimmerNumber = Convert.ToInt16(FirstDimmerNumberSelector.Value);
            UserParameters.EndDimmerNumber = Convert.ToInt16(LastDimmerNumberSelector.Value);
            UserParameters.DimmerUniverses.Add(Convert.ToInt16(DimmersUniverseSelector.Value));

            UserParameters.StartDistroNumber = Convert.ToInt16(FirstDistroNumberSelector.Value);
            UserParameters.EndDistroNumber = Convert.ToInt16(LastDistroNumberSelector.Value);

            UserParameters.ChannelNumberColumnIndex = ChannelMappingComboBox.SelectedIndex;
            UserParameters.DimmerNumberColumnIndex = DimmerNumberMappingComboBox.SelectedIndex;
            UserParameters.InstrumentTypeColumnIndex = InstrumentNameMappingComboBox.SelectedIndex;
            UserParameters.MulticoreNameColumnIndex = MulticoreNameMappingComboBox.SelectedIndex;
            UserParameters.PositionColumnIndex = PositionMappingComboBox.SelectedIndex;

            if (FiveKDimmerAddressDataGrid.Rows.Count > 0)
            {
                for (int index = 0; index < FiveKDimmerAddressDataGrid.Rows.Count; index++)
                {
                    DataGridViewRow row = FiveKDimmerAddressDataGrid.Rows[index];
                    Globals.DMX address;

                    address.Universe = Convert.ToInt16(row.Cells[0].Value);
                    address.Channel = Convert.ToInt16(row.Cells[1].Value);
                }
            }

            if (UniverseColumnSelectPanel.Visible == true)
            {
                if (NoUniverseDataCheckBox.Checked == false)
                {
                    UserParameters.UniverseDataColumnIndex = UniverseDMXColumnsComboBox.SelectedIndex;
                    UserParameters.DMXAddressImportFormat = CollectDMXAddressFormatValue();
                }

                else
                {
                    UserParameters.DMXAddressImportFormat = ImportFormatting.NoUniverseData;
                }
            }

            if (DistroFormatComboBox.SelectedIndex == 0)
            {
                UserParameters.DistroNumberPrefix = DistroNumberPrefixTextBox.Text.Trim();
            }
        }

        private void FiveKDimmersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FiveKDimmersCheckBox.Checked == true)
            {
                FiveKPanel.Enabled = true;
                FiveKPanel.Visible = true;
            }

            else
            {
                FiveKPanel.Enabled = false;
                FiveKPanel.Visible = false;
            }
        }

        private ImportFormatting CollectDimmerFormatValue()
        {
            int valueIndex = DimmerFormatComboBox.SelectedIndex;

            switch (valueIndex)
            {
                case 0:
                    return ImportFormatting.Format1;
                case 1:
                    return ImportFormatting.Format2;
                case 2:
                    return ImportFormatting.Format3;
                case 3:
                    return ImportFormatting.Format4;
                default:
                    Console.WriteLine("CollectDimmerFormatValue Hit the default case. Reverting to Format1");
                    return ImportFormatting.Format1;
            }
        }

        private ImportFormatting CollectDistroFormatValue()
        {
            int valueIndex = DistroFormatComboBox.SelectedIndex;

            switch (valueIndex)
            {
                case 0:
                    return ImportFormatting.Format1;
                case 1:
                    return ImportFormatting.Format2;
                case 2:
                    return ImportFormatting.Format3;
                case 3:
                    return ImportFormatting.Format4;
                default:
                    Console.WriteLine("CollectDistroFormatValue Hit the default case. Reverting to Format1");
                    return ImportFormatting.Format1;
            }
        }

        private ImportFormatting CollectDMXAddressFormatValue()
        {
            int valueIndex = DMXAddressFormatComboBox.SelectedIndex;

            switch (valueIndex)
            {
                case 0:
                    return ImportFormatting.Format1;
                case 1:
                    return ImportFormatting.Format2;
                case 2:
                    return ImportFormatting.Format3;
                case 3:
                    return ImportFormatting.Format4;
                default:
                    Console.WriteLine("CollectDMXAddressFormatValue has Hit the default case. Reverting to Format1");
                    return ImportFormatting.Format1;
            }
        }


        private void PopulateUniverseHeadersComboBox()
        {
            UniverseDMXColumnsComboBox.Items.AddRange(FileImport.CollectHeaders());
        }

        private void DimmerFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DimmerFormatComboBox.SelectedIndex == 1)
            {
                UniverseColumnSelectPanel.Visible = true;
            }

            else
            {
                UniverseColumnSelectPanel.Visible = false;
            }
        }

        private void PopulateColumnMappingComboBoxes()
        {
            string[] headers = FileImport.CollectHeaders();

            // Populate Comboboxes
            ChannelMappingComboBox.Items.AddRange(headers);
            DimmerNumberMappingComboBox.Items.AddRange(headers);
            InstrumentNameMappingComboBox.Items.AddRange(headers);
            MulticoreNameMappingComboBox.Items.AddRange(headers);
            PositionMappingComboBox.Items.AddRange(headers);

            // Choose Most likely column and set as current option.

            // Channel
            foreach (var element in headers)
            {
                if (element.Contains("Channel"))
                {
                    ChannelMappingComboBox.SelectedItem = element;
                    break;
                }
            }

            // Dimmer Number
            foreach (var element in headers)
            {
                if (element.Contains("Dimmer"))
                {
                    DimmerNumberMappingComboBox.SelectedItem = element;
                    break;
                }
            }

            // Instrument Name
            foreach (var element in headers)
            {
                if (element.Contains("Instrument"))
                {
                    InstrumentNameMappingComboBox.SelectedItem = element;
                    break;
                }
            }

            // Multicore Name
            foreach (var element in headers)
            {
                if (element.Contains("Multicore") || element.Contains("Circuit"))
                {
                    MulticoreNameMappingComboBox.SelectedItem = element;
                    break;
                }
            }

            // Position
            foreach (var element in headers)
            {
                if (element.Contains("Position"))
                {
                    PositionMappingComboBox.SelectedItem = element;
                    break;
                }
            }
        }

        private void NoUniverseDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUniverseDataCheckBox.Checked == true)
            {
                UniverseDMXColumnsComboBox.Enabled = false;
                DMXAddressFormatComboBox.Enabled = false;
            }

            else
            {
                UniverseDMXColumnsComboBox.Enabled = true;
                DMXAddressFormatComboBox.Enabled = true;
            }
        }

        private void DistroFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DistroFormatComboBox.SelectedIndex == 0)
            {
                DistroNumberPrefixPanel.Visible = true;
            }

            else
            {
                DistroNumberPrefixPanel.Visible = false;
            }
        }
    }
}
