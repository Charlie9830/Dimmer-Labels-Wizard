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
    public partial class CellSeperator : UserControl
    {
        public int SplitIndex;

        // Default constructor to Allow Designer Compatability.
        public CellSeperator()
        {
            InitializeComponent();

            this.MouseEnter += new EventHandler(this.CellSeperator_MouseEnter);
            this.MouseLeave += new EventHandler(this.CellSeperator_MouseLeave);
            this.Click += new EventHandler(this.CellSeperator_Click);
        }

        public CellSeperator(int splitIndex)
        {
            SplitIndex = splitIndex;

            InitializeComponent();

            this.MouseEnter += new EventHandler(this.CellSeperator_MouseEnter);
            this.MouseLeave += new EventHandler(this.CellSeperator_MouseLeave);
            this.Click += new EventHandler(this.CellSeperator_Click);
        }

        private void CellSeperator_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Silver;
        }

        private void CellSeperator_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.DimGray;
        }

        private void CellSeperator_Click(object sender, EventArgs e)
        {
            CellSeperatorSelectEventArgs eventArgs = new CellSeperatorSelectEventArgs();
            eventArgs.SplitIndex = this.SplitIndex;

            OnCellSeperatorSelectEvent(eventArgs);
        }

        
        public delegate void CellSeperatorEventHandler(object sender, CellSeperatorSelectEventArgs e);
        public event CellSeperatorEventHandler CellSeperatorSelectEvent;

        protected void OnCellSeperatorSelectEvent(CellSeperatorSelectEventArgs e)
        {
            CellSeperatorSelectEvent(this, e);
        }
    }


    public class CellSeperatorSelectEventArgs : EventArgs
    {
        public int SplitIndex;
    }
}
