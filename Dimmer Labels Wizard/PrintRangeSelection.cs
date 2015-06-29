using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dimmer_Labels_Wizard
{
    public partial class PrintRangeSelection : UserControl
    {
        public int RackQty;

        public List<int> RackRange = new List<int>();

        public PrintRangeSelection()
        {
            InitializeComponent();

            AllRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            RackRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            SelectionRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
        }
  
        private void PrintRangeSelection_Load(object sender, EventArgs e)
        {
            AllRadioButton.Checked = true;

            LowerRangeSelector.Enabled = false;
            UpperRangeSelector.Enabled = false;

            SelectionTextBox.Enabled = false;
            ExampleText.Enabled = false;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (NoneRadioButton.Checked == true)
            {
                LowerRangeSelector.Enabled = false;
                UpperRangeSelector.Enabled = false;
                SelectionTextBox.Enabled = false;
                ExampleText.Enabled = false;
            }

            if (AllRadioButton.Checked == true)
            {
                LowerRangeSelector.Enabled = false;
                UpperRangeSelector.Enabled = false;
                SelectionTextBox.Enabled = false;
                ExampleText.Enabled = false;
            }

            if (RackRadioButton.Checked == true)
            {
                LowerRangeSelector.Enabled = true;
                UpperRangeSelector.Enabled = true;
                SelectionTextBox.Enabled = false;
                ExampleText.Enabled = false;
            }

            if (SelectionRadioButton.Checked == true)
            {
                LowerRangeSelector.Enabled = false;
                UpperRangeSelector.Enabled = false;
                SelectionTextBox.Enabled = true;
                ExampleText.Enabled = true;
            }
        }

        public void GenerateRackRange()
        {
            RackRange.Clear();
            // None
            if (NoneRadioButton.Checked == true)
            {
                // No Action required.
            }

            // All
            if (AllRadioButton.Checked == true)
            {
                int rackNumber = 1;

                for (int count = 1; count <= RackQty; count++)
                {
                    RackRange.Add(rackNumber);
                    rackNumber++;
                }
            }

            // Rack
            if (RackRadioButton.Checked == true)
            {
                int rackNumber = (int)LowerRangeSelector.Value;
                for (int count = rackNumber; count <= (int)UpperRangeSelector.Value; count++)
                {
                    RackRange.Add(rackNumber);
                    rackNumber++;
                }
            }

            // Selection
            if (SelectionRadioButton.Checked == true)
            {
                int[] racks = ParseSelectionText();
                if (racks != null)
                {
                    RackRange.AddRange(racks);
                }
            }
        }

        // Returns array of Rack Numbers. Null if Selection Text box string parse Failed.
        private int[] ParseSelectionText()
        {
            char delimiter = ',';
            char hyphen = '-';
            char minus = '-';

            string selectionText = SelectionTextBox.Text;
            string[] selections = selectionText.Split(delimiter);
            List<int> returnList = new List<int>();

            // Remove delimiters, hyphens, spaces and Minus signs from String. String.Trim() does not work.
            string testParse = selectionText.Replace(delimiter.ToString(),"");
            testParse = testParse.Replace(hyphen.ToString(), "");
            testParse = testParse.Replace(minus.ToString(), "");
            testParse = testParse.Replace(" ", "");

            int tryParseOutResult;

            if (int.TryParse(testParse, out tryParseOutResult) == false)
            {
                string errorMessage = "Non Permitted character detected in selection box." +
                    "Only Numeric Characters, Spaces, '-' and ',' are allowed.";
                MessageBox.Show(errorMessage, "Error");

                return null;
            }
            foreach (var element in selections)
            {
                if (element != " " && element != "")
                // Range Selection.
                if (element.Contains(hyphen))
                {
                    string[] rackNumbers = element.Split(hyphen);
                    int lowerRange = Convert.ToInt32(rackNumbers.First().Trim());
                    int upperRange = Convert.ToInt32(rackNumbers.Last().Trim());

                    for (int count = lowerRange; count <= upperRange; count++)
                    {
                        if (returnList.Contains(count) == false)
                        {
                            returnList.Add(count);
                        }
                    }
                }

                // Single Selection
                else
                {
                    element.Trim();
                    int rackNumber = Convert.ToInt32(element);

                    if (returnList.Contains(rackNumber) == false)
                    {
                        returnList.Add(rackNumber);
                    }
                }
            }
        return returnList.ToArray();
        }

    }
}
