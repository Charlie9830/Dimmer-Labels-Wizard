using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FriendlyUnitGroupBy
    {
        #region Constructors.
        public FriendlyUnitGroupBy(UnitGroupBy unitGroupBy, string friendlyName)
        {
            UnitGroupBy = unitGroupBy;
            FriendlyName = friendlyName;
        }
        #endregion

        #region Properties.
        public UnitGroupBy UnitGroupBy { get; set; }
        public string FriendlyName { get; set; }
        #endregion

        #region Overrides.
        public override string ToString()
        {
            return FriendlyName;
        }
        #endregion
    }
}
