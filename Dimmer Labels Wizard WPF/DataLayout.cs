using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public struct DataLayout
    {
        public int FirstIndex;
        public int Length;
        public double FontSize;

        private Typeface _Font;
        private string _DisplayedText;

        public DataLayout(int firstIndex, int length, string rowData,
            Typeface font, double fontSize)
        {
            FirstIndex = firstIndex;
            FontSize = fontSize;
            _Font = font;
            Length = length;

            _DisplayedText = rowData.Substring(FirstIndex, Length);
        }

        #region Properties
        public string DisplayedText
        {
            get
            {
                return _DisplayedText;
            }
        }
        public Size TextSize
        {
            get
            {
                if (FontSize != 0d)
                {
                    return LabelCell.MeasureText(_DisplayedText, _Font, FontSize);
                }

                else
                {
                    return new Size(0, 0);
                }
            }
        }
        #endregion

    }
}
