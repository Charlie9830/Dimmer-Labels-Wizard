using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FriendlySortField
    {
        public FriendlySortField(SortField sortField, string friendlyName)
        {
            SortField = sortField;
            FriendlyName = friendlyName;
        }

        public SortField SortField { get; set; }
        public string FriendlyName { get; set; }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
