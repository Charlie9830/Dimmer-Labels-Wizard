using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard
{
    public partial class LabelSetupPart2 : UserControl
    {
        private int currentRowIndex = -1;

        public LabelSetupPart2()
        {
            InitializeComponent();
        }

        private string[] ShowFields = {"Channel Number", "Instrument Name", "Multicore Name",
                                           "Position" , "User Field 1", "User Field 2",
                                      "User Field 3", "User Field 4"};

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
                case 4:
                    return LabelField.UserField1;
                case 5:
                    return LabelField.UserField2;
                case 6:
                    return LabelField.UserField3;
                case 7:
                    return LabelField.UserField4;
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

                if (GetColor(element,showField) != Colors.Transparent)
                {
                    Color WPFColor = GetColor(element,showField);
                    byte A = WPFColor.A;
                    byte R = WPFColor.R;
                    byte G = WPFColor.G;
                    byte B = WPFColor.B;

                    System.Drawing.Color GDIColor = System.Drawing.Color.FromArgb(A,R,G,B);

                    ColorTable.Rows[rowIndex].Cells[colorDisplayColumnIndex].Style.BackColor = GDIColor;
                }
            }
            if (currentRowIndex != -1)
            {
                ColorTable.FirstDisplayedScrollingRowIndex = currentRowIndex;
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
                        case LabelField.UserField1:
                            if (Items.Contains(footer.PreviousReference.UserField1) != true)
                            {
                                Items.Add(footer.PreviousReference.UserField1);
                            }
                            break;
                        case LabelField.UserField2:
                            if (Items.Contains(footer.PreviousReference.UserField2) != true)
                            {
                                Items.Add(footer.PreviousReference.UserField2);
                            }
                            break;
                        case LabelField.UserField3:
                            if (Items.Contains(footer.PreviousReference.UserField3) != true)
                            {
                                Items.Add(footer.PreviousReference.UserField3);
                            }
                            break;
                        case LabelField.UserField4:
                            if (Items.Contains(footer.PreviousReference.UserField4) != true)
                            {
                                Items.Add(footer.PreviousReference.UserField4);
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
            if (ColorTable.SelectedCells.Count == 1)
            {
                // Collect Physical row Index.
                int rowIndex = ColorTable.SelectedCells[0].RowIndex;
                string item = (string)ColorTable.Rows[rowIndex].Cells[ItemColumn.Index].Value;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    byte A = colorDialog.Color.A;
                    byte R = colorDialog.Color.R;
                    byte G = colorDialog.Color.G;
                    byte B = colorDialog.Color.B;

                    Color color = new Color();
                    color.A = A;
                    color.R = R;
                    color.G = G;
                    color.B = B;

                    AssignColor(item, GetShowField(ShowFieldsComboBox.SelectedIndex), color);

                    currentRowIndex = ColorTable.FirstDisplayedScrollingRowIndex;

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
                    byte A = colorDialog.Color.A;
                    byte R = colorDialog.Color.R;
                    byte G = colorDialog.Color.G;
                    byte B = colorDialog.Color.B;

                    Color color = new Color();
                    color.A = A;
                    color.R = R;
                    color.G = G;
                    color.B = B;
                    
                    AssignColor(items.ToArray(), GetShowField(ShowFieldsComboBox.SelectedIndex),color);
                    
                    currentRowIndex = ColorTable.FirstDisplayedScrollingRowIndex;

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
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.InstrumentName:
                            if (footer.PreviousReference.InstrumentName == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.MulticoreName:
                            if (footer.PreviousReference.MulticoreName == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.Position:
                            if (footer.PreviousReference.Position == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField1:
                            if (footer.PreviousReference.UserField1 == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField2:
                            if (footer.PreviousReference.UserField2 == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField3:
                            if (footer.PreviousReference.UserField3 == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField4:
                            if (footer.PreviousReference.UserField4 == item)
                            {
                                footer.BackgroundBrush = new SolidColorBrush(color);
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
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.InstrumentName:
                            if (header.PreviousReference.InstrumentName == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.MulticoreName:
                            if (header.PreviousReference.MulticoreName == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.Position:
                            if (header.PreviousReference.Position == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField1:
                            if (header.PreviousReference.UserField1 == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField2:
                            if (header.PreviousReference.UserField2 == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField3:
                            if (header.PreviousReference.UserField3 == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
                            }
                            break;
                        case LabelField.UserField4:
                            if (header.PreviousReference.UserField4 == item)
                            {
                                header.BackgroundBrush = new SolidColorBrush(color);
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
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.InstrumentName:
                                if (footer.PreviousReference.InstrumentName == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.MulticoreName:
                                if (footer.PreviousReference.MulticoreName == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.Position:
                                if (footer.PreviousReference.Position == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField1:
                                if (footer.PreviousReference.UserField1 == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField2:
                                if (footer.PreviousReference.UserField2 == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField3:
                                if (footer.PreviousReference.UserField3 == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField4:
                                if (footer.PreviousReference.UserField4 == item)
                                {
                                    footer.BackgroundBrush = new SolidColorBrush(color);
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
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.InstrumentName:
                                if (header.PreviousReference.InstrumentName == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.MulticoreName:
                                if (header.PreviousReference.MulticoreName == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.Position:
                                if (header.PreviousReference.Position == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField1:
                                if (header.PreviousReference.UserField1 == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField2:
                                if (header.PreviousReference.UserField2 == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField3:
                                if (header.PreviousReference.UserField3 == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
                                }
                                break;
                            case LabelField.UserField4:
                                if (header.PreviousReference.UserField4 == item)
                                {
                                    header.BackgroundBrush = new SolidColorBrush(color);
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
                    case LabelField.UserField1:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.UserField1 == item);
                        break;
                    case LabelField.UserField2:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.UserField2 == item);
                        break;
                    case LabelField.UserField3:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.UserField3 == item);
                        break;
                    case LabelField.UserField4:
                        searchResult = labelStrip.Footers.Find(searchItem =>
                            searchItem.PreviousReference.UserField4 == item);
                        break;
                    default:
                        break;
                }

                // Check if a result has been found.
                if (searchResult != null && searchResult.BackgroundBrush != null)
                {
                    return searchResult.BackgroundBrush.Color;
                }
            }

            // No matching result has been found.
            return Colors.Transparent;
        }

        // Sets any Remaining Null Colors to White. Called by LabelSetup Continue Button.
        public void SetUndefinedColors()
        {
            foreach (var labelStrip in Globals.LabelStrips)
            {
                // Footers
                foreach (var footer in labelStrip.Footers)
                {
                    if (footer.BackgroundBrush == null)
                    {
                        footer.BackgroundBrush = new SolidColorBrush(Colors.White);
                    }
                }

                // Headers
                foreach (var header in labelStrip.Headers)
                {
                    if (header.BackgroundBrush == null)
                    {
                        header.BackgroundBrush = new SolidColorBrush(Colors.White);
                    }
                }
            }
        }
    }
}
