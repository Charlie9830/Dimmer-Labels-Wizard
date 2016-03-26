using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FriendlyImportFormat
    {
        #region Constructors.
        public FriendlyImportFormat(ImportFormat importFormat, string friendlyName)
        {
            ImportFormat = importFormat;
            FriendlyName = friendlyName;
        }
        #endregion

        #region Properties.
        public ImportFormat ImportFormat { get; set; }
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
