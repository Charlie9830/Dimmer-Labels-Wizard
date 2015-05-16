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
    public partial class FORM_UnparseableDataDisplay : Form
    {
        public FORM_UnparseableDataDisplay()
        {
            InitializeComponent();
        }

        private void FORM_UnparseableDataDisplay_Load(object sender, EventArgs e)
        {
            PopulateUnparseableDataGridView();
        }

        private void PopulateUnparseableDataGridView()
        {
            foreach(var element in Globals.UnParseableData)
            {
                UnparseableDataGridView.Rows.Add(element.ChannelNumber,element.DimmerNumberText,element.InstrumentName,element.MulticoreName,element.ImportIndex);
            }
        }
    }
}
