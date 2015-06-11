﻿using System;
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

            FORM_UserParameterEntry UserParamEntry = new FORM_UserParameterEntry();
            UserParamEntry.ShowDialog();

            FORM_MainWindow mainWindow = new FORM_MainWindow();
            mainWindow.ShowDialog();

            

            //// Setup Mock User Paramater Inputs.
            //UserParameters.StartDimmerNumber = 1;
            //UserParameters.EndDimmerNumber = 24;
            //UserParameters.StartDistroNumber = 1001;
            //UserParameters.EndDistroNumber = 1024;
            //UserParameters.DimmerUniverses.Add(1);
            //UserParameters.DistroImportFormat = ImportFormatting.Format2;
            //UserParameters.DimmerImportFormat = ImportFormatting.Format2;
            //UserParameters.DistroNumberPrefix = "N";
            //UserParameters.DMXAddressImportFormat = ImportFormatting.NoUniverseData;

            //UserParameters.ChannelNumberColumnIndex = 0;
            //UserParameters.DimmerNumberColumnIndex = 1;
            //UserParameters.InstrumentTypeColumnIndex = 2;
            //UserParameters.MulticoreNameColumnIndex = 3;

            UserParameters.HardCodeRackNumbers();

            UserParameters.LabelWidthInMM = 25;
            UserParameters.LabelHeightInMM = 20;

            FileImport.ImportFile();
            
            if (Globals.UnParseableData.Count > 0)
            {
                FORM_UnparseableDataDisplay UnparseableDataDisplay = new FORM_UnparseableDataDisplay();
                UnparseableDataDisplay.ShowDialog();
            }

            Console.WriteLine("Sanitation Starting");

            DataHandling.SanitizeDimDistroUnits();

            Console.WriteLine("Sanitation Complete");

            FORM_InstrumentNameEntry instrumentNameEntry = new FORM_InstrumentNameEntry();
            instrumentNameEntry.ShowDialog();

            Output.ExportToRackLabel();

            UserParameters.SetDefaultRackLabelSettings();

            FORM_LabelEditor NextWindow = new FORM_LabelEditor();
            NextWindow.ShowDialog();
        }
    }
}
