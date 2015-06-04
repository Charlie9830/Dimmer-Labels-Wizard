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
    public partial class FontStyleControl : UserControl
    {
        public FontStyleControl()
        {
            InitializeComponent();

            BoldCheckBox.CheckedChanged += new EventHandler(this.CheckBoxes_StateChanged);
            ItalicsCheckBox.CheckedChanged += new EventHandler(this.CheckBoxes_StateChanged);
            UnderLineCheckBox.CheckedChanged += new EventHandler(this.CheckBoxes_StateChanged);
        }

        public FontStyle FontStyle
        {
            get
            {
                fontStyle = GetCheckBoxStates();
                return fontStyle;
            }

            set
            {
                fontStyle = value;
                SetCheckBoxStates(fontStyle);
            }
        }

        private FontStyle fontStyle;

        private FontStyle GetCheckBoxStates()
        {
            System.Drawing.FontStyle returnStyle = System.Drawing.FontStyle.Regular;

            if (BoldCheckBox.Checked == true)
            {
                returnStyle |= System.Drawing.FontStyle.Bold;
            }

            if (ItalicsCheckBox.Checked == true)
            {
                returnStyle |= System.Drawing.FontStyle.Italic;
            }

            if (UnderLineCheckBox.Checked == true)
            {
                returnStyle |= System.Drawing.FontStyle.Underline;
            }

            return returnStyle;
        }

        private void SetCheckBoxStates(FontStyle inputFontStyle)
        {
            // Check required CheckBoxes.
            if (inputFontStyle == FontStyle.Regular)
            {
                BoldCheckBox.Checked = false;
                ItalicsCheckBox.Checked = false;
                UnderLineCheckBox.Checked = false;
            }

            else
            {
                BoldCheckBox.Checked = inputFontStyle.HasFlag(FontStyle.Bold) ? true : false;
                ItalicsCheckBox.Checked = inputFontStyle.HasFlag(FontStyle.Italic) ? true : false;
                UnderLineCheckBox.Checked = inputFontStyle.HasFlag(FontStyle.Underline) ? true : false;
            }
        }

        private void CheckBoxes_StateChanged(object sender, EventArgs e)
        {
            System.Drawing.FontStyle currentStyle = fontStyle;

            // If FontStyle has really changed.
            if (currentStyle != GetCheckBoxStates())
            {
                fontStyle = GetCheckBoxStates();
                OnFontStyleChanged();
            }
        }

        //public delegate void FontStyleStateChangedEventHandler(object sender, EventArgs e);
        public event EventHandler FontStyleChanged;

        protected void OnFontStyleChanged()
        {
            FontStyleChanged(this, new EventArgs());
        }
    }
}
