using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;

namespace Dimmer_Labels_Wizard
{
    class Program
    {
        static void Main(string[] args)
        {
            FORM_UserParameterEntry UserParamEntry = new FORM_UserParameterEntry();
            UserParamEntry.ShowDialog();

            // Setup Mock User Paramater Inputs
            UserParameters.DimmerImportFormat = ImportFormatting.Format2;
            UserParameters.DistroImportFormat = ImportFormatting.Format1;

            UserParameters.LabelWidth = 50;
            UserParameters.LabelHeight = 60;

            UserParameters.StartDimmerNumber = 1;
            UserParameters.EndDimmerNumber = 223;
            UserParameters.StartDistroNumber = 1;
            UserParameters.EndDistroNumber = 72;

            UserParameters.HardCodeRackNumbers();
            FileImport.ImportFile();

            FORM_UnparseableDataDisplay UnparseableDataDisplay = new FORM_UnparseableDataDisplay();
            UnparseableDataDisplay.ShowDialog();

            DataHandling.SanitizeDimDistroUnits();

            Output.ExportToRackLabel();

            UserParameters.SetDefaultRackLabelSettings();

            FORM_LabelEditor NextWindow = new FORM_LabelEditor();
            NextWindow.ShowDialog();
        }
    }
}
