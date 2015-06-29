using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class LabelSetupPart1 : UserControl
    {
        private string[] LabelFields = {"Leave Blank", "Channel Number", "Instrument Name", "Multicore Name",
                                           "Position", "User Field 1", "User Field 2", "User Field 3", "User Field 4" };

        public LabelSetupPart1()
        {
            InitializeComponent();
        }

        private void LabelSetupPart1_Load(object sender, EventArgs e)
        {
            PopulateComboBoxes();

            SingleLabelPreviewPanel.Visible = false;

            DimmerLabelWidthSelector.Value = 18;
            DimmerLabelHeightSelector.Value = 16;

            DistroLabelWidthSelector.Value = 18;
            DistroLabelHeightSelector.Value = 16;

            #region ToolTipSetup
            // ToolTip Setup
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(HeaderComboBox, "Select Text to occupy Header Position");
            toolTip.SetToolTip(FooterTopComboBox, "Select Text to occupy Footer Top Position");
            toolTip.SetToolTip(FooterMiddleComboBox, "Select Text to occupy Footer Middle Position");
            toolTip.SetToolTip(FooterBottomComboBox, "Select Text to occupy Footer Bottom Position");
            #endregion
        }

        private void PopulateComboBoxes()
        {
            foreach (var element in LabelFields)
            {
                HeaderComboBox.Items.Add(element);
                FooterTopComboBox.Items.Add(element);
                FooterMiddleComboBox.Items.Add(element);
                FooterBottomComboBox.Items.Add(element);

                HeaderComboBox.SelectedIndex = 0;
                FooterTopComboBox.SelectedIndex = 0;
                FooterMiddleComboBox.SelectedIndex = 0;
                FooterBottomComboBox.SelectedIndex = 0;
            }
        }

        // Called from outside Class. Called by FORM_LabelSetup.ContinueButtonClick().
        public void UpdateUserParameters()
        {
            UserParameters.HeaderField = GetLabelField(HeaderComboBox.SelectedIndex);
            UserParameters.FooterTopField = GetLabelField(FooterTopComboBox.SelectedIndex);
            UserParameters.FooterMiddleField = GetLabelField(FooterMiddleComboBox.SelectedIndex);
            UserParameters.FooterBottomField = GetLabelField(FooterBottomComboBox.SelectedIndex);

            UserParameters.DimmerLabelWidthInMM = (int)DimmerLabelWidthSelector.Value;
            UserParameters.DimmerLabelHeightInMM = (int)DimmerLabelHeightSelector.Value;

            UserParameters.DistroLabelWidthInMM = (int)DistroLabelWidthSelector.Value;
            UserParameters.DistroLabelHeightInMM = (int)DistroLabelHeightSelector.Value;

            UserParameters.SingleLabel = SingleLabelStyleCheckBox.Checked;
            UserParameters.HeaderBackGroundColourOnly = HeaderOnlyBackgroundColorCheckBox.Checked;
        }

        private LabelField GetLabelField(int index)
        {
            switch (index)
            {
                case 0:
                    return LabelField.NoAssignment;
                case 1:
                    return LabelField.ChannelNumber;
                case 2:
                    return LabelField.InstrumentName;
                case 3:
                    return LabelField.MulticoreName;
                case 4:
                    return LabelField.Position;
                case 5:
                    return LabelField.UserField1;
                case 6:
                    return LabelField.UserField2;
                case 7:
                    return LabelField.UserField3;
                case 8:
                    return LabelField.UserField4;
                default:
                    return LabelField.NoAssignment;
            }
        }

        private void SingleLabelStyleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SingleLabelStyleCheckBox.Checked == true)
            {
                SingleLabelPreviewPanel.Visible = true;
                FooterTopComboBox.Enabled = false;
            }

            else
            {
                SingleLabelPreviewPanel.Visible = false;
                FooterTopComboBox.Enabled = true;
            }
        }
    }
}
