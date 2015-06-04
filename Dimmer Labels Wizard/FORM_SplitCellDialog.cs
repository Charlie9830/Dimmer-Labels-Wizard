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
    public partial class FORM_SplitCellDialog : Form
    {
        public bool ValuesChanged;

        private LabelStripSelection labelSelection;
        private int splitIndex;

        public FORM_SplitCellDialog()
        {
            InitializeComponent();

            // Don't use this Constructor.

        }

        public FORM_SplitCellDialog(LabelStripSelection activeLabelStrip, int SplitIndex)
        {
            InitializeComponent();
            labelSelection = activeLabelStrip;
            splitIndex = SplitIndex;

            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            OkButton.Enabled = false;

            LeftCellTextBox.TextChanged += new EventHandler(this.TextBoxes_TextChanged);
            RightCellTextBox.TextChanged += new EventHandler(this.TextBoxes_TextChanged);
        }

        private void FORM_SplitCellDialog_Load(object sender, EventArgs e)
        {
            PopulateTextBoxes();
        }

        private void PopulateTextBoxes()
        {
            LeftCellTextBox.Text = labelSelection.LabelStrip.Headers[splitIndex].Data;
            RightCellTextBox.Text = labelSelection.LabelStrip.Headers[splitIndex + 1].Data;
        }

        // Update Cell Data if Value of Textboxes is different. Returns True if Values were updated.
        private void UpdateCellData()
        {
            if (CascadeChangesCheckBox.Checked == false)
            {
                labelSelection.LabelStrip.Headers[splitIndex].Data = LeftCellTextBox.Text;
                labelSelection.LabelStrip.Headers[splitIndex + 1].Data = RightCellTextBox.Text;
            }

            else
            {
                if (LeftCellTextBox.Text != labelSelection.LabelStrip.Headers[splitIndex].Data)
                {
                    // Set the First Cell.
                    labelSelection.LabelStrip.Headers[splitIndex].Data = LeftCellTextBox.Text;

                    // Cascade the Changes Back.
                    for (int index = splitIndex; index >= 0; index--)
                    {
                        if (index - 1 > 0)
                        {
                            if (labelSelection.LabelStrip.Headers[index - 1].Data != LeftCellTextBox.Text)
                            {
                                labelSelection.LabelStrip.Headers[index - 1].Data = LeftCellTextBox.Text;
                            }

                            else
                            {
                                break;
                            }
                        }
                    }
                }

                if (RightCellTextBox.Text != labelSelection.LabelStrip.Headers[splitIndex + 1].Data)
                {
                    // Set the First Cell.
                    labelSelection.LabelStrip.Headers[splitIndex + 1].Data = RightCellTextBox.Text;

                    // Cascade the Changes Forward.
                    for (int index = splitIndex + 1; index < labelSelection.LabelStrip.Headers.Count; index++)
                    {
                        if (index + 1 < labelSelection.LabelStrip.Headers.Count)
                        {
                            if (labelSelection.LabelStrip.Headers[index + 1].Data != RightCellTextBox.Text)
                            {
                                labelSelection.LabelStrip.Headers[index + 1].Data = RightCellTextBox.Text;
                            }

                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        // Checks if an Update of the Cell Data is Required.
        private bool CellDataUpdateRequired()
        {
            if (LeftCellTextBox.Text != labelSelection.LabelStrip.Headers[splitIndex].Data)
            {
                return true;
            }

            if (RightCellTextBox.Text != labelSelection.LabelStrip.Headers[splitIndex + 1].Data)
            {
                return true;
            }

            return false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            UpdateCellData();
        }

        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            if (CellDataUpdateRequired() == true)
            {
                OkButton.Enabled = true;
            }

            else
            {
                OkButton.Enabled = false;
            }
        }
    }
}
