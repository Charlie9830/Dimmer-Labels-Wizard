using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FriendlyLabelField
    {
        #region Constructors.
        public FriendlyLabelField(LabelField labelField, string friendlyName)
        {
            LabelField = labelField;
            FriendlyName = friendlyName;
        }
        #endregion

        #region Properties.
        public LabelField LabelField { get; set; }
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
