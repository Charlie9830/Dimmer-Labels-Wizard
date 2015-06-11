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
    public partial class DimmerRangeInputControl : UserControl
    {
        public List<DimmerRangeSelector> Selectors = new List<DimmerRangeSelector>();
        public List<Label> Labels = new List<Label>();

        private int SelectorQty = 1;
        private int SelectorSeperation = 4;
        private int LabelYOffset = 10;
        
        public DimmerRangeInputControl()
        {
            InitializeComponent();
        }

        private void DimmerRangeInputControl_Load(object sender, EventArgs e)
        {

        }

        private void AddDimmerRangeSelector()
        {
            Selectors.Add(new DimmerRangeSelector());
            
            int selectorHeight = Selectors.Last().Height;
            Selectors.Last().Parent = this;
            Selectors.Last().BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            Labels.Add(new Label());
            Labels.Last().Parent = this;
            Labels.Last().Text = "Range " + SelectorQty;

            if (Selectors.Count == 1)
            {
                Selectors.Last().Location = new Point(60, 18);
                Labels.Last().Location = new Point(4, Selectors.Last().Location.Y + LabelYOffset);
            }

            else
            {
                Selectors.Last().Location = new Point(60, (Selectors[Selectors.Count - 2].Bottom) + SelectorSeperation);
                Labels.Last().Location = new Point(4, Selectors.Last().Location.Y + LabelYOffset);
            }

            SelectorQty += 1;
        }

        private void RemoveDimmerRangeSelector()
        {
            if (Selectors.Count != 0)
            {
                this.Controls.Remove(Selectors.Last());
                Selectors.RemoveAt(Selectors.Count - 1);

                this.Controls.Remove(Labels.Last());
                Labels.RemoveAt(Labels.Count - 1);

                SelectorQty -= 1;
            }
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            AddDimmerRangeSelector();
        }

        private void MinusButton_Click(object sender, EventArgs e)
        {
            RemoveDimmerRangeSelector();
        }
    }
}
