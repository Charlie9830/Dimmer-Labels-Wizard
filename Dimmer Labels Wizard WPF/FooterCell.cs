using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace Dimmer_Labels_Wizard_WPF
{
    public class FooterCell : LabelCell
    {
        public FooterCell()
        {
        }

        public FooterCell(FooterCellStorage storage)
        {
            TopData = storage.TopData;
            TopFontSize = storage.TopFontSize;
            TopFont = RebuildFont(storage.TopFontFamilyName, storage.TopOpenTypeFontWeight,
                storage.TopFontStyle);

            MiddleData = storage.MiddleData;
            MiddleFontSize = storage.MiddleFontSize;
            MiddleFont = RebuildFont(storage.MiddleFontFamilyName, storage.MiddleOpenTypeFontWeight,
                storage.MiddleFontStyle);

            BottomData = storage.BottomData;
            BottomFontSize = storage.BottomFontSize;
            BottomFont = RebuildFont(storage.BottomFontFamilyName, storage.BottomOpenTypeFontWeight,
                storage.BottomFontStyle);

            // Set LabelCell Values.
            TextBrush = new SolidColorBrush(storage.BaseStorage.TextColor.ToColor());
            BackgroundBrush = new SolidColorBrush(storage.BaseStorage.BackgroundColor.ToColor());
            PreviousReference = storage.BaseStorage.PreviousReference;
        }

        // Top Line Properties
        protected double _TopFontSize;
        protected Typeface _TopFont;

        public string TopData { get; set; }
        public Typeface TopFont
        {
            get
            {
                return _TopFont;
            }

            set
            {
                _TopFont = value;
            }
        }
        public double TopFontSize
        {
            get
            {
                return _TopFontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _TopFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }

        // Middle Line Properties
        protected double _MiddleFontSize;
        protected Typeface _MiddleFont;

        public string MiddleData { get; set; }
        public Typeface MiddleFont
        {
            get
            {
                return _MiddleFont;
            }
            set
            {
                _MiddleFont = value;
            }
        }
        public double MiddleFontSize
        {
            get
            {
                return _MiddleFontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _MiddleFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }

        //Bottom line Properties
        protected double _BottomFontSize;
        protected Typeface _BottomFont;

        public string BottomData { get; set; }
        public Typeface BottomFont
        {
            get
            {
                return _BottomFont;
            }
            set
            {
                _BottomFont = value;
            }
        }
        public double BottomFontSize
        {
            get
            {
                return _BottomFontSize;
            }

            set
            {
                // Round to Nearest Quarter.
                _BottomFontSize = Math.Round(value * 4, MidpointRounding.AwayFromZero) / 4;
            }
        }


        #region Serialization Methods
        public FooterCellStorage GenerateStorage()
        {
            FooterCellStorage storage = new FooterCellStorage();
            storage.TopData = TopData;
            storage.TopFontSize = TopFontSize;
            storage.TopFontFamilyName = TopFont.FontFamily.Source;
            storage.TopOpenTypeFontWeight = TopFont.Weight.ToOpenTypeWeight();
            storage.TopFontStyle = TopFont.Style.ToString();

            storage.MiddleData = MiddleData;
            storage.MiddleFontSize = MiddleFontSize;
            storage.MiddleFontFamilyName = MiddleFont.FontFamily.Source;
            storage.MiddleOpenTypeFontWeight = MiddleFont.Weight.ToOpenTypeWeight();
            storage.MiddleFontStyle = MiddleFont.Style.ToString();

            storage.BottomData = BottomData;
            storage.BottomFontSize = BottomFontSize;
            storage.BottomFontFamilyName = BottomFont.FontFamily.Source;
            storage.BottomOpenTypeFontWeight = BottomFont.Weight.ToOpenTypeWeight();
            storage.BottomFontStyle = BottomFont.Style.ToString();

            // Collect Base Class's Data. "Pack your things Dad, we are going to the Nursing home".
            storage.BaseStorage = GenerateLabelCellStorage();

            return storage;
        }
        #endregion
    }

    // Wraps a Footercell together with a TextBlock Position Enumeration.
    public class FooterCellWrapper
    {
        public FooterCell Cell { get; set; }
        public FooterTextPosition Position { get; set; }

        public FooterCellWrapper()
        {

        }

        public FooterCellWrapper(FooterCell cell, FooterTextPosition position)
        {
            Cell = cell;
            Position = position;
        }
    }

    
    public class FooterCellStorage
    {
        public string TopData;
        public double TopFontSize;
        public string TopFontFamilyName;
        public int TopOpenTypeFontWeight;
        public string TopFontStyle;

        public string MiddleData;
        public double MiddleFontSize;
        public string MiddleFontFamilyName;
        public int MiddleOpenTypeFontWeight;
        public string MiddleFontStyle;

        public string BottomData;
        public double BottomFontSize;
        public string BottomFontFamilyName;
        public int BottomOpenTypeFontWeight;
        public string BottomFontStyle;

        public LabelCellStorage BaseStorage;
    }
}
