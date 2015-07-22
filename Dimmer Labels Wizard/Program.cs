using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Forms.MainWindow = new FORM_MainWindow();
            Application.Run(Forms.MainWindow);

            if (Globals.DebugActive == true)
            {
                // Setup Mock User Paramater Inputs.
                #region Hardcoded User Paramaters.
                UserParameters.CreateDimmerObjects = false;
                //UserParameters.DimmerRanges.Add(new Globals.DimmerRange(1, 1, 24));
                // UserParameters.DimmerRanges.Sort();

                UserParameters.CreateDistroObjects = true;
                UserParameters.StartDistroNumber = 1;
                UserParameters.EndDistroNumber = 60; 
                UserParameters.DistroImportFormat = ImportFormatting.Format1;
                UserParameters.DistroNumberPrefix = "N";
                UserParameters.DimmerImportFormat = ImportFormatting.Format3;

                UserParameters.DimmerRanges.Add(new Globals.DimmerRange(1, 1, 48));

                UserParameters.DMXAddressImportFormat = ImportFormatting.NoUniverseData;

                UserParameters.ChannelNumberColumnIndex = 0;
                UserParameters.DimmerNumberColumnIndex = 1;
                UserParameters.InstrumentTypeColumnIndex = 2;
                UserParameters.MulticoreNameColumnIndex = 3;
                UserParameters.PositionColumnIndex = 4;

                UserParameters.DimmerLabelWidthInMM = 16;
                UserParameters.DimmerLabelHeightInMM = 18;

                UserParameters.DistroLabelWidthInMM = 16;
                UserParameters.DistroLabelHeightInMM = 18;

                UserParameters.HeaderField = LabelField.MulticoreName;
                UserParameters.FooterMiddleField = LabelField.ChannelNumber;
                UserParameters.FooterBottomField = LabelField.InstrumentName;

                UserParameters.SingleLabel = true;
                #endregion

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

                Output.ExportToRackLabel();

                foreach (var element in Globals.LabelStrips)
                {
                    element.SetBackgroundColor(Colors.White);
                }

                UserParameters.SetDefaultRackLabelSettings();

                FORM_LabelEditorHost NextWindow = new FORM_LabelEditorHost();
                NextWindow.ShowDialog();
            }
        }
    }
}
