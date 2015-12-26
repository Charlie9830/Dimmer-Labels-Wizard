using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    public class LabelStripTemplate : Style
    {
        public LabelStripTemplate()
        {
            TargetType = typeof(LabelStrip);

            // Setters.
            Setters.Add(new Setter(LabelStrip.UpperCellCountProperty, 1));
            Setters.Add(new Setter(LabelStrip.StripModeProperty, LabelStripMode.Dual));
            Setters.Add(new Setter(LabelStrip.LineWeightProperty, 1d));
            Setters.Add(new Setter(LabelStrip.StripHeightProperty, 70d));

            // Child Template
            Setters.Add(new Setter(LabelStrip.UpperCellsTemplateProperty, new LabelCellTemplate()));
        }
    }

    public class LabelCellTemplate : Style
    {
        public LabelCellTemplate()
        {
            TargetType = typeof(LabelCell);

            DimmerDistroUnit testUnit = new DimmerDistroUnit() { ChannelNumber = "Test Unit" };

            // Setters.
            Setters.Add(new Setter(LabelCell.WidthProperty, 70d));
            Setters.Add(new Setter(LabelCell.CellDataModeProperty, CellDataMode.MixedField));
            Setters.Add(new Setter(LabelCell.RowCountProperty, 2));

            // Child Template
            List<CellRowTemplate> cellRowTemplates = new List<CellRowTemplate>();

            cellRowTemplates.Add(new CellRowTemplate());
            cellRowTemplates.Last().Setters.Add(new Setter(CellRow.DataFieldProperty, LabelField.ChannelNumber));
            cellRowTemplates.Last().Setters.Add(new Setter(CellRow.DesiredFontSizeProperty, 14d));
            cellRowTemplates.Last().Setters.Add(new Setter(CellRow.FontProperty, new Typeface("Arial")));

            cellRowTemplates.Add(new CellRowTemplate());
            cellRowTemplates.Last().Setters.Add(new Setter(CellRow.DataFieldProperty, LabelField.InstrumentName));
            cellRowTemplates.Last().Setters.Add(new Setter(CellRow.DesiredFontSizeProperty, 12d));
            cellRowTemplates.Last().Setters.Add(new Setter(CellRow.FontProperty, new Typeface("Arial")));


            Setters.Add(new Setter(LabelCell.RowTemplatesProperty, cellRowTemplates));
        }
    }

    public class CellRowTemplate : Style
    {
        public CellRowTemplate()
        {
            TargetType = typeof(CellRow);

            // Setters

        }
    }
}
