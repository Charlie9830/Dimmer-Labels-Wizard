using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FriendlySearchField
    {
        public FriendlySearchField(SearchField searchField, string friendlyName)
        {
            SearchField = searchField;
            FriendlyName = friendlyName;
        }

        public SearchField SearchField { get; set; }
        public string FriendlyName { get; set; }


        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
