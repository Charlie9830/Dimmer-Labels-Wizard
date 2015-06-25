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
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            FORM_MainWindow mainWindow = new FORM_MainWindow();
            mainWindow.ShowDialog();

            if (Globals.DebugActive == false)
            {
                FORM_UserParameterEntry UserParamEntry = new FORM_UserParameterEntry();
                UserParamEntry.ShowDialog();

                UserParameters.GenerateDistroRange();

                FileImport.ImportFile();

                if (Globals.UnParseableData.Count > 0)
                {
                    FORM_UnparseableDataDisplay UnparseableDataDisplay = new FORM_UnparseableDataDisplay();
                    UnparseableDataDisplay.ShowDialog();
                }

                foreach (var element in Globals.ClashingRangeData)
                {
                    Console.WriteLine("Clashing Data Dimmer Text {0}", element.DimmerNumberText);
                }

                Console.WriteLine("Sanitation Starting");
                DataHandling.SanitizeDimDistroUnits();
                Console.WriteLine("Sanitation Complete");

                FORM_InstrumentNameEntry instrumentNameEntry = new FORM_InstrumentNameEntry();
                instrumentNameEntry.ShowDialog();

                FORM_LabelSetup labelSetup = new FORM_LabelSetup();
                labelSetup.ShowDialog();

                UserParameters.SetDefaultRackLabelSettings();

                FORM_LabelEditor NextWindow = new FORM_LabelEditor();
                NextWindow.ShowDialog();
            }

            else
            {
                // Setup Mock User Paramater Inputs.
                #region Hardcoded User Paramaters.
                UserParameters.CreateDimmerObjects = true;
                UserParameters.DimmerRanges.Add(new Globals.DimmerRange(1, 1, 12));
                UserParameters.DimmerRanges.Add(new Globals.DimmerRange(2, 1, 24));
                UserParameters.DimmerRanges.Sort();

                UserParameters.CreateDistroObjects = false;
                UserParameters.StartDistroNumber = 1001; //1001;
                UserParameters.EndDistroNumber = 1024; //1024;
                UserParameters.DistroImportFormat = ImportFormatting.Format2;
                UserParameters.DimmerImportFormat = ImportFormatting.Format1;

                UserParameters.DMXAddressImportFormat = ImportFormatting.NoUniverseData;

                UserParameters.ChannelNumberColumnIndex = 0;
                UserParameters.DimmerNumberColumnIndex = 1;
                UserParameters.InstrumentTypeColumnIndex = 2;
                UserParameters.MulticoreNameColumnIndex = 3;
                UserParameters.PositionColumnIndex = 4;

                UserParameters.LabelWidthInMM = 20;
                UserParameters.LabelHeightInMM = 20;
                #endregion

                UserParameters.GenerateDistroRange();

                FileImport.ImportFile();

                if (Globals.UnParseableData.Count > 0)
                {
                    FORM_UnparseableDataDisplay UnparseableDataDisplay = new FORM_UnparseableDataDisplay();
                    UnparseableDataDisplay.ShowDialog();
                }

                Console.WriteLine("Sanitation Starting");
                DataHandling.SanitizeDimDistroUnits();
                Console.WriteLine("Sanitation Complete");

                UserParameters.HeaderField = LabelField.Position;
                UserParameters.FooterMiddleField = LabelField.ChannelNumber;
                UserParameters.FooterBottomField = LabelField.InstrumentName;

                Output.ExportToRackLabel();

                foreach (var element in Globals.LabelStrips)
                {
                    element.SetBackgroundColor(System.Drawing.Color.White);
                }

                UserParameters.SetDefaultRackLabelSettings();

                FORM_LabelEditor NextWindow = new FORM_LabelEditor();
                NextWindow.ShowDialog();
            }
        }
    }
}
