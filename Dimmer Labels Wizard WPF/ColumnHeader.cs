using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class ColumnHeader
    {
        #region Constructors.
        public ColumnHeader(string header, int index)
        {
            Header = header;
            HeaderLowerCase = header.ToLower();
            Index = index;
        }
        #endregion

        #region Properties.
        public string Header { get; set; }
        public string HeaderLowerCase { get; set; }
        public int Index { get; set; }
        #endregion

        #region Overrides.
        public override string ToString()
        {
            return Header;
        }
        #endregion


    }
}
