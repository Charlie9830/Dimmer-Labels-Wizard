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
    public partial class LabelSetupPart2 : UserControl
    {
        public LabelSetupPart2()
        {
            InitializeComponent();
        }

        private string[] ShowFields = {"Channel Number", "Instrument Name", "Multicore Name",
                                           "Position" };

        private List<string> Items = new List<string>();

        private void LabelSetupPart2_Load(object sender, EventArgs e)
        {
            #region ToolTip Setup
            // ToolTip Setup
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(ShowFieldsComboBox, "Filter by Label Field");
            #endregion
        }

        public void RenderControl()
        {
            PopulateShowFieldsComboBox();

            // Set Index to "Position" entry.
            ShowFieldsComboBox.SelectedIndex = 3;
            PopulateColorTable(GetShowField(ShowFieldsComboBox.SelectedIndex));
        }

        private void PopulateShowFieldsComboBox()
        {
            ShowFieldsComboBox.Items.AddRange(ShowFields);
        }

        private LabelField GetShowField(int index)
        {
            switch (index)
            {
                case 0:
                    return LabelField.ChannelNumber;
                case 1:
                    return LabelField.InstrumentName;
                case 2:
                    return LabelField.MulticoreName;
                case 3:
                    return LabelField.Position;
                default:
                    return LabelField.NoAssignment;
            }
        }

        private void PopulateColorTable(LabelField showField)
        {
            int itemColumnIndex = ItemColumn.Index;
            int colorDisplayColumnIndex = ColorDisplayColumn.Index;

            ColorTable.Rows.Clear();

            PopulateItemsList(showField);
            int rowIndex;
            foreach (var element in Items)
            {
                rowIndex = ColorTable.Rows.Add(new DataGridViewRow());
                ColorTable.Rows[rowIndex].Cells[itemColumnIndex].Value = element;

                if (GetColor(element,showField) != Color.Transparent)
                {
                    ColorTable.Rows[rowIndex].Cells[colorDisplayColumnIndex].Style.BackColor = GetColor(element,showField);

                    // Update Table. Gets Called kinda Recursively causing a Stack Overflow.
                    // PopulateColorTable(GetShowField(ShowFieldsComboBox.SelectedIndex));
                }
            }
        }

        // Called by PopulateColorTable
        private void PopulateItemsList(LabelField showField)
        {
            Items.Clear();

            // Add elements to Item list as long as they aren't already added.
            foreach (var labelStrip in Globals.LabelStrips)
            {
                foreach (var footer in labelStrip.Footers)
                {
                    switch (showField)
                    {
                        case LabelField.ChannelNumber:
                            if (Items.Contains(footer.PreviousReference.ChannelNumber) != true)
                            {
                                Items.Add(footer.PreviousReference.ChannelNumber);
                            }
                            break;
                        case LabelField.InstrumentName:
                            if (Items.Contains(footer.PreviousReference.InstrumentName) != true)
                            {
                                Items.Add(footer.PreviousReference.InstrumentName);
                            }
                            break;
                        case LabelField.MulticoreName:
                            if (Items.Contains(footer.PreviousReference.MulticoreName) != true)
                            {
                                Items.Add(footer.PreviousReference.MulticoreName);
                            }
                            break;
                        case LabelField.Position:
                            if (Items.Contains(footer.PreviousReference.Position) != true)
                            {
                                Items.Add(footer.PreviousReference.Position);
                            }
                            break;
                        default:
                            Items.Add(footer.PreviousReference.Position);
                            break;
                    }
                }
            }
        }

        private void ShowFieldsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateColorTable(GetShowField(ShowFieldsComboBox.SelectedIndex));
        }

        private void ColorSelectButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(ColorTable.SelectedCells.Count);

            if (ColorTable.SelectedCells.Count == 1)
            {
                // Collect Physical row Index.
                int rowIndex = ColorTable.SelectedCells[0].RowIndex;
                string item = (string)ColorTable.Rows[rowIndex].Cells[ItemColumn.Index].Value;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    AssignColor(item, GetShowField(ShowFieldsComboBox.SelectedIndex), colorDialog.Color);

                    // Update and Re Draw Table.
                    PopulateColorTable(GetShowField(ShowFieldsComboBox.SelectedIndex));
                }
            }

            if (ColorTable.SelectedCells.Count > 1)
            {
                List<string> items = new List<string>();

                for (int index = 0; index < ColorTable.SelectedCells.Count; index++)
                {
                    // Collect Physical Row Index
                    int rowIndex = ColorTable.SelectedCells[index].RowIndex;
                    items.Add((string)ColorTable.Rows[rowIndex].Cells[ItemColumn.Index].Value);
                }

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    AssignColor(items.ToArray(), GetShowField(ShowFieldsComboBox.SelectedIndex), colorDialog.Color);

                    // Update and Re Draw Table.
                    PopulateColorTable(GetShowField(ShowFieldsComboBox.SelectedIndex));
                }
                
            }
        }

        private void AssignColor(string item, LabelField showField, Color color)
        {
            foreach (var labelStrip in Globals.LabelStrips)
            {
                #region Footers Loop and Switch
                foreach (var footer in labelStrip.Footers)
                {
                    switch (showField)
                    {
                        case LabelField.ChannelNumber:
                            if (footer.PreviousReference.ChannelNumber == item)
                            {
                                footer.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        case LabelField.InstrumentName:
                            if (footer.PreviousReference.InstrumentName == item)
                            {
                                footer.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        case LabelField.MulticoreName:
                            if (footer.PreviousReference.MulticoreName == item)
                            {
                                footer.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        case LabelField.Position:
                            if (footer.PreviousReference.Position == item)
                            {
                                footer.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region Headers Loop and Switch
                foreach (var header in labelStrip.Headers)
                {
                    switch (showField)
                    {
                        case LabelField.ChannelNumber:
                            if (header.PreviousReference.ChannelNumber == item)
                            {
                                header.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        case LabelField.InstrumentName:
                            if (header.PreviousReference.InstrumentName == item)
                            {
                                header.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        case LabelField.MulticoreName:
                            if (header.PreviousReference.MulticoreName == item)
                            {
                                header.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        case LabelField.Position:
                            if (header.PreviousReference.Position == item)
                            {
                                header.BackgroundColor = new SolidBrush(color);
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

            }
        }

        // Overload Accepts multiple Items.
        private void AssignColor(string[] items, LabelField showField, Color color)
        {
            foreach (var item in items)
            {
                foreach (var labelStrip in Globals.LabelStrips)
                {
                    #region Footers Loop and Switch
                    foreach (var footer in labelStrip.Footers)
                    {
                        switch (showField)
                        {
                            case LabelField.ChannelNumber:
                                if (footer.PreviousReference.ChannelNumber == item)
                                {
                                    footer.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            case LabelField.InstrumentName:
                                if (footer.PreviousReference.InstrumentName == item)
                                {
                                    footer.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            case LabelField.MulticoreName:
                                if (footer.PreviousReference.MulticoreName == item)
                                {
                                    footer.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            case LabelField.Position:
                                if (footer.PreviousReference.Position == item)
                                {
                                    footer.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region Headers Loop and Switch
                    foreach (var header in labelStrip.Headers)
                    {
                        switch (showField)
                        {
                            case LabelField.ChannelNumber:
                                if (header.PreviousReference.ChannelNumber == item)
                                {
                                    header.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            case LabelField.InstrumentName:
                                if (header.PreviousReference.InstrumentName == item)
                                {
                                    header.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            case LabelField.MulticoreName:
                                if (header.PreviousReference.MulticoreName == item)
                                {
                                    header.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            case LabelField.Position:
                                if (header.PreviousReference.Position == item)
                                {
                                    header.BackgroundColor = new SolidBrush(color);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                }
            }
        }

        // Attempts to get existing Label Color. Returns Transparent if Color cannot be found.
        private Color GetColor(string item, LabelField showField)
        {
            FooterCell searchResult = null;

            foreach (var labelStrip in Globals.LabelStrips)
            {
                switch (showField)
                {
                    case LabelField.ChannelNumber:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.ChannelNumber == item);
                        break;
                    case LabelField.InstrumentName:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.InstrumentName == item);
                        break;
                    case LabelField.MulticoreName:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.MulticoreName == item);
                        break;
                    case LabelField.Position:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.Position == item);
                        break;
                    default:
                        break;
                }

                // Check if a result has been found.
                if (searchResult != null && searchResult.BackgroundColor != null)
                {
                    return searchResult.BackgroundColor.Color;
                }
            }

            // No matching result has been found.
            return Color.Transparent;
        }

        // Sets any Remaining Null Colors to White. Called by LabelSetup Continue Button.
        public void SetUndefinedColors()
        {
            foreach (var labelStrip in Globals.LabelStrips)
            {
                // Footers
                foreach (var footer in labelStrip.Footers)
                {
                    if (footer.BackgroundColor == null)
                    {
                        footer.BackgroundColor = new SolidBrush(Color.White);
                    }
                }

                // Headers
                foreach (var header in labelStrip.Headers)
                {
                    if (header.BackgroundColor == null)
                    {
                        header.BackgroundColor = new SolidBrush(Color.White);
                    }
                }
            }
        }
    }
}
