using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dimmer_Labels_Wizard
{
    public static class Forms
    {
        // Global Storage for Forms. Forms must be Initalized from Elsewhere.
        public static FORM_MainWindow MainWindow;
        public static FORM_UserParameterEntry UserParameterEntry;
        public static FORM_UnparseableDataDisplay UnparseableDataDisplay;
        public static FORM_InstrumentNameEntry InstrumentNameEntry;
        public static FORM_LabelSetup LabelSetup;
        public static FORM_LabelEditor LabelEditor;
    }
}
