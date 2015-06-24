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
                                           "Position" };

        public LabelSetupPart1()
        {
            InitializeComponent();
        }

        private void LabelSetupPart1_Load(object sender, EventArgs e)
        {
            PopulateComboBoxes();

            LabelWidthSelector.Value = 18;
            LabelHeightSelector.Value = 16;

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

            UserParameters.LabelWidthInMM = (int)LabelWidthSelector.Value;
            UserParameters.LabelHeightInMM = (int)LabelHeightSelector.Value;

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
                default:
                    return LabelField.NoAssignment;
            }
        }
    }
}
