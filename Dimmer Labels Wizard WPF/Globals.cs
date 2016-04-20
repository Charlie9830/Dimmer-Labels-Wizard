using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class Globals
    {
        static Globals()
        {
            // Construct the Default LabelStrip Template.

            // Upper Strip is in Single Field Mode.
            // Lower Strip Cell Row Templates.
            var lowerTopRow = new CellRowTemplate()
            {
                DataField = LabelField.MulticoreName,
                Font = new Typeface("Arial"),
                DesiredFontSize = 12d,
            };

            var lowerMiddleRow = new CellRowTemplate()
            {
                DataField = LabelField.ChannelNumber,
                Font = new Typeface("Arial"),
                DesiredFontSize = 16d,
            };

            var lowerBottomRow = new CellRowTemplate()
            {
                DataField = LabelField.InstrumentName,
                Font = new Typeface("Arial"),
                DesiredFontSize = 12d,
            };

            // Upper Cell Template.
            var upperCellTemplate = new LabelCellTemplate()
            {
                CellDataMode = CellDataMode.SingleField,
                RowHeightMode = CellRowHeightMode.Static,
                SingleFieldDataField = LabelField.Position,
                SingleFieldDesiredFontSize = 16d,
                SingleFieldFont = new Typeface("Arial"),
            };

            // Generate List of CellRowTemplates for construction of lowerCellTemplate.
            var lowerCellRowTemplates = new List<CellRowTemplate>();
            lowerCellRowTemplates.Add(lowerTopRow);
            lowerCellRowTemplates.Add(lowerMiddleRow);
            lowerCellRowTemplates.Add(lowerBottomRow);

            var lowerCellTemplate = new LabelCellTemplate()
            {
                CellDataMode = CellDataMode.MultiField,
                RowHeightMode = CellRowHeightMode.Automatic,
                CellRowTemplates = lowerCellRowTemplates,
            };


            // Generate LabelStripTemplate
            var labelStripTemplate = new LabelStripTemplate()
            {
                Name = "Default",
                IsBuiltIn = true,
                StripMode = LabelStripMode.Dual,
                StripHeight = 70d,
                UpperCellTemplate = upperCellTemplate,
                LowerCellTemplate = lowerCellTemplate,
            };

            // Templates.Add(labelStripTemplate);
            DefaultTemplate = labelStripTemplate;
        }

        // Standard FontSizes.
        public static double[] StandardFontSizes = new double[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        // List to hold DimDistroUnit Objects
        public static List<DimmerDistroUnit> DimmerDistroUnits = new List<DimmerDistroUnit>();

        /// <summary>
        /// Queries DimmerDistroUnits for Dimmers.
        /// </summary>
        public static IEnumerable<DimmerDistroUnit> Dimmers
        {
            get
            {
                return from unit in DimmerDistroUnits
                       where unit.RackUnitType == RackType.Dimmer
                       orderby unit.UniverseNumber
                       orderby unit.DimmerNumber
                       select unit;
            }
        }

        /// <summary>
        /// Queries DimmerDistroUnits for Distros.
        /// </summary>
        public static IEnumerable<DimmerDistroUnit> Distros
        {
            get
            {
                return from unit in DimmerDistroUnits
                       where unit.RackUnitType == RackType.Distro
                       orderby unit.DimmerNumber
                       select unit;
            }
        }

        // List to Hold StripData Objects.
        //public static ObservableCollection<Strip> Strips = new ObservableCollection<Strip>();

        // By Default, this is contained within the Templates Collection.
        public static LabelStripTemplate DefaultTemplate;

        // Template Storage.
        //public static ObservableCollection<LabelStripTemplate> Templates = 
        //    new ObservableCollection<LabelStripTemplate>();

        // Color Storage. (Dimmer, Color).
        public static Dictionary<int, Color> DimmerLabelColors = new Dictionary<int, Color>();
        public static Dictionary<int, Color> DistroLabelColors = new Dictionary<int, Color>();

    }
}

