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
    public partial class FORM_GlobalApplyWarning : Form
    {
        public bool DontShowAgain;

        public FORM_GlobalApplyWarning()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (DontShowAgainComboBox.Checked == true)
            {
                DontShowAgain = true;
            }
            else
            {
                DontShowAgain = false;
            }
        }

        private void FORM_GlobalApplyWarning_Load(object sender, EventArgs e)
        {
            DontShowAgain = false;
        }
    }
}
