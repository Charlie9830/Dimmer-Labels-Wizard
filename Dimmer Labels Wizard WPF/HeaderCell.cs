using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Markup;

namespace Dimmer_Labels_Wizard_WPF
{
    public class HeaderCell : LabelCell
    {
        public HeaderCell()
        {

        }

        public HeaderCell(HeaderCellStorage storageObject)
        {
            Data = storageObject.Data;
            FontSize = storageObject.FontSize;

            Font = RebuildFont(storageObject.FontFamilyName, storageObject.OpenTypeFontWeight,
                storageObject.FontStyle);

            // Set LabelCell Values.
            TextBrush = new SolidColorBrush(storageObject.BaseStorage.TextColor.ToColor());
            BackgroundBrush = new SolidColorBrush(storageObject.BaseStorage.BackgroundColor.ToColor());
            PreviousReference = storageObject.BaseStorage.PreviousReference;
        }

        protected double _FontSize;
        protected Typeface _Font;

        public string Data { get; set; }
        public Typeface Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
            }
        }
        public double FontSize 
        {
            get
            {
                return _FontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _FontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }
        
  
        // Serialization Methods
        public HeaderCellStorage GenerateStorage()
        {
            HeaderCellStorage storage = new HeaderCellStorage();
            storage.Data = Data;
            storage.FontSize = FontSize;
            storage.FontFamilyName = Font.FontFamily.Source;
            storage.OpenTypeFontWeight = Font.Weight.ToOpenTypeWeight();
            storage.FontStyle = Font.Style.ToString();

            // Collect Base Class's Data. "Pack your things Dad, we are going to the Nursing home".
            storage.BaseStorage = GenerateLabelCellStorage();

            return storage;
        }
    }

    
    public class HeaderCellStorage
    {
        public string Data;
        public double FontSize;
        public string FontFamilyName;
        public int OpenTypeFontWeight;
        public string FontStyle;

        public LabelCellStorage BaseStorage;
    }


    public class HeaderCellWrapper
    {
        // Wraps a List of HeaderCells so they can be Tagged to outlines during Rendering.
        public List<HeaderCell> Cells = new List<HeaderCell>();
    }
}
