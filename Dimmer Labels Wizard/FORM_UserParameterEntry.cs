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
        private bool DistroFormatSelectionMade = false;
        private bool DimmerFormatSelectionMade = false;
        private bool UniverseDMXColumnSelectionMade = false;
        private bool UniverseNumberFormatSelectionMade = false;

        private FORM_UserParameterEntry_Help helpWindow = new FORM_UserParameterEntry_Help();

        public FORM_UserParameterEntry()
        {
            InitializeComponent();
        }

        private void FORM_UserParameterEntry_Load(object sender, EventArgs e)
        {
            helpWindow.ShowInTaskbar = false;

            UniverseColumnSelectPanel.Enabled = false;
            DistroNumberPrefixPanel.Enabled = false;
            DimmerRangeInput.Enabled = false;
            DistroNumberPanel.Enabled = false;
            DistroFormatComboBox.Enabled = false;
            DistroNumberFormatLabel.Enabled = false;
            DimmerFormatComboBox.Enabled = false;
            DimmerNumberFormatLabel.Enabled = false;

            PopulateUniverseHeadersComboBox();
            PopulateColumnMappingComboBoxes();

            #region ToolTipSetup
            // ToolTip Setup
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            // ToolTip Data.
            toolTip.SetToolTip(CreateDimmerLabelsCheckBox, "Generate Labels for Dimmers");
            toolTip.SetToolTip(CreateDistroLabelsCheckBox, "Generate Labels for Distros");
            toolTip.SetToolTip(DimmerRangeInput, "Create ranges for Consecutive Dimmer Channels");
            toolTip.SetToolTip(DistroFormatComboBox, "Number Formatting for Distros in the CSV 'Dimmer' Column");
            toolTip.SetToolTip(DimmerFormatComboBox, "Number Formatting for Dimmers in the CSV 'Dimmer' Column");
            toolTip.SetToolTip(DistroNumberPrefixTextBox, "Letter Character(s) preceeding Distro Number in CSV 'Dimmer' Column");
            toolTip.SetToolTip(NoUniverseDataCheckBox, "Use only if all Dimmer reside in the same Universe.");
            toolTip.SetToolTip(UniverseDMXColumnsComboBox, "CSV Column name of External DMX Address infomation.");
            toolTip.SetToolTip(DMXAddressFormatComboBox, "Number Formating for External DMX Address");
            toolTip.SetToolTip(CSVColumnMappingPanel, "Map available Label Fields to CSV file Column Headers");
            #endregion
        }

        
        private void ContinueButton_Click(object sender, EventArgs e)
        {
            helpWindow.Close();

            // Sanity Checks User Inputs, Displays Error messages for incorrect input.
            if (CheckUserInputSanity() == true)
            {
                UpdateUserParameters();
                this.Close();
            }

            else
            {
                // Reshow the Form.
                this.Show();
            }
        }

        // Needs Sanity Checking. Have User selections actually been entered?
        private void UpdateUserParameters()
        {
            UserParameters.DimmerImportFormat = CollectDimmerFormatValue();
            UserParameters.DistroImportFormat = CollectDistroFormatValue();

            UserParameters.CreateDimmerObjects = CreateDimmerLabelsCheckBox.Checked;
            UserParameters.CreateDistroObjects = CreateDistroLabelsCheckBox.Checked;

            foreach (var element in DimmerRangeInput.Selectors)
            {
                if (element.ValidEntry == true)
                {
                    UserParameters.DimmerRanges.Add(element.Range);
                }
            }

            UserParameters.DimmerRanges.Sort();

            UserParameters.StartDistroNumber = Convert.ToInt16(FirstDistroNumberSelector.Value);
            UserParameters.EndDistroNumber = Convert.ToInt16(LastDistroNumberSelector.Value);

            // As value "None" has been inserted at Index 0. UserParameter will be set to -1 if "None" is Selected. Otherwise set
            // to its CSV Column Index.
            UserParameters.ChannelNumberColumnIndex = ChannelMappingComboBox.SelectedIndex - 1;
            UserParameters.DimmerNumberColumnIndex = DimmerNumberMappingComboBox.SelectedIndex - 1;
            UserParameters.InstrumentTypeColumnIndex = InstrumentNameMappingComboBox.SelectedIndex - 1;
            UserParameters.MulticoreNameColumnIndex = MulticoreNameMappingComboBox.SelectedIndex - 1;
            UserParameters.PositionColumnIndex = PositionMappingComboBox.SelectedIndex - 1;

            if (UniverseColumnSelectPanel.Enabled == true)
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

        private ImportFormatting CollectDimmerFormatValue()
        {
            if (DimmerFormatComboBox.Enabled == false)
            {
                return ImportFormatting.NoAssignment;
            }

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
            if (DistroFormatComboBox.Enabled == false)
            {
                return ImportFormatting.NoAssignment;
            }

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
            DimmerFormatSelectionMade = true;

            if (DimmerFormatComboBox.SelectedIndex == 1)
            {
                UniverseColumnSelectPanel.Enabled = true;
            }

            else
            {
                UniverseColumnSelectPanel.Enabled = false;
            }
        }

        private void PopulateColumnMappingComboBoxes()
        {
            // Append "None" to beginning of List.
            List<string> headerList = new List<string>();
            headerList.Add("None");
            headerList.AddRange(FileImport.CollectHeaders());

            string[] headers = headerList.ToArray();

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
            DistroFormatSelectionMade = true;

            if (DistroFormatComboBox.SelectedIndex == 0)
            {
                DistroNumberPrefixPanel.Enabled = true;
            }

            else
            {
                DistroNumberPrefixPanel.Enabled = false;
            }
        }

        private void CreateDimmerLabelsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateDimmerLabelsCheckBox.Checked == true)
            {
                DimmerRangeInput.Enabled = true;
                DimmerFormatComboBox.Enabled = true;
                DimmerNumberFormatLabel.Enabled = true;
            }

            else
            {
                DimmerRangeInput.Enabled = false;
                DimmerFormatComboBox.Enabled = false;
                DimmerNumberFormatLabel.Enabled = false;
            }
        }

        private void CreateDistroLabelsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateDistroLabelsCheckBox.Checked == true)
            {
                DistroNumberPanel.Enabled = true;
                DistroFormatComboBox.Enabled = true;
                DistroNumberFormatLabel.Enabled = true;
            }

            else
            {
                DistroNumberPanel.Enabled = false;
                DistroFormatComboBox.Enabled = false;
                DistroNumberFormatLabel.Enabled = false;
            }
        }

        private void UniverseDMXColumnsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UniverseDMXColumnSelectionMade = true;
        }

        private void DMXAddressFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UniverseNumberFormatSelectionMade = true;
        }

        #region SanityChecks
        // Control Method.
        private bool CheckUserInputSanity()
        {
            if (DimmerRangeSanity().SanityCheck == false)
            {
                MessageBox.Show(DimmerRangeSanity().ErrorMessage);
                return false;
            }

            if (DistroRangeSanity().SanityCheck == false)
            {
                MessageBox.Show(DimmerRangeSanity().ErrorMessage);
                return false;
            }

            if (DistroFormatSanity().SanityCheck == false)
            {
                MessageBox.Show(DistroFormatSanity().ErrorMessage);
                return false;
            }

            if (DimmerFormatSanity().SanityCheck == false)
            {
                MessageBox.Show(DimmerFormatSanity().ErrorMessage);
                return false;
            }

            if (CreateLabelsSanity().SanityCheck == false)
            {
                MessageBox.Show(CreateLabelsSanity().ErrorMessage);
                return false;
            }

            return true;
        }

        private Globals.BoolString DimmerRangeSanity()
        {
            bool returnValue = true;
            string errorMessage = "";

            // If User has elected to Create Dimmers, but havent entered any ranges.
            if (CreateDimmerLabelsCheckBox.Checked == true
                && DimmerRangeInput.Selectors.Count == 0)
            {
                returnValue = false;
                errorMessage = "You have elected to create Dimmer Labels, but have not entered any Dimmer Ranges.";

                return new Globals.BoolString(returnValue, errorMessage);
            }

            // If A Range is Invalid
            foreach (var element in DimmerRangeInput.Selectors)
            {
                if (element.ValidEntry == false)
                {
                    returnValue = false;
                    errorMessage = "Dimmer Range " + (DimmerRangeInput.Selectors.IndexOf(element) + 1)
                        + "is Invalid";
                    break;
                }
            }
            return new Globals.BoolString(returnValue, errorMessage);
        }

        private Globals.BoolString DistroRangeSanity()
        {
            bool returnValue = true;
            string errorMessage = "";

            // If User hasn't entered an Invalid Distro Range.
            if (CreateDistroLabelsCheckBox.Checked == true &&
                FirstDistroNumberSelector.Value > LastDistroNumberSelector.Value)
            {
                returnValue = true;
                errorMessage = "First Distro Number cannot be greater than Last Distro Number.";
            }

            return new Globals.BoolString(returnValue,errorMessage);
        }

        // If user has elected to Create Labels but has not selected a Format Value.
        private Globals.BoolString DistroFormatSanity()
        {
            bool returnValue = true;
            string errorMessage = "";

            // If user has elected to Create Labels but has not selected a Format Value.
            if (CreateDistroLabelsCheckBox.Checked == true &&
                DistroFormatSelectionMade == false)
            {
                returnValue = false;
                errorMessage = "You have elected to Create Distro Labels, but have not selected a Distro Number Format.";
                return new Globals.BoolString(returnValue,errorMessage);
            }

            // If User has not entered a Valid Prefix.
            if (DistroNumberPrefixPanel.Enabled == true
                && DistroNumberPrefixTextBox.Text == "Enter Letter Prefix")
            {
                returnValue = false;
                errorMessage = "Please enter a Distro Number Prefix";
                return new Globals.BoolString(returnValue, errorMessage);
            }

            return new Globals.BoolString(returnValue, errorMessage);
            
        }

        
        private Globals.BoolString DimmerFormatSanity()
        {
            bool returnValue = true;
            string errorMessage = "";

            // If user has elected to Create Labels but has not selected a Format Value.
            if (CreateDimmerLabelsCheckBox.Checked == true &&
                DimmerFormatSelectionMade == false)
            {
                returnValue = false;
                errorMessage = "You have elected to create Dimmer Labels, but have not selected a Dimmer Number Format.";
                return new Globals.BoolString(returnValue, errorMessage);
            }

            // If user has elected to Create Labels but has not selected a Format Value.
            if (UniverseColumnSelectPanel.Enabled == true &&
                NoUniverseDataCheckBox.Checked == false &&
                UniverseDMXColumnSelectionMade == false &&
                UniverseNumberFormatSelectionMade == false)
            {
                returnValue = false;
                errorMessage = "You have selected a Dimmer Format that requires external Universe Data" +
                   "Please select a Universe Column and Number Format or elect to not import Universe Data";
            }

            return new Globals.BoolString(returnValue, errorMessage);
        }

        private Globals.BoolString CreateLabelsSanity()
        {
            bool returnValue = true;
            string errorMessage = "";

            if (CreateDimmerLabelsCheckBox.Checked == false &&
                CreateDistroLabelsCheckBox.Checked == false)
            {
                returnValue = false;
                errorMessage = "Cannot continue. No 'Create Label' selections have been made";
            }

            return new Globals.BoolString(returnValue, errorMessage);
        }

        #endregion

        private void HelpButton_Click(object sender, EventArgs e)
        {
            helpWindow.Show();
        }


    }
}
